
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IUseCommand))]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		protected override void PlayerProcessEvents()
		{
			switch (DobjArtifact.Uid)
			{
				case 34:

					// Telescope

					Globals.Engine.PrintEffectDesc(121);

					GotoCleanup = true;

					break;

				case 129:

					// Cargo

					Globals.Engine.PrintEffectDesc(128);

					GotoCleanup = true;

					break;

				case 45:

					// Detonator

					var artifact = Globals.ADB[129];

					Debug.Assert(artifact != null);

					var artifact01 = Globals.ADB[43];

					Debug.Assert(artifact01 != null);

					var monster = Globals.MDB[38];

					Debug.Assert(monster != null);

					if (artifact01.IsCarriedByContainer(artifact) && artifact.IsCarriedByMonster(monster))
					{
						var artifact02 = Globals.ADB[137];

						Debug.Assert(artifact02 != null);

						var ac = artifact02.GetArtifactClass(Enums.ArtifactType.DoorGate);

						Debug.Assert(ac != null);

						if (!ac.IsOpen())
						{
							// Blow up bandits with explosive-rigged Cargo

							Globals.Out.Print("You activate the detonator...");

							Globals.Out.Print("{0}", Globals.LineSep);

							Globals.Engine.PrintEffectDesc(138);

							Globals.In.KeyPress(Globals.Buf);

							Globals.GameState.Die = 0;

							Globals.ExitType = Enums.ExitType.FinishAdventure;

							Globals.MainLoop.ShouldShutdown = true;

							NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

							GotoCleanup = true;
						}
						else
						{
							Globals.Engine.PrintEffectDesc(137);

							NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

							GotoCleanup = true;
						}
					}
					else
					{
						Globals.Engine.PrintEffectDesc(136);

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

						GotoCleanup = true;
					}

					break;

				default:

					base.PlayerProcessEvents();

					break;
			}
		}
	}
}
