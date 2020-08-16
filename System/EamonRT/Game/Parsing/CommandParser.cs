
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : ICommandParser
	{
		public IMonster _actorMonster;

		public IRoom _actorRoom;

		public virtual StringBuilder InputBuf { get; set; }

		public virtual string LastInputStr { get; set; }

		public virtual string[] Tokens { get; set; }

		public virtual long CurrToken { get; set; }

		public virtual long PrepTokenIndex { get; set; }

		public virtual IPrep Prep { get; set; }

		public virtual IMonster ActorMonster 
		{ 
			get
			{
				return NextCommand != null ? NextCommand.ActorMonster : _actorMonster;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.ActorMonster = value;
				}
				else
				{
					_actorMonster = value;
				}
			}
		}

		public virtual IRoom ActorRoom
		{
			get
			{
				return NextCommand != null ? NextCommand.ActorRoom : _actorRoom;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.ActorRoom = value;
				}
				else
				{
					_actorRoom = value;
				}
			}
		}

		public virtual IGameBase Dobj
		{
			get
			{
				return NextCommand?.Dobj;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.Dobj = value;
				}
			}
		}

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return NextCommand?.DobjArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return NextCommand?.DobjMonster;
			}
		}

		public virtual IGameBase Iobj
		{
			get
			{
				return NextCommand?.Iobj;
			}

			set
			{
				if (NextCommand != null)
				{
					NextCommand.Iobj = value;
				}
			}
		}

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return NextCommand?.IobjArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return NextCommand?.IobjMonster;
			}
		}

		public virtual IParserData DobjData { get; set; }

		public virtual IParserData IobjData { get; set; }

		public virtual IParserData ObjData { get; set; }

		public virtual IState NextState { get; set; }

		public virtual ICommand NextCommand
		{
			get
			{
				return NextState as ICommand;
			}
		}

		public virtual void PlayerFinishParsingAttackCommand()
		{
			ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

			ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			ObjData.ArtifactNotFoundFunc = NextCommand.PrintNobodyHereByThatName;

			PlayerResolveMonster();
		}

		public virtual void PlayerFinishParsingBlastCommand()
		{
			ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

			ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			ObjData.ArtifactNotFoundFunc = NextCommand.PrintNobodyHereByThatName;

			PlayerResolveMonster();
		}

		public virtual void PlayerFinishParsingFreeCommand()
		{
			ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

			ObjData.MonsterMatchFunc = PlayerMonsterMatch01;

			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingGiveCommand()
		{
			long i;

			var giveCommand = NextCommand as IGiveCommand;

			Debug.Assert(giveCommand != null);

			ParseName();

			if (long.TryParse(ObjData.Name, out i) && i > 0)
			{
				giveCommand.GoldAmount = i;
			}

			if (giveCommand.GoldAmount == 0)
			{
				ObjData.ArtifactMatchFunc = () =>
				{
					if (ObjData.FilterArtifactList.Count > 1)
					{
						NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

						NextState = Globals.CreateInstance<IStartState>();
					}
					else if (ObjData.FilterArtifactList.Count < 1)
					{
						ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsWornByCharacter()
						};

						ObjData.ArtifactMatchFunc = () =>
						{
							if (ObjData.FilterArtifactList.Count > 1)
							{
								NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

								NextState = Globals.CreateInstance<IStartState>();
							}
							else if (ObjData.FilterArtifactList.Count < 1)
							{
								ObjData.ArtifactNotFoundFunc();

								NextState = Globals.CreateInstance<IStartState>();
							}
							else
							{
								NextCommand.PrintWearingRemoveFirst(ObjData.FilterArtifactList[0]);

								NextState = Globals.CreateInstance<IStartState>();
							}
						};

						PlayerResolveArtifact();
					}
					else
					{
						ObjData.RevealEmbeddedArtifactFunc(ActorRoom, ObjData.FilterArtifactList[0]);

						SetArtifact(ObjData.FilterArtifactList[0]);
					}
				};

				PlayerResolveArtifact();
			}

			if (giveCommand.GoldAmount > 0 || DobjArtifact != null)
			{
				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}To whom? ", Environment.NewLine);

				PlayerResolveMonster();
			}
		}

		public virtual void PlayerFinishParsingHealCommand()
		{
			if (!Globals.IsRulesetVersion(5) && CurrToken < Tokens.Length)
			{
				PlayerResolveMonster();
			}
			else
			{
				Dobj = gCharMonster;
			}
		}

		public virtual void PlayerFinishParsingRequestCommand()
		{
			ParseName();

			ObjData = IobjData;

			ObjData.QueryDescFunc = () => string.Format("{0}From whom? ", Environment.NewLine);

			PlayerResolveMonster();

			if (IobjMonster != null)
			{
				ObjData = DobjData;

				ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByMonster(IobjMonster),
					a => a.IsWornByMonster(IobjMonster)
				};

				ObjData.ArtifactMatchFunc = PlayerArtifactMatch02;

				ObjData.ArtifactNotFoundFunc = () =>
				{
					gOut.Print("{0}{1} have it.",
						IobjMonster.GetTheName(true),
						IobjMonster.EvalPlural(" doesn't", " don't"));
				};

				PlayerResolveArtifact();
			}
		}

		public virtual void PlayerFinishParsingCloseCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingDrinkCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingDropCommand()
		{
			ParseName();

			if (string.Equals(ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IDropCommand>().DropAll = true;
			}
			else
			{
				ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter(),
					a => a.IsWornByCharacter(),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively)
				};

				ObjData.ArtifactNotFoundFunc = NextCommand.PrintDontHaveIt;

				PlayerResolveArtifact();
			}
		}

		public virtual void PlayerFinishParsingEatCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingExamineCommand()
		{
			ParseName();

			NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

			if (string.Equals(ObjData.Name, "room", StringComparison.OrdinalIgnoreCase) || string.Equals(ObjData.Name, "area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;
			}
			else
			{
				ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively),
					a => a.IsWornByCharacter()
				};

				if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
				{
					ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				ObjData.MonsterNotFoundFunc = NextCommand.PrintYouSeeNothingSpecial;

				PlayerResolveArtifact();
			}
		}

		public virtual void PlayerFinishParsingGetCommand()
		{
			ParseName();

			if (string.Equals(ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IGetCommand>().GetAll = true;
			}
			else
			{
				ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
				};

				ObjData.ArtifactNotFoundFunc = NextCommand.PrintCantVerbThat;

				PlayerResolveArtifact();
			}
		}

		public virtual void PlayerFinishParsingLightCommand()
		{
			if (!ActorMonster.IsInRoomLit())
			{
				ObjData.ArtifactNotFoundFunc = () => { };
			}

			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingOpenCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingPutCommand()
		{
			PlayerResolveArtifact();

			if (DobjArtifact != null)
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}Put {1} {2} what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType) ? gEngine.EvalContainerType(NextCommand.ContainerType, "inside", "on", "under", "behind") : "in");

				PlayerResolveArtifact();

				if (IobjArtifact != null)
				{
					if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
					{
						var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

						var defaultAc = IobjArtifact.GetArtifactCategory(artTypes);

						NextCommand.ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
					}
				}
			}
		}

		public virtual void PlayerFinishParsingReadCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingReadyCommand()
		{
			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingRemoveCommand()
		{
			ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsWornByCharacter()
			};

			ObjData.ArtifactMatchFunc = () =>
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				if (ObjData.FilterArtifactList.Count > 1)
				{
					NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

					NextState = Globals.CreateInstance<IStartState>();
				}
				else if (ObjData.FilterArtifactList.Count < 1)
				{
					ObjData = IobjData;

					ObjData.QueryDescFunc = () => string.Format("{0}From {1}what? ", Environment.NewLine, Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType) ? gEngine.EvalContainerType(NextCommand.ContainerType, "inside ", "on ", "under ", "behind ") : "");

					PlayerResolveArtifact();

					if (IobjArtifact != null)
					{
						if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
						{
							var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

							var defaultAc = IobjArtifact.GetArtifactCategory(artTypes);

							NextCommand.ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
						}

						var ac = gEngine.EvalContainerType(NextCommand.ContainerType, IobjArtifact.InContainer, IobjArtifact.OnContainer, IobjArtifact.UnderContainer, IobjArtifact.BehindContainer);

						if (ac != null)
						{
							if (ac != IobjArtifact.InContainer || ac.IsOpen())
							{
								ObjData = DobjData;

								ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
								{
									a => a.IsCarriedByContainer(IobjArtifact) && a.GetCarriedByContainerContainerType() == NextCommand.ContainerType
								};

								ObjData.ArtifactMatchFunc = PlayerArtifactMatch;

								ObjData.ArtifactNotFoundFunc = NextCommand.PrintDontFollowYou;

								PlayerResolveArtifact();
							}
							else
							{
								NextCommand.PrintMustFirstOpen(IobjArtifact);

								NextState = Globals.CreateInstance<IStartState>();
							}
						}
						else
						{
							NextCommand.PrintDontFollowYou();

							NextState = Globals.CreateInstance<IStartState>();
						}
					}
				}
				else
				{
					ObjData.RevealEmbeddedArtifactFunc(ActorRoom, ObjData.FilterArtifactList[0]);

					SetArtifact(ObjData.FilterArtifactList[0]);
				}
			};

			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingUseCommand()
		{
			PlayerResolveArtifact();

			if (DobjArtifact != null && NextCommand.IsIobjEnabled && CurrToken < Tokens.Length)
			{
				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}Use {1} on who or what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

				ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				ObjData.MonsterNotFoundFunc = NextCommand.PrintDontHaveItNotHere;

				PlayerResolveArtifact();
			}
		}

		public virtual void PlayerFinishParsingWearCommand()
		{
			ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively),
				a => a.IsWornByCharacter()
			};

			PlayerResolveArtifact();
		}

		public virtual void PlayerFinishParsingInventoryCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				if (ActorRoom.IsLit())
				{
					if (Globals.IsRulesetVersion(5))
					{
						PlayerResolveArtifact();
					}
					else
					{
						ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

						PlayerResolveMonster();
					}
				}
				else
				{
					NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Dobj = gCharMonster;
			}
		}

		public virtual void PlayerFinishParsingLookCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;

				FinishParsing();
			}
		}

		public virtual void PlayerFinishParsingQuitCommand()
		{
			if (CurrToken < Tokens.Length && string.Equals(Tokens[CurrToken], "hall", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IQuitCommand>().GoToMainHall = true;

				CurrToken++;
			}
		}

		public virtual void PlayerFinishParsingRestoreCommand()
		{
			RetCode rc;
			long i;

			var restoreCommand = NextCommand as IRestoreCommand;

			Debug.Assert(restoreCommand != null);

			if (CurrToken < Tokens.Length && long.TryParse(Tokens[CurrToken], out i) && i >= 1 && i <= gEngine.NumSaveSlots)
			{
				restoreCommand.SaveSlot = i;

				CurrToken++;
			}

			var filesets = Globals.Database.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (restoreCommand.SaveSlot < 1 || restoreCommand.SaveSlot > filesetsCount)
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
						restoreCommand.SaveSlot = i;

						break;
					}
					else if (i == gEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (restoreCommand.SaveSlot < 1 || restoreCommand.SaveSlot > filesetsCount)
			{
				if (gGameState.Die == 1)
				{
					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					NextState = Globals.CreateInstance<IStartState>();
				}
			}
		}

		public virtual void PlayerFinishParsingSaveCommand()
		{
			RetCode rc;
			long i;

			var saveCommand = NextCommand as ISaveCommand;

			Debug.Assert(saveCommand != null);

			if (CurrToken < Tokens.Length && long.TryParse(Tokens[CurrToken], out i) && i >= 1 && i <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveSlot = i;

				CurrToken++;
			}

			if (CurrToken < Tokens.Length && saveCommand.SaveSlot >= 1 && saveCommand.SaveSlot <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveName = string.Join(" ", Tokens.Skip((int)CurrToken));

				if (saveCommand.SaveName.Length > Constants.FsNameLen)
				{
					saveCommand.SaveName = saveCommand.SaveName.Substring(0, Constants.FsNameLen);
				}

				CurrToken += (Tokens.Length - CurrToken);
			}
			else
			{
				saveCommand.SaveName = "Quick Saved Game";
			}

			var filesets = Globals.Database.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (saveCommand.SaveSlot < 1 || saveCommand.SaveSlot > gEngine.NumSaveSlots)
			{
				saveCommand.SaveName = "";

				while (true)
				{
					gOut.Print("Saved games:");

					for (i = 0; i < gEngine.NumSaveSlots; i++)
					{
						gOut.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					gOut.Print("{0,3}. {1}", i + 1, "(Don't save, return to game)");

					gOut.Write("{0}Enter 1-{1} for saved position: ", Environment.NewLine, i + 1);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, 3, null, ' ', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					i = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (i >= 1 && i <= gEngine.NumSaveSlots)
					{
						saveCommand.SaveSlot = i;

						break;
					}
					else if (i == gEngine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (saveCommand.SaveSlot > filesetsCount + 1 && saveCommand.SaveSlot <= gEngine.NumSaveSlots)
			{
				saveCommand.SaveSlot = filesetsCount + 1;

				gOut.Print("[Using #{0} instead.]", saveCommand.SaveSlot);
			}

			if (saveCommand.SaveSlot < 1 || saveCommand.SaveSlot > filesetsCount + 1)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public virtual void PlayerFinishParsingSayCommand()
		{
			var sayCommand = NextCommand as ISayCommand;

			Debug.Assert(sayCommand != null);

			if (CurrToken < Tokens.Length)
			{
				sayCommand.OriginalPhrase = InputBuf.ToString().Replace(Tokens[0] + " ", "");

				CurrToken += (Tokens.Length - CurrToken);
			}

			while (true)
			{
				if (string.IsNullOrWhiteSpace(sayCommand.OriginalPhrase))
				{
					gOut.Write("{0}{1} who or what? ", Environment.NewLine, NextCommand.Verb.FirstCharToUpper());

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					sayCommand.OriginalPhrase = Globals.Buf.ToString();
				}
				else
				{
					break;
				}
			}
		}

		public virtual void PlayerFinishParsingSettingsCommand()
		{
			long longValue = 0;

			bool boolValue = false;

			var settingsCommand = NextCommand as ISettingsCommand;

			Debug.Assert(settingsCommand != null);

			if (CurrToken + 1 < Tokens.Length)
			{
				if (string.Equals(Tokens[CurrToken], "verboserooms", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseRooms = boolValue;

					CurrToken += 2;
				}
				else if (string.Equals(Tokens[CurrToken], "verbosemonsters", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseMonsters = boolValue;

					CurrToken += 2;
				}
				else if (string.Equals(Tokens[CurrToken], "verboseartifacts", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseArtifacts = boolValue;

					CurrToken += 2;
				}
				else if (string.Equals(Tokens[CurrToken], "maturecontent", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.MatureContent = boolValue;

					CurrToken += 2;
				}
				else if (string.Equals(Tokens[CurrToken], "pausecombatms", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 10000)
				{
					settingsCommand.PauseCombatMs = longValue;

					CurrToken += 2;
				}
				else
				{
					settingsCommand.PrintUsage();

					NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				settingsCommand.PrintUsage();

				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public virtual void PlayerFinishParsingFleeCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				NextCommand.Cast<IFleeCommand>().Direction = gEngine.GetDirection(Tokens[CurrToken]);

				if (NextCommand.Cast<IFleeCommand>().Direction != 0)
				{
					CurrToken++;
				}
				else if (ActorRoom.IsLit())
				{
					ParseName();

					ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
					{
						a => a.IsInRoom(ActorRoom),
						a => a.IsEmbeddedInRoom(ActorRoom),
						a => a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
					};

					ObjData.ArtifactNotFoundFunc = NextCommand.PrintNothingHereByThatName;

					PlayerResolveArtifact();
				}
				else
				{
					NextState = Globals.CreateInstance<IStartState>();
				}
			}
		}

		public virtual void PlayerFinishParsingGoCommand()
		{
			ParseName();

			ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			ObjData.ArtifactNotFoundFunc = NextCommand.PrintNothingHereByThatName;

			PlayerResolveArtifact();
		}

		public virtual void PlayerArtifactMatch()
		{
			if (ObjData.FilterArtifactList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterArtifactList.Count < 1)
			{
				ObjData.ArtifactNotFoundFunc();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				ObjData.RevealEmbeddedArtifactFunc(ActorRoom, ObjData.FilterArtifactList[0]);

				SetArtifact(ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerArtifactMatch01()
		{
			if (ObjData.FilterArtifactList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterArtifactList.Count < 1)
			{
				PlayerResolveMonster();
			}
			else
			{
				ObjData.RevealEmbeddedArtifactFunc(ActorRoom, ObjData.FilterArtifactList[0]);

				SetArtifact(ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerArtifactMatch02()
		{
			if (ObjData.FilterArtifactList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterArtifactList[0], ObjData.FilterArtifactList[1]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterArtifactList.Count < 1)
			{
				ObjData.ArtifactNotFoundFunc();

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				ObjData.RevealEmbeddedArtifactFunc(ActorRoom, ObjData.FilterArtifactList[0]);

				SetArtifact(ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerMonsterMatch()
		{
			if (ObjData.FilterMonsterList.Count < 1)
			{
				ObjData.MonsterNotFoundFunc();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				SetMonster(ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerMonsterMatch01()
		{
			if (ObjData.FilterMonsterList.Count < 1)
			{
				ObjData.MonsterNotFoundFunc();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				NextCommand.PrintCantVerbObj(ObjData.FilterMonsterList[0]);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void PlayerMonsterMatch02()
		{
			if (ObjData.FilterMonsterList.Count < 1)
			{
				ObjData.MonsterNotFoundFunc();

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				SetMonster(ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerMonsterMatch03()
		{
			if (ObjData.FilterMonsterList.Count < 1)
			{
				PlayerResolveArtifact();
			}
			else
			{
				SetMonster(ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerResolveArtifact()
		{
			if (GetArtifact() == null)
			{
				if (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					ParseName();
				}

				if (!string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (ObjData.ArtifactWhereClauseList == null)
					{
						ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
							a => a.IsEmbeddedInRoom(ActorRoom),
							a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
						};
					}

					if (ObjData.RevealEmbeddedArtifactFunc == null)
					{
						ObjData.RevealEmbeddedArtifactFunc = gEngine.RevealEmbeddedArtifact;
					}

					if (ObjData.GetArtifactListFunc == null)
					{
						ObjData.GetArtifactListFunc = gEngine.GetArtifactList;
					}

					if (ObjData.FilterArtifactListFunc == null)
					{
						ObjData.FilterArtifactListFunc = gEngine.FilterArtifactList;
					}

					if (ObjData.ArtifactMatchFunc == null)
					{
						ObjData.ArtifactMatchFunc = PlayerArtifactMatch;
					}

					if (ObjData.ArtifactNotFoundFunc == null)
					{
						ObjData.ArtifactNotFoundFunc = NextCommand.PrintDontHaveItNotHere;
					}

					PlayerResolveArtifactProcessWhereClauseList();

					ObjData.ArtifactMatchFunc();
				}
				else
				{
					NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace({1}.Name)", NextCommand.Name, GetActiveObjData());
					});
				}
			}
		}

		public virtual void PlayerResolveArtifactProcessWhereClauseList()
		{
			ObjData.GetArtifactList = new List<IArtifact>();

			foreach (var whereClauseFuncs in ObjData.ArtifactWhereClauseList)
			{
				ObjData.GetArtifactList = ObjData.GetArtifactListFunc(whereClauseFuncs);

				Debug.Assert(ObjData.GetArtifactList != null);

				var filterArtifactList = ObjData.FilterArtifactListFunc(ObjData.GetArtifactList, ObjData.Name);

				Debug.Assert(filterArtifactList != null);

				ObjData.FilterArtifactList = filterArtifactList.GroupBy(a => a.Name.ToLower()).Select(a => a.First()).ToList();

				Debug.Assert(ObjData.FilterArtifactList != null);

				if (ObjData.FilterArtifactList.Count > 0)
				{
					break;
				}
			}
		}

		public virtual void PlayerResolveMonster()
		{
			if (GetMonster() == null)
			{
				if (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					ParseName();
				}

				if (!string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (ObjData.MonsterWhereClauseList == null)
					{
						ObjData.MonsterWhereClauseList = new List<Func<IMonster, bool>>()
						{
							m => m.IsInRoom(ActorRoom) && m != ActorMonster
						};
					}

					if (ObjData.GetMonsterListFunc == null)
					{
						ObjData.GetMonsterListFunc = gEngine.GetMonsterList;
					}

					if (ObjData.FilterMonsterListFunc == null)
					{
						ObjData.FilterMonsterListFunc = gEngine.FilterMonsterList;
					}

					if (ObjData.MonsterMatchFunc == null)
					{
						ObjData.MonsterMatchFunc = PlayerMonsterMatch;
					}

					if (ObjData.MonsterNotFoundFunc == null)
					{
						ObjData.MonsterNotFoundFunc = NextCommand.PrintNobodyHereByThatName;
					}

					PlayerResolveMonsterProcessWhereClauseList();

					ObjData.MonsterMatchFunc();
				}
				else
				{
					NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace({1}.Name)", NextCommand.Name, GetActiveObjData());
					});
				}
			}
		}

		public virtual void PlayerResolveMonsterProcessWhereClauseList()
		{
			ObjData.GetMonsterList = new List<IMonster>();

			foreach (var whereClauseFuncs in ObjData.MonsterWhereClauseList)
			{
				ObjData.GetMonsterList = ObjData.GetMonsterListFunc(whereClauseFuncs);

				Debug.Assert(ObjData.GetMonsterList != null);

				ObjData.FilterMonsterList = ObjData.FilterMonsterListFunc(ObjData.GetMonsterList, ObjData.Name);

				Debug.Assert(ObjData.FilterMonsterList != null);

				if (ObjData.FilterMonsterList.Count > 0)
				{
					break;
				}
			}
		}

		public virtual void PlayerFinishParsing()
		{
			var methodName = string.Format("PlayerFinishParsing{0}", NextCommand.GetType().Name);

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			// Note: verb-only Commands can omit the handler; those that accept Dobj/Iobj require it

			if (methodInfo != null)
			{
				methodInfo.Invoke(this, null);
			}
		}

		public virtual void MonsterFinishParsing()
		{
			// do nothing
		}

		public virtual bool ShouldStripTrailingPunctuation()
		{
			return NextCommand is ISaveCommand || NextCommand is ISayCommand ? false : true;
		}

		public virtual void FinishParsing()
		{
			Debug.Assert(NextCommand != null);

			Debug.Assert(ActorMonster != null);

			Debug.Assert(ActorRoom != null);

			if (ActorMonster.IsCharacterMonster())
			{
				PlayerFinishParsing();
			}
			else
			{
				MonsterFinishParsing();
			}
		}

		public virtual string GetActiveObjData()
		{
			return ObjData == DobjData ? "DobjData" : "IobjData";
		}

		public virtual void SetArtifact(IArtifact artifact)
		{
			Debug.Assert(NextCommand != null);

			if (ObjData == DobjData)
			{
				Dobj = artifact;
			}
			else
			{
				Iobj = artifact;
			}

			ObjData.Obj = artifact;
		}

		public virtual void SetMonster(IMonster monster)
		{
			Debug.Assert(NextCommand != null);

			if (ObjData == DobjData)
			{
				Dobj = monster;
			}
			else
			{
				Iobj = monster;
			}

			ObjData.Obj = monster;
		}

		public virtual IArtifact GetArtifact()
		{
			Debug.Assert(NextCommand != null);

			return ObjData == DobjData ? DobjArtifact : IobjArtifact;
		}

		public virtual IMonster GetMonster()
		{
			Debug.Assert(NextCommand != null);

			return ObjData == DobjData ? DobjMonster : IobjMonster;
		}

		public virtual StringBuilder ReplacePrepositions(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetFormat(" {0} ", Regex.Replace(buf.ToString(), @"\s+", " ").ToLower().Trim());

			buf = buf.Replace(" in to ", " into ");

			buf = buf.Replace(" inside ", " in ");

			buf = buf.Replace(" from in ", " fromin ");

			buf = buf.Replace(" on to ", " onto ");

			buf = buf.Replace(" on top of ", " on ");

			buf = buf.Replace(" from on ", " fromon ");

			buf = buf.Replace(" below ", " under ").Replace(" beneath ", " under ").Replace(" underneath ", " under ");

			buf = buf.Replace(" from under ", " fromunder ");

			buf = buf.Replace(" in back of ", " behind ");

			buf = buf.Replace(" from behind ", " frombehind ");

			return buf.Trim();
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			Tokens = null;

			CurrToken = 0;

			PrepTokenIndex = -1;

			Prep = null;

			_actorMonster = null;

			_actorRoom = null;

			DobjData = Globals.CreateInstance<IParserData>();

			IobjData = Globals.CreateInstance<IParserData>();

			ObjData = DobjData;

			NextState = null;
		}

		public virtual void ParseName()
		{
			RetCode rc;

			Debug.Assert(NextCommand != null);

			if (ObjData.Name == null)
			{
				ObjData.Name = "";

				while (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (CurrToken < Tokens.Length && PrepTokenIndex < 0)
					{
						PrepTokenIndex = NextCommand.IsDobjPrepEnabled || NextCommand.IsIobjEnabled ? Array.FindLastIndex(Tokens, token => gEngine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, token, StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) != null) : -1;

						Prep = PrepTokenIndex >= 0 ? gEngine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, Tokens[PrepTokenIndex], StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) : null;

						if (((ObjData == DobjData && NextCommand.IsDobjPrepEnabled) || (ObjData == IobjData && NextCommand.IsIobjEnabled)) && PrepTokenIndex == CurrToken)
						{
							NextCommand.Prep = Globals.CloneInstance(Prep);

							NextCommand.ContainerType = Prep.ContainerType;

							var numTokens = Tokens.Length - CurrToken;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)(CurrToken + 1)).Take((int)(numTokens - 1)));

							CurrToken += numTokens;
						}
						else if ((ObjData == DobjData && NextCommand.IsIobjEnabled) && PrepTokenIndex > CurrToken)
						{
							NextCommand.Prep = Globals.CloneInstance(Prep);

							NextCommand.ContainerType = Prep.ContainerType;

							var numTokens = PrepTokenIndex - CurrToken;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)numTokens));

							CurrToken += numTokens;
						}
						else
						{
							PrepTokenIndex = -1;

							Prep = null;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

							CurrToken = Tokens.Length;
						}
					}

					if (ObjData == IobjData && CurrToken < Tokens.Length && PrepTokenIndex >= 0)
					{
						CurrToken = PrepTokenIndex + 1;
					}

					if (CurrToken < Tokens.Length && string.IsNullOrWhiteSpace(ObjData.Name))
					{
						ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

						CurrToken = Tokens.Length;
					}

					if (string.IsNullOrWhiteSpace(ObjData.Name))
					{
						Debug.Assert(ActorMonster.IsCharacterMonster());

						if (ObjData == DobjData)
						{
							if (ObjData.QueryDescFunc == null)
							{
								ObjData.QueryDescFunc = () => string.Format("{0}{1} {2}who or what? ", Environment.NewLine, NextCommand.Verb.FirstCharToUpper(), NextCommand.IsDobjPrepEnabled && NextCommand.Prep != null && Enum.IsDefined(typeof(ContainerType), NextCommand.Prep.ContainerType) ? gEngine.EvalContainerType(NextCommand.Prep.ContainerType, "inside ", "on ", "under ", "behind ") : "");
							}
						}
						else
						{
							Debug.Assert(ObjData.QueryDescFunc != null);
						}

						gOut.Write(ObjData.QueryDescFunc());

						Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

						Globals.Buf = ReplacePrepositions(Globals.Buf);

						if (ShouldStripTrailingPunctuation())
						{
							Globals.Buf.SetFormat("{0}", Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim());
						}

						Tokens = Tokens.Concat(Globals.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
					}
					else
					{
						var mySeen = false;

						Globals.Buf.SetFormat("{0}", ObjData.Name);

						rc = gEngine.StripPrepsAndArticles(Globals.Buf, ref mySeen);

						Debug.Assert(gEngine.IsSuccess(rc));

						ObjData.Name = Globals.Buf.ToString().Trim();
					}
				}
			}
		}

		public virtual void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			// do nothing
		}

		public virtual void Execute()
		{
			Debug.Assert(ActorMonster != null);

			ActorRoom = ActorMonster.GetInRoom();

			Debug.Assert(ActorRoom != null);

			if (InputBuf.Length == 0)
			{
				InputBuf.SetFormat("{0}", LastInputStr);

				if (InputBuf.Length > 0 && ActorMonster.IsCharacterMonster())
				{
					if (Environment.NewLine.Length == 1 && Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
					{
						Globals.CursorPosition.Y--;
					}

					gOut.SetCursorPosition(Globals.CursorPosition);

					if (Globals.LineWrapUserInput)
					{
						gEngine.LineWrap(InputBuf.ToString(), Globals.Buf, Globals.CommandPrompt.Length);
					}
					else
					{
						Globals.Buf.SetFormat("{0}", InputBuf.ToString());
					}

					gOut.WordWrap = false;

					gOut.WriteLine(Globals.Buf);

					gOut.WordWrap = true;
				}
			}

			LastInputStr = InputBuf.ToString();

			InputBuf = ReplacePrepositions(InputBuf);

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (CurrToken < Tokens.Length)
			{
				if (Tokens.Length == 1)
				{
					Globals.Buf.SetFormat("{0}", Tokens[CurrToken]);

					Tokens[CurrToken] = Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim();
				}

				if (Tokens[CurrToken].Length == 0)
				{
					Tokens[CurrToken] = "???";
				}
				else if (string.Equals(Tokens[CurrToken], "at", StringComparison.OrdinalIgnoreCase))
				{
					Tokens[CurrToken] = "a";
				}

				var command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && string.Equals(x.Verb, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) && x.IsEnabled(ActorMonster));

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => string.Equals(s, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && (x.Verb.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || x.Verb.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || s.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command != null)
				{
					CurrToken++;

					NextState = Activator.CreateInstance(command.GetType()) as IState;

					Debug.Assert(NextCommand != null);

					NextCommand.CommandParser = this;

					NextCommand.ActorMonster = _actorMonster;

					NextCommand.ActorRoom = _actorRoom;

					NextCommand.Dobj = DobjData.Obj;

					NextCommand.Iobj = IobjData.Obj;

					if (ShouldStripTrailingPunctuation() && Tokens.Length > 1)
					{
						Globals.Buf.SetFormat("{0}", Tokens[Tokens.Length - 1]);

						Tokens[Tokens.Length - 1] = Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim();
					}

					if (ActorMonster.IsCharacterMonster())
					{
						CheckPlayerCommand(false);

						if (NextCommand != null)
						{
							if (ActorRoom.IsLit() || NextCommand.IsDarkEnabled)
							{
								FinishParsing();

								if (NextCommand != null)
								{
									CheckPlayerCommand(true);
								}
							}
							else
							{
								NextState = null;
							}
						}
					}
					else
					{
						FinishParsing();
					}

					if (NextState == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}
				}
			}
		}

		public CommandParser()
		{
			InputBuf = new StringBuilder(Constants.BufSize);

			LastInputStr = "";
		}
	}
}
