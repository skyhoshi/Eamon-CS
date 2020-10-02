
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GetCommand : Command, IGetCommand
	{
		public long _playerDobjArtifactCount;

		public long _playerDobjArtifactWeight;

		public long _playerActorMonsterInventoryWeight;

		public long _monsterDobjArtifactCount;

		public long _monsterDobjArtifactWeight;

		public long _monsterActorMonsterInventoryWeight;

		public bool _newlineFlag;

		public virtual bool GetAll { get; set; }

		/// <summary></summary>
		public virtual ArtifactType[] PlayerArtTypes { get; set; }

		/// <summary></summary>
		public virtual ArtifactType[] MonsterArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> PlayerFumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> MonsterFumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> TakenArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory PlayerDobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory MonsterDobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory WeaponArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IMonster WeaponAffinityMonster { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual string MonsterName { get; set; }

		/// <summary></summary>
		public virtual long PlayerDobjArtifactCount
		{
			get
			{
				return _playerDobjArtifactCount;
			}

			set
			{
				_playerDobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long PlayerDobjArtifactWeight
		{
			get
			{
				return _playerDobjArtifactWeight;
			}

			set
			{
				_playerDobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long PlayerActorMonsterInventoryWeight
		{
			get
			{
				return _playerActorMonsterInventoryWeight;
			}

			set
			{
				_playerActorMonsterInventoryWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long MonsterDobjArtifactCount
		{
			get
			{
				return _monsterDobjArtifactCount;
			}

			set
			{
				_monsterDobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long MonsterDobjArtifactWeight
		{
			get
			{
				return _monsterDobjArtifactWeight;
			}

			set
			{
				_monsterDobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long MonsterActorMonsterInventoryWeight
		{
			get
			{
				return _monsterActorMonsterInventoryWeight;
			}

			set
			{
				_monsterActorMonsterInventoryWeight = value;
			}
		}

		/// <summary></summary>
		public virtual bool OmitWeightCheck { get; set; }

		/// <summary></summary>
		public virtual bool IsCarriedByContainer { get; set; }

		/// <summary></summary>
		public virtual bool NewlineFlag
		{
			get
			{
				return _newlineFlag;
			}

			set
			{
				_newlineFlag = value;
			}
		}

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(GetAll || DobjArtifact != null);

			if (GetAll)
			{
				// screen out all weapons in the room which have monsters present with affinities to those weapons

				TakenArtifactList = ActorRoom.GetTakeableList().Where(a => gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -a.Uid - 1 && m != ActorMonster).FirstOrDefault() == null).ToList();
			}
			else
			{
				TakenArtifactList = new List<IArtifact>() { DobjArtifact };
			}

			if (TakenArtifactList.Count > 0)
			{
				PlayerArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

				NewlineFlag = false;

				foreach (var artifact in TakenArtifactList)
				{
					PlayerDobjArtAc = artifact.GetArtifactCategory(PlayerArtTypes, false);

					if (PlayerDobjArtAc == null)
					{
						PlayerDobjArtAc = artifact.GetCategories(0);
					}

					Debug.Assert(PlayerDobjArtAc != null);

					OmitWeightCheck = artifact.IsCarriedByCharacter(true);

					ProcessArtifact(artifact, PlayerDobjArtAc, ref _newlineFlag);

					if (artifact.IsCarriedByCharacter())
					{
						// when a weapon is picked up all monster affinities to that weapon are broken

						PlayerFumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -artifact.Uid - 1 && m != ActorMonster);

						foreach (var monster in PlayerFumbleMonsterList)
						{
							monster.Weapon = -1;
						}

						WeaponArtifactAc = artifact.GeneralWeapon;

						if (artifact.IsReadyableByCharacter() && (WeaponArtifact == null || gEngine.WeaponPowerCompare(artifact, WeaponArtifact) > 0) && (!GetAll || TakenArtifactList.Count == 1 || gGameState.Sh < 1 || WeaponArtifactAc.Field5 < 2))
						{
							WeaponArtifact = artifact;
						}
					}
				}

				if (NewlineFlag)
				{
					gOut.WriteLine();

					NewlineFlag = false;
				}

				if (ActorRoom.IsLit())
				{
					if (!gEngine.AutoDisplayUnseenArtifactDescs && !GetAll && DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.Seen)
					{
						Globals.Buf.Clear();

						rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Write("{0}", Globals.Buf);

						DobjArtifact.Seen = true;
					}
				}

				if (ActorMonster.Weapon <= 0 && WeaponArtifact != null && NextState == null)
				{
					RedirectCommand = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(RedirectCommand);

					RedirectCommand.Dobj = WeaponArtifact;

					NextState = RedirectCommand;
				}
			}
			else
			{
				gOut.Print("There's nothing for you to get.");

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			MonsterArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			MonsterDobjArtAc = DobjArtifact.GetArtifactCategory(MonsterArtTypes, false);

			if (MonsterDobjArtAc == null)
			{
				MonsterDobjArtAc = DobjArtifact.GetCategories(0);
			}

			if (MonsterDobjArtAc != null && MonsterDobjArtAc.Type != ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (MonsterDobjArtAc.Type != ArtifactType.DeadBody || MonsterDobjArtAc.Field1 == 1) && MonsterDobjArtAc.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				MonsterDobjArtifactCount = 0;

				MonsterDobjArtifactWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref _monsterDobjArtifactCount, ref _monsterDobjArtifactWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref _monsterDobjArtifactCount, ref _monsterDobjArtifactWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				MonsterActorMonsterInventoryWeight = 0;

				rc = ActorMonster.GetFullInventoryWeight(ref _monsterActorMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || (MonsterDobjArtifactWeight <= ActorMonster.GetWeightCarryableGronds() && MonsterDobjArtifactWeight + MonsterActorMonsterInventoryWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.GroupCount))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					Debug.Assert(gCharMonster != null);

					if (gCharMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsLit())
						{
							MonsterName = ActorMonster.EvalPlural(ActorMonster.GetTheName(true), ActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

							gOut.Print("{0} picks up {1}.", MonsterName, DobjArtifact.GetTheName());
						}
						else
						{
							MonsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

							gOut.Print("{0} picks up {1}.", MonsterName, "a weapon");
						}
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

					MonsterFumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -DobjArtifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in MonsterFumbleMonsterList)
					{
						monster.Weapon = -1;
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == ArtifactType.DisguisedMonster)
			{
				ProcessAction(() => gEngine.RevealDisguisedMonster(ActorRoom, artifact), ref nlFlag);
			}
			else if (artifact.Weight > 900)
			{
				ProcessAction(() => PrintDontBeAbsurd(), ref nlFlag);
			}
			else if (artifact.IsUnmovable01())
			{
				ProcessAction(() => PrintCantVerbThat(artifact), ref nlFlag);
			}
			else
			{
				PlayerDobjArtifactCount = 0;

				PlayerDobjArtifactWeight = artifact.Weight;

				if (artifact.GeneralContainer != null)
				{
					rc = artifact.GetContainerInfo(ref _playerDobjArtifactCount, ref _playerDobjArtifactWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = artifact.GetContainerInfo(ref _playerDobjArtifactCount, ref _playerDobjArtifactWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				PlayerActorMonsterInventoryWeight = 0;

				rc = ActorMonster.GetFullInventoryWeight(ref _playerActorMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (ac.Type == ArtifactType.DeadBody && ac.Field1 != 1)
				{
					ProcessAction(() => PrintBestLeftAlone(artifact), ref nlFlag);
				}
				else if (!OmitWeightCheck && (PlayerDobjArtifactWeight + PlayerActorMonsterInventoryWeight > ActorMonster.GetWeightCarryableGronds()))
				{
					ProcessAction(() => PrintTooHeavy(artifact), ref nlFlag);
				}
				else if (ac.Type == ArtifactType.BoundMonster)
				{
					ProcessAction(() => PrintMustBeFreed(artifact), ref nlFlag);
				}
				else
				{
					WeaponAffinityMonster = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

					if (WeaponAffinityMonster != null)
					{
						ProcessAction(() => PrintObjBelongsToActor(artifact, WeaponAffinityMonster), ref nlFlag);
					}
					else
					{
						IsCarriedByContainer = artifact.IsCarriedByContainer();

						artifact.SetCarriedByCharacter();

						if (NextState is IRequestCommand)
						{
							PrintReceived(artifact);
						}
						else if (NextState is IRemoveCommand || IsCarriedByContainer)
						{
							PrintRetrieved(artifact);
						}
						else
						{
							PrintTaken(artifact);
						}

						nlFlag = true;
					}
				}
			}
		}

		public virtual void ProcessAction(Action action, ref bool nlFlag)
		{
			Debug.Assert(action != null);

			if (nlFlag)
			{
				gOut.WriteLine();

				nlFlag = false;
			}

			action();

			if (!PreserveNextState && NextState != null)
			{
				NextState = null;
			}
		}

		public GetCommand()
		{
			Synonyms = new string[] { "take" };

			SortOrder = 160;

			Name = "GetCommand";

			Verb = "get";

			Type = CommandType.Manipulation;
		}
	}
}
