
// StatusCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : Command, IStatusCommand
	{
		/// <summary></summary>
		public const long PpeAfterPlayerStatus = 1;

		public override void PlayerExecute()
		{
			RetCode rc;

			var artUids = new long[] { Globals.GameState.Ar, Globals.GameState.Sh };

			var armorClass = Armor.SkinClothes;

			foreach (var artUid in artUids)
			{
				if (artUid > 0)
				{
					var artifact = Globals.ADB[artUid];

					Debug.Assert(artifact != null);

					var ac = artifact.Wearable;

					Debug.Assert(ac != null);

					armorClass += ac.Field1;
				}
			}

			var armor = Globals.Engine.GetArmors(armorClass);

			Debug.Assert(armor != null);
			
			Globals.Buf.SetFormat("{0}", armor.Name);

			var charWeight = 0L;

			rc = ActorMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var args = Globals.CreateInstance<IStatDisplayArgs>(x =>
			{
				x.Monster = ActorMonster;
				x.ArmorString = Globals.Buf.ToString();
				x.SpellAbilities = Globals.GameState.Sa;
				x.Speed = Globals.GameState.Speed;
				x.CharmMon = Globals.Engine.GetCharismaFactor(Globals.Character.GetStats(Stat.Charisma));
				x.Weight = charWeight;
			});

			rc = Globals.Character.StatDisplay(args);

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

			Type = CommandType.Miscellaneous;
		}
	}
}
