
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(IExamineCommand))]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, Framework.Commands.IExamineCommand
	{
		public virtual bool ExamineConsole { get; set; }

		public virtual void RevealArtifact(long artifactUid, bool examineConsole = false)
		{
			var artifact = gADB[artifactUid];

			Debug.Assert(artifact != null);

			if (!artifact.Seen)
			{
				artifact.SetInRoom(ActorRoom);

				var command = Globals.CreateInstance<IExamineCommand>(x =>
				{
					((Framework.Commands.IExamineCommand)x).ExamineConsole = examineConsole;
				});

				CopyCommandData(command);

				command.Dobj = artifact;

				NextState = command;
			}
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterArtifactFullDescPrint)
			{
				switch (DobjArtifact.Uid)
				{
					case 2:
					case 83:

						// Engraving/fake wall

						if (gGameState.FakeWallExamines < 2)
						{
							gOut.Print("Examining {0} reveals something curious:", DobjArtifact.GetTheName());
						}

						gGameState.FakeWallExamines++;

						gEngine.PrintEffectDesc(40 + gGameState.FakeWallExamines);

						if (gGameState.FakeWallExamines > 2)
						{
							gGameState.FakeWallExamines = 2;
						}

						break;

					case 23:

						// Magnetic fusion power plant

						gEngine.PrintEffectDesc(37);

						var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.DfMonster = ActorMonster;

							x.OmitArmor = true;
						});

						combatSystem.ExecuteCalculateDamage(1, 6);

						break;

					case 25:

						// Pool pals

						if (!gGameState.Shark)
						{
							var largeHammerheadMonster = gMDB[7];

							Debug.Assert(largeHammerheadMonster != null);

							largeHammerheadMonster.SetInRoom(ActorRoom);

							var smallHammerheadMonster = gMDB[8];

							Debug.Assert(smallHammerheadMonster != null);

							smallHammerheadMonster.SetInRoom(ActorRoom);

							gEngine.PrintEffectDesc(1);

							gGameState.Shark = true;

							NextState = Globals.CreateInstance<IStartState>();
						}

						break;

					case 45:

						RevealArtifact(46);

						break;

					case 63:

						if (ExamineConsole)
						{
							RevealArtifact(64, true);
						}
						else
						{
							RevealArtifact(65);
						}

						break;

					case 58:

						RevealArtifact(59);

						break;

					case 59:

						RevealArtifact(60);

						break;

					case 64:

						if (!ExamineConsole)
						{
							RevealArtifact(66);
						}

						break;

					case 66:

						RevealArtifact(67);

						break;

					case 67:

						RevealArtifact(68);

						break;

					case 68:

						RevealArtifact(69);

						break;

					case 69:

						RevealArtifact(70);

						break;

					case 62:

						RevealArtifact(63, true);

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
