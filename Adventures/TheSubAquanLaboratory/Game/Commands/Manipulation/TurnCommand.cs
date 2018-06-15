
// TurnCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class TurnCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITurnCommand
	{
		protected virtual bool IsCharUOrD(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'U' || ch == 'D';
		}

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 65:

					// Alphabet dial

					Globals.Out.Write("{0}Turn it toward which end of the alphabet (Up or Down) (U/D): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, IsCharUOrD, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'U')
					{
						if (!gameState.AlphabetDial)
						{
							Globals.Engine.PrintEffectDesc(45);

							gameState.AlphabetDial = true;
						}
						else
						{
							Globals.Out.Print("The dial is already turned up to its maximum.");
						}
					}
					else
					{
						if (gameState.AlphabetDial)
						{
							Globals.Engine.PrintEffectDesc(46);

							gameState.AlphabetDial = false;
						}
						else
						{
							Globals.Out.Print("The dial is at its absolute lowest.");
						}
					}

					goto Cleanup;

				default:

					PrintCantVerbObj(DobjArtifact);

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
			PlayerResolveArtifact();
		}

		public TurnCommand()
		{
			SortOrder = 450;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "TurnCommand";

			Verb = "turn";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
