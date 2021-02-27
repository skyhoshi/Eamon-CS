﻿
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
using EamonRT.Game.Exceptions;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : ICommandParser
	{
		public string _lastHimNameStr;

		public string _lastHerNameStr;

		public string _lastItNameStr;

		public string _lastThemNameStr;

		public IMonster _actorMonster;

		public IRoom _actorRoom;

		public virtual StringBuilder InputBuf { get; set; }

		public virtual string LastInputStr { get; set; }

		public virtual string LastHimNameStr 
		{ 
			get
			{
				return _lastHimNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastHimNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{{Changing him:  \"{0}\"}}", value);
				}

				_lastHimNameStr = value;
			}
		}

		public virtual string LastHerNameStr
		{
			get
			{
				return _lastHerNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastHerNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{{Changing her:  \"{0}\"}}", value);
				}

				_lastHerNameStr = value;
			}
		}

		public virtual string LastItNameStr
		{
			get
			{
				return _lastItNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastItNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{{Changing it:  \"{0}\"}}", value);
				}

				_lastItNameStr = value;
			}
		}

		public virtual string LastThemNameStr
		{
			get
			{
				return _lastThemNameStr;
			}

			set
			{
				if (gGameState != null && gGameState.ShowPronounChanges && !_lastThemNameStr.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{{Changing them:  \"{0}\"}}", value);
				}

				_lastThemNameStr = value;
			}
		}

		public virtual string CurrInputStr { get; set; }

		public virtual string OrigInputStr { get; set; }

		public virtual string CommandFormatStr { get; set; }

		public virtual string NewCommandStr { get; set; }

		public virtual string[] Tokens { get; set; }

		public virtual long CurrToken { get; set; }

		public virtual long NameIndex { get; set; }

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
			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster,
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			PlayerResolveRecord();
		}

		public virtual void PlayerFinishParsingBlastCommand()
		{
			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster,
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			PlayerResolveRecord();
		}

		public virtual void PlayerFinishParsingFreeCommand()
		{
			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster
			};

			PlayerResolveRecord();
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
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
					r => r is IArtifact a && a.IsWornByCharacter()
				};

				PlayerResolveRecord(false);
			}

			if (NextCommand != null && (giveCommand.GoldAmount > 0 || DobjArtifact != null))
			{
				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}To whom? ", Environment.NewLine);

				PlayerResolveRecord(true, false);
			}
		}

		public virtual void PlayerFinishParsingHealCommand()
		{
			if (!Globals.IsRulesetVersion(5) && CurrToken < Tokens.Length)
			{
				PlayerResolveRecord(true, false);
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

			PlayerResolveRecord(true, false);

			if (IobjMonster != null)
			{
				ObjData = DobjData;

				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && a.IsCarriedByMonster(IobjMonster),
					r => r is IArtifact a && a.IsWornByMonster(IobjMonster)
				};

				ObjData.RecordMatchFunc = PlayerRecordMatch01;

				ObjData.RecordNotFoundFunc = () =>
				{
					gOut.Print("{0}{1} have it.", IobjMonster.GetTheName(true), IobjMonster.EvalPlural(" doesn't", " don't"));
				};

				PlayerResolveRecord(false);
			}
		}

		public virtual void PlayerFinishParsingCloseCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingDrinkCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingDropCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IDropCommand>().DropAll = true;
			}
			else
			{
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && a.IsCarriedByCharacter(),
					r => r is IArtifact a && a.IsWornByCharacter(),
					r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively)
				};

				ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveIt;

				PlayerResolveRecord(false);
			}
		}

		public virtual void PlayerFinishParsingEatCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingExamineCommand()
		{
			ParseName();

			NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

			if (ObjData.Name.Equals("room", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("area", StringComparison.OrdinalIgnoreCase))
			{
				var command = Globals.CreateInstance<ILookCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;
			}
			else
			{
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => (r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
					r => r is IArtifact a && a.IsWornByCharacter()
				};

				if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
				{
					ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				ObjData.RecordMatchFunc = PlayerRecordMatch01;

				ObjData.RecordNotFoundFunc = NextCommand.PrintYouSeeNothingSpecial;

				PlayerResolveRecord();
			}
		}

		public virtual void PlayerFinishParsingGetCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IGetCommand>().GetAll = true;
			}
			else
			{
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && a.IsInRoom(ActorRoom),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
				};

				ObjData.RecordNotFoundFunc = NextCommand.PrintCantVerbThat;

				PlayerResolveRecord(false);
			}
		}

		public virtual void PlayerFinishParsingLightCommand()
		{
			if (!ActorMonster.IsInRoomLit())
			{
				ObjData.RecordNotFoundFunc = () => { };
			}

			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingOpenCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingPutCommand()
		{
			PlayerResolveRecord(false);

			if (DobjArtifact != null)
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}Put {1} {2} what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType) ? gEngine.EvalContainerType(NextCommand.ContainerType, "inside", "on", "under", "behind") : "in");

				PlayerResolveRecord(false);

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
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingReadyCommand()
		{
			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingRemoveCommand()
		{
			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsWornByCharacter()
			};

			ObjData.RecordMatchFunc = () =>
			{
				NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

				if (ObjData.FilterRecordList.Count > 1)
				{
					NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

					NextState = Globals.CreateInstance<IStartState>();
				}
				else if (ObjData.FilterRecordList.Count == 1)
				{
					if (ObjData.FilterRecordList[0] is IArtifact artifact)
					{
						ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
					}

					SetRecord(ObjData.FilterRecordList[0]);
				}
			};

			PlayerResolveRecord(false);

			if (NextCommand != null && DobjArtifact == null)
			{
				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}From {1}what? ", Environment.NewLine, Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType) ? gEngine.EvalContainerType(NextCommand.ContainerType, "inside ", "on ", "under ", "behind ") : "");

				PlayerResolveRecord(false);

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
						if (ac != IobjArtifact.InContainer || ac.IsOpen() || IobjArtifact.ShouldExposeInContentsWhenClosed())
						{
							ObjData = DobjData;

							ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
							{
								r => r is IArtifact a && a.IsCarriedByContainer(IobjArtifact) && a.GetCarriedByContainerContainerType() == NextCommand.ContainerType
							};

							ObjData.RecordMatchFunc = PlayerRecordMatch;

							ObjData.RecordNotFoundFunc = NextCommand.PrintDontFollowYou;

							PlayerResolveRecord(false);
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
		}

		public virtual void PlayerFinishParsingUseCommand()
		{
			PlayerResolveRecord(false);

			if (DobjArtifact != null && NextCommand.IsIobjEnabled && CurrToken < Tokens.Length)
			{
				ObjData = IobjData;

				ObjData.QueryDescFunc = () => string.Format("{0}Use {1} on who or what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => (r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
				};

				ObjData.RecordMatchFunc = PlayerRecordMatch01;

				ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;

				PlayerResolveRecord();
			}
		}

		public virtual void PlayerFinishParsingWearCommand()
		{
			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)),
				r => r is IArtifact a && a.IsWornByCharacter()
			};

			PlayerResolveRecord(false);
		}

		public virtual void PlayerFinishParsingInventoryCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				if (ActorRoom.IsLit())
				{
					if (Globals.IsRulesetVersion(5))
					{
						PlayerResolveRecord(false);
					}
					else
					{
						ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
						{
							r => (r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom))) || (r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster),
							r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
							r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
						};

						ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;

						PlayerResolveRecord();
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
			if (CurrToken < Tokens.Length && Tokens[CurrToken].Equals("hall", StringComparison.OrdinalIgnoreCase))
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
				sayCommand.OriginalPhrase = InputBuf.ToString().Substring((Tokens[0] + " ").Length);

				CurrToken += (Tokens.Length - CurrToken);
			}

			while (true)
			{
				if (string.IsNullOrWhiteSpace(sayCommand.OriginalPhrase))
				{
					gOut.Write("{0}{1} who or what? ", Environment.NewLine, NextCommand.Verb.FirstCharToUpper());

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					sayCommand.OriginalPhrase = Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").Trim();
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
				if (Tokens[CurrToken].Equals("verboserooms", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseRooms = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("verbosemonsters", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseMonsters = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("verboseartifacts", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.VerboseArtifacts = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("maturecontent", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.MatureContent = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("enhancedparser", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.EnhancedParser = boolValue;

					CurrToken += 2;
				}
				else if (gGameState.EnhancedParser && Tokens[CurrToken].Equals("showpronounchanges", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ShowPronounChanges = boolValue;

					CurrToken += 2;
				}
				else if (gGameState.EnhancedParser && Tokens[CurrToken].Equals("showfulfillmessages", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ShowFulfillMessages = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("pausecombatms", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 10000)
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

					ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
					{
						r => r is IArtifact a && a.IsInRoom(ActorRoom),
						r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
						r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
					};

					ObjData.RecordNotFoundFunc = NextCommand.PrintNothingHereByThatName;

					PlayerResolveRecord(false);
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

			ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
			{
				r => r is IArtifact a && a.IsInRoom(ActorRoom),
				r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
				r => r is IArtifact a && a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively)
			};

			ObjData.RecordNotFoundFunc = NextCommand.PrintNothingHereByThatName;

			PlayerResolveRecord(false);
		}

		public virtual void PlayerRecordMatch()
		{
			if (ObjData.FilterRecordList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterRecordList.Count < 1)
			{
				ObjData.RecordNotFoundFunc();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else 
			{
				if (ObjData.FilterRecordList[0] is IArtifact artifact)
				{
					ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
				}

				SetRecord(ObjData.FilterRecordList[0]);
			}
		}

		public virtual void PlayerRecordMatch01()
		{
			if (ObjData.FilterRecordList.Count > 1)
			{
				NextCommand.PrintDoYouMeanObj1OrObj2(ObjData.FilterRecordList[0], ObjData.FilterRecordList[1]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else if (ObjData.FilterRecordList.Count < 1)
			{
				ObjData.RecordNotFoundFunc();

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				if (ObjData.FilterRecordList[0] is IArtifact artifact)
				{
					ObjData.RevealEmbeddedArtifactFunc(ActorRoom, artifact);
				}

				SetRecord(ObjData.FilterRecordList[0]);
			}
		}

		public virtual void PlayerResolveRecord(bool includeMonsters = true, bool includeArtifacts = true)
		{
			if (GetRecord() == null)
			{
				if (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					ParseName();
				}

				if (!string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (ObjData.RecordWhereClauseList == null)
					{
						ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>();

						if (includeMonsters)
						{
							ObjData.RecordWhereClauseList.AddRange(
								new Func<IGameBase, bool>[] {
									r => r is IMonster m && m.IsInRoom(ActorRoom) && m != ActorMonster
								}
							);
						}

						if (includeArtifacts)
						{
							ObjData.RecordWhereClauseList.AddRange(
								new Func<IGameBase, bool>[] {
									r => r is IArtifact a && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)),
									r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
									r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
								}
							);
						}
					}

					if (ObjData.RevealEmbeddedArtifactFunc == null)
					{
						ObjData.RevealEmbeddedArtifactFunc = gEngine.RevealEmbeddedArtifact;
					}

					if (ObjData.GetRecordListFunc == null)
					{
						ObjData.GetRecordListFunc = gEngine.GetRecordList;
					}

					if (ObjData.FilterRecordListFunc == null)
					{
						ObjData.FilterRecordListFunc = gEngine.FilterRecordList;
					}

					if (ObjData.RecordMatchFunc == null)
					{
						ObjData.RecordMatchFunc = PlayerRecordMatch;
					}

					if (ObjData.RecordNotFoundFunc == null)
					{
						if (includeMonsters)
						{
							ObjData.RecordNotFoundFunc = NextCommand.PrintNobodyHereByThatName;
						}
						else
						{
							ObjData.RecordNotFoundFunc = NextCommand.PrintDontHaveItNotHere;
						}
					}

					PlayerResolveRecordProcessWhereClauseList();

					ObjData.RecordMatchFunc();
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

		public virtual void PlayerResolveRecordProcessWhereClauseList()
		{
			ObjData.GetRecordList = new List<IGameBase>();

			foreach (var whereClauseFunc in ObjData.RecordWhereClauseList)
			{
				ObjData.GetRecordList = ObjData.GetRecordListFunc(whereClauseFunc);

				Debug.Assert(ObjData.GetRecordList != null);

				ObjData.FilterRecordList = ObjData.FilterRecordListFunc(ObjData.GetRecordList, ObjData.Name);

				Debug.Assert(ObjData.FilterRecordList != null);

				if (ObjData.FilterRecordList.Count > 0)
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
				try
				{
					methodInfo.Invoke(this, null);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
					{
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
					}
					else
					{
						throw;
					}
				}
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

		public virtual void SetRecord(IGameBase record)
		{
			Debug.Assert(NextCommand != null);

			if (ObjData == DobjData)
			{
				Dobj = record;
			}
			else
			{
				Iobj = record;
			}

			ObjData.Obj = record;
		}

		public virtual IGameBase GetRecord()
		{
			Debug.Assert(NextCommand != null);

			return ObjData == DobjData ? Dobj : Iobj;
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			CurrInputStr = "";

			OrigInputStr = "";

			CommandFormatStr = "";

			NewCommandStr = "";

			Tokens = null;

			CurrToken = 0;

			NameIndex = -1;

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
					if (NextCommand.IsDobjPrepEnabled || NextCommand.IsIobjEnabled)
					{
						Predicate<string> prepFindFunc = token => gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(token, StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) != null;

						PrepTokenIndex = NextCommand.IsDobjPrepEnabled ? Array.FindIndex(Tokens, prepFindFunc) : NextCommand.IsIobjEnabled ? Array.FindLastIndex(Tokens, prepFindFunc) : -1;

						Prep = PrepTokenIndex >= 0 ? gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(Tokens[PrepTokenIndex], StringComparison.OrdinalIgnoreCase) && NextCommand.IsPrepEnabled(prep)) : null;

						if (Prep != null)
						{
							NextCommand.Prep = Globals.CloneInstance(Prep);

							NextCommand.ContainerType = Prep.ContainerType;
						}
					}

					var numTokens = Tokens.Length - CurrToken;

					if (((ObjData == DobjData && NextCommand.IsDobjPrepEnabled) || (ObjData == IobjData && NextCommand.IsIobjEnabled)) && PrepTokenIndex == CurrToken)
					{
						CurrToken++;

						numTokens--;
					}
					else if (ObjData == DobjData && NextCommand.IsIobjEnabled && PrepTokenIndex >= CurrToken)
					{
						numTokens = PrepTokenIndex - CurrToken;
					}

					ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)numTokens));

					CurrToken += numTokens;

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

						Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").Trim());

						var newTokenList = Globals.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();

						var origTokenList = OrigInputStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();

						if (CurrToken == 1 && CurrToken < origTokenList.Count)
						{
							origTokenList.InsertRange((int)CurrToken, newTokenList);
						}
						else
						{
							origTokenList.AddRange(newTokenList);
						}

						Globals.Buf.SetFormat("{0}", string.Join(" ", origTokenList.ToArray()));

						OrigInputStr = Globals.Buf.ToString();

						Globals.Buf.SetFormat("{0}", gEngine.NormalizePlayerInput(Globals.Buf).ToString());

						CurrInputStr = string.Format(" {0} ", Globals.Buf.ToString());

						Globals.Buf.SetFormat("{0}", gEngine.ReplacePrepositions(Globals.Buf).ToString());

						if (ShouldStripTrailingPunctuation())
						{
							Globals.Buf.SetFormat("{0}", Globals.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim());
						}

						Tokens = Globals.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					}
					else
					{
						ObjData.Name = string.Format(" {0} ", ObjData.Name);

						if (gGameState.EnhancedParser && ObjData == DobjData)
						{
							NameIndex = CurrInputStr.IndexOf(ObjData.Name);

							if (NameIndex < 0)
							{
								throw new GeneralParsingErrorException();
							}

							CommandFormatStr = CurrInputStr.Substring(0, (int)NameIndex) + "{0}" + CurrInputStr.Substring((int)NameIndex + ObjData.Name.Length);
						}

						var objNameTokens = ObjData.Name.IndexOf(" , ") >= 0 ? ObjData.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { ObjData.Name };

						objNameTokens = objNameTokens.Where(objNameToken => !string.IsNullOrWhiteSpace(objNameToken) && Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? objNameToken.IndexOf(" " + token + " ") >= 0 : token[0] != ',' && objNameToken.IndexOf(token) >= 0) < 0).ToArray();

						for (var i = 0; i < objNameTokens.Length; i++)
						{
							var mySeen = false;

							Globals.Buf.SetFormat("{0}", objNameTokens[i].Trim());

							rc = gEngine.StripPrepsAndArticles(Globals.Buf, ref mySeen);

							Debug.Assert(gEngine.IsSuccess(rc));

							objNameTokens[i] = string.Format(" {0} ", Globals.Buf.ToString().Trim());
						}

						ObjData.Name = string.Join(",", objNameTokens).Trim();
					}
				}

				if (gGameState.EnhancedParser)
				{
					if (ObjData.Name.Equals("him", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(LastHimNameStr))
					{
						ObjData.Name = LastHimNameStr;
					}
					else if (ObjData.Name.Equals("her", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(LastHerNameStr))
					{
						ObjData.Name = LastHerNameStr;
					}
					else if ((ObjData.Name.Equals("it", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("that", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrWhiteSpace(LastItNameStr))
					{
						ObjData.Name = LastItNameStr;
					}
					else if ((ObjData.Name.Equals("them", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("those", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrWhiteSpace(LastThemNameStr) && (ObjData == DobjData || LastThemNameStr.IndexOf(" , ") < 0))
					{
						ObjData.Name = LastThemNameStr;
					}

					if (ObjData == DobjData && ObjData.Name.IndexOf(" , ") >= 0)
					{
						throw new InvalidDobjNameListException(string.Format(" {0} ", ObjData.Name));
					}
				}
			}
		}

		public virtual void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster)
		{
			if (gGameState.EnhancedParser && obj != null && !string.IsNullOrWhiteSpace(objDataName))
			{
				var objDataName01 = string.Format(" {0} ", objDataName);

				if (Array.FindIndex(Constants.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? objDataName01.IndexOf(" " + token + " ") >= 0 : objDataName01.IndexOf(token) >= 0) < 0 && Array.FindIndex(Constants.PronounTokens, token => objDataName01.IndexOf(" " + token + " ") >= 0) < 0)
				{
					if (artifact != null)
					{
						if (artifact.IsPlural)
						{
							LastThemNameStr = Globals.CloneInstance(objDataName);
						}
						else
						{
							LastItNameStr = Globals.CloneInstance(objDataName);
						}
					}
					else
					{
						Debug.Assert(monster != null);

						if (monster.OrigGroupCount > 1)
						{
							LastThemNameStr = Globals.CloneInstance(objDataName);
						}
						else
						{
							if (monster.Gender == Gender.Male)
							{
								LastHimNameStr = Globals.CloneInstance(objDataName);
							}
							else if (monster.Gender == Gender.Female)
							{
								LastHerNameStr = Globals.CloneInstance(objDataName);
							}
							else
							{
								LastItNameStr = Globals.CloneInstance(objDataName);
							}
						}
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

			InputBuf.SetFormat("{0}", Regex.Replace(InputBuf.ToString(), @"\s+", " ").Trim());

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

			OrigInputStr = InputBuf.ToString();

			LastInputStr = InputBuf.ToString();

			InputBuf = gEngine.NormalizePlayerInput(InputBuf);

			CurrInputStr = string.Format(" {0} ", InputBuf.ToString());

			InputBuf = gEngine.ReplacePrepositions(InputBuf);

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			try
			{
				if (CurrToken < Tokens.Length)
				{
					if (Tokens.Length == 1)
					{
						Globals.Buf.SetFormat("{0}", Tokens[CurrToken]);

						Tokens[CurrToken] = Globals.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim();
					}

					if (Tokens[CurrToken].Length == 0)
					{
						Tokens[CurrToken] = "???";
					}
					else if (Tokens[CurrToken].Equals("at", StringComparison.OrdinalIgnoreCase))
					{
						Tokens[CurrToken] = "a";
					}

					var command = gEngine.GetCommandUsingToken(ActorMonster, Tokens[CurrToken]);

					if (command != null)
					{
						CurrToken++;

						if (gEngine.IsQuotedStringCommand(command))
						{
							InputBuf.SetFormat("{0}", OrigInputStr);

							Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
						}

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

							Tokens[Tokens.Length - 1] = Globals.Buf.TrimEndPunctuationMinusUniqueChars().ToString().Trim();
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

				if (Dobj != null)
				{
					SetLastNameStrings(Dobj, DobjData.Name, DobjArtifact, DobjMonster);

					SetLastNameStrings(Iobj, IobjData.Name, IobjArtifact, IobjMonster);
				}
			}
			catch (InvalidDobjNameListException ex)
			{
				Debug.Assert(!string.IsNullOrWhiteSpace(ex.DobjNameStr));

				Debug.Assert(!string.IsNullOrWhiteSpace(CommandFormatStr));

				NewCommandStr = string.Format(CommandFormatStr, ex.DobjNameStr).Trim();

				if (gSentenceParser.ParserInputStrList.Count > 0)
				{
					gSentenceParser.ParserInputStrList.Insert(0, NewCommandStr);
				}
				else
				{
					gSentenceParser.ParserInputStrList.Add(NewCommandStr);
				}

				NextState = Globals.CreateInstance<IGetPlayerInputState>(x =>
				{
					x.RestartCommand = true;
				});
			}
			catch (GeneralParsingErrorException)
			{
				Debug.Assert(NextState != null);

				NextState.PrintDontFollowYou02();

				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public CommandParser()
		{
			InputBuf = new StringBuilder(Constants.BufSize);

			LastInputStr = "";

			LastHimNameStr = "";

			LastHerNameStr = "";

			LastItNameStr = "";

			LastThemNameStr = "";

			Clear();
		}
	}
}
