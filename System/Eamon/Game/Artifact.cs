
// Artifact.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Artifact : Editable, IArtifact
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual IField NameField { get; set; }

		#endregion

		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IHaveListedName

		public virtual string Name { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		#endregion

		#region Interface IArtifact

		public virtual string StateDesc { get; set; }

		public virtual string Desc { get; set; }

		public virtual bool IsCharOwned { get; set; }

		public virtual bool IsPlural { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual Enums.PluralType PluralType { get; set; }

		public virtual long Value { get; set; }

		public virtual long Weight { get; set; }

		public virtual long Location { get; set; }

		public virtual Classes.IArtifactClass[] Classes { get; set; }

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

		#region Interface IValidator

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			var result = !string.IsNullOrWhiteSpace(Name);

			if (result && Name.Length > Constants.ArtNameLen)
			{
				for (var i = Constants.ArtNameLen; i < Name.Length; i++)
				{
					if (Name[i] != '#')
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
			return StateDesc != null && StateDesc.Length <= Constants.ArtStateDescLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Desc) == false && Desc.Length <= Constants.ArtDescLen;
		}

		protected virtual bool ValidatePluralType(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidPluralType(PluralType);
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), ArticleType);
		}

		protected virtual bool ValidateValue(IField field, IValidateArgs args)
		{
			return Value >= Constants.MinGoldValue && Value <= Constants.MaxGoldValue;
		}

		protected virtual bool ValidateClassesType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var result = true;

			var activeClass = true;

			var i = Convert.ToInt64(field.UserData);

			for (var h = 1; h <= i; h++)
			{
				if (GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				result = Globals.Engine.IsValidArtifactType(GetClasses(i).Type);

				if (result)
				{
					for (var h = 0; h < Classes.Length; h++)
					{
						if (h != i && GetClasses(h).Type != Enums.ArtifactType.None)
						{
							if ((GetClasses(h).Type == GetClasses(i).Type) ||
									(GetClasses(h).Type == Enums.ArtifactType.Gold && GetClasses(i).Type == Enums.ArtifactType.Treasure) ||
									(GetClasses(h).Type == Enums.ArtifactType.Treasure && GetClasses(i).Type == Enums.ArtifactType.Gold) ||
									(GetClasses(h).Type == Enums.ArtifactType.Weapon && GetClasses(i).Type == Enums.ArtifactType.MagicWeapon) ||
									(GetClasses(h).Type == Enums.ArtifactType.MagicWeapon && GetClasses(i).Type == Enums.ArtifactType.Weapon) ||
									(GetClasses(h).Type == Enums.ArtifactType.Container && GetClasses(i).Type == Enums.ArtifactType.DoorGate) ||
									(GetClasses(h).Type == Enums.ArtifactType.DoorGate && GetClasses(i).Type == Enums.ArtifactType.Container) ||
									(GetClasses(h).Type == Enums.ArtifactType.BoundMonster && GetClasses(i).Type == Enums.ArtifactType.DisguisedMonster) ||
									(GetClasses(h).Type == Enums.ArtifactType.DisguisedMonster && GetClasses(i).Type == Enums.ArtifactType.BoundMonster))
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
				result = GetClasses(i).Type == Enums.ArtifactType.None;
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
				if (GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = GetClasses(i).Field5 >= -50 && GetClasses(i).Field5 <= 50;

						break;

					case Enums.ArtifactType.Container:

						result = GetClasses(i).Field5 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.LightSource:

						result = GetClasses(i).Field5 >= -1;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.BoundMonster:
					case Enums.ArtifactType.DisguisedMonster:

						result = GetClasses(i).Field5 > 0;

						break;

					case Enums.ArtifactType.Wearable:

						result = Globals.Engine.IsValidArtifactArmor(GetClasses(i).Field5);

						break;

					case Enums.ArtifactType.DeadBody:

						result = GetClasses(i).Field5 >= 0 && GetClasses(i).Field5 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = GetClasses(i).Field5 == 0;
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
				if (GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = Enum.IsDefined(typeof(Enums.Weapon), GetClasses(i).Field6);

						break;

					case Enums.ArtifactType.Container:

						result = (GetClasses(i).Field6 >= 0 && GetClasses(i).Field6 <= 1) || Globals.Engine.IsArtifactFieldStrength(GetClasses(i).Field6);

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:

						result = GetClasses(i).Field6 >= 0;

						break;

					case Enums.ArtifactType.Readable:
					case Enums.ArtifactType.DisguisedMonster:

						result = GetClasses(i).Field6 > 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = GetClasses(i).Field6 >= -2;         // -2=Broken

						break;

					case Enums.ArtifactType.BoundMonster:

						result = GetClasses(i).Field6 >= -1;

						break;

					case Enums.ArtifactType.Wearable:

						result = Enum.IsDefined(typeof(Enums.Clothing), GetClasses(i).Field6);

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = GetClasses(i).Field6 == 0;
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
				if (GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = GetClasses(i).Field7 >= 1 && GetClasses(i).Field7 <= 25;

						break;

					case Enums.ArtifactType.Container:
					case Enums.ArtifactType.BoundMonster:

						result = GetClasses(i).Field7 >= 0;

						break;

					case Enums.ArtifactType.Drinkable:
					case Enums.ArtifactType.Edible:
					case Enums.ArtifactType.Readable:

						result = GetClasses(i).Field7 >= 0 && GetClasses(i).Field7 <= 1;

						break;

					case Enums.ArtifactType.DoorGate:

						result = (GetClasses(i).Field7 >= 0 && GetClasses(i).Field7 <= 1) || Globals.Engine.IsArtifactFieldStrength(GetClasses(i).Field7);

						break;

					case Enums.ArtifactType.DisguisedMonster:

						result = GetClasses(i).Field7 > 0;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = GetClasses(i).Field7 == 0;
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
				if (GetClasses(h).Type == Enums.ArtifactType.None)
				{
					activeClass = false;

					break;
				}
			}

			if (activeClass)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Weapon:
					case Enums.ArtifactType.MagicWeapon:

						result = GetClasses(i).Field8 >= 1 && GetClasses(i).Field8 <= 25;

						break;

					case Enums.ArtifactType.Container:

						result = GetClasses(i).Field8 >= 0;

						break;

					case Enums.ArtifactType.DoorGate:

						result = GetClasses(i).Field8 >= 0 && GetClasses(i).Field8 <= 1;

						break;

					default:

						// do nothing

						break;
				}
			}
			else
			{
				result = GetClasses(i).Field8 == 0;
			}

			return result;
		}

		protected virtual bool ValidateInterdependenciesDesc(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, false, false, ref invalidUid);

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

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

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

			var monUid = GetWornByMonsterUid();

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
				monUid = GetCarriedByMonsterUid();

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
					var roomUid = GetInRoomUid();

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
						roomUid = GetEmbeddedInRoomUid();

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
							var artUid = GetCarriedByContainerUid();

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

			Debug.Assert(i >= 0 && i < Classes.Length);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Container:
					{ 
						var artUid = GetClasses(i).Field5;

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
						var effectUid = GetClasses(i).Field5;

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
						var roomUid = GetClasses(i).Field5;

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
						var monUid = GetClasses(i).Field5;

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
						var monUid = GetClasses(i).Field5;

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

			Debug.Assert(i >= 0 && i < Classes.Length);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.Readable:
					{
						var effectUid = GetClasses(i).Field5;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < GetClasses(i).Field6; j++, effectUid++)
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
						var artUid = GetClasses(i).Field6;

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
						var artUid = GetClasses(i).Field6;

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
						var effectUid = GetClasses(i).Field6;

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

			Debug.Assert(i >= 0 && i < Classes.Length);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				switch (GetClasses(i).Type)
				{
					case Enums.ArtifactType.BoundMonster:
					{
						var monUid = GetClasses(i).Field7;

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
						var effectUid = GetClasses(i).Field6;

						if (effectUid > 0)
						{
							effectUid++;

							for (var j = 1; j < GetClasses(i).Field7; j++, effectUid++)
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

		#region Interface IEditable

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

			switch (GetClasses(i).Type)
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

			switch (GetClasses(i).Type)
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

			switch (GetClasses(i).Type)
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

			switch (GetClasses(i).Type)
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

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Uid, Globals.Engine.Capitalize(Name));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Name);
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

				if (!string.IsNullOrWhiteSpace(StateDesc))
				{
					args.Buf.Clear();

					args.Buf.Append(StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), StateDesc);
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
					var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Desc);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Seen));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(IsCharOwned));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(IsPlural));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(IsListed));
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

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

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
						Globals.Engine.BuildValue(51, ' ', 8, (long)PluralType, null,
						PluralType == Enums.PluralType.None ? "No change" :
						PluralType == Enums.PluralType.S ? "Use 's'" :
						PluralType == Enums.PluralType.Es ? "Use 'es'" :
						PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)PluralType);
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
						Globals.Engine.BuildValue(51, ' ', 8, (long)ArticleType, null,
						ArticleType == Enums.ArticleType.None ? "No article" :
						ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)ArticleType);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Value);
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
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Weight);
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
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Location);
				}
			}
		}

		protected virtual void ListClassesType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields || i == 0 || GetClasses(i - 1).Type != Enums.ArtifactType.None)
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)GetClasses(i).Type);
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
				if (!args.ExcludeROFields || GetClasses(i).Type != Enums.ArtifactType.None)
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetClasses(i).Field5);
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
				if (!args.ExcludeROFields || GetClasses(i).Type != Enums.ArtifactType.None)
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetClasses(i).Field6);
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
				if (!args.ExcludeROFields || GetClasses(i).Type != Enums.ArtifactType.None)
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetClasses(i).Field7);
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
				if (!args.ExcludeROFields || GetClasses(i).Type != Enums.ArtifactType.None)
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
						Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetClasses(i).Field8);
					}
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Name = args.Buf.Trim().ToString();

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

			var stateDesc = StateDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				StateDesc = args.Buf.Trim().ToString();

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

			var desc = Desc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", desc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ArtDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Desc = args.Buf.Trim().ToString();

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

			var seen = Seen;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Seen = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var isCharOwned = IsCharOwned;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isCharOwned));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IsCharOwned = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var isPlural = IsPlural;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IsPlural = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var isListed = IsListed;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IsListed = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var pluralType = PluralType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());

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

			var articleType = ArticleType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());

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

			var value = Value;

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
					Value = Convert.ToInt64(args.Buf.Trim().ToString());
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

			var artType = args.EditRec ? Globals.Engine.GetArtifactTypes(GetClasses(0).Type) : null;

			Debug.Assert(!args.EditRec || artType != null);

			var fieldDesc = args.FieldDesc;

			var weight = Weight;

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
					Weight = Convert.ToInt64(args.Buf.Trim().ToString());
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

			var artType = args.EditRec ? Globals.Engine.GetArtifactTypes(GetClasses(0).Type) : null;

			Debug.Assert(!args.EditRec || artType != null);

			var fieldDesc = args.FieldDesc;

			var location = Location;

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
					Location = Convert.ToInt64(args.Buf.Trim().ToString());
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

			if (i == 0 || GetClasses(i - 1).Type != Enums.ArtifactType.None)
			{
				var fieldDesc = args.FieldDesc;

				var type = GetClasses(i).Type;

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
						GetClasses(i).Type = (Enums.ArtifactType)Convert.ToInt64(args.Buf.Trim().ToString());
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

				if (GetClasses(i).Type != Enums.ArtifactType.None)
				{
					if (args.EditRec && GetClasses(i).Type != type)
					{
						var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

						Debug.Assert(artType != null);

						GetClasses(i).Field5 = Convert.ToInt64(artType.Field5EmptyVal);

						GetClasses(i).Field6 = Convert.ToInt64(artType.Field6EmptyVal);

						GetClasses(i).Field7 = Convert.ToInt64(artType.Field7EmptyVal);

						GetClasses(i).Field8 = Convert.ToInt64(artType.Field8EmptyVal);
					}
				}
				else
				{
					for (var k = i; k < Classes.Length; k++)
					{
						GetClasses(k).Type = Enums.ArtifactType.None;

						GetClasses(k).Field5 = 0;

						GetClasses(k).Field6 = 0;

						GetClasses(k).Field7 = 0;

						GetClasses(k).Field8 = 0;
					}
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				GetClasses(i).Type = Enums.ArtifactType.None;
			}
		}

		protected virtual void InputClassesField5(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field5 = GetClasses(i).Field5;

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
						GetClasses(i).Field5 = Convert.ToInt64(args.Buf.Trim().ToString());
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
				GetClasses(i).Field5 = 0;
			}
		}

		protected virtual void InputClassesField6(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field6 = GetClasses(i).Field6;

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
						GetClasses(i).Field6 = Convert.ToInt64(args.Buf.Trim().ToString());
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
				GetClasses(i).Field6 = 0;
			}
		}

		protected virtual void InputClassesField7(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field7 = GetClasses(i).Field7;

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
						GetClasses(i).Field7 = Convert.ToInt64(args.Buf.Trim().ToString());
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
				GetClasses(i).Field7 = 0;
			}
		}

		protected virtual void InputClassesField8(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (GetClasses(i).Type != Enums.ArtifactType.None)
			{
				var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

				Debug.Assert(artType != null);

				var fieldDesc = args.FieldDesc;

				var field8 = GetClasses(i).Field8;

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
						GetClasses(i).Field8 = Convert.ToInt64(args.Buf.Trim().ToString());
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
				GetClasses(i).Field8 = 0;
			}
		}

		#endregion

		#endregion

		#region Class Artifact

		protected virtual string BuildValueWeight(IField field, IBuildValueArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Weight, null, IsUnmovable() ? "Unmovable" : null));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueLocation(IField field, IBuildValueArgs args)
		{
			string lookupMsg;

			Debug.Assert(args != null && args.Buf != null);

			if (IsCarriedByCharacter())
			{
				lookupMsg = "Carried by Player Character";
			}
			else if (IsWornByCharacter())
			{
				lookupMsg = "Worn by Player Character";
			}
			else if (IsCarriedByMonster())
			{
				var monster = GetCarriedByMonster();

				lookupMsg = string.Format("Carried by {0}",
					monster != null ? Globals.Engine.Capitalize(monster.Name.Length > 29 ? monster.Name.Substring(0, 26) + "..." : monster.Name) : Globals.Engine.UnknownName);
			}
			else if (IsWornByMonster())
			{
				var monster = GetWornByMonster();

				lookupMsg = string.Format("Worn by {0}",
					monster != null ? Globals.Engine.Capitalize(monster.Name.Length > 32 ? monster.Name.Substring(0, 29) + "..." : monster.Name) : Globals.Engine.UnknownName);
			}
			else if (IsCarriedByContainer())
			{
				var artifact = GetCarriedByContainer();

				lookupMsg = string.Format("Inside {0}",
					artifact != null ? Globals.Engine.Capitalize(artifact.Name.Length > 33 ? artifact.Name.Substring(0, 30) + "..." : artifact.Name) : Globals.Engine.UnknownName);
			}
			else if (IsEmbeddedInRoom())
			{
				var room = GetEmbeddedInRoom();

				lookupMsg = string.Format("Embedded in {0}",
					room != null ? Globals.Engine.Capitalize(room.Name.Length > 28 ? room.Name.Substring(0, 25) + "..." : room.Name) : Globals.Engine.UnknownName);
			}
			else if (IsInRoom())
			{
				var room = GetInRoom();

				lookupMsg = room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName;
			}
			else
			{
				lookupMsg = null;
			}

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, Location, null, lookupMsg));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesType(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var artType = Globals.Engine.GetArtifactTypes(GetClasses(i).Type);

			args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, (long)GetClasses(i).Type, null, artType != null ? artType.Name : "None"));

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField5(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var stringVal = string.Format("{0}%", GetClasses(i).Field5);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, 0, stringVal, null));

					break;

				case Enums.ArtifactType.Container:

					if (GetClasses(i).Field5 > 0)
					{
						var artifact = Globals.ADB[GetClasses(i).Field5];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, null));
					}

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DisguisedMonster:

					var monster = Globals.MDB[GetClasses(i).Field5];

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));

					break;

				case Enums.ArtifactType.DoorGate:

					if (GetClasses(i).Field5 > 0)
					{
						var room = Globals.RDB[GetClasses(i).Field5];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					var armor = Globals.Engine.GetArmors((Enums.Armor)GetClasses(i).Field5);

					Debug.Assert(armor != null);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, armor.Name));

					break;

				case Enums.ArtifactType.DeadBody:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, GetClasses(i).Field5 == 1 ? "Takeable" : "Not Takeable"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field5, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField6(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (GetClasses(i).Type)
			{
				case Enums.ArtifactType.Weapon:
				case Enums.ArtifactType.MagicWeapon:

					var weapon = Globals.Engine.GetWeapons((Enums.Weapon)GetClasses(i).Field6);

					Debug.Assert(weapon != null);

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, weapon.Name));

					break;

				case Enums.ArtifactType.Container:

					var lookupMsg = string.Empty;

					if (IsFieldStrength(GetClasses(i).Field6))
					{
						lookupMsg = string.Format("Strength of {0}", GetFieldStrength(GetClasses(i).Field6));
					}

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, IsFieldStrength(GetClasses(i).Field6) ? lookupMsg : GetClasses(i).Field6 == 1 ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:
				case Enums.ArtifactType.DoorGate:

					if (GetClasses(i).Field6 > 0)
					{
						var artifact = Globals.ADB[GetClasses(i).Field6];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, null));
					}

					break;

				case Enums.ArtifactType.Wearable:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, Globals.Engine.GetClothingNames((Enums.Clothing)GetClasses(i).Field6)));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field6, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField7(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (GetClasses(i).Type)
			{
				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Readable:
				case Enums.ArtifactType.Edible:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field7, null, GetClasses(i).IsOpen() ? "Open" : "Closed"));

					break;

				case Enums.ArtifactType.BoundMonster:

					if (GetClasses(i).Field7 > 0)
					{
						var monster = Globals.MDB[GetClasses(i).Field7];

						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field7, null, monster != null ? Globals.Engine.Capitalize(monster.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field7, null, null));
					}

					break;

				case Enums.ArtifactType.DoorGate:

					var lookupMsg = string.Empty;

					if (IsFieldStrength(GetClasses(i).Field7))
					{
						lookupMsg = string.Format("Strength of {0}", GetFieldStrength(GetClasses(i).Field7));
					}

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field7, null, IsFieldStrength(GetClasses(i).Field7) ? lookupMsg : GetClasses(i).IsOpen() ? "Open" : "Closed"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field7, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual string BuildValueClassesField8(IField field, IBuildValueArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			switch (GetClasses(i).Type)
			{
				case Enums.ArtifactType.DoorGate:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field8, null, GetClasses(i).Field8 == 1 ? "Hidden" : "Normal"));

					break;

				default:

					args.Buf.Append(Globals.Engine.BuildValue(args.BufSize, args.FillChar, args.Offset, GetClasses(i).Field8, null, null));

					break;
			}

			return args.Buf.ToString();
		}

		protected virtual void SetArtifactUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetArtifactUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

		public override void FreeFields()
		{
			base.FreeFields();

			NameField = null;
		}

		public override IList<IField> GetFields()
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
						x.GetValue = () => Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => IsUidRecycled;
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
						x.GetValue = () => StateDesc;
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
						x.GetValue = () => Desc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Seen";
						x.PrintDesc = PrintDescSeen;
						x.List = ListSeen;
						x.Input = InputSeen;
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsCharOwned";
						x.PrintDesc = PrintDescIsCharOwned;
						x.List = ListIsCharOwned;
						x.Input = InputIsCharOwned;
						x.GetPrintedName = () => "Is Char Owned";
						x.GetValue = () => IsCharOwned;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsPlural";
						x.PrintDesc = PrintDescIsPlural;
						x.List = ListIsPlural;
						x.Input = InputIsPlural;
						x.GetPrintedName = () => "Is Plural";
						x.GetValue = () => IsPlural;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsListed";
						x.PrintDesc = PrintDescIsListed;
						x.List = ListIsListed;
						x.Input = InputIsListed;
						x.GetPrintedName = () => "Is Listed";
						x.GetValue = () => IsListed;
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
						x.GetValue = () => PluralType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.PrintDesc = PrintDescArticleType;
						x.List = ListArticleType;
						x.Input = InputArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Value";
						x.Validate = ValidateValue;
						x.PrintDesc = PrintDescValue;
						x.List = ListValue;
						x.Input = InputValue;
						x.GetPrintedName = () => "Value";
						x.GetValue = () => Value;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Weight";
						x.PrintDesc = PrintDescWeight;
						x.List = ListWeight;
						x.Input = InputWeight;
						x.BuildValue = BuildValueWeight;
						x.GetPrintedName = () => "Weight";
						x.GetValue = () => Weight;
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
						x.GetValue = () => Location;
					})
				};

				for (var i = 0; i < Classes.Length; i++)
				{
					var j = i;

					Fields.Add
					(
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
							x.GetValue = () => GetClasses(j).Type;
						})
					);

					Fields.Add
					(
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
								var artType = Globals.Engine.GetArtifactTypes(GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field5Name : "Field5");
							};
							x.GetValue = () => GetClasses(j).Field5;
						})
					);

					Fields.Add
					(
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
								var artType = Globals.Engine.GetArtifactTypes(GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field6Name : "Field6");
							};
							x.GetValue = () => GetClasses(j).Field6;
						})
					);

					Fields.Add
					(
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
								var artType = Globals.Engine.GetArtifactTypes(GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field7Name : "Field7");
							};
							x.GetValue = () => GetClasses(j).Field7;
						})
					);

					Fields.Add
					(
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
								var artType = Globals.Engine.GetArtifactTypes(GetClasses(j).Type);

								return string.Format("Cls #{0} {1}", j + 1, artType != null ? artType.Field8Name : "Field8");
							};
							x.GetValue = () => GetClasses(j).Field8;
						})
					);
				}
			}

			return Fields;
		}

		#endregion

		#region Interface IHaveChildren

		public virtual void SetParentReferences()
		{
			foreach (var ac in Classes)
			{
				if (ac != null)
				{
					ac.Parent = this;
				}
			}
		}

		#endregion

		#region Interface IHaveListedName

		public virtual string GetPluralName(IField field, StringBuilder buf)
		{
			IEffect effect;
			long effectUid;
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

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

		public virtual string GetPluralName01(StringBuilder buf)
		{
			return GetPluralName(GetField("Name"), buf);
		}

		public virtual string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			switch (articleType)
			{
				case Enums.ArticleType.None:

					buf.AppendFormat
					(
						"{0}{1}{2}",
						EvalPlural(Name, GetPluralName(field, new StringBuilder(Constants.BufSize))),
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
						EvalPlural(Name, GetPluralName(field, new StringBuilder(Constants.BufSize))),
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
						EvalPlural(Name, GetPluralName(field, new StringBuilder(Constants.BufSize))),
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

		public virtual string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
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

		public virtual IField GetNameField()
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
					x.GetValue = () => Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Interface IEditable

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Name").GetPrintedName(), null), Name);

			if (string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) || args.ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null), Desc);
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

		#region Interface IComparable

		public virtual int CompareTo(IArtifact artifact)
		{
			return this.Uid.CompareTo(artifact.Uid);
		}

		#endregion

		#region Interface IArtifact

		public virtual Classes.IArtifactClass GetClasses(long index)
		{
			return Classes[index];
		}

		public virtual string GetSynonyms(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetClasses(long index, Classes.IArtifactClass value)
		{
			Classes[index] = value;
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
			return Location > 2000;
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
			var ac = GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

			return ac != null && ac.IsWeapon(weapon);
		}

		public virtual bool IsAttackable()
		{
			var ac = GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.DeadBody, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DoorGate });

			return ac != null && (ac.Type == Enums.ArtifactType.DeadBody || ac.Type == Enums.ArtifactType.DisguisedMonster || ac.GetBreakageStrength() >= 1000);
		}

		public virtual bool IsAttackable01(ref Classes.IArtifactClass ac)
		{
			ac = GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.DeadBody, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DoorGate }, false);

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
			return GetArtifactClass(Enums.ArtifactType.Gold) != null;
		}

		public virtual bool IsTreasure()
		{
			return GetArtifactClass(Enums.ArtifactType.Treasure) != null;
		}

		public virtual bool IsWeapon()
		{
			return GetArtifactClass(Enums.ArtifactType.Weapon) != null;
		}

		public virtual bool IsWeapon01()
		{
			var ac = GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

			return ac != null;
		}

		public virtual bool IsMagicWeapon()
		{
			return GetArtifactClass(Enums.ArtifactType.MagicWeapon) != null;
		}

		public virtual bool IsContainer()
		{
			return GetArtifactClass(Enums.ArtifactType.Container) != null;
		}

		public virtual bool IsLightSource()
		{
			return GetArtifactClass(Enums.ArtifactType.LightSource) != null;
		}

		public virtual bool IsDrinkable()
		{
			return GetArtifactClass(Enums.ArtifactType.Drinkable) != null;
		}

		public virtual bool IsReadable()
		{
			return GetArtifactClass(Enums.ArtifactType.Readable) != null;
		}

		public virtual bool IsDoorGate()
		{
			return GetArtifactClass(Enums.ArtifactType.DoorGate) != null;
		}

		public virtual bool IsEdible()
		{
			return GetArtifactClass(Enums.ArtifactType.Edible) != null;
		}

		public virtual bool IsBoundMonster()
		{
			return GetArtifactClass(Enums.ArtifactType.BoundMonster) != null;
		}

		public virtual bool IsWearable()
		{
			return GetArtifactClass(Enums.ArtifactType.Wearable) != null;
		}

		public virtual bool IsArmor()
		{
			var ac = GetArtifactClass(Enums.ArtifactType.Wearable);

			return ac != null && ac.Field5 > 1;
		}

		public virtual bool IsShield()
		{
			var ac = GetArtifactClass(Enums.ArtifactType.Wearable);

			return ac != null && ac.Field5 == 1;
		}

		public virtual bool IsDisguisedMonster()
		{
			return GetArtifactClass(Enums.ArtifactType.DisguisedMonster) != null;
		}

		public virtual bool IsDeadBody()
		{
			return GetArtifactClass(Enums.ArtifactType.DeadBody) != null;
		}

		public virtual bool IsUser1()
		{
			return GetArtifactClass(Enums.ArtifactType.User1) != null;
		}

		public virtual bool IsUser2()
		{
			return GetArtifactClass(Enums.ArtifactType.User2) != null;
		}

		public virtual bool IsUser3()
		{
			return GetArtifactClass(Enums.ArtifactType.User3) != null;
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return IsPlural ? pluralValue : singularValue;
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsEmbeddedInRoomLit() ? lightValue : darkValue;
		}

		public virtual Classes.IArtifactClass GetArtifactClass(Enums.ArtifactType artifactType)
		{
			if (GetClasses(0) != null && GetClasses(0).Type != Enums.ArtifactType.None)
			{
				return Classes.FirstOrDefault(ac => ac != null && ac.Type == artifactType);
			}
			else
			{
				return null;
			}
		}

		public virtual Classes.IArtifactClass GetArtifactClass(Enums.ArtifactType[] artifactTypes, bool classArrayPrecedence = true)
		{
			Classes.IArtifactClass result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetClasses(0) != null && GetClasses(0).Type != Enums.ArtifactType.None)
			{
				if (classArrayPrecedence)
				{
					result = Classes.FirstOrDefault(ac => ac != null && artifactTypes.Contains(ac.Type));
				}
				else
				{
					foreach (var at in artifactTypes)
					{
						result = Classes.FirstOrDefault(ac => ac != null && ac.Type == at);

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

		public virtual IList<Classes.IArtifactClass> GetArtifactClasses(Enums.ArtifactType[] artifactTypes)
		{
			IList<Classes.IArtifactClass> result = null;

			if (artifactTypes == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (GetClasses(0) != null && GetClasses(0).Type != Enums.ArtifactType.None)
			{
				result = Classes.Where(ac => ac != null && artifactTypes.Contains(ac.Type)).ToList();
			}
			else
			{
				result = new List<Classes.IArtifactClass>() { null };
			}

		Cleanup:

			return result;
		}

		public virtual RetCode SetArtifactClassCount(long count)
		{
			RetCode rc;

			if (count < 1 || count > Constants.NumArtifactClasses)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var classes01 = new Classes.IArtifactClass[count];

			var i = 0L;

			if (classes01.Length < Classes.Length)
			{
				while (i < classes01.Length)
				{
					classes01[i] = GetClasses(i);

					i++;
				}
			}
			else
			{
				while (i < Classes.Length)
				{
					classes01[i] = GetClasses(i);

					i++;
				}

				while (i < classes01.Length)
				{
					classes01[i] = Globals.CreateInstance<Classes.IArtifactClass>(x =>
					{
						x.Parent = this;
					});

					i++;
				}
			}

			Classes = classes01;

		Cleanup:

			return rc;
		}

		public virtual RetCode SyncArtifactClasses(Classes.IArtifactClass artifactClass)
		{
			RetCode rc;

			if (artifactClass == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			switch (artifactClass.Type)
			{
				case Enums.ArtifactType.Container:
				case Enums.ArtifactType.DoorGate:
				{
					foreach (var ac in Classes)
					{
						if (ac != null && ac != artifactClass && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.IsLockable())
							{
								ac.SetKeyUid(artifactClass.GetKeyUid());
							}

							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactClass.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.Drinkable:
				case Enums.ArtifactType.Edible:
				{
					foreach (var ac in Classes)
					{
						if (ac != null && ac != artifactClass && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.Drinkable || ac.Type == Enums.ArtifactType.Edible)
							{
								ac.Field5 = artifactClass.Field5;

								ac.Field6 = artifactClass.Field6;
							}

							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactClass.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.Readable:
				{
					foreach (var ac in Classes)
					{
						if (ac != null && ac != artifactClass && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.IsOpenable())
							{
								ac.SetOpen(artifactClass.IsOpen());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.BoundMonster:
				{
					foreach (var ac in Classes)
					{
						if (ac != null && ac != artifactClass && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.DisguisedMonster)
							{
								ac.Field5 = artifactClass.Field5;
							}

							if (ac.IsLockable())
							{
								ac.SetKeyUid(artifactClass.GetKeyUid());
							}
						}
					}

					break;
				}

				case Enums.ArtifactType.DisguisedMonster:
				{
					foreach (var ac in Classes)
					{
						if (ac != null && ac != artifactClass && ac.Type != Enums.ArtifactType.None)
						{
							if (ac.Type == Enums.ArtifactType.BoundMonster)
							{
								ac.Field5 = artifactClass.Field5;
							}
						}
					}

					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SyncArtifactClasses()
		{
			RetCode rc;

			rc = RetCode.Success;

			foreach (var ac in Classes)
			{
				if (ac != null && ac.Type != Enums.ArtifactType.None)
				{
					rc = SyncArtifactClasses(ac);

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

		public virtual string BuildValue(long bufSize, char fillChar, long offset, IField field)
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

		#endregion

		#region Class Artifact

		public Artifact()
		{
			SetUidIfInvalid = SetArtifactUidIfInvalid;

			IsUidRecycled = true;

			Name = "";

			StateDesc = "";

			Desc = "";

			Classes = new Classes.IArtifactClass[]
			{
				Globals.CreateInstance<Classes.IArtifactClass>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactClass>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactClass>(x =>
				{
					x.Parent = this;
				}),
				Globals.CreateInstance<Classes.IArtifactClass>(x =>
				{
					x.Parent = this;
				})
			};
		}

		#endregion

		#endregion
	}
}
