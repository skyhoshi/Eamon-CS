﻿
// QuitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class QuitCommand : Command, IQuitCommand
	{
		public virtual bool GoToMainHall { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			if (GoToMainHall)
			{
				gOut.Write("{0}Return to the Main Hall (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					gGameState.Die = -1;

					Globals.ExitType = ExitType.GoToMainHall;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}
			}
			else
			{
				if (Globals.Database.GetFilesetsCount() == 0)
				{
					gOut.Print("[You haven't saved a game yet but {0} will be left here should you choose to return.  Use \"quit hall\" if you don't want {1} to stay.]",
						ActorMonster.Name,
						ActorMonster.EvalGender("him", "her", "it"));
				}

				gOut.Write("{0}Do you really want to quit (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					Globals.ExitType = ExitType.Quit;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public QuitCommand()
		{
			SortOrder = 430;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Uid = 61;

			Name = "QuitCommand";

			Verb = "quit";

			Type = CommandType.Miscellaneous;
		}
	}
}
