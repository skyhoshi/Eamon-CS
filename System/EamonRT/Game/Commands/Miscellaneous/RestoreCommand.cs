
// RestoreCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
	public class RestoreCommand : Command, IRestoreCommand
	{
		public virtual long SaveSlot { get; set; }

		public override void PlayerExecute()
		{
			IFileset fileset;
			RetCode rc;

			var origCurrState = Globals.CurrState;

			var filesetsCount = Globals.Database.GetFilesetsCount();

			Debug.Assert(filesetsCount <= gEngine.NumSaveSlots);

			Debug.Assert(SaveSlot >= 1 && SaveSlot <= filesetsCount);

			var filesets = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

			fileset = filesets[(int)SaveSlot - 1];

			var config = Globals.CreateInstance<IConfig>();

			try
			{
				rc = Globals.Database.LoadConfigs(fileset.ConfigFileName, printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadConfigs function call failed");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Config = gEngine.GetConfig();

				if (Globals.Config == null || Globals.Config.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, Globals.Config == null ? "Globals.Config == null" : "Globals.Config.Uid <= 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				config.RtFilesetFileName = Globals.CloneInstance(Globals.Config.RtFilesetFileName);

				config.RtCharacterFileName = Globals.CloneInstance(fileset.CharacterFileName);

				config.RtModuleFileName = Globals.CloneInstance(fileset.ModuleFileName);

				config.RtRoomFileName = Globals.CloneInstance(fileset.RoomFileName);

				config.RtArtifactFileName = Globals.CloneInstance(fileset.ArtifactFileName);

				config.RtEffectFileName = Globals.CloneInstance(fileset.EffectFileName);

				config.RtMonsterFileName = Globals.CloneInstance(fileset.MonsterFileName);

				config.RtHintFileName = Globals.CloneInstance(fileset.HintFileName);

				config.RtGameStateFileName = Globals.CloneInstance(fileset.GameStateFileName);

				rc = config.LoadGameDatabase(printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadGameDatabase function call failed");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				// fileset is now invalid

				Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault();

				if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Adventuring || string.IsNullOrWhiteSpace(gCharacter.Name) || string.Equals(gCharacter.Name, "NONE", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Error.Write("{0}Error: {1}",
						Environment.NewLine,
						gCharacter == null ? "gCharacter == null" :
						gCharacter.Uid <= 0 ? "gCharacter.Uid <= 0" :
						gCharacter.Status != Status.Adventuring ? "gCharacter.Status != Status.Adventuring" :
						string.IsNullOrWhiteSpace(gCharacter.Name) ? "string.IsNullOrWhiteSpace(gCharacter.Name)" :
						"string.Equals(gCharacter.Name, \"NONE\", StringComparison.OrdinalIgnoreCase)");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Module = gEngine.GetModule();

				if (Globals.Module == null || Globals.Module.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, Globals.Module == null ? "Globals.Module == null" : "Globals.Module.Uid <= 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.GameState = gEngine.GetGameState();

				if (gGameState == null || gGameState.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, gGameState == null ? "gGameState == null" : "gGameState.Uid <= 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				var room = gRDB[gGameState.Ro];

				if (room == null)
				{
					Globals.Error.Write("{0}Error: room == null", Environment.NewLine);

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				var artifacts = Globals.Database.ArtifactTable.Records.ToList();

				foreach (var artifact in artifacts)
				{
					if (artifact.IsCarriedByMonsterUid(gGameState.Cm))
					{
						artifact.SetCarriedByCharacter();
					}
					else if (artifact.IsWornByMonsterUid(gGameState.Cm))
					{
						artifact.SetWornByCharacter();
					}
				}

				var monsters = Globals.Database.MonsterTable.Records.ToList();

				foreach (var monster in monsters)
				{
					monster.InitGroupCount = monster.GroupCount;

					monster.ResolveFriendlinessPct(gCharacter);
				}

				rc = gEngine.ValidateRecordsAfterDatabaseLoaded();

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: ValidateRecordsAfterDatabaseLoaded function call failed");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}
			}
			finally
			{
				config.Dispose();
			}

			gCommandParser.LastInputStr = "";

			gGameState.R2 = gGameState.Ro;

			gOut.Print("Game restored.");

			gEngine.CreateInitialState(true);

			NextState = Globals.CurrState;

			Globals.CurrState = origCurrState;

		Cleanup:

			;
		}

		public override void PlayerFinishParsing()
		{
			RetCode rc;
			long i;

			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length && long.TryParse(gCommandParser.Tokens[gCommandParser.CurrToken], out i) && i >= 1 && i <= gEngine.NumSaveSlots)
			{
				SaveSlot = i;

				gCommandParser.CurrToken++;
			}

			var filesets = Globals.Database.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (SaveSlot < 1 || SaveSlot > filesetsCount)
			{
				while (true)
				{
					if (gGameState.Die == 1)
					{
						gOut.Print("{0}", Globals.LineSep);
					}

					gOut.Print("Saved games:");

					for (i = 0; i < gEngine.NumSaveSlots; i++)
					{
						gOut.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					gOut.Print("{0,3}. {1}", i + 1, "(Don't restore, return to game)");

					if (gGameState.Die == 1)
					{
						gOut.Print("{0}", Globals.LineSep);
					}

					gOut.Write("{0}Your choice (1-{1}): ", Environment.NewLine, i + 1);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, 3, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					i = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (i >= 1 && i <= filesetsCount)
					{
						SaveSlot = i;

						break;
					}
					else if (i == gEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (SaveSlot < 1 || SaveSlot > filesetsCount)
			{
				if (gGameState.Die == 1)
				{
					gCommandParser.NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public RestoreCommand()
		{
			SortOrder = 420;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "RestoreCommand";

			Verb = "restore";

			Type = CommandType.Miscellaneous;
		}
	}
}
