
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : Command, IRemoveCommand
	{
		public virtual bool GetCommandCalled { get; set; }

		protected virtual bool IsDobjArtifactDisguisedMonster()
		{
			return DobjArtifact.IsDisguisedMonster();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			if (IobjArtifact != null)
			{
				var count = 0L;

				var weight = DobjArtifact.Weight;

				if (!DobjArtifact.IsUnmovable01())
				{
					var rc = DobjArtifact.GetContainerInfo(ref count, ref weight, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				if (!GetCommandCalled)
				{
					if (IobjArtifact.IsCarriedByCharacter())
					{
						Globals.GameState.Wt -= weight;
					}

					NextState = Globals.CreateInstance<IGetCommand>(x =>
					{
						x.PreserveNextState = true;
					});

					CopyCommandData(NextState as ICommand, false);

					NextState.NextState = Globals.CreateInstance<IRemoveCommand>(x =>
					{
						x.GetCommandCalled = true;
					});

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (IobjArtifact.IsCarriedByCharacter())
					{
						Globals.GameState.Wt += weight;
					}

					if (!IsDobjArtifactDisguisedMonster())
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (!DobjArtifact.Seen)
				{
					Globals.Buf.Clear();

					var rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);

					DobjArtifact.Seen = true;
				}

				if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByCharacter() && NextState == null)
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand, false);
				}
			}
			else
			{
				var arArtifact = Globals.ADB[Globals.GameState.Ar];

				var shArtifact = Globals.ADB[Globals.GameState.Sh];

				var arAc = arArtifact != null ? arArtifact.GetArtifactClass(Enums.ArtifactType.Wearable) : null;

				var shAc = shArtifact != null ? shArtifact.GetArtifactClass(Enums.ArtifactType.Wearable) : null;

				if (DobjArtifact.Uid == Globals.GameState.Sh)
				{
					ActorMonster.Armor = arAc != null ? arAc.Field5 / 2 : 0;

					Globals.GameState.Sh = 0;
				}

				if (DobjArtifact.Uid == Globals.GameState.Ar)
				{
					ActorMonster.Armor = shAc != null ? shAc.Field5 : 0;

					Globals.GameState.Ar = 0;
				}

				DobjArtifact.SetCarriedByCharacter();

				PrintRemoved(DobjArtifact);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsWornByCharacter()
			};

			CommandParser.ObjData.ArtifactMatchFunc = () =>
			{
				if (CommandParser.ObjData.FilterArtifactList.Count > 1)
				{
					PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

					CommandParser.NextState.Dispose();

					CommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
				else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
				{
					CommandParser.ObjData = CommandParser.IobjData;

					CommandParser.ObjData.QueryDesc = string.Format("{0}From what? ", Environment.NewLine);

					PlayerResolveArtifact();

					if (IobjArtifact != null)
					{
						var ac = IobjArtifact.GetArtifactClass(Enums.ArtifactType.Container);

						if (ac != null)
						{
							if (ac.IsOpen())
							{
								CommandParser.ObjData = CommandParser.DobjData;

								CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
								{
									a => a.IsCarriedByContainer(IobjArtifact)
								};

								CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch;

								CommandParser.ObjData.ArtifactNotFoundFunc = PrintDontFollowYou;

								PlayerResolveArtifact();
							}
							else
							{
								PrintMustFirstOpen(IobjArtifact);

								CommandParser.NextState.Dispose();

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
						}
						else
						{
							PrintDontFollowYou();

							CommandParser.NextState.Dispose();

							CommandParser.NextState = Globals.CreateInstance<IStartState>();
						}
					}
				}
				else
				{
					CommandParser.ObjData.RevealEmbeddedArtifactFunc(ActorRoom, CommandParser.ObjData.FilterArtifactList[0]);

					CommandParser.SetArtifact(CommandParser.ObjData.FilterArtifactList[0]);
				}
			};

			PlayerResolveArtifact();
		}

		public RemoveCommand()
		{
			SortOrder = 220;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "RemoveCommand";

			Verb = "remove";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
