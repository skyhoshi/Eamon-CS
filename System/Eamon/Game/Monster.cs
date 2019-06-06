
// Monster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Monster : GameBase, IMonster
	{
		#region Protected Fields

		protected long _courage;

		#endregion

		#region Public Properties

		#region Interface IMonster

		public virtual string StateDesc { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual PluralType PluralType { get; set; }

		public virtual long Hardiness { get; set; }

		public virtual long Agility { get; set; }

		public virtual long GroupCount { get; set; }

		public virtual long AttackCount { get; set; }

		public virtual long Courage
		{
			get
			{
				return Globals.EnableGameOverrides && Globals.IsRulesetVersion(5) && IsWeaponless(false) ? _courage / 2 : _courage;
			}

			set
			{
				_courage = value;
			}
		}

		public virtual long Location { get; set; }

		public virtual CombatCode CombatCode { get; set; }

		public virtual long Armor { get; set; }

		public virtual long Weapon { get; set; }

		public virtual long NwDice { get; set; }

		public virtual long NwSides { get; set; }

		public virtual long DeadBody { get; set; }

		public virtual Friendliness Friendliness { get; set; }

		public virtual Gender Gender { get; set; }

		public virtual long InitGroupCount { get; set; }

		public virtual long OrigGroupCount { get; set; }

		public virtual Friendliness OrigFriendliness { get; set; }

		public virtual long DmgTaken { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeMonsterUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		public override string GetPluralName(string fieldName, StringBuilder buf)
		{
			IEffect effect;
			long effectUid;
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

			effect = Globals.EDB[effectUid];

			if (effect != null)
			{
				buf.Append(effect.Desc.Substring(0, Math.Min(Constants.MonNameLen, effect.Desc.Length)).Trim());
			}
			else
			{
				buf.Append(Name);

				if (buf.Length > 0 && PluralType == PluralType.YIes)
				{
					buf.Length--;
				}

				buf.Append(PluralType == PluralType.None ? "" :
						PluralType == PluralType.Es ? "es" :
						PluralType == PluralType.YIes ? "ies" :
						"s");
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			StringBuilder buf01;
			string result;
			long gc;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			buf01 = new StringBuilder(Constants.BufSize);

			gc = groupCountOne ? 1 : GroupCount;

			switch (articleType)
			{
				case ArticleType.None:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append(gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) : "");
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				case ArticleType.The:

					if (gc > 10)
					{
						buf01.AppendFormat("{0}{1} ", "the ", gc);
					}
					else
					{
						buf01.AppendFormat
						(
							"{0}{1}",
							gc > 1 ? "the " :
							ArticleType == ArticleType.None ? "" :
							"the ",
							gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) : ""
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				default:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append
						(
							gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) :
							ArticleType == ArticleType.None ? "" :
							ArticleType == ArticleType.The ? "the " :
							ArticleType == ArticleType.Some ? "some " :
							ArticleType == ArticleType.An ? "an " :
							"a "
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;
			}

			if (buf.Length > 0 && upshift)
			{
				buf[0] = Char.ToUpper(buf[0]);
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					GetDecoratedName02(true, true, false, false, new StringBuilder(Constants.BufSize)));
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || !string.IsNullOrWhiteSpace(Desc))
			{
				buf.Append(Environment.NewLine);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IMonster monster)
		{
			return this.Uid.CompareTo(monster.Uid);
		}

		#endregion

		#region Interface IMonster

		public virtual bool IsDead()
		{
			return DmgTaken >= Hardiness;
		}

		public virtual bool IsCarryingWeapon()
		{
			return Weapon > 0 && Weapon < 1001;
		}

		public virtual bool IsWeaponless(bool includeWeaponFumble)
		{
			return includeWeaponFumble ? Weapon < 0 : Weapon == -1;
		}

		public virtual bool HasDeadBody()
		{
			return DeadBody > 0 && DeadBody < 1001;
		}

		public virtual bool HasWornInventory()
		{
			return true;
		}

		public virtual bool HasCarriedInventory()
		{
			return true;
		}

		public virtual bool HasHumanNaturalAttackDescs()
		{
			return false;
		}

		public virtual bool IsInRoom()
		{
			return Location > 0 && Location < 1001;
		}

		public virtual bool IsInLimbo()
		{
			return Location == 0;
		}

		public virtual bool IsInRoomUid(long roomUid)
		{
			return Location == roomUid;
		}

		public virtual bool IsInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsInRoomUid(room.Uid);
		}

		public virtual bool CanMoveToRoom(bool fleeing)
		{
			return true;
		}

		public virtual bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			return CanMoveToRoom(fleeing);
		}

		public virtual bool CanMoveToRoom(IRoom room, bool fleeing)
		{
			Debug.Assert(room != null);

			return CanMoveToRoomUid(room.Uid, fleeing);
		}

		public virtual bool CanAttackWithMultipleWeapons()
		{
			return false;
		}

		public virtual long GetCarryingWeaponUid()
		{
			return IsCarryingWeapon() ? Weapon : 0;
		}

		public virtual long GetDeadBodyUid()
		{
			return HasDeadBody() ? DeadBody : 0;
		}

		public virtual long GetInRoomUid()
		{
			return IsInRoom() ? Location : 0;
		}

		public virtual IRoom GetInRoom()
		{
			var uid = GetInRoomUid();

			return Globals.RDB[uid];
		}

		public virtual void SetInRoomUid(long roomUid)
		{
			Location = roomUid;

			var gameState = Globals?.Engine.GetGameState();

			if (IsCharacterMonster() && gameState != null)
			{
				gameState.Ro = roomUid;
			}
		}

		public virtual void SetInLimbo()
		{
			SetInRoomUid(0);
		}

		public virtual void SetInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetInRoomUid(room.Uid);
		}

		public virtual bool IsInRoomLit()
		{
			var room = GetInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool ShouldFleeRoom()
		{
			return CheckNBTLHostility();
		}

		public virtual bool ShouldReadyWeapon()
		{
			return true;
		}

		public virtual bool ShouldShowContentsWhenExamined()
		{
			return false;
		}

		public virtual bool ShouldProcessInGameLoop()
		{
			var gameState = Globals?.Engine.GetGameState();

			return gameState != null && Location == gameState.Ro && !IsCharacterMonster();
		}

		public virtual bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return !Globals.IsRulesetVersion(5) && (Friendliness == Friendliness.Enemy || (Friendliness == Friendliness.Neutral && artifact.Value < 3000));
		}

		public virtual bool CheckNBTLHostility()
		{
			var gameState = Globals?.Engine.GetGameState();

			return gameState != null && Friendliness != Friendliness.Neutral && gameState.GetNBTL(Friendliness == Friendliness.Friend ? Friendliness.Enemy : Friendliness.Friend) > 0;
		}

		public virtual bool CheckCourage()
		{
			var result = false;

			if (Globals.Engine != null)
			{
				var gameState = Globals.Engine.GetGameState();

				if (Globals.IsRulesetVersion(5) && gameState != null)
				{
					var rl = (long)Math.Round((double)gameState.GetDTTL(Friendliness) / (double)gameState.GetNBTL(Friendliness) * 100 + Globals.Engine.RollDice(1, 41, -21));

					result = rl <= Courage;
				}
				else
				{
					var s = (DmgTaken > 0 || OrigGroupCount > GroupCount ? 1 : 0) + (DmgTaken + 4 >= Hardiness ? 1 : 0);

					var rl = Globals.Engine.RollDice(1, 100, s * 5);

					result = rl <= Courage;           // Courage >= 100 ||
				}
			}

			return result;
		}

		public virtual T EvalFriendliness<T>(T enemyValue, T neutralValue, T friendValue)
		{
			return Globals.Engine.EvalFriendliness(Friendliness, enemyValue, neutralValue, friendValue);
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return Globals.Engine.EvalGender(Gender, maleValue, femaleValue, neutralValue);
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return Globals.Engine.EvalPlural(GroupCount > 1, singularValue, pluralValue);
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual void ResolveFriendlinessPct(long charisma)
		{
			if (Globals.Engine.IsValidMonsterFriendlinessPct(Friendliness))
			{
				if (Globals.IsRulesetVersion(5))
				{
					var f = (long)Friendliness - 100;

					if (f > 0 && f < 100)
					{
						f += Globals.Engine.GetCharismaFactor(charisma);
					}

					var k = Friendliness.Friend;

					var rl = Globals.Engine.RollDice(1, 100, 0);

					if (rl > f)
					{
						k--;

						rl = Globals.Engine.RollDice(1, 100, 0);

						if (rl > f)
						{
							k--;
						}
					}

					Friendliness = k;
				}
				else
				{
					var f = (long)Friendliness - 100;

					var k = Friendliness.Friend;

					var rl = Globals.Engine.RollDice(1, 100, 0);

					if (f > 0 && f < 100)
					{
						rl -= Globals.Engine.GetCharismaFactor(charisma);
					}

					if (rl > f)
					{
						k--;

						rl = Globals.Engine.RollDice(1, 100, 0);

						if (f > 0 && f < 100)
						{
							rl -= Globals.Engine.GetCharismaFactor(charisma);
						}

						if (rl > f)
						{
							k--;
						}
					}

					Friendliness = k;
				}
			}
		}

		public virtual void ResolveFriendlinessPct(ICharacter character)
		{
			if (character != null)
			{
				ResolveFriendlinessPct(character.GetStats(Stat.Charisma));
			}
		}

		public virtual void CalculateGiftFriendlinessPct(long value)
		{
			Debug.Assert(Globals.IsRulesetVersion(5));

			OrigFriendliness -= 100;

			OrigFriendliness = (Friendliness)((double)OrigFriendliness * (1 + (double)value / 33));       // Scaled to EDX values; originally 100

			if (OrigFriendliness < 0)
			{
				OrigFriendliness = 0;
			}
			else if ((long)OrigFriendliness > 100)
			{
				OrigFriendliness = (Friendliness)100;
			}

			OrigFriendliness += 100;

			Friendliness = OrigFriendliness;
		}

		public virtual bool IsCharacterMonster()
		{
			var gameState = Globals?.Engine.GetGameState();

			return gameState != null && gameState.Cm == Uid;
		}

		public virtual bool IsStateDescSideNotes()
		{
			if (!string.IsNullOrWhiteSpace(StateDesc))
			{
				var regex = new Regex(@".*\(.+\)");

				return regex.IsMatch(StateDesc);
			}
			else
			{
				return false;
			}
		}

		public virtual long GetWeightCarryableGronds()
		{
			return Globals.Engine.GetWeightCarryableGronds(Hardiness);
		}

		public virtual long GetFleeingMemberCount()
		{
			return Globals.Engine.RollDice(1, GroupCount, 0);
		}

		public virtual long GetMaxMemberAttackCount()
		{
			return 5;
		}

		public virtual IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsCarriedByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsCarriedByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.GeneralContainer != null)
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsWornByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsWornByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.GeneralContainer != null)
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsCarriedByCharacter() || a.IsWornByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsCarriedByMonster(this) || a.IsWornByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.GeneralContainer != null)
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			long c, w, mwt;

			rc = RetCode.Success;

			mwt = 0;

			var list = GetContainedList(monsterFindFunc, artifactFindFunc);

			foreach (var a in list)
			{
				c = 0;

				w = a.Weight;

				Debug.Assert(!Globals.Engine.IsUnmovable01(w));

				if (recurse && a.GeneralContainer != null)
				{
					rc = a.GetContainerInfo(ref c, ref w, (ContainerType)(-1), recurse);

					if (Globals.Engine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}
				}

				if (w <= 10 * Hardiness && mwt + w <= 10 * Hardiness * GroupCount)
				{
					mwt += w;
				}
				else
				{
					a.Location = Location >= 0 ? Location : 0;

					if (Weapon == a.Uid)
					{
						a.RemoveStateDesc(a.GetReadyWeaponDesc());

						Weapon = -1;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			var list = GetContainedList(monsterFindFunc, artifactFindFunc, recurse);

			foreach (var a in list)
			{
				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}
			}

			return rc;
		}

		public virtual void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = "dead!";
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = (Globals.IsRulesetVersion(5) ? "very " : "") + "badly injured.";
				}
				else if (x == 3)
				{
					result = "in pain.";
				}
				else if (x == 2)
				{
					result = "hurting.";
				}
				else if (x == 1)
				{
					result = "still in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}

		public virtual string GetAttackDescString(IArtifact artifact)
		{
			var rl = Globals.Engine.RollDice(1, 3, artifact == null && HasHumanNaturalAttackDescs() ? 3 : 0);

			var ac = artifact != null ? artifact.GeneralWeapon : null;

			return Globals.Engine.GetAttackDescString((Weapon)(ac != null ? ac.Field2 : 0), rl);
		}

		public virtual string GetMissDescString(IArtifact artifact)
		{
			var i = Globals.Engine.RollDice(1, 2, 0);

			var ac = artifact != null ? artifact.GeneralWeapon : null;

			return Globals.Engine.GetMissDescString((Weapon)(ac != null ? ac.Field2 : 0), i);
		}

		public virtual string GetArmorDescString()
		{
			return "armor";
		}

		#endregion

		#region Class Monster

		public Monster()
		{
			StateDesc = "";
		}

		#endregion

		#endregion
	}
}
