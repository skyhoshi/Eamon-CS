
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Eamon.ThirdParty;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : EamonDD.Game.Engine, Framework.IEngine
	{
		public virtual long StartRoom { get; set; }

		public virtual long NumSaveSlots { get; set; }

		public virtual long ScaledHardinessUnarmedMaxDamage { get; set; }

		public virtual double ScaledHardinessMaxDamageDivisor { get; set; }

		public virtual bool EnforceMonsterWeightLimits { get; set; }

		public virtual bool UseMonsterScaledHardinessValues { get; set; }

		public virtual Enums.PoundCharPolicy PoundCharPolicy { get; set; }

		protected virtual long ConvertWeaponsToArtifacts()
		{
			long cw = -1;

			foreach (var weapon in Globals.Character.Weapons)
			{
				if (weapon.IsActive())
				{
					var artifact = ConvertWeaponToArtifact(weapon);

					Debug.Assert(artifact != null);

					var ac = artifact.GeneralWeapon;

					Debug.Assert(ac != null);

					if (artifact.IsCarriedByCharacter() && (cw == -1 || WeaponPowerCompare(artifact.Uid, cw) > 0) && (Globals.GameState.Sh < 1 || ac.Field5 < 2))
					{
						cw = artifact.Uid;

						Debug.Assert(cw > 0);
					}
				}
			}

			return cw;
		}

		protected virtual long ConvertArmorToArtifacts()
		{
			var armorNames = new string[]
			{
					"",
					"leather",
					"chain",
					"plate",
					"magic"
			};

			var a2 = (long)Globals.Character.ArmorClass / 2;

			var x = (long)Globals.Character.ArmorClass % 2;

			var s = a2 + ((4 - a2) * (a2 > 4 ? 1 : 0));

			if (a2 > 0)
			{
				var armor = Globals.Character.Armor;

				Debug.Assert(armor != null);

				var artifact = Globals.CreateInstance<IArtifact>(y =>
				{
					y.SetArtifactCategoryCount(1);

					y.Uid = Globals.Database.GetArtifactUid();

					y.Name = armor.IsActive() ? Globals.CloneInstance(armor.Name) : string.Format("{0} armor", armorNames[s]);

					Debug.Assert(!string.IsNullOrWhiteSpace(y.Name));

					y.Desc = armor.IsActive() && !string.IsNullOrWhiteSpace(armor.Desc) ? Globals.CloneInstance(armor.Desc) : null;

					y.Seen = true;

					y.IsCharOwned = true;

					y.IsListed = true;

					if (armor.IsActive())
					{
						y.IsPlural = armor.IsPlural;

						y.PluralType = armor.PluralType;

						y.ArticleType = armor.ArticleType;

						y.GetCategories(0).Field1 = armor.Field1;

						y.GetCategories(0).Field2 = armor.Field2;

						y.GetCategories(0).Type = armor.Type;

						y.Value = armor.Value;

						y.Weight = armor.Weight;
					}
					else
					{
						y.IsPlural = false;

						y.PluralType = Enums.PluralType.None;

						y.ArticleType = Enums.ArticleType.Some;

						y.GetCategories(0).Field1 = a2 * 2;

						y.GetCategories(0).Field2 = 0;

						y.GetCategories(0).Type = Enums.ArtifactType.Wearable;

						var ima = false;

						y.Value = (long)GetArmorPriceOrValue(Globals.Character.ArmorClass, false, ref ima);

						y.Weight = (a2 == 1 ? 15 : a2 == 2 ? 25 : 35);
					}

					if (Globals.GameState.Wt + y.Weight <= 10 * Globals.Character.GetStats(Enums.Stat.Hardiness))
					{
						y.SetWornByCharacter();

						Globals.GameState.Wt += y.Weight;
					}
					else
					{
						y.SetInRoomUid(StartRoom);
					}
				});

				var rc = Globals.Database.AddArtifact(artifact);

				Debug.Assert(IsSuccess(rc));

				if (artifact.IsWornByCharacter())
				{
					Globals.GameState.Ar = artifact.Uid;

					Debug.Assert(Globals.GameState.Ar > 0);
				}
			}

			if (x == 1)
			{
				var shield = Globals.Character.Shield;

				Debug.Assert(shield != null);

				var artifact = Globals.CreateInstance<IArtifact>(y =>
				{
					y.SetArtifactCategoryCount(1);

					y.Uid = Globals.Database.GetArtifactUid();

					y.Name = shield.IsActive() ? Globals.CloneInstance(shield.Name) : "shield";

					Debug.Assert(!string.IsNullOrWhiteSpace(y.Name));

					y.Desc = shield.IsActive() && !string.IsNullOrWhiteSpace(shield.Desc) ? Globals.CloneInstance(shield.Desc) : null;

					y.Seen = true;

					y.IsCharOwned = true;

					y.IsListed = true;

					if (shield.IsActive())
					{
						y.IsPlural = shield.IsPlural;

						y.PluralType = shield.PluralType;

						y.ArticleType = shield.ArticleType;

						y.GetCategories(0).Field1 = shield.Field1;

						y.GetCategories(0).Field2 = shield.Field2;

						y.GetCategories(0).Type = shield.Type;

						y.Value = shield.Value;

						y.Weight = shield.Weight;
					}
					else
					{
						y.IsPlural = false;

						y.PluralType = Enums.PluralType.S;

						y.ArticleType = Enums.ArticleType.A;

						y.GetCategories(0).Field1 = 1;

						y.GetCategories(0).Field2 = 0;

						y.GetCategories(0).Type = Enums.ArtifactType.Wearable;

						y.Value = Constants.ShieldPrice;

						y.Weight = 15;
					}

					if (Globals.GameState.Wt + y.Weight <= 10 * Globals.Character.GetStats(Enums.Stat.Hardiness))
					{
						y.SetWornByCharacter();

						Globals.GameState.Wt += y.Weight;
					}
					else
					{
						y.SetInRoomUid(StartRoom);
					}
				});

				var rc = Globals.Database.AddArtifact(artifact);

				Debug.Assert(IsSuccess(rc));

				if (artifact.IsWornByCharacter())
				{
					Globals.GameState.Sh = artifact.Uid;

					Debug.Assert(Globals.GameState.Sh > 0);
				}
			}

			return (a2 + x) + (a2 >= 3 ? 2 : 0);
		}

		protected virtual void PrintTooManyWeapons()
		{
			Globals.Out.Print("As you enter the Main Hall, Lord William Missilefire approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		protected virtual void PrintDeliverGoods()
		{
			Globals.Out.Print("You deliver your goods to Sam Slicker, the local buyer for such things.  He examines your items and pays you what they are worth.");
		}

		protected virtual void PrintGoodsPayment(bool goodsExist, long payment)
		{
			Globals.Out.Print("{0}He pays you {1} gold piece{2} total.", goodsExist ? Environment.NewLine : "", payment, payment != 1 ? "s" : "");
		}

		protected virtual void SetScaledHardiness(IMonster monster, long damageFactor)
		{
			Debug.Assert(monster != null);

			Debug.Assert(damageFactor > 0);

			if (monster.Field1 > 0)
			{
				monster.Hardiness = damageFactor * monster.Field1;
			}
		}

		protected virtual void PlayerSpellCastBrainOverload(Enums.Spell s, Classes.ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Spell), s));

			Debug.Assert(spell != null);

			Globals.Out.Print("The strain of attempting to cast {0} overloads your brain and you forget it completely{1}.", spell.Name, Globals.IsRulesetVersion(5) ? "" : " for the rest of this adventure");

			Globals.GameState.SetSa(s, 0);

			if (Globals.IsRulesetVersion(5))
			{
				Globals.Character.SetSpellAbilities(s, 0);
			}
		}

		public virtual void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} {1} alive!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		public virtual long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2)
		{
			Debug.Assert(artifact1 != null && artifact2 != null);

			var ac1 = artifact1.GeneralWeapon;

			Debug.Assert(ac1 != null);

			var ac2 = artifact2.GeneralWeapon;

			Debug.Assert(ac2 != null);

			var result1 = ac1.Field3 * ac1.Field4;

			var result2 = ac2.Field3 * ac2.Field4;

			return result1 > result2 ? 1 : result1 < result2 ? -1 : 0;
		}

		public virtual long WeaponPowerCompare(long artifactUid1, long artifactUid2)
		{
			return WeaponPowerCompare(Globals.ADB[artifactUid1], Globals.ADB[artifactUid2]);
		}

		public virtual IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList)
		{
			IArtifact cw = null;

			Debug.Assert(artifactList != null);

			foreach (var artifact in artifactList)
			{
				var ac = artifact.GeneralWeapon;

				if (ac != null && (cw == null || WeaponPowerCompare(artifact, cw) > 0))
				{
					cw = artifact;

					Debug.Assert(cw.Uid > 0);
				}
			}

			return cw;
		}

		public virtual long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList)
		{
			Debug.Assert(artifactList != null);

			var cw = GetMostPowerfulWeapon(artifactList);

			return cw != null ? cw.Uid : 0;     // Note: -1 not returned!
		}

		public virtual void InitWtValueAndEnforceLimits()
		{
			Classes.IArtifactCategory ac;
			RetCode rc;
			long c, w;

			var artifacts = GetArtifactList(a => a.IsCarriedByCharacter() || a.IsWornByCharacter());

			foreach (var artifact in artifacts)
			{
				if (artifact.IsWornByCharacter())
				{
					ac = artifact.Wearable;

					if (ac != null && ac.Field1 > 0)
					{
						artifact.SetCarriedByCharacter();
					}
				}

				Debug.Assert(!artifact.IsUnmovable01());

				c = 0;

				w = artifact.Weight;

				ac = artifact.Container;

				if (ac != null)
				{
					rc = artifact.GetContainerInfo(ref c, ref w, true);

					Debug.Assert(IsSuccess(rc));
				}

				if (Globals.GameState.Wt + w <= 10 * Globals.Character.GetStats(Enums.Stat.Hardiness))
				{
					Globals.GameState.Wt += w;
				}
				else
				{
					artifact.SetInRoomUid(StartRoom);
				}
			}
		}

		public virtual void AddPoundCharsToArtifactNames()
		{
			var nameList = new List<IGameBase>();

			var artifactList = PoundCharPolicy == Enums.PoundCharPolicy.PlayerArtifactsOnly ? Globals.Database.ArtifactTable.Records.Where(a => a.IsCharOwned).ToList() :
									PoundCharPolicy == Enums.PoundCharPolicy.AllArtifacts ? Globals.Database.ArtifactTable.Records.ToList() :
									new List<IArtifact>();

			artifactList.Reverse();

			var i = 0;

			while (i < artifactList.Count && artifactList[i].IsCharOwned)
			{
				i++;
			}

			if (i > 0)
			{
				artifactList.Reverse(0, i);
			}

			if (artifactList.Count > i)
			{
				artifactList.Reverse(i, artifactList.Count - i);
			}

			nameList.AddRange(artifactList);

			AddPoundCharsToRecordNames(nameList);
		}

		public virtual void AddMissingDescs()
		{
			var monsters = GetMonsterList(m => string.IsNullOrWhiteSpace(m.Desc) || string.Equals(m.Desc, "NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var monster in monsters)
			{
				monster.Desc = string.Format("{0} {1}.", monster.EvalPlural("This is", "These are"), monster.GetDecoratedName02(false, true, false, false, Globals.Buf));
			}

			var artifacts = GetArtifactList(a => string.IsNullOrWhiteSpace(a.Desc) || string.Equals(a.Desc, "NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var artifact in artifacts)
			{
				artifact.Desc = string.Format("{0} {1}.", artifact.EvalPlural("This is", "These are"), artifact.GetDecoratedName02(false, true, false, false, Globals.Buf));
			}
		}

		public virtual void InitSaArray()
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				var i = (long)sv;

				Globals.GameState.SetSa(i, Globals.Character.GetSpellAbilities(i));
			}
		}

		public virtual void CreateCommands()
		{
			var commands = Globals.ClassMappingsDictionary.Keys.Where(x => x.GetInterfaces().Contains(typeof(ICommand)));

			foreach (var command in commands)
			{
				if (Globals.Module.NumDirs == 10 || !(command.IsSameOrSubclassOf(typeof(INeCommand)) || command.IsSameOrSubclassOf(typeof(INwCommand)) || command.IsSameOrSubclassOf(typeof(ISeCommand)) || command.IsSameOrSubclassOf(typeof(ISwCommand))))
				{
					Globals.CommandList.Add(Globals.CreateInstance<ICommand>(command));
				}
			}

			Globals.CommandList = Globals.CommandList.OrderBy(x => x.SortOrder).ToList();
		}

		public virtual void InitArtifacts()
		{
			var artifacts = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifacts)
			{
				var rc = artifact.SyncArtifactCategories();

				Debug.Assert(IsSuccess(rc));

				TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
			}
		}

		public virtual void InitMonsters()
		{
			if (UseMonsterScaledHardinessValues)
			{
				InitMonsterScaledHardinessValues();
			}

			var monsters = Globals.Database.MonsterTable.Records.ToList();

			foreach (var monster in monsters)
			{
				monster.InitGroupCount = monster.GroupCount;

				if (EnforceMonsterWeightLimits && !monster.IsCharacterMonster())
				{
					var rc = monster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}

				if (monster.Weapon > 0)
				{
					var artifact = Globals.ADB[monster.Weapon];

					if (artifact != null)
					{
						artifact.AddStateDesc(artifact.GetReadyWeaponDesc());
					}
				}

				TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
			}
		}

		public virtual void InitMonsterScaledHardinessValues()
		{
			var maxDamage = ScaledHardinessUnarmedMaxDamage;

			var monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(monster != null);

			if (monster.Weapon > 0)       // will always be most powerful weapon
			{
				var artifact = Globals.ADB[monster.Weapon];

				Debug.Assert(artifact != null);

				var ac = artifact.GeneralWeapon;

				Debug.Assert(ac != null);

				maxDamage = ac.Field3 * ac.Field4;
			}

			var damageFactor = (long)Math.Round((double)maxDamage / ScaledHardinessMaxDamageDivisor);

			if (damageFactor < 1)
			{
				damageFactor = 1;
			}

			var monsters = Globals.Database.MonsterTable.Records.ToList();

			foreach (var m in monsters)
			{
				SetScaledHardiness(m, damageFactor);
			}
		}

		public virtual IArtifact ConvertWeaponToArtifact(Classes.ICharacterArtifact weapon)
		{
			Debug.Assert(weapon != null);

			var artifact = Globals.CreateInstance<IArtifact>(x =>
			{
				x.SetArtifactCategoryCount(1);

				x.Uid = Globals.Database.GetArtifactUid();

				x.Name = weapon.Name.Trim().TrimEnd('#');

				Debug.Assert(!string.IsNullOrWhiteSpace(x.Name));

				x.Desc = !string.IsNullOrWhiteSpace(weapon.Desc) ? Globals.CloneInstance(weapon.Desc) : null;

				x.Seen = true;

				x.IsCharOwned = true;

				x.IsListed = true;

				x.IsPlural = weapon.IsPlural;

				x.PluralType = weapon.PluralType;

				x.ArticleType = weapon.ArticleType;

				x.GetCategories(0).Field1 = weapon.Field1;

				x.GetCategories(0).Field2 = weapon.Field2;

				x.GetCategories(0).Field3 = weapon.Field3;

				x.GetCategories(0).Field4 = weapon.Field4;

				x.GetCategories(0).Field5 = weapon.Field5;

				if (weapon.Type != 0)
				{
					x.GetCategories(0).Type = weapon.Type;

					x.Value = weapon.Value;

					x.Weight = weapon.Weight;
				}
				else
				{
					var d = weapon.Field3 * weapon.Field4;

					x.GetCategories(0).Type = (weapon.Field1 >= 15 || d >= 25) ? Enums.ArtifactType.MagicWeapon : Enums.ArtifactType.Weapon;

					var imw = false;

					x.Value = (long)GetWeaponPriceOrValue(weapon, false, ref imw);

					x.Weight = 15;
				}

				if (Globals.GameState.Wt + x.Weight <= 10 * Globals.Character.GetStats(Enums.Stat.Hardiness))
				{
					x.SetCarriedByCharacter();

					Globals.GameState.Wt += x.Weight;
				}
				else
				{
					x.SetInRoomUid(StartRoom);
				}
			});

			var rc = Globals.Database.AddArtifact(artifact);

			Debug.Assert(IsSuccess(rc));

			return artifact;
		}

		public virtual Classes.ICharacterArtifact ConvertArtifactToWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GeneralWeapon;

			Debug.Assert(ac != null);

			var weapon = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
			{
				x.Name = artifact.Name.Trim().TrimEnd('#');

				Debug.Assert(!string.IsNullOrWhiteSpace(x.Name));

				if (!string.IsNullOrWhiteSpace(artifact.Desc))
				{
					Globals.Buf.Clear();

					var rc = ResolveUidMacros(artifact.Desc, Globals.Buf, true, true);

					Debug.Assert(IsSuccess(rc));

					if (Globals.Buf.Length <= Constants.CharArtDescLen)
					{
						x.Desc = Globals.CloneInstance(Globals.Buf.ToString());
					}
				}

				x.IsPlural = artifact.IsPlural;

				x.PluralType = artifact.PluralType;

				x.ArticleType = artifact.ArticleType;

				x.Value = artifact.Value;

				x.Weight = artifact.Weight;

				x.Type = ac.Type;

				x.Field1 = ac.Field1;

				x.Field2 = ac.Field2;

				x.Field3 = ac.Field3;

				x.Field4 = ac.Field4;

				x.Field5 = ac.Field5;
			});

			return weapon;
		}

		public virtual IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false)
		{
			Debug.Assert(artifact != null);

			var monster = Globals.CreateInstance<IMonster>(x =>
			{
				x.Uid = Globals.Database.GetMonsterUid();

				x.IsUidRecycled = true;

				x.Name = Globals.CloneInstance(artifact.Name);

				x.Seen = artifact.Seen;

				x.ArticleType = artifact.ArticleType;

				x.StateDesc = Globals.CloneInstance(artifact.StateDesc);

				x.Desc = Globals.CloneInstance(artifact.Desc);

				x.IsListed = artifact.IsListed;

				x.PluralType = artifact.PluralType;

				x.Hardiness = 16;

				x.Agility = 15;

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.Courage = 100;

				x.Location = artifact.Location;

				x.CombatCode = Enums.CombatCode.Weapons;

				x.NwDice = 1;

				x.NwSides = 4;

				x.Friendliness = Enums.Friendliness.Friend;

				x.Gender = Enums.Gender.Male;

				x.OrigGroupCount = 1;

				x.OrigFriendliness = (Enums.Friendliness)200;
			});

			if (initialize != null)
			{
				initialize(monster);
			}

			if (addToDatabase)
			{
				var rc = Globals.Database.AddMonster(monster);

				Debug.Assert(IsSuccess(rc));
			}

			return monster;
		}

		public virtual IMonster ConvertCharacterToMonster()
		{
			var monster = Globals.CreateInstance<IMonster>(x =>
			{
				x.Uid = Globals.Database.GetMonsterUid();

				x.Name = Globals.Character.Name.Trim();

				x.Desc = string.Format("You are the {0} {1}.", Globals.Character.EvalGender("mighty", "fair", "androgynous"), Globals.Character.Name);

				x.Hardiness = Globals.Character.GetStats(Enums.Stat.Hardiness);

				x.Agility = Globals.Character.GetStats(Enums.Stat.Agility);

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.OrigGroupCount = 1;

				x.Armor = ConvertArmorToArtifacts();

				x.Weapon = ConvertWeaponsToArtifacts();

				x.Friendliness = Enums.Friendliness.Friend;

				x.OrigFriendliness = (Enums.Friendliness)200;

				x.Gender = Globals.Character.Gender;
			});

			var rc = Globals.Database.AddMonster(monster);

			Debug.Assert(IsSuccess(rc));

			return monster;
		}

		public virtual void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList)
		{
			Debug.Assert(monster != null && weaponList != null);

			ResetMonsterStats(monster);

			Globals.Character.Name = monster.Name.Trim();

			Globals.Character.SetStats(Enums.Stat.Hardiness, monster.Hardiness);

			Globals.Character.SetStats(Enums.Stat.Agility, monster.Agility);

			Globals.Character.Gender = monster.Gender;

			for (var i = 0; i < Globals.Character.Weapons.Length; i++)
			{
				Globals.Character.SetWeapons(i, (i < weaponList.Count ? ConvertArtifactToWeapon(weaponList[i]) : Globals.CreateInstance<Classes.ICharacterArtifact>()));

				Globals.Character.GetWeapons(i).Parent = Globals.Character;
			}

			Globals.Character.AddPoundCharsToWeaponNames();

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		public virtual void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null);

			if (Globals.GameState.Speed > 0)
			{
				monster.Agility /= 2;
			}

			Globals.GameState.Speed = 0;
		}

		public virtual void SetArmorClass()
		{
			var artUids = new long[] { Globals.GameState.Ar, Globals.GameState.Sh };

			Globals.Character.ArmorClass = Enums.Armor.SkinClothes;

			Globals.Character.Armor = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
			{
				x.Parent = Globals.Character;
			});

			Globals.Character.Shield = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
			{
				x.Parent = Globals.Character;
			});

			foreach (var artUid in artUids)
			{
				if (artUid > 0)
				{
					var artifact = Globals.ADB[artUid];

					Debug.Assert(artifact != null);

					var ac = artifact.Wearable;

					Debug.Assert(ac != null);

					Globals.Character.ArmorClass += ac.Field1;

					var ca = (artUid == Globals.GameState.Ar) ? Globals.Character.Armor : Globals.Character.Shield;

					ca.Name = artifact.Name.Trim().TrimEnd('#');

					Debug.Assert(!string.IsNullOrWhiteSpace(ca.Name));

					if (!string.IsNullOrWhiteSpace(artifact.Desc))
					{
						Globals.Buf.Clear();

						var rc = ResolveUidMacros(artifact.Desc, Globals.Buf, true, true);

						Debug.Assert(IsSuccess(rc));

						if (Globals.Buf.Length <= Constants.CharArtDescLen)
						{
							ca.Desc = Globals.CloneInstance(Globals.Buf.ToString());
						}
					}

					ca.IsPlural = artifact.IsPlural;

					ca.PluralType = artifact.PluralType;

					ca.ArticleType = artifact.ArticleType;

					ca.Value = artifact.Value;

					ca.Weight = artifact.Weight;

					ca.Type = ac.Type;

					ca.Field1 = ac.Field1;

					ca.Field2 = ac.Field2;

					ca.Field3 = ac.Field3;

					ca.Field4 = ac.Field4;

					ca.Field5 = ac.Field5;

					artifact.SetInLimbo();
				}
			}

			// Globals.GameState.Ar = 0;

			// Globals.GameState.Sh = 0;
		}

		public virtual void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			long c;

			Debug.Assert(weaponList != null);

			weaponList.Clear();

			var artifacts = GetArtifactList(a => a.IsWornByCharacter());

			foreach (var artifact in artifacts)
			{
				artifact.SetCarriedByCharacter();
			}

			do
			{
				c = 0;

				artifacts = GetArtifactList(a => a.IsCarriedByCharacter());

				foreach (var artifact in artifacts)
				{
					var ac = artifact.Container;

					if (ac != null)
					{
						var artifacts01 = GetArtifactList(a => a.IsCarriedByContainer(artifact));

						foreach (var artifact01 in artifacts01)
						{
							if (artifact01.Seen == true || ac.Field2 == 1)
							{
								artifact01.SetCarriedByCharacter();

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);

			artifacts = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifacts)
			{
				artifact.Seen = false;

				if (artifact.IsCarriedByCharacter())
				{
					var ac = artifact.GeneralWeapon;

					if (ac != null && ac == artifact.GetCategories(0) && artifact.IsReadyableByCharacter())
					{
						weaponList.Add(artifact);

						artifact.SetInLimbo();
					}
				}
			}

			if (weaponList.Count > Globals.Character.Weapons.Length)
			{
				Globals.Out.Print("{0}", Globals.LineSep);
			}
		}

		public virtual void SellExcessWeapons(IList<IArtifact> weaponList)
		{
			Debug.Assert(weaponList != null);

			if (weaponList.Count > Globals.Character.Weapons.Length)
			{
				PrintTooManyWeapons();

				while (weaponList.Count > Globals.Character.Weapons.Length)
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Buf.Clear();

					var rc = ListRecords(weaponList.Cast<IGameBase>().ToList(), true, true, Globals.Buf);

					Debug.Assert(IsSuccess(rc));

					Globals.Out.WriteLine("{0}Your weapons are:{0}{1}", Environment.NewLine, Globals.Buf);

					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Write("{0}Enter the number of a weapon to sell: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize01, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharDigit, null);

					Debug.Assert(IsSuccess(rc));

					Globals.Thread.Sleep(150);

					var m = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (m >= 1 && m <= weaponList.Count)
					{
						weaponList[(int)m - 1].SetCarriedByCharacter();

						weaponList.RemoveAt((int)m - 1);
					}
				}
			}
		}

		public virtual void SellInventoryToMerchant(bool sellInventory = true)
		{
			var c = 0L;

			var w = 0L;

			PrintDeliverGoods();

			if (sellInventory)
			{
				var c2 = Globals.Character.GetMerchantAdjustedCharisma();

				var rtio = GetMerchantRtio(c2);

				var artifacts = GetArtifactList(a => a.IsCarriedByCharacter());

				foreach (var artifact in artifacts)
				{
					var m = artifact.Gold != null ? artifact.Value : GetMerchantBidPrice(artifact.Value, rtio);

					if (m < 0)
					{
						m = 0;
					}

					if (m > 0)
					{
						Globals.Buf01.SetFormat("{0} gold piece{1}", m, m > 1 ? "s" : "");
					}
					else
					{
						Globals.Buf01.SetFormat("nothing");
					}

					var ac = artifact.Drinkable;

					Globals.Out.Write("{0}{1}{2} {3} worth {4}.",
						Environment.NewLine,
						artifact.GetDecoratedName03(true, false, false, false, Globals.Buf),
						ac != null && ac.Field2 < 1 && !artifact.Name.Contains("empty", StringComparison.OrdinalIgnoreCase) ? " (empty)" : "",
						artifact.EvalPlural("is", "are"),
						Globals.Buf01);

					w = w + m;

					c = 1;
				}

				Globals.Character.HeldGold += w;
			}

			PrintGoodsPayment(c == 1, w);

			Globals.In.KeyPress(Globals.Buf);
		}

		public virtual void DeadMenu(IMonster monster, bool printLineSep, ref bool restoreGame)
		{
			Debug.Assert(monster != null);

			restoreGame = false;

			ResetMonsterStats(monster);

			if (printLineSep)
			{
				Globals.Out.Print("{0}", Globals.LineSep);
			}

			Globals.Out.Print("You have perished.  Now what?");

			Globals.Out.Write("{0} 1. Restore a saved game", Environment.NewLine);

			Globals.Out.Write("{0} 2. Start over (saved games will be deleted)", Environment.NewLine);

			Globals.Out.Print(" 3. Give up, accept death");

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Your choice: ", Environment.NewLine);

			Globals.Buf.Clear();

			var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsChar1To3, null);

			Debug.Assert(IsSuccess(rc));

			var i = Convert.ToInt64(Globals.Buf.Trim().ToString());

			if (i == 3)
			{
				Globals.ExitType = Enums.ExitType.GoToMainHall;

				Globals.MainLoop.ShouldShutdown = false;
			}
			else if (i == 2)
			{
				Globals.ExitType = Enums.ExitType.StartOver;

				Globals.MainLoop.ShouldShutdown = false;
			}
			else
			{
				restoreGame = true;
			}
		}

		public virtual void LightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

			Debug.Assert(IsSuccess(rc));

			Globals.GameState.Ls = 0;

			Globals.GameState.Lt = (long)Globals.RDB[Globals.GameState.Ro].LightLvl;

			Globals.Out.Print("{0} goes out.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
		}

		public virtual void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			if (monster.Friendliness > Enums.Friendliness.Enemy)
			{
				var room = Globals.RDB[Globals.GameState.Ro];

				Debug.Assert(room != null);

				monster.Friendliness--;

				monster.OrigFriendliness -= 100;

				monster.OrigFriendliness = (Enums.Friendliness)((long)monster.OrigFriendliness / 2);

				monster.OrigFriendliness += 100;

				CheckEnemies();

				if (monster.IsInRoom(room) && monster.Friendliness == Enums.Friendliness.Enemy)
				{
					Globals.Out.Write("{0}{1} get{2} angry!{3}",
						Environment.NewLine,
						monster.GetDecoratedName03(true, true, false, false, Globals.Buf),
						monster.EvalPlural("s", ""),
						printFinalNewLine ? Environment.NewLine : "");
				}
			}
		}

		public virtual void MonsterSmiles(IMonster monster)
		{
			Debug.Assert(monster != null);

			if (Globals.IsRulesetVersion(5) && monster.Friendliness == Enums.Friendliness.Friend)
			{
				Globals.Out.Write("{0}{1} {2}{3} back.",
					Environment.NewLine,
					monster.GetDecoratedName03(true, true, false, false, Globals.Buf),
					monster.EvalFriendliness("growl", "ignore", "smile"),
					monster.EvalPlural("s", ""));
			}
			else
			{
				Globals.Out.Write("{0}{1} {2}{3} {4}you.",
					Environment.NewLine,
					monster.GetDecoratedName03(true, true, false, false, Globals.Buf),
					monster.EvalFriendliness("growl", "ignore", "smile"),
					monster.EvalPlural("s", ""),
					monster.Friendliness != Enums.Friendliness.Neutral ? "at " : "");
			}
		}

		public virtual void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			RetCode rc;

			// OfMonster may be null or non-null

			Debug.Assert(DfMonster != null && !DfMonster.IsCharacterMonster());

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			if (DfMonster.GroupCount > 1)
			{
				if (DfMonster.Weapon > 0)
				{
					var weapon = GetNthArtifact(DfMonster.GetCarriedList(), DfMonster.GroupCount - 1, a => a.IsReadyableByMonster(DfMonster) && a.Uid != DfMonster.Weapon);

					if (weapon != null)
					{
						weapon.SetInRoom(room);
					}
				}

				DfMonster.GroupCount--;

				DfMonster.DmgTaken = 0;

				if (EnforceMonsterWeightLimits)
				{
					rc = DfMonster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}
			}
			else
			{
				if (DfMonster.Weapon > 0)
				{
					var weapon = Globals.ADB[DfMonster.Weapon];

					Debug.Assert(weapon != null);

					rc = weapon.RemoveStateDesc(weapon.GetReadyWeaponDesc());

					Debug.Assert(IsSuccess(rc));

					DfMonster.Weapon = -1;
				}

				if (Globals.IsRulesetVersion(5))
				{
					Globals.GameState.ModDTTL(DfMonster.Friendliness, -DfMonster.DmgTaken);
				}

				DfMonster.SetInLimbo();

				DfMonster.GroupCount = DfMonster.OrigGroupCount;

				// DfMonster.Friendliness = DfMonster.OrigFriendliness;

				DfMonster.DmgTaken = 0;

				var artifactList = GetArtifactList(a => a.IsCarriedByMonster(DfMonster) || a.IsWornByMonster(DfMonster));

				foreach (var artifact in artifactList)
				{
					artifact.SetInRoom(room);
				}

				ProcessMonsterDeathEvents(DfMonster);

				if (DfMonster.DeadBody > 0)
				{
					var deadBody = Globals.ADB[DfMonster.DeadBody];

					Debug.Assert(deadBody != null);

					if (!deadBody.IsCharOwned)
					{
						deadBody.SetInRoom(room);
					}
				}
			}

			Globals.GameState.ModNBTL(DfMonster.Friendliness, -DfMonster.Hardiness);
		}

		public virtual void ProcessMonsterDeathEvents(IMonster monster)
		{
			Debug.Assert(monster != null && !monster.IsCharacterMonster());

			// --> Add effects of monster's death here
		}

		public virtual void RevealDisguisedMonster(IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			var ac = artifact.DisguisedMonster;

			Debug.Assert(ac != null);

			PrintMonsterAlive(artifact);

			if (ac.Field2 > 0)
			{
				for (var i = 1; i <= ac.Field3; i++)
				{
					var effect = Globals.EDB[ac.Field2 + i - 1];

					if (effect != null)
					{
						Globals.Buf.Clear();

						rc = effect.BuildPrintedFullDesc(Globals.Buf);
					}
					else
					{
						Globals.Buf.SetPrint("{0}", "???");

						rc = RetCode.Success;
					}

					Debug.Assert(IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);
				}
			}

			artifact.SetInLimbo();

			var monster = Globals.MDB[ac.Field1];

			Debug.Assert(monster != null);

			monster.SetInRoomUid(Globals.GameState.Ro);

			CheckEnemies();
		}

		public virtual void RevealEmbeddedArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			if (artifact.IsEmbeddedInRoom(room))
			{
				artifact.SetInRoom(room);

				var ac = artifact.DoorGate;

				if (ac != null)
				{
					ac.Field4 = 0;
				}

				if (!artifact.Seen)
				{
					Globals.Buf.Clear();

					var rc = artifact.BuildPrintedFullDesc(Globals.Buf, false);

					Debug.Assert(IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);

					artifact.Seen = true;
				}
			}
		}

		public virtual void RemoveWeight(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.IsCarriedByCharacter() || artifact.IsWornByCharacter())
			{
				var count = 0L;

				var weight = artifact.Weight;

				var rc = artifact.GetContainerInfo(ref count, ref weight, true);

				Debug.Assert(IsSuccess(rc));

				Globals.GameState.Wt -= weight;

				Debug.Assert(Globals.GameState.Wt >= 0);
			}
		}

		public virtual IArtifact GetBlockedDirectionArtifact(long ro, long r2, Enums.Direction dir)
		{
			return null;
		}

		public virtual void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			found = false;

			roomUid = 0;

			if (!artifact.IsCharOwned)
			{
				var ac = artifact.DoorGate;

				if (ac != null)
				{
					if (artifact.IsInRoom(room) || artifact.IsEmbeddedInRoom(room))
					{
						found = true;
					}
				}

				if (found)
				{
					if (artifact.Seen && room.IsLit())
					{
						ac.Field4 = 0;
					}

					if (ac.Field4 != 0)
					{
						found = false;
					}
				}

				if (found && ac.IsOpen())
				{
					roomUid = ac.Field1;
				}
			}
		}

		public virtual void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			numExits = 0;

			var directionValues = EnumUtil.GetValues<Enums.Direction>();

			for (var i = 0; i < Globals.Module.NumDirs; i++)
			{
				var dv = directionValues[i];

				var found = false;

				var roomUid = 0L;

				var artUid = room.GetDirectionDoorUid(dv);

				if (artUid > 0)
				{
					var artifact = Globals.ADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDirs(dv);
				}

				if (roomUid != 0 && (!monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, dv) != null))
				{
					roomUid = 0;
				}

				if (IsValidRoomUid01(roomUid) && (monster.IsCharacterMonster() || (roomUid > 0 && Globals.RDB[roomUid] != null)))
				{
					numExits++;
				}
			}
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Enums.Direction direction, ref bool found, ref long roomUid)
		{
			long rl;

			Debug.Assert(room != null);

			Debug.Assert(room.Dirs.Count(x => x != 0 && x != room.Uid) > 0);

			Debug.Assert(monster != null);

			direction = 0;

			do
			{
				rl = RollDice(1, Globals.Module.NumDirs, 0);

				found = false;

				roomUid = 0;

				var artUid = room.GetDirectionDoorUid((Enums.Direction)rl);

				if (artUid > 0)
				{
					var artifact = Globals.ADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDirs(rl);
				}

				if (roomUid != 0 && (!monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, (Enums.Direction)rl) != null))
				{
					roomUid = 0;
				}
			}
			while (roomUid == 0 || roomUid == room.Uid || IsValidRoomDirectionDoorUid01(roomUid) || (!monster.IsCharacterMonster() && (roomUid < 1 || Globals.RDB[roomUid] == null)));

			direction = (Enums.Direction)rl;
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Enums.Direction direction)
		{
			var found = false;

			var roomUid = 0L;

			GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
		}

		public virtual IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(numMonsters > 0);

			var monsterList = new List<IMonster>();

			var origList = GetMonsterList(whereClauseFuncs);

			if (numMonsters > origList.Count)
			{
				numMonsters = origList.Count;
			}

			while (numMonsters > 0)
			{
				var rl = (int)RollDice(1, origList.Count, 0);

				monsterList.Add(origList[rl - 1]);

				origList.RemoveAt(rl - 1);

				numMonsters--;
			}

			return monsterList;
		}

		public virtual IList<IArtifact> FilterArtifactList(IList<IArtifact> artifactList, string name)
		{
			Debug.Assert(artifactList != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var filteredArtifactList = artifactList.Where(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase)).ToList();

			if (filteredArtifactList.Count == 0)
			{
				filteredArtifactList = artifactList.Where(a => a.IsPlural && string.Equals(a.GetPluralName01(Globals.Buf), name, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			if (filteredArtifactList.Count == 0)
			{
				filteredArtifactList = artifactList.Where(a => a.Synonyms != null && a.Synonyms.FirstOrDefault(s => string.Equals(s, name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			if (filteredArtifactList.Count == 0)
			{
				filteredArtifactList = artifactList.Where(a => a.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) || a.Name.EndsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			if (filteredArtifactList.Count == 0)
			{
				filteredArtifactList = artifactList.Where(a => a.IsPlural && (a.GetPluralName01(Globals.Buf).StartsWith(name, StringComparison.OrdinalIgnoreCase) || a.GetPluralName01(Globals.Buf01).EndsWith(name, StringComparison.OrdinalIgnoreCase))).ToList();
			}

			if (filteredArtifactList.Count == 0)
			{
				filteredArtifactList = artifactList.Where(a => a.Synonyms != null && a.Synonyms.FirstOrDefault(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase) || s.EndsWith(name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			return filteredArtifactList;
		}

		public virtual IList<IMonster> FilterMonsterList(IList<IMonster> monsterList, string name)
		{
			Debug.Assert(monsterList != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var tokens = name.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			long i = -1;

			if (tokens.Length > 1)
			{
				i = GetNumberFromString(tokens[0]);

				if (i > 0)
				{
					name = name.Substring(tokens[0].Length + 1);
				}
				else
				{
					i = -1;
				}
			}

			var filteredMonsterList = monsterList.Where(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase)).ToList();

			if (filteredMonsterList.Count == 0)
			{
				filteredMonsterList = monsterList.Where(m => m.OrigGroupCount > 1 && string.Equals(m.GetPluralName01(Globals.Buf), name, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			if (filteredMonsterList.Count == 0)
			{
				filteredMonsterList = monsterList.Where(m => m.Synonyms != null && m.Synonyms.FirstOrDefault(s => string.Equals(s, name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			if (filteredMonsterList.Count == 0)
			{
				filteredMonsterList = monsterList.Where(m => m.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) || m.Name.EndsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			if (filteredMonsterList.Count == 0)
			{
				filteredMonsterList = monsterList.Where(m => m.OrigGroupCount > 1 && (m.GetPluralName01(Globals.Buf).StartsWith(name, StringComparison.OrdinalIgnoreCase) || m.GetPluralName01(Globals.Buf01).EndsWith(name, StringComparison.OrdinalIgnoreCase))).ToList();
			}

			if (filteredMonsterList.Count == 0)
			{
				filteredMonsterList = monsterList.Where(m => m.Synonyms != null && m.Synonyms.FirstOrDefault(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase) || s.EndsWith(name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			if (i > 0)
			{
				filteredMonsterList.RemoveAll(m => m.OrigGroupCount == 1 || m.OrigGroupCount < i);
			}

			return filteredMonsterList;
		}

		public virtual IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var filteredRecordList = recordList.Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)).ToList();

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(x =>
				{
					var result = false;

					var m = x as IMonster;

					if (m != null)
					{
						result = m.OrigGroupCount > 1;
					}
					else
					{
						var a = x as IArtifact;

						Debug.Assert(a != null);

						result = a.IsPlural;
					}

					if (result)
					{
						result = string.Equals(x.GetPluralName01(Globals.Buf), name, StringComparison.OrdinalIgnoreCase);
					}

					return result;
				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => string.Equals(s, name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(x => x.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) || x.Name.EndsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(x =>
				{
					var result = false;

					var m = x as IMonster;

					if (m != null)
					{
						result = m.OrigGroupCount > 1;
					}
					else
					{
						var a = x as IArtifact;

						Debug.Assert(a != null);

						result = a.IsPlural;
					}

					if (result)
					{
						result = x.GetPluralName01(Globals.Buf).StartsWith(name, StringComparison.OrdinalIgnoreCase) || x.GetPluralName01(Globals.Buf01).EndsWith(name, StringComparison.OrdinalIgnoreCase);
					}

					return result;
				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase) || s.EndsWith(name, StringComparison.OrdinalIgnoreCase)) != null).ToList();
			}

			return filteredRecordList;
		}

		public virtual IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var charMonster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(charMonster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList = GetMonsterList(m => m.Uid != monster.Uid && m.Uid != charMonster.Uid && m.IsInRoom(room));

			var artifactList = GetArtifactList(a => a.IsReadyableByMonster(monster) && (a.IsCarriedByMonster(monster) || (a.IsInRoom(room) && monsterList.FirstOrDefault(m => m.Weapon == -a.Uid - 1) == null && (charMonster.Weapon > 0 || !a.IsCharOwned || monster.Friendliness == Enums.Friendliness.Friend)))).OrderByDescending(a01 =>
			{
				if (monster.Weapon != -a01.Uid - 1)
				{
					var ac = a01.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field3 * ac.Field4;
				}
				else
				{
					return long.MaxValue;
				}
			}).ToList();

			// filter out two-handed weapons if monster wearing shield

			var shield = monster.GetWornList().FirstOrDefault(a =>
			{
				var ac = a.Wearable;

				Debug.Assert(ac != null);

				return ac.Field1 == 1;
			});

			if (shield != null)
			{
				artifactList = artifactList.Where(a =>
				{
					var ac = a.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field5 < 2;
					
				}).ToList();
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList =
					monster.Friendliness == Enums.Friendliness.Friend ? GetMonsterList(m => m.Friendliness == Enums.Friendliness.Enemy && m.IsInRoom(room)) :
					monster.Friendliness == Enums.Friendliness.Enemy ? GetMonsterList(m => m.Friendliness == Enums.Friendliness.Friend && m.IsInRoom(room)) :
					new List<IMonster>();

			return monsterList;
		}

		public virtual RetCode BuildCommandList(IList<ICommand> commands, Enums.CommandType cmdType, StringBuilder buf, ref bool newSeen)
		{
			StringBuilder buf02, buf03;
			RetCode rc;
			int i;

			if (commands == null || !Enum.IsDefined(typeof(Enums.CommandType), cmdType) || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf02 = new StringBuilder(Constants.BufSize);

			buf03 = new StringBuilder(Constants.BufSize);

			i = 0;

			foreach (var c in commands)
			{
				if (cmdType == Enums.CommandType.None || c.Type == cmdType)
				{
					i++;

					buf02.SetFormat("{0}{1}",
						c.GetPrintedVerb(),
						c.IsNew ? " (*)" : "");

					buf03.AppendFormat("{0,-15}{1}",
						buf02.ToString(),
						(i % 5) == 0 ? Environment.NewLine : "");

					if (c.IsNew)
					{
						newSeen = true;
					}
				}
			}

			buf.AppendFormat("{0}{1}{2}",
				buf03.Length > 0 ? Environment.NewLine : "",
				buf03.Length > 0 ? buf03.ToString() : "(None)",
				i == 0 || (i % 5) != 0 ? Environment.NewLine : "");

			Cleanup:

			return rc;
		}

		public virtual bool ResurrectDeadBodies(params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => (a.IsCarriedByCharacter() || a.IsInRoomUid(Globals.GameState.Ro)) && a.DeadBody != null
				};
			}

			var found = false;

			var artifacts = GetArtifactList(whereClauseFuncs);
			
			foreach (var artifact in artifacts)
			{
				var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.DeadBody == artifact.Uid);

				if (monster != null && monster.OrigGroupCount == 1)
				{
					monster.SetInRoomUid(Globals.GameState.Ro);

					monster.DmgTaken = 0;

					RemoveWeight(artifact);

					artifact.SetInLimbo();

					Globals.Out.Print("{0} {1}", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), Globals.IsRulesetVersion(5) ? "comes alive!" : "comes to life!");

					found = true;
				}
			}
			
			if (found)
			{
				CheckEnemies();
			}

			return found;
		}

		public virtual bool MakeArtifactsVanish(params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => a.IsInRoomUid(Globals.GameState.Ro) && !a.IsUnmovable()
				};
			}

			var artifacts = GetArtifactList(whereClauseFuncs);

			foreach (var artifact in artifacts)
			{
				RemoveWeight(artifact);

				artifact.SetInLimbo();

				Globals.Out.Print("{0} vanishes!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}

			return artifacts.Count > 0;
		}

		public virtual bool CheckNBTLHostility(IMonster monster)
		{
			Debug.Assert(monster != null);

			return monster.Friendliness != Enums.Friendliness.Neutral && Globals.GameState.GetNBTL(monster.Friendliness == Enums.Friendliness.Friend ? Enums.Friendliness.Enemy : Enums.Friendliness.Friend) > 0;
		}

		public virtual bool CheckCourage(IMonster monster)
		{
			bool result;

			Debug.Assert(monster != null);

			if (Globals.IsRulesetVersion(5))
			{
				var rl = (long)Math.Round((double)Globals.GameState.GetDTTL(monster.Friendliness) / (double)Globals.GameState.GetNBTL(monster.Friendliness) * 100 + RollDice(1, 41, -21));

				result = rl <= monster.Courage;
			}
			else
			{
				var s = (monster.DmgTaken > 0 || monster.OrigGroupCount > monster.GroupCount ? 1 : 0) + (monster.DmgTaken + 4 >= monster.Hardiness ? 1 : 0);

				var rl = RollDice(1, 100, s * 5);

				result = rl <= monster.Courage;           // monster.Courage >= 100 ||
			}

			return result;
		}

		public virtual bool CheckPlayerSpellCast(Enums.Spell spellValue, bool shouldAllowSkillGains)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Spell), spellValue));

			var result = false;

			var rl = 0L;

			var s = spellValue;

			var spell = GetSpells(spellValue);

			Debug.Assert(spell != null);

			if (Globals.GameState.GetSa(s) > 0 && Globals.Character.GetSpellAbilities(s) > 0)
			{
				rl = RollDice(1, 100, 0);
			}

			if (rl == 100)
			{
				PlayerSpellCastBrainOverload(s, spell);

				goto Cleanup;
			}

			if (rl > 0 && rl < 95 && (rl < 5 || rl <= Globals.GameState.GetSa(s)))
			{
				result = true;

				Globals.GameState.SetSa(s, (long)((double)Globals.GameState.GetSa(s) * .5 + 1));

				if (shouldAllowSkillGains)
				{
					rl = RollDice(1, 100, 0);

					rl += Globals.Character.GetIntellectBonusPct();

					if (rl > Globals.Character.GetSpellAbilities(s))
					{
						if (!Globals.IsRulesetVersion(5))
						{
							Globals.Out.Print("Your ability to cast {0} just increased!", spell.Name);
						}

						Globals.Character.ModSpellAbilities(s, 2);

						if (Globals.Character.GetSpellAbilities(s) > spell.MaxValue)
						{
							Globals.Character.SetSpellAbilities(s, spell.MaxValue);
						}
					}
				}
			}
			else
			{
				Globals.Out.Print("Nothing happens.");

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		public virtual void CheckPlayerSkillGains(Classes.IArtifactCategory ac, long af)
		{
			Debug.Assert(ac != null && ac.IsWeapon01());

			var s = (Enums.Weapon)ac.Field2;

			var rl = RollDice(1, 100, 0);

			if (rl > 75)
			{
				rl = RollDice(1, 100, 0);

				rl += Globals.Character.GetIntellectBonusPct();

				if (rl > Globals.Character.GetWeaponAbilities(s))
				{
					var weapon = GetWeapons(s);

					Debug.Assert(weapon != null);

					if (!Globals.IsRulesetVersion(5))
					{
						Globals.Out.Print("Your {0} ability just increased!", weapon.Name);
					}

					Globals.Character.ModWeaponAbilities(s, 2);

					if (Globals.Character.GetWeaponAbilities(s) > weapon.MaxValue)
					{
						Globals.Character.SetWeaponAbilities(s, weapon.MaxValue);
					}
				}

				var x = Math.Abs(af);

				if (x > 0)
				{
					rl = RollDice(1, x, 0);

					rl += (long)Math.Round(((double)x / 100.0) * (double)Globals.Character.GetIntellectBonusPct());

					if (rl > Globals.Character.ArmorExpertise)
					{
						if (!Globals.IsRulesetVersion(5))
						{
							Globals.Out.Print("Your armor expertise just increased!");
						}

						Globals.Character.ArmorExpertise += 2;

						if (Globals.Character.ArmorExpertise <= 66 && Globals.Character.ArmorExpertise > x)
						{
							Globals.Character.ArmorExpertise = x;
						}

						if (Globals.Character.ArmorExpertise > 79)
						{
							Globals.Character.ArmorExpertise = 79;
						}
					}
				}
			}
		}

		public virtual void CheckPlayerCommand(ICommand command, bool afterFinishParsing)
		{
			Debug.Assert(command != null);

			// do nothing
		}

		public virtual void CheckToExtinguishLightSource()
		{
			Debug.Assert(Globals.GameState.Ls > 0);

			var artifact = Globals.ADB[Globals.GameState.Ls];

			Debug.Assert(artifact != null);

			var ac = artifact.LightSource;

			Debug.Assert(ac != null);

			if (ac.Field1 != -1)
			{
				Globals.Out.Write("{0}It's not dark here.  Extinguish {1} (Y/N): ", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

				Globals.Buf.Clear();

				var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharYOrN, null);

				Debug.Assert(IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

					Debug.Assert(IsSuccess(rc));

					Globals.GameState.Ls = 0;
				}
			}
		}

		public virtual void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var monsterList = GetMonsterList(m => m.IsInRoom(oldRoom)).ToList();

			foreach (var m in monsterList)
			{
				m.SetInRoom(newRoom);
			}

			CheckEnemies();

			var artifactList = GetArtifactList(a => a.IsInRoom(oldRoom)).ToList();

			foreach (var a in artifactList)
			{
				a.SetInRoom(newRoom);
			}

			if (includeEmbedded)
			{
				artifactList = GetArtifactList(a => a.IsEmbeddedInRoom(oldRoom)).ToList();

				foreach (var a in artifactList)
				{
					a.SetEmbeddedInRoom(newRoom);
				}
			}
		}

		// Note: this method should only be used when oldRoom and newRoom are logically equivalent

		public virtual void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var gameState = GetGameState();

			Debug.Assert(gameState != null);

			TransportRoomContentsBetweenRooms(oldRoom, newRoom);

			if (effect != null)
			{
				PrintEffectDesc(effect);
			}

			gameState.Ro = newRoom.Uid;

			gameState.R2 = gameState.Ro;
		}

		public virtual void PrintMacroReplacedPagedString(string str, StringBuilder buf)
		{
			Debug.Assert(str != null && buf != null);

			buf.Clear();

			var rc = ResolveUidMacros(str, buf, true, true);

			Debug.Assert(IsSuccess(rc));

			Globals.Out.WriteLine();

			var pages = buf.ToString().Split(new string[] { Constants.PageSep }, StringSplitOptions.RemoveEmptyEntries);

			for (var i = 0; i < pages.Length; i++)
			{
				if (i > 0)
				{
					Globals.Out.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);
				}

				Globals.Out.Write("{0}", pages[i]);

				if (i < pages.Length - 1)
				{
					Globals.Out.WriteLine();

					Globals.In.KeyPress(buf);
				}
			}

			Globals.Out.WriteLine();
		}

		public virtual void CreateArtifactSynonyms(long artifactUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var artifact = Globals.ADB[artifactUid];

			Debug.Assert(artifact != null);

			artifact.Synonyms = Globals.CloneInstance(synonyms);
		}

		public virtual void CreateMonsterSynonyms(long monsterUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var monster = Globals.MDB[monsterUid];

			Debug.Assert(monster != null);

			monster.Synonyms = Globals.CloneInstance(synonyms);
		}

		public virtual void GetOddsToHit(IMonster ofMonster, IMonster dfMonster, Classes.IArtifactCategory ac, long af, ref long oddsToHit)
		{
			Debug.Assert(ofMonster != null);

			Debug.Assert(dfMonster != null);

			Debug.Assert(ac == null || (ac != null && ac.IsWeapon01()));

			var x = ofMonster.Agility;

			var f = dfMonster.Agility;

			var a = ofMonster.Armor;

			var d = dfMonster.Armor;

			if (a > 8)
			{
				a = 8;
			}

			if (d > 8)
			{
				d = 8;
			}

			if (x > 30)
			{
				x = 30;
			}

			if (f > 30)
			{
				f = 30;
			}

			var odds = 50 + 2 * (x - f - a + d);

			if (ac != null)
			{
				d = ac.Field1;

				if (d > 50)
				{
					d = 50;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 2.0));
			}

			if (ofMonster.IsCharacterMonster())
			{
				Debug.Assert(ac != null);

				odds += ((af + Globals.Character.ArmorExpertise) * (-af > Globals.Character.ArmorExpertise ? 1 : 0));

				d = Globals.Character.GetWeaponAbilities(ac.Field2);

				if (d > 122)
				{
					d = 122;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 4.0));
			}

			oddsToHit = odds;
		}

		public virtual void CreateInitialState(bool printLineSep)
		{
			if (Globals.GameState.Die != 1)
			{
				Globals.CurrState = Globals.CreateInstance<IAfterPlayerMoveState>();
			}
			else
			{
				Globals.CurrState = Globals.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = printLineSep;
				});
			}
		}

		public virtual void CheckEnemies()
		{
			Array.Clear(Globals.GameState.NBTL, 0, (int)Globals.GameState.NBTL.Length);

			Globals.GameState.SetNBTL(Enums.Friendliness.Friend, Globals.MDB[Globals.GameState.Cm].Hardiness);

			if (Globals.IsRulesetVersion(5))
			{
				Array.Clear(Globals.GameState.DTTL, 0, (int)Globals.GameState.DTTL.Length);

				Globals.GameState.SetDTTL(Enums.Friendliness.Friend, Globals.MDB[Globals.GameState.Cm].DmgTaken);
			}

			var monsters = GetMonsterList(m => !m.IsCharacterMonster() && m.Location == Globals.GameState.Ro);

			foreach (var monster in monsters)
			{
				monster.ResolveFriendlinessPct(Globals.Character);

				Globals.GameState.ModNBTL(monster.Friendliness, monster.Hardiness * monster.GroupCount);

				if (Globals.IsRulesetVersion(5))
				{
					Globals.GameState.ModDTTL(monster.Friendliness, monster.DmgTaken);
				}
			}
		}

		public virtual void MoveMonsters()
		{
			long rl = 0;

			var monsters = GetMonsterList(m => !m.IsCharacterMonster() && m.Seen && m.Location == Globals.GameState.R3);

			foreach (var monster in monsters)
			{
				monster.ResolveFriendlinessPct(Globals.Character);

				if (monster.CanMoveToRoomUid(Globals.GameState.Ro, false))
				{
					if (monster.Friendliness == Enums.Friendliness.Enemy)
					{
						rl = RollDice(1, 100, 0);

						if (rl <= monster.Courage)
						{
							monster.Location = Globals.GameState.Ro;
						}
					}
					else if (monster.Friendliness == Enums.Friendliness.Friend)
					{
						monster.Location = Globals.GameState.Ro;
					}
				}
			}
		}

		public virtual void RtProcessArgv(bool secondPass, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (string.Equals(Globals.Argv[i], "--workingDirectory", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (string.Equals(Globals.Argv[i], "--filePrefix", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (string.Equals(Globals.Argv[i], "--ignoreMutex", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (string.Equals(Globals.Argv[i], "--configFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && !secondPass)
					{
						Globals.ConfigFileName = Globals.Argv[i].Trim();
					}
				}
				else if (string.Equals(Globals.Argv[i], "--filesetFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtFilesetFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--characterFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtCharacterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--moduleFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtModuleFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--roomFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtRoomFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--artifactFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtArtifactFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--effectFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtEffectFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--monsterFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtMonsterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--hintFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtHintFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--gameStateFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-gsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtGameStateFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (string.Equals(Globals.Argv[i], "--deleteGameState", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-dgs", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						Globals.DeleteGameStateFromMainHall = true;
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Globals.Out.Print("{0}", Globals.LineSep);
					}

					Globals.Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}

		public Engine()
		{
			StartRoom = Constants.StartRoom;

			NumSaveSlots = Constants.NumSaveSlots;

			ScaledHardinessUnarmedMaxDamage = Constants.ScaledHardinessUnarmedMaxDamage;

			ScaledHardinessMaxDamageDivisor = Constants.ScaledHardinessMaxDamageDivisor;

			EnforceMonsterWeightLimits = true;

			PoundCharPolicy = Enums.PoundCharPolicy.AllArtifacts;
		}
	}
}
