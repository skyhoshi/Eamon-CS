
// LightCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LightCommand : Command, ILightCommand
	{
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.LightSource);

			if (ac != null)
			{
				if (!DobjArtifact.IsUnmovable() && !DobjArtifact.IsCarriedByCharacter())
				{
					PrintTakingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>();

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<ILightCommand>();

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				if (ac.Field1 == 0)
				{
					Globals.Out.Print("{0} won't light.", DobjArtifact.EvalPlural("It", "They"));

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (Globals.GameState.Ls == DobjArtifact.Uid)
				{
					Globals.Out.Write("{0}Extinguish {1} (Y/N): ", Environment.NewLine, DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetProvidingLightDesc());

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.GameState.Ls = 0;

						Globals.GameState.Lt = (long)Globals.RDB[Globals.GameState.Ro].LightLvl;
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (Globals.GameState.Ls > 0)
				{
					var lsArtifact = Globals.ADB[Globals.GameState.Ls];

					Debug.Assert(lsArtifact != null && lsArtifact.IsLightSource());

					Globals.Engine.LightOut(lsArtifact);
				}

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetProvidingLightDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.GameState.Ls = DobjArtifact.Uid;

				Globals.GameState.Lt = 1;

				Globals.Out.Print("You've lit {0}.", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
			}
			else
			{
				if (ActorMonster.IsInRoomLit() || DobjArtifact.IsCarriedByCharacter())
				{
					PrintCantVerbObj(DobjArtifact);
				}

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (!ActorMonster.IsInRoomLit())
			{
				CommandParser.ObjData.ArtifactNotFoundFunc = () => { };
			}

			PlayerResolveArtifact();
		}

		public LightCommand()
		{
			SortOrder = 170;

			IsDarkEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			IsMonsterEnabled = false;

			Name = "LightCommand";

			Verb = "light";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
