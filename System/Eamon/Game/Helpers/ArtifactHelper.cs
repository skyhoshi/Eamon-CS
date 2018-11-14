
// ArtifactHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class ArtifactHelper : Helper<IArtifact>, IArtifactHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		protected virtual string GetPrintedNameStateDesc()
		{
			return "State Description";
		}

		protected virtual string GetPrintedNameIsCharOwned()
		{
			return "Is Char Owned";
		}

		protected virtual string GetPrintedNameIsPlural()
		{
			return "Is Plural";
		}

		protected virtual string GetPrintedNameIsListed()
		{
			return "Is Listed";
		}

		protected virtual string GetPrintedNamePluralType()
		{
			return "Plural Type";
		}

		protected virtual string GetPrintedNameCategoriesType()
		{
			var i = Index;

			return string.Format("Cat #{0} Type", i + 1);
		}

		protected virtual string GetPrintedNameCategoriesField1()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field1Name : "Field1");
		}

		protected virtual string GetPrintedNameCategoriesField2()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field2Name : "Field2");
		}

		protected virtual string GetPrintedNameCategoriesField3()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field3Name : "Field3");
		}

		protected virtual string GetPrintedNameCategoriesField4()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field4Name : "Field4");
		}

		protected virtual string GetPrintedNameCategoriesField5()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			return string.Format("Cat #{0} {1}", i + 1, artType != null ? artType.Field5Name : "Field5");
		}

		#endregion

		#region GetName Methods

		protected virtual string GetNameCategories(bool addToNamesList)
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				GetName("CategoriesType", addToNamesList);
				GetName("CategoriesField1", addToNamesList);
				GetName("CategoriesField2", addToNamesList);
				GetName("CategoriesField3", addToNamesList);
				GetName("CategoriesField4", addToNamesList);
				GetName("CategoriesField5", addToNamesList);
			}

			return "Categories";
		}

		protected virtual string GetNameCategoriesType(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Type", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameCategoriesField1(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field1", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameCategoriesField2(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field2", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameCategoriesField3(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field3", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameCategoriesField4(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field4", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameCategoriesField5(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Categories[{0}].Field5", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		protected virtual object GetValueCategoriesType()
		{
			var i = Index;

			return Record.GetCategories(i).Type;
		}

		protected virtual object GetValueCategoriesField1()
		{
			var i = Index;

			return Record.GetCategories(i).Field1;
		}

		protected virtual object GetValueCategoriesField2()
		{
			var i = Index;

			return Record.GetCategories(i).Field2;
		}

		protected virtual object GetValueCategoriesField3()
		{
			var i = Index;

			return Record.GetCategories(i).Field3;
		}

		protected virtual object GetValueCategoriesField4()
		{
			var i = Index;

			return Record.GetCategories(i).Field4;
		}

		protected virtual object GetValueCategoriesField5()
		{
			var i = Index;

			return Record.GetCategories(i).Field5;
		}

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName()
		{
			var result = !string.IsNullOrWhiteSpace(Record.Name);

			if (result && Record.Name.Length > Constants.ArtNameLen)
			{
				for (var i = Constants.ArtNameLen; i < Record.Name.Length; i++)
				{
					if (Record.Name[i] != '#')
					{
						result = false;

						break;
					}
				}
			}

			return result;
		}

		protected virtual bool ValidateStateDesc()
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= Constants.ArtStateDescLen;
		}

		protected virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ArtDescLen;
		}

		protected virtual bool ValidatePluralType()
		{
			return Globals.Engine.IsValidPluralType(Record.PluralType);
		}

		protected virtual bool ValidateArticleType()
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), Record.ArticleType);
		}

		protected virtual bool ValidateValue()
		{
			return Record.Value >= Constants.MinGoldValue && Record.Value <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateCategories()
		{
			var result = true;

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				result = ValidateField("CategoriesType") &&
								ValidateField("CategoriesField1") &&
								ValidateField("CategoriesField2") &&
								ValidateField("CategoriesField3") &&
								ValidateField("CategoriesField4") &&
								ValidateField("CategoriesField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateCategoriesType()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				result = Globals.Engine.IsValidArtifactType(Record.GetCategories(i).Type);

				if (result)
				{
					for (var h = 0; h < Record.Categories.Length; h++)
					{
						if (h != i && Record.GetCategories(h).Type != Enums.ArtifactType.None)
						{
							if ((Record.GetCategories(h).Type == Record.GetCategories(i).Type) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.Gold && Record.GetCategories(i).Type == Enums.ArtifactType.Treasure) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.Treasure && Record.GetCategories(i).Type == Enums.ArtifactType.Gold) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.Weapon && Record.GetCategories(i).Type == Enums.ArtifactType.MagicWeapon) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.MagicWeapon && Record.GetCategories(i).Type == Enums.ArtifactType.Weapon) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.Container && Record.GetCategories(i).Type == Enums.ArtifactType.DoorGate) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.DoorGate && Record.GetCategories(i).Type == Enums.ArtifactType.Container) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.BoundMonster && Record.GetCategories(i).Type == Enums.ArtifactType.DisguisedMonster) ||
									(Record.GetCategories(h).Type == Enums.ArtifactType.DisguisedMonster && Record.GetCategories(i).Type == Enums.ArtifactType.BoundMonster))
							{
								result = false;

								break;
							}
						}
					}
				}
			}
			else
			{
				result = Record.GetCategories(i).Type == Enums.ArtifactType.None;
			}

			return result;
		}

		protected virtual bool ValidateCategoriesField1()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field1 >= -50 && Record.GetCategories(i).Field1 <= 50;

						break;

					case Enums.ArtifactType.Container:

						result = Record.GetCategories(i).Field1 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.LightSource:

						result = Record.GetCategories(i).Field1 >= -1;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.BoundMonster:
					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field1 > 0;

						break;

					case Enums.ArtifactType.Wearable:

						result = Globals.Engine.IsValidArtifactArmor(Record.GetCategories(i).Field1);

						break;

					case Enums.ArtifactType.DeadBody:

						result = Record.GetCategories(i).Field1 >= 0 && Record.GetCategories(i).Field1 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field1 == 0;
			}

			return result;
		}

		protected virtual bool ValidateCategoriesField2()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Enum.IsDefined(typeof(Enums.Weapon), Record.GetCategories(i).Field2);

						break;

					case Enums.ArtifactType.Container:

						result = (Record.GetCategories(i).Field2 >= 0 && Record.GetCategories(i).Field2 <= 1) || Globals.Engine.IsArtifactFieldStrength(Record.GetCategories(i).Field2);

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:

						result = Record.GetCategories(i).Field2 >= 0;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field2 > 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = Record.GetCategories(i).Field2 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.BoundMonster:

						result = Record.GetCategories(i).Field2 >= -1;

						break;

					case Enums.ArtifactType.Wearable:

						result = Enum.IsDefined(typeof(Enums.Clothing), Record.GetCategories(i).Field2);

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field2 == 0;
			}

			return result;
		}

		protected virtual bool ValidateCategoriesField3()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field3 >= 1 && Record.GetCategories(i).Field3 <= 25;

						break;

					case Enums.ArtifactType.Container:
					case Enums.ArtifactType.BoundMonster:

						result = Record.GetCategories(i).Field3 >= 0;

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:
					case Enums.ArtifactType.Readable:

						result = Record.GetCategories(i).Field3 >= 0 && Record.GetCategories(i).Field3 <= 1;

						break;

					case Enums.ArtifactType.DoorGate:

						result = (Record.GetCategories(i).Field3 >= 0 && Record.GetCategories(i).Field3 <= 1) || Globals.Engine.IsArtifactFieldStrength(Record.GetCategories(i).Field3);

						break;

					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetCategories(i).Field3 > 0;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field3 == 0;
			}

			return result;
		}

		protected virtual bool ValidateCategoriesField4()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetCategories(i).Field4 >= 1 && Record.GetCategories(i).Field4 <= 25;

						break;

					case Enums.ArtifactType.Container:

						result = Record.GetCategories(i).Field4 >= 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = Record.GetCategories(i).Field4 >= 0 && Record.GetCategories(i).Field4 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field4 == 0;
			}

			return result;
		}

		protected virtual bool ValidateCategoriesField5()
		{
			var result = true;

			var activeCategory = true;

			var i = Index;

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetCategories(h).Type == Enums.ArtifactType.None)
				{
					activeCategory = false;

					break;
				}
			}

			if (activeCategory)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						if (Record.GetCategories(i).Field5 == 0)	// auto-upgrade old weapons
						{
							Record.GetCategories(i).Field5 = Record.GetCategories(i).Field2 == (long)Enums.Weapon.Bow ? 2 : 1;
						}

						result = Record.GetCategories(i).Field5 >= 1 && Record.GetCategories(i).Field5 <= 2;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetCategories(i).Field5 == 0;
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		protected virtual bool ValidateInterdependenciesDesc()
		{
			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Desc, Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Desc"), "effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesPluralType()
		{
			var result = true;

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
				var effect = Globals.EDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("PluralType"), "effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesLocation()
		{
			var result = true;

			var monUid = Record.GetWornByMonsterUid();

			if (monUid > 0)
			{
				var monster = Globals.MDB[monUid];

				if (monster == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "monster", monUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IMonster);

					NewRecordUid = monUid;

					goto Cleanup;
				}
			}
			else
			{
				monUid = Record.GetCarriedByMonsterUid();

				if (monUid > 0)
				{
					var monster = Globals.MDB[monUid];

					if (monster == null)
					{
						result = false;

						Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "monster", monUid, "which doesn't exist");

						ErrorMessage = Buf.ToString();

						RecordType = typeof(IMonster);

						NewRecordUid = monUid;

						goto Cleanup;
					}
				}
				else
				{
					var roomUid = Record.GetInRoomUid();

					if (roomUid > 0)
					{
						var room = Globals.RDB[roomUid];

						if (room == null)
						{
							result = false;

							Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "room", roomUid, "which doesn't exist");

							ErrorMessage = Buf.ToString();

							RecordType = typeof(IRoom);

							NewRecordUid = roomUid;

							goto Cleanup;
						}
					}
					else
					{
						roomUid = Record.GetEmbeddedInRoomUid();

						if (roomUid > 0)
						{
							var room = Globals.RDB[roomUid];

							if (room == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "room", roomUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IRoom);

								NewRecordUid = roomUid;

								goto Cleanup;
							}
						}
						else
						{
							var artUid = Record.GetCarriedByContainerUid();

							if (artUid > 0)
							{
								var artifact = Globals.ADB[artUid];

								if (artifact == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "artifact", artUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IArtifact);

									NewRecordUid = artUid;

									goto Cleanup;
								}
								else if (artifact.Container == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "artifact", artUid, "which should be a container, but isn't");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IArtifact);

									EditRecord = artifact;

									goto Cleanup;
								}
							}
						}
					}
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesCategories()
		{
			var result = true;

			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				result = ValidateFieldInterdependencies("CategoriesType") &&
								ValidateFieldInterdependencies("CategoriesField1") &&
								ValidateFieldInterdependencies("CategoriesField2") &&
								ValidateFieldInterdependencies("CategoriesField3") &&
								ValidateFieldInterdependencies("CategoriesField4") &&
								ValidateFieldInterdependencies("CategoriesField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateInterdependenciesCategoriesField1()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Container:
					{
						var artUid = Record.GetCategories(i).Field1;

						if (artUid > 0)
						{
							var artifact = Globals.ADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.Readable:
					{
						var effectUid = Record.GetCategories(i).Field1;

						if (effectUid > 0)
						{
							var effect = Globals.EDB[effectUid];

							if (effect == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "effect", effectUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = effectUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.DoorGate:
					{
						var roomUid = Record.GetCategories(i).Field1;

						if (roomUid > 0)
						{
							var room = Globals.RDB[roomUid];

							if (room == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "room", roomUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IRoom);

								NewRecordUid = roomUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.BoundMonster:
					{
						var monUid = Record.GetCategories(i).Field1;

						if (monUid > 0)
						{
							var monster = Globals.MDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.DisguisedMonster:
					{
						var monUid = Record.GetCategories(i).Field1;

						if (monUid > 0)
						{
							var monster = Globals.MDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField1"), "monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
					}
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesCategoriesField2()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.Readable:
					{
						var effectUid = Record.GetCategories(i).Field1;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < Record.GetCategories(i).Field2; j++, effectUid++)
							{
								var effect = Globals.EDB[effectUid];

								if (effect == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "effect", effectUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IEffect);

									NewRecordUid = effectUid;

									goto Cleanup;
								}
							}
						}

						break;
					}

					case Enums.ArtifactType.DoorGate:
					{
						var artUid = Record.GetCategories(i).Field2;

						if (artUid > 0)
						{
							var artifact = Globals.ADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.BoundMonster:
					{
						var artUid = Record.GetCategories(i).Field2;

						if (artUid > 0)
						{
							var artifact = Globals.ADB[artUid];

							if (artifact == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "artifact", artUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IArtifact);

								NewRecordUid = artUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.DisguisedMonster:
					{
						var effectUid = Record.GetCategories(i).Field2;

						if (effectUid > 0)
						{
							var effect = Globals.EDB[effectUid];

							if (effect == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField2"), "effect", effectUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IEffect);

								NewRecordUid = effectUid;

								goto Cleanup;
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
					}
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesCategoriesField3()
		{
			var result = true;

			var i = Index;

			Debug.Assert(i >= 0 && i < Record.Categories.Length);

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetCategories(i).Type)
				{
					case Enums.ArtifactType.BoundMonster:
					{
						var monUid = Record.GetCategories(i).Field3;

						if (monUid > 0)
						{
							var monster = Globals.MDB[monUid];

							if (monster == null)
							{
								result = false;

								Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "monster", monUid, "which doesn't exist");

								ErrorMessage = Buf.ToString();

								RecordType = typeof(IMonster);

								NewRecordUid = monUid;

								goto Cleanup;
							}
						}

						break;
					}

					case Enums.ArtifactType.DisguisedMonster:
					{
						var effectUid = Record.GetCategories(i).Field2;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < Record.GetCategories(i).Field3; j++, effectUid++)
							{
								var effect = Globals.EDB[effectUid];

								if (effect == null)
								{
									result = false;

									Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("CategoriesField3"), "effect", effectUid, "which doesn't exist");

									ErrorMessage = Buf.ToString();

									RecordType = typeof(IEffect);

									NewRecordUid = effectUid;

									goto Cleanup;
								}
							}
						}

						break;
					}

					default:
					{
						// do nothing

						break;
					}
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the artifact." + Environment.NewLine + Environment.NewLine + "Artifact names should always be in singular form and capitalized when appropriate.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescStateDesc()
		{
			var fullDesc = "Enter the state description of the artifact (will typically be empty).";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the artifact.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescSeen()
		{
			var fullDesc = "Enter the Seen status of the artifact.";

			var briefDesc = "0=Not seen; 1=Seen";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsCharOwned()
		{
			var fullDesc = "Enter the Is Char Owned status of the artifact.";

			var briefDesc = "0=Not char owned; 1=Char owned";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsPlural()
		{
			var fullDesc = "Enter the Is Plural status of the artifact.";

			var briefDesc = "0=Singular; 1=Plural";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsListed()
		{
			var fullDesc = "Enter the Is Listed status of the artifact." + Environment.NewLine + Environment.NewLine + "If true, the artifact will be included in any listing (room, inventory, etc); if false, it will not.";

			var briefDesc = "0=Not listed; 1=Listed";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescPluralType()
		{
			var fullDesc = "Enter the plural type of the artifact.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use effect uid N as plural name";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArticleType()
		{
			var fullDesc = "Enter the article type of the artifact.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescValue()
		{
			var fullDesc = "Enter the value of the artifact in gold pieces.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeight()
		{
			var fullDesc = "Enter the weight of the artifact." + Environment.NewLine + Environment.NewLine + "Be sure to factor bulk and encumberance into weight values.";

			var briefDesc = "-999=Fixtures, doors, buildings, structures, etc; 1-5=Handheld object; 6-10=Medium sized items; 15-35=Weapons, equipment, etc; 999=Heavy furniture, giant objects, etc";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescLocation()
		{
			var fullDesc = "Enter the location of the artifact.";

			var briefDesc = "(-1000 - N)=Worn by monster uid N; -999=Worn by player; (-N - 1)=Carried by monster uid N; -1=Carried by player; 0=Limbo; 1-1000=Room uid; (1000 + N)=Inside artifact uid N; (2000 + N)=Embedded in room uid N";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCategoriesType()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			fullDesc.AppendFormat("Enter the type of the artifact (category #{0}).", i + 1);

			var artTypeValues = EnumUtil.GetValues<Enums.ArtifactType>(at => at != Enums.ArtifactType.None);

			for (var j = 0; j < artTypeValues.Count; j++)
			{
				var artType = Globals.Engine.GetArtifactTypes(artTypeValues[j]);

				Debug.Assert(artType != null);

				briefDesc.AppendFormat("{0}{1}={2}", i > 0 && j == 0 ? "-1=None; " : j != 0 ? "; " : "", (long)artTypeValues[j], artType.Name);
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCategoriesField1()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon complexity (category #{0}).", i + 1);

					briefDesc.Append("-50-50=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the key uid of the artifact (category #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the container.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.LightSource:

					fullDesc.AppendFormat("Enter the light counter of the artifact (category #{0}).{1}{1}This is the number of rounds before the light source is exhausted/goes out.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Never runs out; (GE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:

					fullDesc.AppendFormat("Enter the number of hits healed (or inflicted, if negative) for the artifact (category #{0}).", i + 1);

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);

					break;

				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the artifact's effect uid #1 (category #{0}).{1}{1}This is the first of one or more effects displayed when the artifact is read.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Effect uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the room uid beyond for the artifact (category #{0}).{1}{1}This is the room uid of the room on the opposite side of the door/gate.", i + 1, Environment.NewLine);

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the monster uid of the artifact (category #{0}).{1}{1}This is the monster uid of the entity that is bound.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Bound monster uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Wearable:

					fullDesc.AppendFormat("Enter the armor class of the artifact (category #{0}).", i + 1);

					var armorValues = EnumUtil.GetValues<Enums.Armor>(a => a == Enums.Armor.ClothesShield || ((long)a) % 2 == 0);

					for (var j = 0; j < armorValues.Count; j++)
					{
						var armor = Globals.Engine.GetArmors(armorValues[j]);

						Debug.Assert(armor != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
					}

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the monster uid of the artifact (category #{0}).{1}{1}This is the monster uid of the entity that is disguised.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Disguised monster uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DeadBody:

					fullDesc.AppendFormat("Enter the takeable status of the artifact (category #{0}).{1}{1}Typically, dead bodies should not be takeable unless it serves some useful purpose.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Not takeable; 1=Takeable");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescCategoriesField2()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon type (category #{0}).", i + 1);

					var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

					for (var j = 0; j < weaponValues.Count; j++)
					{
						var weapon = Globals.Engine.GetWeapons(weaponValues[j]);

						Debug.Assert(weapon != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
					}

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (category #{0}).{1}{1}Additionally, you can specify that the container must be forced open.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Closed; 1=Open; (1000 + N)=Forced open with N hits damage");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:

					fullDesc.AppendFormat("Enter the number of times the artifact can be used (category #{0}).", i + 1);

					briefDesc.Append("(GTE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the number of sequential effects used by the artifact (category #{0}).", i + 1);

					briefDesc.Append("(GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the key uid of the artifact (category #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the door/gate.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the key uid of the artifact (category #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the bound monster.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Wearable:

					fullDesc.AppendFormat("Enter the clothing type of the artifact (category #{0}).", i + 1);

					var clothingValues = EnumUtil.GetValues<Enums.Clothing>();

					for (var j = 0; j < clothingValues.Count; j++)
					{
						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)clothingValues[j], Globals.Engine.GetClothingNames(clothingValues[j]));
					}

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the artifact's effect uid #1 (category #{0}).{1}{1}This is the first of one or more effects displayed when the disguised monster is revealed.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Effect uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescCategoriesField3()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon hit dice (category #{0}).", i + 1);

					briefDesc.Append("1-25=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the maximum combined weight allowed inside the artifact (category #{0}).{1}{1}This is the total weight of items immediately inside the container (not including their contents).", i + 1, Environment.NewLine);

					briefDesc.Append("(GE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:
				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (category #{0}).", i + 1);

					briefDesc.Append("0=Closed; 1=Open");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (category #{0}).{1}{1}Additionally, you can specify that the door/gate must be forced open.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Open; 1=Closed; (1000 + N)=Forced open with N hits damage");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the guard uid of the artifact (category #{0}).{1}{1}This is the monster uid of the entity that is guarding the bound monster.", i + 1, Environment.NewLine);

					briefDesc.Append("0=No guard; (GT 0)=Guard monster uid");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the number of sequential effects used by the artifact (category #{0}).", i + 1);

					briefDesc.Append("(GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescCategoriesField4()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon hit dice sides (category #{0}).", i + 1);

					briefDesc.Append("1-25=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the maximum number of items allowed inside the artifact (category #{0}).{1}{1}Additionally, you can specify that the player can't put anything in the container.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Player can't put anything inside; (GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the normal/hidden status of the artifact (category #{0}).", i + 1);

					briefDesc.Append("0=Normal; 1=Hidden");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescCategoriesField5()
		{
			var i = Index;

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon number of hands required (category #{0}).", i + 1);

					briefDesc.Append("1-2=Valid value");

					Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		#endregion

		#region List Methods

		protected virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
			}
		}

		protected virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		protected virtual void ListStateDesc()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					Buf.Clear();

					Buf.Append(Record.StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Record.StateDesc);
				}
			}
		}

		protected virtual void ListDesc()
		{
			if (FullDetail && ShowDesc)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Desc, Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.Desc);
				}

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Desc"), null), Buf);
			}
		}

		protected virtual void ListSeen()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Seen"), null), Convert.ToInt64(Record.Seen));
			}
		}

		protected virtual void ListIsCharOwned()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IsCharOwned"), null), Convert.ToInt64(Record.IsCharOwned));
			}
		}

		protected virtual void ListIsPlural()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IsPlural"), null), Convert.ToInt64(Record.IsPlural));
			}
		}

		protected virtual void ListIsListed()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IsListed"), null), Convert.ToInt64(Record.IsListed));
			}
		}

		protected virtual void ListPluralType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Buf.Clear();

					Buf01.Clear();

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = Globals.EDB[effectUid];

					if (effect != null)
					{
						Buf01.Append(effect.Desc.Length > Constants.ArtNameLen - 6 ? effect.Desc.Substring(0, Constants.ArtNameLen - 9) + "..." : effect.Desc);

						Buf.AppendFormat("Use '{0}'", Buf01.ToString());
					}
					else
					{
						Buf.AppendFormat("Use effect uid {0}", effectUid);
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == Enums.PluralType.None ? "No change" :
						Record.PluralType == Enums.PluralType.S ? "Use 's'" :
						Record.PluralType == Enums.PluralType.Es ? "Use 'es'" :
						Record.PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null), (long)Record.PluralType);
				}
			}
		}

		protected virtual void ListArticleType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == Enums.ArticleType.None ? "No article" :
						Record.ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						Record.ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						Record.ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null), (long)Record.ArticleType);
				}
			}
		}

		protected virtual void ListValue()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Value"), null), Record.Value);
			}
		}

		protected virtual void ListWeight()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Weight"), null),
						BuildValue(51, ' ', 8, "Weight"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Weight"), null), Record.Weight);
				}
			}
		}

		protected virtual void ListLocation()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null),
						BuildValue(51, ' ', 8, "Location"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null), Record.Location);
				}
			}
		}

		protected virtual void ListCategories()
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				ListField("CategoriesType");
				ListField("CategoriesField1");
				ListField("CategoriesField2");
				ListField("CategoriesField3");
				ListField("CategoriesField4");
				ListField("CategoriesField5");
			}

			AddToListedNames = false;
		}

		protected virtual void ListCategoriesType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || i == 0 || Record.GetCategories(i - 1).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesType"), null),
							BuildValue(51, ' ', 8, "CategoriesType"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesType"), null), (long)Record.GetCategories(i).Type);
					}
				}
			}
		}

		protected virtual void ListCategoriesField1()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField1"), null),
							BuildValue(51, ' ', 8, "CategoriesField1"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField1"), null), Record.GetCategories(i).Field1);
					}
				}
			}
		}

		protected virtual void ListCategoriesField2()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField2"), null),
							BuildValue(51, ' ', 8, "CategoriesField2"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField2"), null), Record.GetCategories(i).Field2);
					}
				}
			}
		}

		protected virtual void ListCategoriesField3()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField3"), null),
							BuildValue(51, ' ', 8, "CategoriesField3"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField3"), null), Record.GetCategories(i).Field3);
					}
				}
			}
		}

		protected virtual void ListCategoriesField4()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField4"), null),
							BuildValue(51, ' ', 8, "CategoriesField4"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField4"), null), Record.GetCategories(i).Field4);
					}
				}
			}
		}

		protected virtual void ListCategoriesField5()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField5"), null),
							BuildValue(51, ' ', 8, "CategoriesField5"));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CategoriesField5"), null), Record.GetCategories(i).Field5);
					}
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ArtNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = Buf.Trim().ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputStateDesc()
		{
			var fieldDesc = FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc("StateDesc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("StateDesc"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.ArtStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.StateDesc = Buf.Trim().ToString();

				if (ValidateField("StateDesc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputDesc()
		{
			var fieldDesc = FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", desc);

				PrintFieldDesc("Desc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Desc"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.ArtDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputSeen()
		{
			var fieldDesc = FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc("Seen", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Seen"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Seen"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputIsCharOwned()
		{
			var fieldDesc = FieldDesc;

			var isCharOwned = Record.IsCharOwned;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isCharOwned));

				PrintFieldDesc("IsCharOwned", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("IsCharOwned"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsCharOwned = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsCharOwned"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputIsPlural()
		{
			var fieldDesc = FieldDesc;

			var isPlural = Record.IsPlural;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

				PrintFieldDesc("IsPlural", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("IsPlural"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsPlural = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsPlural"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputIsListed()
		{
			var fieldDesc = FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc("IsListed", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("IsListed"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsListed"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputPluralType()
		{
			var fieldDesc = FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc("PluralType", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("PluralType"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluralType = (Enums.PluralType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("PluralType"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArticleType()
		{
			var fieldDesc = FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc("ArticleType", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ArticleType"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArticleType = (Enums.ArticleType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArticleType"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputValue()
		{
			var fieldDesc = FieldDesc;

			var value = Record.Value;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("Value", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Value"), "25"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "25", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Value = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Value"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeight()
		{
			var artType = EditRec ? Globals.Engine.GetArtifactTypes(Record.GetCategories(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var weight = Record.Weight;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", weight);

				PrintFieldDesc("Weight", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Weight"), artType != null ? artType.WeightEmptyVal : "15"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.WeightEmptyVal : "15", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weight = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Weight"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputLocation()
		{
			var artType = EditRec ? Globals.Engine.GetArtifactTypes(Record.GetCategories(0).Type) : null;

			Debug.Assert(!EditRec || artType != null);

			var fieldDesc = FieldDesc;

			var location = Record.Location;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", location);

				PrintFieldDesc("Location", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Location"), artType != null ? artType.LocationEmptyVal : "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.LocationEmptyVal : "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Location"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputCategories()
		{
			for (Index = 0; Index < Record.Categories.Length; Index++)
			{
				InputField("CategoriesType");
				InputField("CategoriesField1");
				InputField("CategoriesField2");
				InputField("CategoriesField3");
				InputField("CategoriesField4");
				InputField("CategoriesField5");
			}
		}

		protected virtual void InputCategoriesType()
		{
			var i = Index;

			if (i == 0 || Record.GetCategories(i - 1).Type != Enums.ArtifactType.None)
			{
				var fieldDesc = FieldDesc;

				var type = Record.GetCategories(i).Type;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc("CategoriesType", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesType"), i == 0 ? "1" : "-1"));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, i == 0 ? "1" : "-1", null, i == 0 ? (Func<char, bool>)Globals.Engine.IsCharDigit : Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Type = (Enums.ArtifactType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesType"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
				{
					if (EditRec && Record.GetCategories(i).Type != type)
					{
						var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

						Debug.Assert(artType != null);

						Record.GetCategories(i).Field1 = Convert.ToInt64(artType.Field1EmptyVal);

						Record.GetCategories(i).Field2 = Convert.ToInt64(artType.Field2EmptyVal);

						Record.GetCategories(i).Field3 = Convert.ToInt64(artType.Field3EmptyVal);

						Record.GetCategories(i).Field4 = Convert.ToInt64(artType.Field4EmptyVal);

						Record.GetCategories(i).Field5 = Convert.ToInt64(artType.Field5EmptyVal);
					}
				}
				else
				{
					for (var k = i; k < Record.Categories.Length; k++)
					{
						Record.GetCategories(k).Type = Enums.ArtifactType.None;

						Record.GetCategories(k).Field1 = 0;

						Record.GetCategories(k).Field2 = 0;

						Record.GetCategories(k).Field3 = 0;

						Record.GetCategories(k).Field4 = 0;

						Record.GetCategories(k).Field5 = 0;
					}
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Type = Enums.ArtifactType.None;
			}
		}

		protected virtual void InputCategoriesField1()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field1 = Record.GetCategories(i).Field1;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field1);

					PrintFieldDesc("CategoriesField1", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField1"), artType.Field1EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field1EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field1 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField1"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field1 = 0;
			}
		}

		protected virtual void InputCategoriesField2()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field2 = Record.GetCategories(i).Field2;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field2);

					PrintFieldDesc("CategoriesField2", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField2"), artType.Field2EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field2EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field2 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField2"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field2 = 0;
			}
		}

		protected virtual void InputCategoriesField3()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field3 = Record.GetCategories(i).Field3;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field3);

					PrintFieldDesc("CategoriesField3", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField3"), artType.Field3EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field3EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field3 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField3"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field3 = 0;
			}
		}

		protected virtual void InputCategoriesField4()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field4 = Record.GetCategories(i).Field4;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field4);

					PrintFieldDesc("CategoriesField4", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField4"), artType.Field4EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field4EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field4 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField4"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field4 = 0;
			}
		}

		protected virtual void InputCategoriesField5()
		{
			var i = Index;

			if (Record.GetCategories(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = FieldDesc;

				var field5 = Record.GetCategories(i).Field5;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field5);

					PrintFieldDesc("CategoriesField5", EditRec, EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CategoriesField5"), artType.Field5EmptyVal));

					var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field5EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetCategories(i).Field5 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("CategoriesField5"))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.GetCategories(i).Field5 = 0;
			}
		}

		#endregion

		#region BuildValue Methods

		protected virtual string BuildValueWeight()
		{
			Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.Weight, null, Record.IsUnmovable() ? "Unmovable" : null));

			return Buf01.ToString();
		}

		protected virtual string BuildValueLocation()
		{
			string lookupMsg;

			if (Record.IsCarriedByCharacter())
			{
				lookupMsg = "Carried by Player Character";
			}
			else if (Record.IsWornByCharacter())
			{
				lookupMsg = "Worn by Player Character";
			}
			else if (Record.IsCarriedByMonster())
			{
				var monster = Record.GetCarriedByMonster();

				lookupMsg = string.Format("Carried by {0}",
					monster != null ? Globals.Engine.Capitalize(monster.Name.Length > 29 ? monster.Name.Substring(0, 26) + "..." : monster.Name) : Globals.Engine.UnknownName);
			}
			else if (Record.IsWornByMonster())
			{
				var monster = Record.GetWornByMonster();

				lookupMsg = string.Format("Worn by {0}",
					monster != null ? Globals.Engine.Capitalize(monster.Name.Length > 32 ? monster.Name.Substring(0, 29) + "..." : monster.Name) : Globals.Engine.UnknownName);
			}
			else if (Record.IsCarriedByContainer())
			{
				var artifact = Record.GetCarriedByContainer();

				lookupMsg = string.Format("Inside {0}",
					artifact != null ? Globals.Engine.Capitalize(artifact.Name.Length > 33 ? artifact.Name.Substring(0, 30) + "..." : artifact.Name) : Globals.Engine.UnknownName);
			}
			else if (Record.IsEmbeddedInRoom())
			{
				var room = Record.GetEmbeddedInRoom();

				lookupMsg = string.Format("Embedded in {0}",
					room != null ? Globals.Engine.Capitalize(room.Name.Length > 28 ? room.Name.Substring(0, 25) + "..." : room.Name) : Globals.Engine.UnknownName);
			}
			else if (Record.IsInRoom())
			{
				var room = Record.GetInRoom();

				lookupMsg = room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName;
			}
			else
			{
				lookupMsg = null;
			}

			Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.Location, null, lookupMsg));

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesType()
		{
			var i = Index;

			var artType = Globals.Engine.GetArtifactTypes(Record.GetCategories(i).Type);

			Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, (long)Record.GetCategories(i).Type, null, artType != null ? artType.Name : "None"));

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesField1()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var stringVal = string.Format("{0}%", Record.GetCategories(i).Field1);

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, 0, stringVal, null));

					break;

				case Enums.ArtifactType.Container:

					if (Record.GetCategories(i).Field1 > 0)
					{
						var artifact = Globals.ADB[Record.GetCategories(i).Field1];

						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));
					}

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DisguisedMonster:

					var monster = Globals.MDB[Record.GetCategories(i).Field1];

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));

					break;

				case Enums.ArtifactType.DoorGate:

					if (Record.GetCategories(i).Field1 > 0)
					{
						var room = Globals.RDB[Record.GetCategories(i).Field1];

						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					var armor = Globals.Engine.GetArmors((Enums.Armor)Record.GetCategories(i).Field1);

					Debug.Assert(armor != null);

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, armor.Name));

					break;

				case Enums.ArtifactType.DeadBody:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, Record.GetCategories(i).Field1 == 1 ? "Takeable" : "Not Takeable"));

					break;

				default:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field1, null, null));

					break;
			}

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesField2()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var weapon = Globals.Engine.GetWeapons((Enums.Weapon)Record.GetCategories(i).Field2);

					Debug.Assert(weapon != null);

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, weapon.Name));

					break;

				case Enums.ArtifactType.Container:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetCategories(i).Field2))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetCategories(i).Field2));
					}

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, Record.IsFieldStrength(Record.GetCategories(i).Field2) ? lookupMsg : Record.GetCategories(i).Field2 == 1 ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DoorGate:

					if (Record.GetCategories(i).Field2 > 0)
					{
						var artifact = Globals.ADB[Record.GetCategories(i).Field2];

						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, Globals.Engine.GetClothingNames((Enums.Clothing)Record.GetCategories(i).Field2)));

					break;

				default:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field2, null, null));

					break;
			}

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesField3()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Readable:
				case Enums.ArtifactType.Edible:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, Record.GetCategories(i).IsOpen() ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:

					if (Record.GetCategories(i).Field3 > 0)
					{
						var monster = Globals.MDB[Record.GetCategories(i).Field3];

						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, null));
					}

					break;

				case Enums.ArtifactType.DoorGate:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetCategories(i).Field3))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetCategories(i).Field3));
					}

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, Record.IsFieldStrength(Record.GetCategories(i).Field3) ? lookupMsg : Record.GetCategories(i).IsOpen() ? "Open" : "Closed"));

					break;

				default:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field3, null, null));

					break;
			}

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesField4()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				case Enums.ArtifactType.DoorGate:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field4, null, Record.GetCategories(i).Field4 == 1 ? "Hidden" : "Normal"));

					break;

				default:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field4, null, null));

					break;
			}

			return Buf01.ToString();
		}

		protected virtual string BuildValueCategoriesField5()
		{
			var i = Index;

			switch (Record.GetCategories(i).Type)
			{
				default:

					Buf01.Append(Globals.Engine.BuildValue(BufSize, FillChar, Offset, Record.GetCategories(i).Field5, null, null));

					break;
			}

			return Buf01.ToString();
		}

		protected virtual string BuildValue(long bufSize, char fillChar, long offset, string fieldName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fieldName));

			var origBufSize = BufSize;

			var origFillChar = FillChar;

			var origOffset = Offset;

			BufSize = bufSize;

			FillChar = fillChar;

			Offset = offset;

			var result = BuildValue(fieldName);

			BufSize = origBufSize;

			FillChar = origFillChar;

			Offset = origOffset;

			return result;
		}

		#endregion

		#endregion

		#region Class ArtifactHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetArtifactUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Name"), null), Record.Name);

			if (string.Equals(ErrorFieldName, "Desc", StringComparison.OrdinalIgnoreCase) || ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null), Record.Desc);
			}

			if (!string.Equals(ErrorFieldName, "Desc", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}",
					Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName(ErrorFieldName), null),
					Convert.ToInt64(GetValue(ErrorFieldName)));
			}
		}

		#endregion

		#region Class ArtifactHelper

		public ArtifactHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"StateDesc",
				"Desc",
				"Seen",
				"IsCharOwned",
				"IsPlural",
				"IsListed",
				"PluralType",
				"ArticleType",
				"Value",
				"Weight",
				"Location",
				"Categories",
			};
		}

		#endregion

		#endregion
	}
}
