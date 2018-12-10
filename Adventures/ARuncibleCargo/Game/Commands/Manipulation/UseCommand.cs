
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeBeforeArtifactUse)
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

						var cargoArtifact = Globals.ADB[129];

						Debug.Assert(cargoArtifact != null);

						var explosiveDeviceArtifact = Globals.ADB[43];

						Debug.Assert(explosiveDeviceArtifact != null);

						var princeMonster = Globals.MDB[38];

						Debug.Assert(princeMonster != null);

						if (explosiveDeviceArtifact.IsCarriedByContainer(cargoArtifact) && cargoArtifact.IsCarriedByMonster(princeMonster))
						{
							var gatesArtifact = Globals.ADB[137];

							Debug.Assert(gatesArtifact != null);

							var ac = gatesArtifact.DoorGate;

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

								NextState = Globals.CreateInstance<IStartState>();

								GotoCleanup = true;
							}
							else
							{
								Globals.Engine.PrintEffectDesc(137);

								NextState = Globals.CreateInstance<IStartState>();

								GotoCleanup = true;
							}
						}
						else
						{
							Globals.Engine.PrintEffectDesc(136);

							NextState = Globals.CreateInstance<IStartState>();

							GotoCleanup = true;
						}

						break;

					default:

						base.PlayerProcessEvents(eventType);

						break;
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
