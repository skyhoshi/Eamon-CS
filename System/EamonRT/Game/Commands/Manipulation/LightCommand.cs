
// LightCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LightCommand : Command, ILightCommand
	{
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null);

			var ac = gDobjArtifact.LightSource;

			if (ac != null)
			{
				if (!gDobjArtifact.IsUnmovable())
				{
					if (!gDobjArtifact.IsCarriedByCharacter())
					{
						if (!GetCommandCalled)
						{
							RedirectToGetCommand<ILightCommand>(gDobjArtifact);
						}
						else if (gDobjArtifact.DisguisedMonster == null)
						{
							NextState = Globals.CreateInstance<IStartState>();
						}

						goto Cleanup;
					}
				}

				if (ac.Field1 == 0)
				{
					PrintWontLight(gDobjArtifact);

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (gGameState.Ls == gDobjArtifact.Uid)
				{
					gOut.Write("{0}Extinguish {1} (Y/N): ", Environment.NewLine, gDobjArtifact.GetTheName());

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						rc = gDobjArtifact.RemoveStateDesc(gDobjArtifact.GetProvidingLightDesc());

						Debug.Assert(gEngine.IsSuccess(rc));

						gGameState.Ls = 0;

						PrintLightExtinguished(gDobjArtifact);
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (gGameState.Ls > 0)
				{
					var lsArtifact = gADB[gGameState.Ls];

					Debug.Assert(lsArtifact != null && lsArtifact.LightSource != null);

					gEngine.LightOut(lsArtifact);
				}

				rc = gDobjArtifact.AddStateDesc(gDobjArtifact.GetProvidingLightDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gGameState.Ls = gDobjArtifact.Uid;

				PrintLightObj(gDobjArtifact);
			}
			else
			{
				if (gActorMonster.IsInRoomLit() || gDobjArtifact.IsCarriedByCharacter())
				{
					PrintCantVerbObj(gDobjArtifact);
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
			if (!gActorMonster.IsInRoomLit())
			{
				gCommandParser.ObjData.ArtifactNotFoundFunc = () => { };
			}

			PlayerResolveArtifact();
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			return room.IsLit() && (artifact.LightSource != null ? artifact.IsCarriedByCharacter() : true);
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

			Type = CommandType.Manipulation;
		}
	}
}
