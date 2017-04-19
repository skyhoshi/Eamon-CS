
// RestoreCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RestoreCommand : Command, IRestoreCommand
	{
		public virtual long SaveSlot { get; set; }

		protected override void PlayerExecute()
		{
			IFileset fileset;
			RetCode rc;

			var origCurrState = Globals.CurrState;

			var filesetsCount = Globals.Database.GetFilesetsCount();

			Debug.Assert(filesetsCount <= Globals.RtEngine.NumSaveSlots);

			Debug.Assert(SaveSlot >= 1 && SaveSlot <= filesetsCount);

			var filesets = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

			fileset = filesets[(int)SaveSlot - 1];

			var config = Globals.CreateInstance<IConfig>();

			try
			{
				rc = Globals.Database.LoadConfigs(fileset.ConfigFileName, printOutput: false);

				if (Globals.Engine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadConfigs function call failed");

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Config = Globals.Engine.GetConfig();

				if (Globals.Config == null || Globals.Config.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, Globals.Config == null ? "Globals.Config == null" : "Globals.Config.Uid <= 0");

					Globals.ExitType = Enums.ExitType.Error;

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

				if (Globals.Engine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadGameDatabase function call failed");

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				// fileset is now invalid

				Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault();

				if (Globals.Character == null || Globals.Character.Uid <= 0 || Globals.Character.Status != Enums.Status.Adventuring || string.IsNullOrWhiteSpace(Globals.Character.Name) || string.Equals(Globals.Character.Name, "NONE", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Error.Write("{0}Error: {1}",
						Environment.NewLine,
						Globals.Character == null ? "Globals.Character == null" :
						Globals.Character.Uid <= 0 ? "Globals.Character.Uid <= 0" :
						Globals.Character.Status != Enums.Status.Adventuring ? "Globals.Character.Status != Enums.Status.Adventuring" :
						string.IsNullOrWhiteSpace(Globals.Character.Name) ? "string.IsNullOrWhiteSpace(Globals.Character.Name)" :
						"string.Equals(Globals.Character.Name, \"NONE\", StringComparison.OrdinalIgnoreCase)");

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Module = Globals.Engine.GetModule();

				if (Globals.Module == null || Globals.Module.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, Globals.Module == null ? "Globals.Module == null" : "Globals.Module.Uid <= 0");

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.GameState = Globals.Engine.GetGameState();

				if (Globals.GameState == null || Globals.GameState.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: {1}", Environment.NewLine, Globals.GameState == null ? "Globals.GameState == null" : "Globals.GameState.Uid <= 0");

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				var room = Globals.RDB[Globals.GameState.Ro];

				if (room == null)
				{
					Globals.Error.Write("{0}Error: room == null", Environment.NewLine);

					Globals.ExitType = Enums.ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				var artifacts = Globals.Database.ArtifactTable.Records.ToList();

				foreach (var artifact in artifacts)
				{
					if (artifact.IsCarriedByMonsterUid(Globals.GameState.Cm))
					{
						artifact.SetCarriedByCharacter();
					}
					else if (artifact.IsWornByMonsterUid(Globals.GameState.Cm))
					{
						artifact.SetWornByCharacter();
					}
				}

				var monsters = Globals.Database.MonsterTable.Records.ToList();

				foreach (var monster in monsters)
				{
					monster.InitGroupCount = monster.GroupCount;
				}
			}
			finally
			{
				config.Dispose();
			}

			Globals.CommandParser.LastInputStr = "";

			Globals.GameState.R2 = Globals.GameState.Ro;

			Globals.Database.Compact();

			Globals.Out.WriteLine("{0}Game restored.", Environment.NewLine);

			Globals.RtEngine.CreateInitialState(true);

			NextState = Globals.CurrState;

			Globals.CurrState = origCurrState;

		Cleanup:

			;
		}

		protected override void PlayerFinishParsing()
		{
			RetCode rc;
			long i;

			if (CommandParser.CurrToken < CommandParser.Tokens.Length && long.TryParse(CommandParser.Tokens[CommandParser.CurrToken], out i) && i >= 1 && i <= Globals.RtEngine.NumSaveSlots)
			{
				SaveSlot = i;

				CommandParser.CurrToken++;
			}

			var filesets = Globals.Database.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (SaveSlot < 1 || SaveSlot > filesetsCount)
			{
				while (true)
				{
					if (Globals.GameState.Die == 1)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
					}

					Globals.Out.WriteLine("{0}Saved games:", Environment.NewLine);

					for (i = 0; i < Globals.RtEngine.NumSaveSlots; i++)
					{
						Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					Globals.Out.WriteLine("{0}{1,3}. {2}", Environment.NewLine, i + 1, "(Don't restore, return to game)");

					if (Globals.GameState.Die == 1)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
					}

					Globals.Out.Write("{0}Your choice (1-{1}): ", Environment.NewLine, i + 1);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, 3, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					i = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (i >= 1 && i <= filesetsCount)
					{
						SaveSlot = i;

						break;
					}
					else if (i == Globals.RtEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (SaveSlot < 1 || SaveSlot > filesetsCount)
			{
				CommandParser.NextState.Dispose();

				if (Globals.GameState.Die == 1)
				{
					CommandParser.NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					CommandParser.NextState = Globals.CreateInstance<IStartState>();
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

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
