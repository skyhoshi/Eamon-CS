
// StatusCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : Command, IStatusCommand
	{
		/// <summary>
		/// This event fires after the player's status text has been printed.
		/// </summary>
		public const long PpeAfterPlayerStatus = 1;

		public override void PlayerExecute()
		{
			var ar = Globals.GameState.Ar;

			var sh = Globals.GameState.Sh;

			Globals.Buf.Clear();

			if (ar > 0)
			{
				var artifact = Globals.ADB[ar];

				Debug.Assert(artifact != null);

				Globals.Buf.AppendFormat("{0}", artifact.EvalPlural(artifact.Name, artifact.GetPluralName01(Globals.Buf01)));
			}

			if (sh > 0)
			{
				if (ar > 0)
				{
					Globals.Buf.Append(" and ");
				}

				var artifact = Globals.ADB[sh];

				Debug.Assert(artifact != null);

				Globals.Buf.AppendFormat("{0}", artifact.EvalPlural(artifact.Name, artifact.GetPluralName01(Globals.Buf01)));
			}

			if (Globals.Buf.Length == 0)
			{
				Globals.Buf.SetFormat("(none)");
			}

			var args = Globals.CreateInstance<IStatDisplayArgs>(x =>
			{
				x.Monster = ActorMonster;
				x.ArmorString = Globals.Buf.ToString().FirstCharToUpper();
				x.SpellAbilities = Globals.GameState.Sa;
				x.Speed = Globals.GameState.Speed;
				x.CharmMon = Globals.Engine.GetCharismaFactor(Globals.Character.GetStats(Enums.Stat.Charisma));
				x.Weight = Globals.GameState.Wt;
			});

			var rc = Globals.Character.StatDisplay(args);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			PlayerProcessEvents(PpeAfterPlayerStatus);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public StatusCommand()
		{
			SortOrder = 370;

			IsDarkEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			IsMonsterEnabled = false;

			Name = "StatusCommand";

			Verb = "status";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
