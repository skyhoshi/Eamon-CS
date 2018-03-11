
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Artifact : GameBase, IArtifact
	{
		#region Public Properties

		#region Interface IArtifact

		public virtual string StateDesc { get; set; }

		public virtual bool IsCharOwned { get; set; }

		public virtual bool IsPlural { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual Enums.PluralType PluralType { get; set; }

		public virtual long Value { get; set; }

		public virtual long Weight { get; set; }

		public virtual long Location { get; set; }

		public virtual Classes.IArtifactCategory[] Categories { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeArtifactUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IGameBase

		public override void SetParentReferences()
		{
			foreach (var ac in Categories)
			{
				if (ac != null)
				{
					ac.Parent = this;
				}
			}
		}

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
				buf.Append(effect.Desc.Substring(0, Math.Min(Constants.ArtNameLen, effect.Desc.Length)).Trim());
			}
			else
			{
				buf.Append(Name);

				if (buf.Length > 0 && PluralType == Enums.PluralType.YIes)
				{
					buf.Length--;
				}

				buf.Append(PluralType == Enums.PluralType.None ? "" :
						PluralType == Enums.PluralType.Es ? "es" :
						PluralType == Enums.PluralType.YIes ? "ies" :
						"s");
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName) || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			buf.Clear();

			switch (articleType)
			{
				case Enums.ArticleType.None:

					buf.AppendFormat
					(
						"{0}{1}{2}",
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				case Enums.ArticleType.The:

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						ArticleType == Enums.ArticleType.None ? "" :
						ArticleType == Enums.ArticleType.The ? "the " :
						IsCharOwned && showCharOwned ? "your " :
						"the ",
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				default:

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						ArticleType == Enums.ArticleType.None ? "" :
						ArticleType == Enums.ArticleType.The ? "the " :
						IsCharOwned && showCharOwned ? "your " :
						ArticleType == Enums.ArticleType.Some ? "some " :
						ArticleType == Enums.ArticleType.An ? "an " :
						"a ",
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
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

		public virtual int CompareTo(IArtifact artifact)
		{
			return this.Uid.CompareTo(artifact.Uid);
		}

		#endregion

		#region Interface IArtifact

		public virtual Classes.IArtifactCategory GetCategories(long index)
		{
			return Categories[index];
		}

		public virtual string GetSynonyms(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetCategories(long index, Classes.IArtifactCategory value)
		{
			Categories[index] = value;
		}

		public virtual void SetSynonyms(long index, string value)
		{
			Synonyms[index] = value;
		}

		public virtual bool IsCarriedByCharacter()
		{
			return Location == -1;
		}

		public virtual bool IsCarriedByMonster()
		{
			return Location < -1 && Location > -999;
		}

		public virtual bool IsCarriedByContainer()
		{
			return Location > 1000 && Location < 2001;
		}

		public virtual bool IsWornByCharacter()
		{
			return Location == -999;
		}

		public virtual bool IsWornByMonster()
		{
			return Location < -1000;
		}

		public virtual bool IsReadyableByCharacter()
		{
			return IsWeapon01();
		}

		public virtual bool IsInRoom()
		{
			return Location > 0 && Location < 1001;
		}

		public virtual bool IsEmbeddedInRoom()
		{
			return Location > 2000 && Location < 3001;
		}

		public virtual bool IsInLimbo()
		{
			return Location == 0;
		}

		public virtual bool IsCarriedByMonsterUid(long monsterUid)
		{
			return Location == (-monsterUid - 1);
		}

		public virtual bool IsCarriedByContainerUid(long containerUid)
		{
			return Location == (containerUid + 1000);
		}

		public virtual bool IsWornByMonsterUid(long monsterUid)
		{
			return Location == (-monsterUid - 1000);
		}

		public virtual bool IsReadyableByMonsterUid(long monsterUid)
		{
			return IsWeapon01();
		}

		public virtual bool IsInRoomUid(long roomUid)
		{
			return Location == roomUid;
		}

		public virtual bool IsEmbeddedInRoomUid(long roomUid)
		{
			return Location == (roomUid + 2000);
		}

		public virtual bool IsCarriedByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			return IsCarriedByMonsterUid(monster.Uid);
		}

		public virtual bool IsCarriedByContainer(IArtifact container)
		{
			Debug.Assert(container != null);

			return IsCarriedByContainerUid(container.Uid);
		}

		public virtual bool IsWornByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			return IsWornByMonsterUid(monster.Uid);
		}

		public virtual bool IsReadyableByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			return IsReadyableByMonsterUid(monster.Uid);
		}

		public virtual bool IsInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsInRoomUid(room.Uid);
		}

		public virtual bool IsEmbeddedInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsEmbeddedInRoomUid(room.Uid);
		}

		public virtual long GetCarriedByMonsterUid()
		{
			return IsCarriedByMonster() ? -Location - 1 : 0;
		}

		public virtual long GetCarriedByContainerUid()
		{
			return IsCarriedByContainer() ? Location - 1000 : 0;
		}

		public virtual long GetWornByMonsterUid()
		{
			return IsWornByMonster() ? -Location - 1000 : 0;
		}

		public virtual long GetInRoomUid()
		{
			return IsInRoom() ? Location : 0;
		}

		public virtual long GetEmbeddedInRoomUid()
		{
			return IsEmbeddedInRoom() ? Location - 2000 : 0;
		}

		public virtual IMonster GetCarriedByMonster()
		{
			var uid = GetCarriedByMonsterUid();

			return Globals.MDB[uid];
		}

		public virtual IArtifact GetCarriedByContainer()
		{
			var uid = GetCarriedByContainerUid();

			return Globals.ADB[uid];
		}

		public virtual IMonster GetWornByMonster()
		{
			var uid = GetWornByMonsterUid();

			return Globals.MDB[uid];
		}

		public virtual IRoom GetInRoom()
		{
			var uid = GetInRoomUid();

			return Globals.RDB[uid];
		}

		public virtual IRoom GetEmbeddedInRoom()
		{
			var uid = GetEmbeddedInRoomUid();

			return Globals.RDB[uid];
		}

		public virtual void SetCarriedByCharacter()
		{
			Location = -1;
		}

		public virtual void SetCarriedByMonsterUid(long monsterUid)
		{
			Location = (-monsterUid - 1);
		}

		public virtual void SetCarriedByContainerUid(long containerUid)
		{
			Location = (containerUid + 1000);
		}

		public virtual void SetWornByCharacter()
		{
			Location = -999;
		}

		public virtual void SetWornByMonsterUid(long monsterUid)
		{
			Location = (-monsterUid - 1000);
		}

		public virtual void SetInRoomUid(long roomUid)
		{
			Location = roomUid;
		}

		public virtual void SetEmbeddedInRoomUid(long roomUid)
		{
			Location = (roomUid + 2000);
		}

		public virtual void SetInLimbo()
		{
			Location = 0;
		}

		public virtual void SetCarriedByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			SetCarriedByMonsterUid(monster.Uid);
		}

		public virtual void SetCarriedByContainer(IArtifact container)
		{
			Debug.Assert(container != null);

			SetCarriedByContainerUid(container.Uid);
		}

		public virtual void SetWornByMonster(IMonster monster)
		{
			Debug.Assert(monster != null);

			SetWornByMonsterUid(monster.Uid);
		}

		public virtual void SetInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetInRoomUid(room.Uid);
		}

		public virtual void SetEmbeddedInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetEmbeddedInRoomUid(room.Uid);
		}

		public virtual bool IsInRoomLit()
		{
			var room = GetInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool IsEmbeddedInRoomLit()
		{
			var room = GetEmbeddedInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool IsFieldStrength(long value)
		{
			return Globals.Engine.IsArtifactFieldStrength(value);
		}

		public virtual long GetFieldStrength(long value)
		{
			return Globals.Engine.GetArtifactFieldStrength(value);
		}

		public virtual bool IsWeapon(Enums.Weapon weapon)
		{
			var ac = GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

			return ac != null && ac.IsWeapon(weapon);
		}

		public virtual bool IsAttackable()
		{
			var ac = GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.DeadBody, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DoorGate });

			return ac != null && (ac.Type == Enums.ArtifactType.DeadBody || ac.Type == Enums.ArtifactType.DisguisedMonster || ac.GetBreakageStrength() >= 1000);
		}

		public virtual bool IsAttackable01(ref Classes.IArtifactCategory ac)
		{
			ac = GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.DeadBody, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DoorGate }, false);

			return !Globals.IsRulesetVersion(5) && ac != null;
		}

		public virtual bool IsUnmovable()
		{
			return Globals.Engine.IsUnmovable(Weight);
		}

		public virtual bool IsUnmovable01()
		{
			return Globals.Engine.IsUnmovable01(Weight);
		}

		public virtual bool IsGold()
		{
			return GetArtifactCategory(Enums.ArtifactType.Gold) != null;
		}

		public virtual bool IsTreasure()
		{
			return GetArtifactCategory(Enums.ArtifactType.Treasure) != null;
		}

		public virtual bool IsWeapon()
		{
			return GetArtifactCategory(Enums.ArtifactType.Weapon) != null;
		}

		public virtual bool IsWeapon01()
		{
			var ac = GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

			return ac != null;
		}

		public virtual bool IsMagicWeapon()
		{
			return GetArtifactCategory(Enums.ArtifactType.MagicWeapon) != null;
		}

		public virtual bool IsContainer()
		{
			return GetArtifactCategory(Enums.ArtifactType.Container) != null;
		}

		public virtual bool IsLightSource()
		{
			return GetArtifactCategory(Enums.ArtifactType.LightSource) != null;
		}

		public virtual bool IsDrinkable()
		{
			return GetArtifactCategory(Enums.ArtifactType.Drinkable) != null;
		}

		public virtual bool IsReadable()
		{
			return GetArtifactCategory(Enums.ArtifactType.Readable) != null;
		}

		public virtual bool IsDoorGate()
		{
			return GetArtifactCategory(Enums.ArtifactType.DoorGate) != null;
		}

		public virtual bool IsEdible()
		{
			return GetArtifactCategory(Enums.ArtifactType.Edible) != null;
		}

		public virtual bool IsBoundMonster()
		{
			return GetArtifactCategory(Enums.ArtifactType.BoundMonster) != null;
		}

		public virtual bool IsWearable()
		{
			return GetArtifactCategory(Enums.ArtifactType.Wearable) != null;
		}

		public virtual bool IsArmor()
		{
			var ac = GetArtifactCategory(Enums.ArtifactType.Wearable);

			return ac != null && ac.Field1 > 1;
		}

		public virtual bool IsShield()
		{
			var ac = GetArtifactCategory(Enums.ArtifactType.Wearable);

			return ac != null && ac.Field1 == 1;
		}

		public virtual bool IsDisguisedMonster()
		{
			return GetArtifactCategory(Enums.ArtifactType.DisguisedMonster) != null;
		}

		public virtual bool IsDeadBody()
		{
			return GetArtifactCategory(Enums.ArtifactType.DeadBody) != null;
		}

		public virtual bool IsUser1()
		{
			return GetArtifactCategory(Enums.ArtifactType.User1) != null;
		}

		public virtual bool IsUser2()
		{
			return GetArtifactCategory(Enums.ArtifactType.User2) != null;
		}

		public virtual bool IsUser3()
		{
			return GetArtifactCategory(Enums.ArtifactType.User3) != null;
		}

		public virtual string GetProvidingLightDesc()
		{
			return "(providing light)";
		}

		public virtual string GetReadyWeaponDesc()
		{
			return "(ready weapon)";
		}

		public virtual string GetBrokenDesc()
		{
			return "(broken)";
		}

		public virtual string GetEmptyDesc()
		{
			return "(empty)";
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return Globals.Engine.EvalPlural(IsPlural, singularValue, pluralValue);
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsEmbeddedInRoomLit() ? lightValue : darkValue;
		}

		public virtual Classes.IArtifactCategory GetArtifactCategory(Enums.ArtifactType artifactType)
		{
			if (GetCategories(0) != null && GetCategories(0).Type != Enums.ArtifactType.None)
			{
				return Categories.FirstOrDefault(ac => ac != null && ac.Type == artifactType);
			}
			else
			{
				return null;
			}
		}

		public virtual Classes.IArtifactCategory GetArtifactCategory(Enums.ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true)
		{
			Classes.IArtifactCategory result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategories(0) != null && GetCategories(0).Type != Enums.ArtifactType.None)
			{
				if (categoryArrayPrecedence)
				{
					result = Categories.FirstOrDefault(ac => ac != null && artifactTypes.Contains(ac.Type));
				}
				else
				{
					foreach (var at in artifactTypes)
					{
						result = Categories.FirstOrDefault(ac => ac != null && ac.Type == at);

						if (result != null)
						{
							break;
						}
					}
				}
			}
			else
			{
				result = null;
			}

		Cleanup:

			return result;
		}

		public virtual IList<Classes.IArtifactCategory> GetArtifactCategories(Enums.ArtifactType[] artifactTypes)
		{
			IList<Classes.IArtifactCategory> result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategories(0) != null && GetCategories(0).Type != Enums.ArtifactType.None)
			{
				result = Categories.Where(ac => ac != null && artifactTypes.Contains(ac.Type)).ToList();
			}
			else
			{
				result = new List<Classes.IArtifactCategory>() { null };
			}

		Cleanup:

			return result;
		}

		public virtual RetCode SetArtifactCategoryCount(long count)
		{
			RetCode rc;

			if (count < 1 || count > Constants.NumArtifactCategories)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var categories01 = new Classes.IArtifactCategory[count];

			var i = 0L;

			if (categories01.Length < Categories.Length)
			{
				while (i < categories01.Length)
				{
					categories01[i] = GetCategories(i);

					i++;
				}
			}
			else
			{
				while (i < Categories.Length)
				{
					categories01[i] = GetCategories(i);

					i++;
				}

				while (i < categories01.Length)
				{
					categories01[i] = Globals.CreateInstance<Classes.IArtifactCategory>(x =>
					{
						x.Parent = this;
					});

					i++;
				}
			}

			Categories = categories01;

		Cleanup:

			return rc;
		}

		public virtual RetCode SyncArtifactCategories(Classes.IArtifactCategory artifactCategory)
		{
			RetCode rc;

			if (artifactCategory == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			switch (artifactCategory.Type)
			{
				case Enums.ArtifactType.Container:
				case Enums.ArtifactType.DoorGate:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.IsLockable())
							{
								ac.SetKeyUid(artifactCategory.GetKeyUid());
							}

							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactCategory.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.Drinkable || ac.Type == Enums.ArtifactType.Edible)
							{
								ac.Field1 = artifactCategory.Field1;

								ac.Field2 = artifactCategory.Field2;
							}

							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactCategory.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.Readable:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactCategory.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.BoundMonster:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.DisguisedMonster)
							{
								ac.Field1 = artifactCategory.Field1;
							}

							if (ac.IsLockable())
							{
								ac.SetKeyUid(artifactCategory.GetKeyUid());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.DisguisedMonster:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.BoundMonster)
							{
								ac.Field1 = artifactCategory.Field1;
							}
						}
					}

					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SyncArtifactCategories()
		{
			RetCode rc;

			rc = RetCode.Success;

			foreach (var ac in Categories)
			{
				if (ac != null && ac.Type != Enums.ArtifactType.None)
				{
					rc = SyncArtifactCategories(ac);

					if (Globals.Engine.IsFailure(rc))
					{
						goto Cleanup;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode AddStateDesc(string stateDesc, bool dupAllowed = false)
		{
			StringBuilder buf;
			RetCode rc;
			int p;

			if (string.IsNullOrWhiteSpace(stateDesc))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			p = StateDesc.IndexOf(stateDesc, StringComparison.OrdinalIgnoreCase);

			if (dupAllowed || p == -1)
			{
				buf = new StringBuilder(Constants.BufSize);

				buf.AppendFormat
				(
					"{0}{1}{2}",
					StateDesc,
					StateDesc.Length > 0 ? " " : "",
					stateDesc
				);

				StateDesc = buf.ToString();
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode RemoveStateDesc(string stateDesc)
		{
			StringBuilder buf;
			RetCode rc;
			int p, q;

			if (string.IsNullOrWhiteSpace(stateDesc))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			p = StateDesc.IndexOf(stateDesc, StringComparison.OrdinalIgnoreCase);

			if (p != -1)
			{
				buf = new StringBuilder(Constants.BufSize);

				buf.Append(StateDesc);

				q = p + stateDesc.Length;

				if (!Char.IsWhiteSpace(buf[p]))
				{
					while (q < buf.Length && Char.IsWhiteSpace(buf[q]))
					{
						q++;
					}
				}

				buf.Remove(p, q - p);

				StateDesc = buf.ToString().Trim();
			}

		Cleanup:

			return rc;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (artifactFindFunc == null)
			{
				artifactFindFunc = a => a.IsCarriedByContainer(this);
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => artifactFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.IsContainer())
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual RetCode GetContainerInfo(ref long count, ref long weight, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			var queue = new Queue<IArtifact>();

			if (IsContainer())
			{
				queue.AddRange(GetContainedList());
			}

			while (queue.Any())
			{
				count++;

				var a = queue.Dequeue();

				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}

				if (recurse && a.IsContainer())
				{
					queue.AddRange(a.GetContainedList());
				}
			}

			return rc;
		}

		#endregion

		#region Class Artifact

		public Artifact()
		{
			StateDesc = "";

			Categories = new Classes.IArtifactCategory[]
			{
				Globals.CreateInstance<Classes.IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactCategory>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
