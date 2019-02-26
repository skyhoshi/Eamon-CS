
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Artifact : GameBase, IArtifact
	{
		#region Protected Fields

		protected IArtifactCategory _lastArtifactCategory;

		#endregion

		#region Public Properties

		#region Interface IArtifact

		public virtual string StateDesc { get; set; }

		public virtual bool IsCharOwned { get; set; }

		public virtual bool IsPlural { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual PluralType PluralType { get; set; }

		public virtual long Value { get; set; }

		public virtual long Weight { get; set; }

		public virtual long Location { get; set; }

		[ExcludeFromSerialization]
		public virtual ArtifactType Type
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Type : ArtifactType.None;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Type = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field1
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Field1 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field1 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field2
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Field2 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field2 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field3
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Field3 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field3 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field4
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Field4 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field4 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual long Field5
		{
			get
			{
				var ac = GetCategories(0);

				return ac != null ? ac.Field5 : 0;
			}

			set
			{
				var ac = GetCategories(0);

				if (ac != null)
				{
					ac.Field5 = value;
				}
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Gold
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Gold);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Treasure
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Treasure);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Weapon
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Weapon);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory MagicWeapon
		{
			get
			{
				return GetArtifactCategory(ArtifactType.MagicWeapon);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory GeneralWeapon
		{
			get
			{
				var artTypes = new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon };

				return GetArtifactCategory(artTypes);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Container
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Container);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory LightSource
		{
			get
			{
				return GetArtifactCategory(ArtifactType.LightSource);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Drinkable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Drinkable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Readable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Readable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DoorGate
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DoorGate);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Edible
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Edible);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory BoundMonster
		{
			get
			{
				return GetArtifactCategory(ArtifactType.BoundMonster);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory Wearable
		{
			get
			{
				return GetArtifactCategory(ArtifactType.Wearable);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DisguisedMonster
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DisguisedMonster);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory DeadBody
		{
			get
			{
				return GetArtifactCategory(ArtifactType.DeadBody);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User1
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User1);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User2
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User2);
			}
		}

		[ExcludeFromSerialization]
		public virtual IArtifactCategory User3
		{
			get
			{
				return GetArtifactCategory(ArtifactType.User3);
			}
		}

		public virtual IArtifactCategory[] Categories { get; set; }

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
				Globals.Database.FreeArtifactUid(Uid);

				Uid = 0;
			}
		}

		#endregion

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
				case ArticleType.None:

					buf.AppendFormat
					(
						"{0}{1}{2}",
						EvalPlural(Name, GetPluralName(fieldName, new StringBuilder(Constants.BufSize))),
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				case ArticleType.The:

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						ArticleType == ArticleType.None ? "" :
						ArticleType == ArticleType.The ? "the " :
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
						ArticleType == ArticleType.None ? "" :
						ArticleType == ArticleType.The ? "the " :
						IsCharOwned && showCharOwned ? "your " :
						ArticleType == ArticleType.Some ? "some " :
						ArticleType == ArticleType.An ? "an " :
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

		public virtual IArtifactCategory GetCategories(long index)
		{
			_lastArtifactCategory = Categories[index];

			return _lastArtifactCategory;
		}

		public virtual string GetSynonyms(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetCategories(long index, IArtifactCategory value)
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
			return GeneralWeapon != null;
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
			return GeneralWeapon != null;
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

		public virtual bool IsWeapon(Weapon weapon)
		{
			var ac = GeneralWeapon;

			return ac != null && ac.IsWeapon(weapon);
		}

		public virtual bool IsAttackable()
		{
			var ac = GetArtifactCategory(new ArtifactType[] { ArtifactType.DeadBody, ArtifactType.DisguisedMonster, ArtifactType.Container, ArtifactType.DoorGate });

			return ac != null && (ac.Type == ArtifactType.DeadBody || ac.Type == ArtifactType.DisguisedMonster || ac.GetBreakageStrength() >= 1000);
		}

		public virtual bool IsAttackable01(ref IArtifactCategory ac)
		{
			ac = GetArtifactCategory(new ArtifactType[] { ArtifactType.DeadBody, ArtifactType.DisguisedMonster, ArtifactType.Container, ArtifactType.DoorGate }, false);

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

		public virtual bool IsArmor()
		{
			var ac = GetArtifactCategory(ArtifactType.Wearable);

			return ac != null && ac.Field1 > 1;
		}

		public virtual bool IsShield()
		{
			var ac = GetArtifactCategory(ArtifactType.Wearable);

			return ac != null && ac.Field1 == 1;
		}

		public virtual bool ShouldShowContentsWhenExamined()
		{
			return false;
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

		public virtual IArtifactCategory GetArtifactCategory(ArtifactType artifactType)
		{
			IArtifactCategory result = null;

			if (_lastArtifactCategory != null && _lastArtifactCategory.Type == artifactType)
			{
				result = _lastArtifactCategory;
			}
			else if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
			{
				result = Categories.FirstOrDefault(ac => ac != null && ac.Type == artifactType);
			}
			else
			{
				result = null;
			}

			_lastArtifactCategory = result;

			return result;
		}

		public virtual IArtifactCategory GetArtifactCategory(ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true)
		{
			IArtifactCategory result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
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

			_lastArtifactCategory = result;

			return result;
		}

		public virtual IList<IArtifactCategory> GetArtifactCategories(ArtifactType[] artifactTypes)
		{
			IList<IArtifactCategory> result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetCategories(0) != null && GetCategories(0).Type != ArtifactType.None)
			{
				result = Categories.Where(ac => ac != null && artifactTypes.Contains(ac.Type)).ToList();
			}
			else
			{
				result = new List<IArtifactCategory>() { null };
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

			var categories01 = new IArtifactCategory[count];

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
					categories01[i] = Globals.CreateInstance<IArtifactCategory>(x =>
					{
						x.Parent = this;
					});

					i++;
				}
			}

			Categories = categories01;

		Cleanup:

			_lastArtifactCategory = null;

			return rc;
		}

		public virtual RetCode SyncArtifactCategories(IArtifactCategory artifactCategory)
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
				case ArtifactType.Container:
				case ArtifactType.DoorGate:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != ArtifactType.None)
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

				case ArtifactType.Drinkable:
				case ArtifactType.Edible:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != ArtifactType.None)
						{
							if (ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible)
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

				case ArtifactType.Readable:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != ArtifactType.None)
						{
							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactCategory.IsOpen());
							}
						}
					}

					break;
				}

				case ArtifactType.BoundMonster:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != ArtifactType.None)
						{
							if (ac.Type == ArtifactType.DisguisedMonster)
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

				case ArtifactType.DisguisedMonster:
				{
					foreach (var ac in Categories)
					{
						if (ac != null && ac != artifactCategory && ac.Type != ArtifactType.None)
						{
							if (ac.Type == ArtifactType.BoundMonster)
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
				if (ac != null && ac.Type != ArtifactType.None)
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

			var list = Globals.Engine.GetArtifactList(a => artifactFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.Container != null)
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

			if (Container != null)
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

				if (recurse && a.Container != null)
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

			Categories = new IArtifactCategory[]
			{
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<IArtifactCategory>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
