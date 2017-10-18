
// ArtifactHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IArtifact>))]
	public class ArtifactHelper : Helper<IArtifact>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
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

		protected virtual bool ValidateStateDesc(IField field, IValidateArgs args)
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= Constants.ArtStateDescLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ArtDescLen;
		}

		protected virtual bool ValidatePluralType(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidPluralType(Record.PluralType);
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), Record.ArticleType);
		}

		protected virtual bool ValidateValue(IField field, IValidateArgs args)
		{
			return Record.Value >= Constants.MinGoldValue && Record.Value <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateClassesType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				result = Globals.Engine.IsValidArtifactType(Record.GetClasses(i).Type);

				if (result)
				{
					for (var h = 0; h < Record.Classes.Length; h++)
					{
						if (h != i && Record.GetClasses(h).Type != Enums.ArtifactType.None)
						{
							if ((Record.GetClasses(h).Type == Record.GetClasses(i).Type) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.Gold && Record.GetClasses(i).Type == Enums.ArtifactType.Treasure) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.Treasure && Record.GetClasses(i).Type == Enums.ArtifactType.Gold) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.Weapon && Record.GetClasses(i).Type == Enums.ArtifactType.MagicWeapon) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.MagicWeapon && Record.GetClasses(i).Type == Enums.ArtifactType.Weapon) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.Container && Record.GetClasses(i).Type == Enums.ArtifactType.DoorGate) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.DoorGate && Record.GetClasses(i).Type == Enums.ArtifactType.Container) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.BoundMonster && Record.GetClasses(i).Type == Enums.ArtifactType.DisguisedMonster) ||
									(Record.GetClasses(h).Type == Enums.ArtifactType.DisguisedMonster && Record.GetClasses(i).Type == Enums.ArtifactType.BoundMonster))
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
				result = Record.GetClasses(i).Type == Enums.ArtifactType.None;
			}

			return result;
		}

		protected virtual bool ValidateClassesField5(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetClasses(i).Field5 >= -50 && Record.GetClasses(i).Field5 <= 50;

						break;

					case Enums.ArtifactType.Container:

						result = Record.GetClasses(i).Field5 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.LightSource:

						result = Record.GetClasses(i).Field5 >= -1;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.BoundMonster:
					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetClasses(i).Field5 > 0;

						break;

					case Enums.ArtifactType.Wearable:

						result = Globals.Engine.IsValidArtifactArmor(Record.GetClasses(i).Field5);

						break;

					case Enums.ArtifactType.DeadBody:

						result = Record.GetClasses(i).Field5 >= 0 && Record.GetClasses(i).Field5 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetClasses(i).Field5 == 0;
			}

			return result;
		}

		protected virtual bool ValidateClassesField6(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Enum.IsDefined(typeof(Enums.Weapon), Record.GetClasses(i).Field6);

						break;

					case Enums.ArtifactType.Container:

						result = (Record.GetClasses(i).Field6 >= 0 && Record.GetClasses(i).Field6 <= 1) || Globals.Engine.IsArtifactFieldStrength(Record.GetClasses(i).Field6);

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:

						result = Record.GetClasses(i).Field6 >= 0;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetClasses(i).Field6 > 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = Record.GetClasses(i).Field6 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.BoundMonster:

						result = Record.GetClasses(i).Field6 >= -1;

						break;

					case Enums.ArtifactType.Wearable:

						result = Enum.IsDefined(typeof(Enums.Clothing), Record.GetClasses(i).Field6);

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetClasses(i).Field6 == 0;
			}

			return result;
		}

		protected virtual bool ValidateClassesField7(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetClasses(i).Field7 >= 1 && Record.GetClasses(i).Field7 <= 25;

						break;

					case Enums.ArtifactType.Container:
					case Enums.ArtifactType.BoundMonster:

						result = Record.GetClasses(i).Field7 >= 0;

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:
					case Enums.ArtifactType.Readable:

						result = Record.GetClasses(i).Field7 >= 0 && Record.GetClasses(i).Field7 <= 1;

						break;

					case Enums.ArtifactType.DoorGate:

						result = (Record.GetClasses(i).Field7 >= 0 && Record.GetClasses(i).Field7 <= 1) || Globals.Engine.IsArtifactFieldStrength(Record.GetClasses(i).Field7);

						break;

					case Enums.ArtifactType.DisguisedMonster:

						result = Record.GetClasses(i).Field7 > 0;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetClasses(i).Field7 == 0;
			}

			return result;
		}

		protected virtual bool ValidateClassesField8(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (Record.GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Record.GetClasses(i).Field8 >= 1 && Record.GetClasses(i).Field8 <= 25;

						break;

					case Enums.ArtifactType.Container:

						result = Record.GetClasses(i).Field8 >= 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = Record.GetClasses(i).Field8 >= 0 && Record.GetClasses(i).Field8 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = Record.GetClasses(i).Field8 == 0;
			}

			return result;
		}

		protected virtual bool ValidateInterdependenciesDesc(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Desc, args.Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", invalidUid, "which doesn't exist");

				args.ErrorMessage = args.Buf.ToString();

				args.RecordType = typeof(IEffect);

				args.NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesPluralType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
				var effect = Globals.EDB[effectUid];

				if (effect == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IEffect);

					args.NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesLocation(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var monUid = Record.GetWornByMonsterUid();

			if (monUid > 0)
			{
				var monster = Globals.MDB[monUid];

				if (monster == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "monster", monUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IMonster);

					args.NewRecordUid = monUid;

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

						args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "monster", monUid, "which doesn't exist");

						args.ErrorMessage = args.Buf.ToString();

						args.RecordType = typeof(IMonster);

						args.NewRecordUid = monUid;

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

							args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "room", roomUid, "which doesn't exist");

							args.ErrorMessage = args.Buf.ToString();

							args.RecordType = typeof(IRoom);

							args.NewRecordUid = roomUid;

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

								args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "room", roomUid, "which doesn't exist");

								args.ErrorMessage = args.Buf.ToString();

								args.RecordType = typeof(IRoom);

								args.NewRecordUid = roomUid;

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

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IArtifact);

									args.NewRecordUid = artUid;

									goto Cleanup;
								}
								else if (!artifact.IsContainer())
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be a container, but isn't");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IArtifact);

									args.EditRecord = artifact;

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

		protected virtual bool ValidateInterdependenciesClassesField5(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.Classes.Length);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Container:
						{
							var artUid = Record.GetClasses(i).Field5;

							if (artUid > 0)
							{
								var artifact = Globals.ADB[artUid];

								if (artifact == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IArtifact);

									args.NewRecordUid = artUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.Readable:
						{
							var effectUid = Record.GetClasses(i).Field5;

							if (effectUid > 0)
							{
								var effect = Globals.EDB[effectUid];

								if (effect == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IEffect);

									args.NewRecordUid = effectUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.DoorGate:
						{
							var roomUid = Record.GetClasses(i).Field5;

							if (roomUid > 0)
							{
								var room = Globals.RDB[roomUid];

								if (room == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "room", roomUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IRoom);

									args.NewRecordUid = roomUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.BoundMonster:
						{
							var monUid = Record.GetClasses(i).Field5;

							if (monUid > 0)
							{
								var monster = Globals.MDB[monUid];

								if (monster == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "monster", monUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IMonster);

									args.NewRecordUid = monUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.DisguisedMonster:
						{
							var monUid = Record.GetClasses(i).Field5;

							if (monUid > 0)
							{
								var monster = Globals.MDB[monUid];

								if (monster == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "monster", monUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IMonster);

									args.NewRecordUid = monUid;

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

		protected virtual bool ValidateInterdependenciesClassesField6(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.Classes.Length);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.Readable:
						{
							var effectUid = Record.GetClasses(i).Field5;

							if (effectUid > 0)
							{
								effectUid++;

								for (var j = 1; j < Record.GetClasses(i).Field6; j++, effectUid++)
								{
									var effect = Globals.EDB[effectUid];

									if (effect == null)
									{
										result = false;

										args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

										args.ErrorMessage = args.Buf.ToString();

										args.RecordType = typeof(IEffect);

										args.NewRecordUid = effectUid;

										goto Cleanup;
									}
								}
							}

							break;
						}

					case Enums.ArtifactType.DoorGate:
						{
							var artUid = Record.GetClasses(i).Field6;

							if (artUid > 0)
							{
								var artifact = Globals.ADB[artUid];

								if (artifact == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IArtifact);

									args.NewRecordUid = artUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.BoundMonster:
						{
							var artUid = Record.GetClasses(i).Field6;

							if (artUid > 0)
							{
								var artifact = Globals.ADB[artUid];

								if (artifact == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IArtifact);

									args.NewRecordUid = artUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.DisguisedMonster:
						{
							var effectUid = Record.GetClasses(i).Field6;

							if (effectUid > 0)
							{
								var effect = Globals.EDB[effectUid];

								if (effect == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IEffect);

									args.NewRecordUid = effectUid;

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

		protected virtual bool ValidateInterdependenciesClassesField7(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.Classes.Length);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (Record.GetClasses(i).Type)
				{
					case Enums.ArtifactType.BoundMonster:
						{
							var monUid = Record.GetClasses(i).Field7;

							if (monUid > 0)
							{
								var monster = Globals.MDB[monUid];

								if (monster == null)
								{
									result = false;

									args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "monster", monUid, "which doesn't exist");

									args.ErrorMessage = args.Buf.ToString();

									args.RecordType = typeof(IMonster);

									args.NewRecordUid = monUid;

									goto Cleanup;
								}
							}

							break;
						}

					case Enums.ArtifactType.DisguisedMonster:
						{
							var effectUid = Record.GetClasses(i).Field6;

							if (effectUid > 0)
							{
								effectUid++;

								for (var j = 1; j < Record.GetClasses(i).Field7; j++, effectUid++)
								{
									var effect = Globals.EDB[effectUid];

									if (effect == null)
									{
										result = false;

										args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

										args.ErrorMessage = args.Buf.ToString();

										args.RecordType = typeof(IEffect);

										args.NewRecordUid = effectUid;

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

		#region PrintFieldDesc Methods

		protected virtual void PrintDescName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name of the artifact." + Environment.NewLine + Environment.NewLine + "Artifact names should always be in singular form and capitalized when appropriate.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescStateDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the state description of the artifact (will typically be empty).";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter a detailed description of the artifact.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescSeen(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Seen status of the artifact.";

			var briefDesc = "0=Not seen; 1=Seen";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsCharOwned(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Is Char Owned status of the artifact.";

			var briefDesc = "0=Not char owned; 1=Char owned";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsPlural(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Is Plural status of the artifact.";

			var briefDesc = "0=Singular; 1=Plural";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsListed(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Is Listed status of the artifact." + Environment.NewLine + Environment.NewLine + "If true, the artifact will be included in any listing (room, inventory, etc); if false, it will not.";

			var briefDesc = "0=Not listed; 1=Listed";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescPluralType(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the plural type of the artifact.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use effect uid N as plural name";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArticleType(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the article type of the artifact.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescValue(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the value of the artifact in gold pieces.";

			var briefDesc = string.Format("{0}-{1}=Valid value", Constants.MinGoldValue, Constants.MaxGoldValue);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescWeight(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the weight of the artifact." + Environment.NewLine + Environment.NewLine + "Be sure to factor bulk and encumberance into weight values.";

			var briefDesc = "-999=Fixtures, doors, buildings, structures, etc; 1-5=Handheld object; 6-10=Medium sized items; 15-35=Weapons, equipment, etc; 999=Heavy furniture, giant objects, etc";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescLocation(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the location of the artifact.";

			var briefDesc = "(-1000 - N)=Worn by monster uid N; -999=Worn by player; (-N - 1)=Carried by monster uid N; -1=Carried by player; 0=Limbo; 1-1000=Room uid; (1000 + N)=Inside artifact uid N; (2000 + N)=Embedded in room uid N";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescClassesType(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			fullDesc.AppendFormat("Enter the type of the artifact (class #{0}).", i + 1);

			var artTypeValues = EnumUtil.GetValues<Enums.ArtifactType>(at => at != Enums.ArtifactType.None);

			for (var j = 0; j < artTypeValues.Count; j++)
			{
				var artType = Globals.Engine.GetArtifactTypes(artTypeValues[j]);

				Debug.Assert(artType != null);

				briefDesc.AppendFormat("{0}{1}={2}", i > 0 && j == 0 ? "-1=None; " : j != 0 ? "; " : "", (long)artTypeValues[j], artType.Name);
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescClassesField5(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon complexity (class #{0}).", i + 1);

					briefDesc.Append("-50-50=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the key uid of the artifact (class #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the container.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.LightSource:

					fullDesc.AppendFormat("Enter the light counter of the artifact (class #{0}).{1}{1}This is the number of rounds before the light source is exhausted/goes out.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Never runs out; (GE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:

					fullDesc.AppendFormat("Enter the number of hits healed (or inflicted, if negative) for the artifact (class #{0}).", i + 1);

					Globals.Engine.AppendFieldDesc(args, fullDesc, null);

					break;

				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the artifact's effect uid #1 (class #{0}).{1}{1}This is the first of one or more effects displayed when the artifact is read.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Effect uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the room uid beyond for the artifact (class #{0}).{1}{1}This is the room uid of the room on the opposite side of the door/gate.", i + 1, Environment.NewLine);

					Globals.Engine.AppendFieldDesc(args, fullDesc, null);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the monster uid of the artifact (class #{0}).{1}{1}This is the monster uid of the entity that is bound.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Bound monster uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Wearable:

					fullDesc.AppendFormat("Enter the armor class of the artifact (class #{0}).", i + 1);

					var armorValues = EnumUtil.GetValues<Enums.Armor>(a => a == Enums.Armor.ClothesShield || ((long)a) % 2 == 0);

					for (var j = 0; j < armorValues.Count; j++)
					{
						var armor = Globals.Engine.GetArmors(armorValues[j]);

						Debug.Assert(armor != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
					}

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the monster uid of the artifact (class #{0}).{1}{1}This is the monster uid of the entity that is disguised.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Disguised monster uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DeadBody:

					fullDesc.AppendFormat("Enter the takeable status of the artifact (class #{0}).{1}{1}Typically, dead bodies should not be takeable unless it serves some useful purpose.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Not takeable; 1=Takeable");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescClassesField6(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon type (class #{0}).", i + 1);

					var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

					for (var j = 0; j < weaponValues.Count; j++)
					{
						var weapon = Globals.Engine.GetWeapons(weaponValues[j]);

						Debug.Assert(weapon != null);

						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
					}

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (class #{0}).{1}{1}Additionally, you can specify that the container must be forced open.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Closed; 1=Open; (1000 + N)=Forced open with N hits damage");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:

					fullDesc.AppendFormat("Enter the number of times the artifact can be used (class #{0}).", i + 1);

					briefDesc.Append("(GTE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the number of sequential effects used by the artifact (class #{0}).", i + 1);

					briefDesc.Append("(GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the key uid of the artifact (class #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the door/gate.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the key uid of the artifact (class #{0}).{1}{1}This is the artifact uid of the key used to lock/unlock the bound monster.", i + 1, Environment.NewLine);

					briefDesc.Append("-1=Can't be unlocked/opened normally; 0=No key; (GT 0)=Key artifact uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Wearable:

					fullDesc.AppendFormat("Enter the clothing type of the artifact (class #{0}).", i + 1);

					var clothingValues = EnumUtil.GetValues<Enums.Clothing>();

					for (var j = 0; j < clothingValues.Count; j++)
					{
						briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)clothingValues[j], Globals.Engine.GetClothingNames(clothingValues[j]));
					}

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the artifact's effect uid #1 (class #{0}).{1}{1}This is the first of one or more effects displayed when the disguised monster is revealed.", i + 1, Environment.NewLine);

					briefDesc.Append("(GT 0)=Effect uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescClassesField7(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon hit dice (class #{0}).", i + 1);

					briefDesc.Append("1-25=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the maximum combined weight allowed inside the artifact (class #{0}).{1}{1}This is the total weight of items immediately inside the container (not including their contents).", i + 1, Environment.NewLine);

					briefDesc.Append("(GE 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:
				case Enums.ArtifactType.Readable:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (class #{0}).", i + 1);

					briefDesc.Append("0=Closed; 1=Open");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the open/closed status of the artifact (class #{0}).{1}{1}Additionally, you can specify that the door/gate must be forced open.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Open; 1=Closed; (1000 + N)=Forced open with N hits damage");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.BoundMonster:

					fullDesc.AppendFormat("Enter the guard uid of the artifact (class #{0}).{1}{1}This is the monster uid of the entity that is guarding the bound monster.", i + 1, Environment.NewLine);

					briefDesc.Append("0=No guard; (GT 0)=Guard monster uid");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DisguisedMonster:

					fullDesc.AppendFormat("Enter the number of sequential effects used by the artifact (class #{0}).", i + 1);

					briefDesc.Append("(GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		protected virtual void PrintDescClassesField8(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = new StringBuilder(Constants.BufSize);

			var briefDesc = new StringBuilder(Constants.BufSize);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					fullDesc.AppendFormat("Enter the artifact's weapon hit dice sides (class #{0}).", i + 1);

					briefDesc.Append("1-25=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.Container:

					fullDesc.AppendFormat("Enter the maximum number of items allowed inside the artifact (class #{0}).{1}{1}Additionally, you can specify that the player can't put anything in the container.", i + 1, Environment.NewLine);

					briefDesc.Append("0=Player can't put anything inside; (GT 0)=Valid value");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				case Enums.ArtifactType.DoorGate:

					fullDesc.AppendFormat("Enter the normal/hidden status of the artifact (class #{0}).", i + 1);

					briefDesc.Append("0=Normal; 1=Hidden");

					Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);

					break;

				default:

					// do nothing

					break;
			}
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
			}
		}

		protected virtual void ListName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Name);
			}
		}

		protected virtual void ListStateDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					args.Buf.Clear();

					args.Buf.Append(Record.StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.StateDesc);
				}
			}
		}

		protected virtual void ListDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail && args.ShowDesc)
			{
				args.Buf.Clear();

				if (args.ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Desc, args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Record.Desc);
				}

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
			}
		}

		protected virtual void ListSeen(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.Seen));
			}
		}

		protected virtual void ListIsCharOwned(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.IsCharOwned));
			}
		}

		protected virtual void ListIsPlural(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.IsPlural));
			}
		}

		protected virtual void ListIsListed(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.IsListed));
			}
		}

		protected virtual void ListPluralType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null && args.Buf01 != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					args.Buf.Clear();

					args.Buf01.Clear();

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = Globals.EDB[effectUid];

					if (effect != null)
					{
						args.Buf01.Append(effect.Desc.Length > Constants.ArtNameLen - 6 ? effect.Desc.Substring(0, Constants.ArtNameLen - 9) + "..." : effect.Desc);

						args.Buf.AppendFormat("Use '{0}'", args.Buf01.ToString());
					}
					else
					{
						args.Buf.AppendFormat("Use effect uid {0}", effectUid);
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == Enums.PluralType.None ? "No change" :
						Record.PluralType == Enums.PluralType.S ? "Use 's'" :
						Record.PluralType == Enums.PluralType.Es ? "Use 'es'" :
						Record.PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.PluralType);
				}
			}
		}

		protected virtual void ListArticleType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == Enums.ArticleType.None ? "No article" :
						Record.ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						Record.ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						Record.ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.ArticleType);
				}
			}
		}

		protected virtual void ListValue(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Value);
			}
		}

		protected virtual void ListWeight(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						BuildValue(51, ' ', 8, field));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Weight);
				}
			}
		}

		protected virtual void ListLocation(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						BuildValue(51, ' ', 8, field));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Location);
				}
			}
		}

		protected virtual void ListClassesType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || i == 0 || Record.GetClasses(i - 1).Type != Enums.ArtifactType.None)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							BuildValue(51, ' ', 8, field));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.GetClasses(i).Type);
					}
				}
			}
		}

		protected virtual void ListClassesField5(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							BuildValue(51, ' ', 8, field));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetClasses(i).Field5);
					}
				}
			}
		}

		protected virtual void ListClassesField6(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							BuildValue(51, ' ', 8, field));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetClasses(i).Field6);
					}
				}
			}
		}

		protected virtual void ListClassesField7(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							BuildValue(51, ' ', 8, field));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetClasses(i).Field7);
					}
				}
			}
		}

		protected virtual void ListClassesField8(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || Record.GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					if (args.LookupMsg)
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							BuildValue(51, ' ', 8, field));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetClasses(i).Field8);
					}
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Record.Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputStateDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.StateDesc = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", desc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputSeen(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIsCharOwned(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var isCharOwned = Record.IsCharOwned;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isCharOwned));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsCharOwned = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIsPlural(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var isPlural = Record.IsPlural;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsPlural = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIsListed(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputPluralType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArticleType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputValue(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var value = Record.Value;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "25"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "25", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Value = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputWeight(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var artType = args.EditRec ? Globals.Engine.GetArtifactTypes(Record.GetClasses(0).Type) : null;

			Debug.Assert(!args.EditRec || artType != null);

			var fieldDesc = args.FieldDesc;

			var weight = Record.Weight;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", weight);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType != null ? artType.WeightEmptyVal : "15"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.WeightEmptyVal : "15", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weight = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputLocation(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var artType = args.EditRec ? Globals.Engine.GetArtifactTypes(Record.GetClasses(0).Type) : null;

			Debug.Assert(!args.EditRec || artType != null);

			var fieldDesc = args.FieldDesc;

			var location = Record.Location;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", location);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType != null ? artType.LocationEmptyVal : "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType != null ? artType.LocationEmptyVal : "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputClassesType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (i == 0 || Record.GetClasses(i - 1).Type != Enums.ArtifactType.None)
			{
				var fieldDesc = args.FieldDesc;

				var type = Record.GetClasses(i).Type;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)type);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), i == 0 ? "1" : "-1"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, i == 0 ? "1" : "-1", null, i == 0 ? (Func<char, bool>)Globals.Engine.IsCharDigit : Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetClasses(i).Type = (Enums.ArtifactType)Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.EditRec && Record.GetClasses(i).Type != type)
					{
						var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

						Debug.Assert(artType != null);

						Record.GetClasses(i).Field5 = Convert.ToInt64(artType.Field5EmptyVal);

						Record.GetClasses(i).Field6 = Convert.ToInt64(artType.Field6EmptyVal);

						Record.GetClasses(i).Field7 = Convert.ToInt64(artType.Field7EmptyVal);

						Record.GetClasses(i).Field8 = Convert.ToInt64(artType.Field8EmptyVal);
					}
				}
				else
				{
					for (var k = i; k < Record.Classes.Length; k++)
					{
						Record.GetClasses(k).Type = Enums.ArtifactType.None;

						Record.GetClasses(k).Field5 = 0;

						Record.GetClasses(k).Field6 = 0;

						Record.GetClasses(k).Field7 = 0;

						Record.GetClasses(k).Field8 = 0;
					}
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.GetClasses(i).Type = Enums.ArtifactType.None;
			}
		}

		protected virtual void InputClassesField5(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field5 = Record.GetClasses(i).Field5;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", field5);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType.Field5EmptyVal));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field5EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetClasses(i).Field5 = Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.GetClasses(i).Field5 = 0;
			}
		}

		protected virtual void InputClassesField6(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field6 = Record.GetClasses(i).Field6;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", field6);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType.Field6EmptyVal));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field6EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetClasses(i).Field6 = Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.GetClasses(i).Field6 = 0;
			}
		}

		protected virtual void InputClassesField7(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field7 = Record.GetClasses(i).Field7;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", field7);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType.Field7EmptyVal));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field7EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetClasses(i).Field7 = Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.GetClasses(i).Field7 = 0;
			}
		}

		protected virtual void InputClassesField8(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (Record.GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field8 = Record.GetClasses(i).Field8;

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", field8);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), artType.Field8EmptyVal));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, artType.Field8EmptyVal, null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetClasses(i).Field8 = Convert.ToInt64(args.Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.GetClasses(i).Field8 = 0;
			}
		}

		#endregion

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Uid";
						x.Validate = ValidateUid;
						x.List = ListUid;
						x.Input = InputUid;
						x.GetPrintedName = () => "Uid";
						x.GetValue = () => Record.Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => Record.IsUidRecycled;
					}),
					GetNameField(),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "StateDesc";
						x.Validate = ValidateStateDesc;
						x.PrintDesc = PrintDescStateDesc;
						x.List = ListStateDesc;
						x.Input = InputStateDesc;
						x.GetPrintedName = () => "State Description";
						x.GetValue = () => Record.StateDesc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Desc";
						x.Validate = ValidateDesc;
						x.ValidateInterdependencies = ValidateInterdependenciesDesc;
						x.PrintDesc = PrintDescDesc;
						x.List = ListDesc;
						x.Input = InputDesc;
						x.GetPrintedName = () => "Description";
						x.GetValue = () => Record.Desc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Seen";
						x.PrintDesc = PrintDescSeen;
						x.List = ListSeen;
						x.Input = InputSeen;
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Record.Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsCharOwned";
						x.PrintDesc = PrintDescIsCharOwned;
						x.List = ListIsCharOwned;
						x.Input = InputIsCharOwned;
						x.GetPrintedName = () => "Is Char Owned";
						x.GetValue = () => Record.IsCharOwned;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsPlural";
						x.PrintDesc = PrintDescIsPlural;
						x.List = ListIsPlural;
						x.Input = InputIsPlural;
						x.GetPrintedName = () => "Is Plural";
						x.GetValue = () => Record.IsPlural;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsListed";
						x.PrintDesc = PrintDescIsListed;
						x.List = ListIsListed;
						x.Input = InputIsListed;
						x.GetPrintedName = () => "Is Listed";
						x.GetValue = () => Record.IsListed;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "PluralType";
						x.Validate = ValidatePluralType;
						x.ValidateInterdependencies = ValidateInterdependenciesPluralType;
						x.PrintDesc = PrintDescPluralType;
						x.List = ListPluralType;
						x.Input = InputPluralType;
						x.GetPrintedName = () => "Plural Type";
						x.GetValue = () => Record.PluralType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.PrintDesc = PrintDescArticleType;
						x.List = ListArticleType;
						x.Input = InputArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => Record.ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Value";
						x.Validate = ValidateValue;
						x.PrintDesc = PrintDescValue;
						x.List = ListValue;
						x.Input = InputValue;
						x.GetPrintedName = () => "Value";
						x.GetValue = () => Record.Value;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Weight";
						x.PrintDesc = PrintDescWeight;
						x.List = ListWeight;
						x.Input = InputWeight;
						x.BuildValue = BuildValueWeight;
						x.GetPrintedName = () => "Weight";
						x.GetValue = () => Record.Weight;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Location";
						x.ValidateInterdependencies = ValidateInterdependenciesLocation;
						x.PrintDesc = PrintDescLocation;
						x.List = ListLocation;
						x.Input = InputLocation;
						x.BuildValue = BuildValueLocation;
						x.GetPrintedName = () => "Location";
						x.GetValue = () => Record.Location;
					})
				};

				for (var i = 0; i < Record.Classes.Length; i++)
				{
					var j = i;

					Fields.AddRange(new List<IField>()
					{
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Classes[{0}].Type", j);
							x.UserData = j;
							x.Validate = ValidateClassesType;
							x.PrintDesc = PrintDescClassesType;
							x.List = ListClassesType;
							x.Input = InputClassesType;
							x.BuildValue = BuildValueClassesType;
							x.GetPrintedName = () => string.Format("Cls #{0} Type", j + 1);
							x.GetValue = () => Record.GetClasses(j).Type;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Classes[{0}].Field5", j);
							x.UserData = j;
							x.Validate = ValidateClassesField5;
							x.ValidateInterdependencies = ValidateInterdependenciesClassesField5;
							x.PrintDesc = PrintDescClassesField5;
							x.List = ListClassesField5;
							x.Input = InputClassesField5;
							x.BuildValue = BuildValueClassesField5;
							x.GetPrintedName = () =>
							{
								var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field5Name : "Field5");
							};
							x.GetValue = () => Record.GetClasses(j).Field5;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Classes[{0}].Field6", j);
							x.UserData = j;
							x.Validate = ValidateClassesField6;
							x.ValidateInterdependencies = ValidateInterdependenciesClassesField6;
							x.PrintDesc = PrintDescClassesField6;
							x.List = ListClassesField6;
							x.Input = InputClassesField6;
							x.BuildValue = BuildValueClassesField6;
							x.GetPrintedName = () =>
							{
								var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field6Name : "Field6");
							};
							x.GetValue = () => Record.GetClasses(j).Field6;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Classes[{0}].Field7", j);
							x.UserData = j;
							x.Validate = ValidateClassesField7;
							x.ValidateInterdependencies = ValidateInterdependenciesClassesField7;
							x.PrintDesc = PrintDescClassesField7;
							x.List = ListClassesField7;
							x.Input = InputClassesField7;
							x.BuildValue = BuildValueClassesField7;
							x.GetPrintedName = () =>
							{
								var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field7Name : "Field7");
							};
							x.GetValue = () => Record.GetClasses(j).Field7;
						}),
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Classes[{0}].Field8", j);
							x.UserData = j;
							x.Validate = ValidateClassesField8;
							x.PrintDesc = PrintDescClassesField8;
							x.List = ListClassesField8;
							x.Input = InputClassesField8;
							x.BuildValue = BuildValueClassesField8;
							x.GetPrintedName = () =>
							{
								var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field8Name : "Field8");
							};
							x.GetValue = () => Record.GetClasses(j).Field8;
						})
					});
				}
			}

			return Fields;
		}

		protected override IField GetNameField()
		{
			if (NameField == null)
			{
				NameField = Globals.CreateInstance<IField>(x =>
				{
					x.Name = "Name";
					x.GetPrintedName = () => "Name";
					x.Validate = ValidateName;
					x.PrintDesc = PrintDescName;
					x.List = ListName;
					x.Input = InputName;
					x.BuildValue = null;
					x.GetValue = () => Record.Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Class ArtifactHelper

		protected virtual string BuildValueWeight(IField field, IBuildValueArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.Weight, null, Record.IsUnmovable() ? "Unmovable" : null));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueLocation(IField field, IBuildValueArgs args)
		{
			string lookupMsg;

			Debug.Assert(args != null && args.Buf != null);

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

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.Location, null, lookupMsg));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesType(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var artType = Globals.Engine.GetArtifactTypes(Record.GetClasses(i).Type);

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, (long)Record.GetClasses(i).Type, null, artType != null ? artType.Name : "None"));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField5(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var stringVal = string.Format("{0}%", Record.GetClasses(i).Field5);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, 0, stringVal, null));

					break;

				case Enums.ArtifactType.Container:

					if (Record.GetClasses(i).Field5 > 0)
					{
						var artifact = Globals.ADB[Record.GetClasses(i).Field5];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, null));
					}

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DisguisedMonster:

					var monster = Globals.MDB[Record.GetClasses(i).Field5];

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));

					break;

				case Enums.ArtifactType.DoorGate:

					if (Record.GetClasses(i).Field5 > 0)
					{
						var room = Globals.RDB[Record.GetClasses(i).Field5];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					var armor = Globals.Engine.GetArmors((Enums.Armor)Record.GetClasses(i).Field5);

					Debug.Assert(armor != null);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, armor.Name));

					break;

				case Enums.ArtifactType.DeadBody:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, Record.GetClasses(i).Field5 == 1 ? "Takeable" : "Not Takeable"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field5, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField6(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var weapon = Globals.Engine.GetWeapons((Enums.Weapon)Record.GetClasses(i).Field6);

					Debug.Assert(weapon != null);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, weapon.Name));

					break;

				case Enums.ArtifactType.Container:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetClasses(i).Field6))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetClasses(i).Field6));
					}

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, Record.IsFieldStrength(Record.GetClasses(i).Field6) ? lookupMsg : Record.GetClasses(i).Field6 == 1 ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DoorGate:

					if (Record.GetClasses(i).Field6 > 0)
					{
						var artifact = Globals.ADB[Record.GetClasses(i).Field6];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, Globals.Engine.GetClothingNames((Enums.Clothing)Record.GetClasses(i).Field6)));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field6, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField7(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Readable:
				case Enums.ArtifactType.Edible:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field7, null, Record.GetClasses(i).IsOpen() ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:

					if (Record.GetClasses(i).Field7 > 0)
					{
						var monster = Globals.MDB[Record.GetClasses(i).Field7];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field7, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field7, null, null));
					}

					break;

				case Enums.ArtifactType.DoorGate:

					var lookupMsg = string.Empty;

					if (Record.IsFieldStrength(Record.GetClasses(i).Field7))
					{
						lookupMsg = string.Format("Strength of {0}", Record.GetFieldStrength(Record.GetClasses(i).Field7));
					}

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field7, null, Record.IsFieldStrength(Record.GetClasses(i).Field7) ? lookupMsg : Record.GetClasses(i).IsOpen() ? "Open" : "Closed"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field7, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField8(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (Record.GetClasses(i).Type)
			{
				case Enums.ArtifactType.DoorGate:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field8, null, Record.GetClasses(i).Field8 == 1 ? "Hidden" : "Normal"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Record.GetClasses(i).Field8, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValue(long bufSize, char fillChar, long offset, IField field)
		{
			string result;

			if (field == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			result = null;

			if (field.BuildValue != null)
			{
				var args = Globals.CreateInstance<IBuildValueArgs>(x =>
				{
					x.BufSize = bufSize;
					x.FillChar = fillChar;
					x.Offset = offset;
				});

				result = field.BuildValue(field, args);

				if (result == null)
				{
					// PrintError

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual void SetArtifactUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetArtifactUid();

				Record.IsUidRecycled = true;
			}
			else if (!editRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Record.Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Name").GetPrintedName(), null), Record.Name);

			if (string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) || args.ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null), Record.Desc);
			}

			if (!string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null),
					Convert.ToInt64(args.ErrorField.GetValue()));
			}
		}

		#endregion

		#region Class ArtifactHelper

		public ArtifactHelper()
		{
			SetUidIfInvalid = SetArtifactUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
