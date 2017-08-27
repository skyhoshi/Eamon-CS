
// Room.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Room : Editable, IRoom
	{
		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IRoom

		public virtual string Name { get; set; }

		public virtual bool Seen { get; set; }

		public virtual string Desc { get; set; }

		public virtual Enums.LightLevel LightLvl { get; set; }

		public virtual Enums.RoomType Type { get; set; }

		public virtual long Zone { get; set; }

		public virtual long[] Dirs { get; set; }

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
				Globals.Database.FreeRoomUid(Uid);

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
			return string.IsNullOrWhiteSpace(Name) == false && Name.Length <= Constants.RmNameLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Desc) == false && Desc.Length <= Constants.RmDescLen;
		}

		protected virtual bool ValidateLightLvl(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), LightLvl);
		}

		protected virtual bool ValidateType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.RoomType), Type);
		}

		protected virtual bool ValidateZone(IField field, IValidateArgs args)
		{
			return Zone > 0;
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

		protected virtual bool ValidateInterdependenciesDirs(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			var i = Convert.ToInt64(field.UserData);

			var dv = (Enums.Direction)i;

			if (Globals.Engine.IsValidDirection(dv))
			{
				if (IsDirectionRoom(dv))
				{
					var roomUid = GetDirs(i);

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
				else if (IsDirectionDoor(dv))
				{
					var artUid = GetDirectionDoorUid(dv);

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
					else if (!artifact.IsDoorGate())
					{
						result = false;

						args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be a door/gate, but isn't");

						args.ErrorMessage = args.Buf.ToString();

						args.RecordType = typeof(IArtifact);

						args.EditRecord = artifact;

						goto Cleanup;
					}
					else if (!artifact.IsInRoom(this) && !artifact.IsEmbeddedInRoom(this))
					{
						result = false;

						args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be located in this room, but isn't");

						args.ErrorMessage = args.Buf.ToString();

						args.RecordType = typeof(IArtifact);

						args.EditRecord = artifact;

						goto Cleanup;
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
			var fullDesc = "Enter the name of the room." + Environment.NewLine + Environment.NewLine + "Room names should always be able to stand alone inside a pair of brackets: [Room Name].";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter a detailed description of the room.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescSeen(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Seen status of the room.";

			var briefDesc = "0=Not seen; 1=Seen";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescLightLvl(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the level of light in the room.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var lightLevelValues = EnumUtil.GetValues<Enums.LightLevel>();

			for (var j = 0; j < lightLevelValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)lightLevelValues[j], Globals.Engine.GetLightLevelNames(lightLevelValues[j]));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescType(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the type of the room.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var roomTypeValues = EnumUtil.GetValues<Enums.RoomType>();

			for (var j = 0; j < roomTypeValues.Count; j++)
			{
				var roomType = Globals.Engine.GetRoomTypes(roomTypeValues[j]);

				Debug.Assert(roomType != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)roomTypeValues[j], roomType.Name);
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescZone(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the zone of the room.";

			var briefDesc = "(GT 0)=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescDirs(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = "Enter a connection value for the direction.";

			var fullDesc01 = "Enter a connection value for each direction the player can move in.";

			var briefDesc = "-999=Return to Main Hall; (LT 0)=Special (user programmed) event; 0=No connection; 1-1000=Room uid; (1000 + N)=Door with artifact uid N";

			if ((args.EditRec && args.EditField) || i == 1)
			{
				if (args.FieldDesc == Enums.FieldDesc.Full)
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, (args.EditRec && args.EditField) ? fullDesc : fullDesc01, briefDesc);
				}
				else if (args.FieldDesc == Enums.FieldDesc.Brief)
				{
					args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, briefDesc);
				}
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

		protected virtual void ListLightLvl(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)LightLvl);
			}
		}

		protected virtual void ListType(IField field, IListArgs args)
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
					var roomType = Globals.Engine.GetRoomTypes(Type);

					Debug.Assert(roomType != null);

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Type, null, roomType.Name));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Type);
				}
			}
		}

		protected virtual void ListZone(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Zone);
			}
		}

		protected virtual void ListDirs(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var dv = (Enums.Direction)i;
			
			if (args.FullDetail && Globals.Engine.IsValidDirection(dv))
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					string lookupMsg = null;

					if (IsDirectionRoom(dv))
					{
						var room = Globals.RDB[GetDirs(i)];

						lookupMsg = room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName;
					}
					else if (IsDirectionExit(dv))
					{
						lookupMsg = "Exit";
					}
					else if (IsDirectionDoor(dv))
					{
						var artifact = GetDirectionDoor(dv);

						lookupMsg = artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName;
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, GetDirs(i), null, lookupMsg));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GetDirs(i));
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

				var rc = Globals.In.ReadField(args.Buf, Constants.RmNameLen, null, '_', '\0', false, null, null, null, null);

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

				var rc = Globals.In.ReadField(args.Buf, Constants.RmDescLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputLightLvl(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var lightLvl = LightLvl;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)lightLvl);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				LightLvl = (Enums.LightLevel)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var type = Type;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)type);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Type = (Enums.RoomType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputZone(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var zone = Zone;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", zone);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Zone = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputDirs(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var dv = (Enums.Direction)i;

			if (Globals.Engine.IsValidDirection(dv))
			{
				var fieldDesc = args.FieldDesc;

				var value = GetDirs(i);

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", value);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

					var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var error = false;

					try
					{
						SetDirs(i, Convert.ToInt64(args.Buf.Trim().ToString()));
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
				SetDirs(i, 0);
			}
		}

		#endregion

		#endregion

		#region Class Room

		protected virtual void SetRoomUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetRoomUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		protected virtual string GetObviousExits()
		{
			var roomType = Globals.Engine.GetRoomTypes(Type);

			Debug.Assert(roomType != null);

			return string.Format("{0}Obvious {1}:  ", Environment.NewLine, roomType.ExitDesc);
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

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
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Name";
						x.Validate = ValidateName;
						x.PrintDesc = PrintDescName;
						x.List = ListName;
						x.Input = InputName;
						x.GetPrintedName = () => "Name";
						x.GetValue = () => Name;
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
						x.Name = "LightLvl";
						x.Validate = ValidateLightLvl;
						x.PrintDesc = PrintDescLightLvl;
						x.List = ListLightLvl;
						x.Input = InputLightLvl;
						x.GetPrintedName = () => "Light Level";
						x.GetValue = () => LightLvl;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Type";
						x.Validate = ValidateType;
						x.PrintDesc = PrintDescType;
						x.List = ListType;
						x.Input = InputType;
						x.GetPrintedName = () => "Type";
						x.GetValue = () => Type;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Zone";
						x.Validate = ValidateZone;
						x.PrintDesc = PrintDescZone;
						x.List = ListZone;
						x.Input = InputZone;
						x.GetPrintedName = () => "Zone";
						x.GetValue = () => Zone;
					})
				};

				var directionValues = EnumUtil.GetValues<Enums.Direction>();

				foreach (var dv in directionValues)
				{
					var i = (long)dv;

					var direction = Globals.Engine.GetDirections(dv);

					Debug.Assert(direction != null);

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Dirs[{0}]", i);
							x.UserData = i;
							x.ValidateInterdependencies = ValidateInterdependenciesDirs;
							x.PrintDesc = PrintDescDirs;
							x.List = ListDirs;
							x.Input = InputDirs;
							x.GetPrintedName = () => direction.Name;
							x.GetValue = () => GetDirs(i);
						})
					);
				}
			}

			return Fields;
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

			if (args.ErrorField.Name.StartsWith("Dirs[", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Assert(args.ErrorField.UserData != null);

				var i = Convert.ToInt64(args.ErrorField.UserData);

				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), GetDirs(i));
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IRoom room)
		{
			return this.Uid.CompareTo(room.Uid);
		}

		#endregion

		#region Interface IRoom

		public virtual long GetDirs(long index)
		{
			return Dirs[index];
		}

		public virtual long GetDirs(Enums.Direction dir)
		{
			return GetDirs((long)dir);
		}

		public virtual void SetDirs(long index, long value)
		{
			Dirs[index] = value;
		}

		public virtual void SetDirs(Enums.Direction dir, long value)
		{
			SetDirs((long)dir, value);
		}

		public virtual bool IsLit()
		{
			var gameState = Globals?.Engine.GetGameState();

			return gameState != null && Uid == gameState.Ro ? gameState.Lt != 0 : LightLvl == Enums.LightLevel.Light;
		}

		public virtual bool IsDirectionInvalid(Enums.Direction dir)
		{
			return GetDirs(dir) == 0;
		}

		public virtual bool IsDirectionRoom(Enums.Direction dir)
		{
			return GetDirs(dir) > 0 && GetDirs(dir) < 1001;
		}

		public virtual bool IsDirectionExit(Enums.Direction dir)
		{
			return GetDirs(dir) == Constants.DirectionExit;
		}

		public virtual bool IsDirectionDoor(Enums.Direction dir)
		{
			return GetDirs(dir) > 1000;
		}

		public virtual bool IsDirectionSpecial(Enums.Direction dir, bool includeExit = true)
		{
			return GetDirs(dir) < 0 && (includeExit || !IsDirectionExit(dir));
		}

		public virtual long GetDirectionDoorUid(Enums.Direction dir)
		{
			return IsDirectionDoor(dir) ? GetDirs(dir) - 1000 : 0;
		}

		public virtual IArtifact GetDirectionDoor(Enums.Direction dir)
		{
			var uid = GetDirectionDoorUid(dir);

			return Globals.ADB[uid];
		}

		public virtual bool IsMonsterListedInRoom(IMonster monster)
		{
			if (monster != null && monster.IsInRoom(this))
			{
				if (monster.IsListed == false)
				{
					monster.Seen = true;
				}

				return monster.IsListed;
			}
			else
			{
				return false;
			}
		}

		public virtual bool IsArtifactListedInRoom(IArtifact artifact)
		{
			if (artifact != null && artifact.IsInRoom(this))
			{
				if (artifact.IsListed == false)
				{
					artifact.Seen = true;
				}

				return artifact.IsListed;
			}
			else
			{
				return false;
			}
		}

		public virtual T EvalLightLevel<T>(T darkValue, T lightValue)
		{
			return IsLit() ? lightValue : darkValue;
		}

		public virtual T EvalRoomType<T>(T indoorsValue, T outdoorsValue)
		{
			return Type == Enums.RoomType.Indoors ? indoorsValue : outdoorsValue;
		}

		public virtual IList<IArtifact> GetTakeableList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = a =>
				{
					var result = a.IsInRoom(this) && a.Weight <= 900 && !a.IsUnmovable01();

					if (result)
					{
						var ac = a.GetArtifactClass(Enums.ArtifactType.DeadBody);

						if (ac != null && ac.Field5 != 1)
						{
							result = false;
						}
					}

					return result;
				};
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => roomFindFunc(a));

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

		public virtual IList<IArtifact> GetEmbeddedList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = a => a.IsEmbeddedInRoom(this);
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => roomFindFunc(a));

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

		public virtual IList<IHaveListedName> GetContainedList(Func<IHaveListedName, bool> roomFindFunc = null, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = x =>
				{
					var m = x as IMonster;

					if (m != null)
					{
						return m.IsInRoom(this);      // && ! m.IsCharacterMonster()
					}
					else
					{
						var a = x as IArtifact;

						Debug.Assert(a != null);

						return a.IsInRoom(this);
					}
				};
			}

			var list = new List<IHaveListedName>();

			list.AddRange(Globals.Engine.GetMonsterList(() => true, m => roomFindFunc(m)));

			list.AddRange(Globals.Engine.GetArtifactList(() => true, a => roomFindFunc(a)));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var x in list)
				{
					var m = x as IMonster;

					if (m != null)
					{
						list01.AddRange(m.GetContainedList(monsterFindFunc, artifactFindFunc, recurse));
					}
					else
					{
						var a = x as IArtifact;

						Debug.Assert(a != null);

						if (a.IsContainer())
						{
							list01.AddRange(a.GetContainedList(artifactFindFunc, recurse));
						}
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			RetCode rc;
			long i, j;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			i = 0;

			var directionValues = EnumUtil.GetValues<Enums.Direction>();

			foreach (var dv in directionValues)
			{
				if (IsDirectionRoom(dv) || IsDirectionExit(dv))
				{
					i++;
				}
			}

			if (i > 0)
			{
				j = 0;

				foreach (var dv in directionValues)
				{
					if (IsDirectionRoom(dv) || IsDirectionExit(dv))
					{
						var direction = Globals.Engine.GetDirections(dv);

						Debug.Assert(direction != null);

						buf.AppendFormat("{0}{1}",
							j == 0 ? "" : j == i - 1 ? " and " : ", ",
							useNames ? (modFunc != null ? modFunc(direction.Name) : direction.Name) :
							(modFunc != null ? modFunc(direction.Abbr) : direction.Abbr));

						if (++j == i)
						{
							break;
						}
					}
				}
			}
			else
			{
				buf.Append("none");
			}

		Cleanup:

			return rc;
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
					Name);
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

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false)
		{
			bool showDesc;
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			showDesc = false;

			if (monsterFindFunc == null)
			{
				monsterFindFunc = IsMonsterListedInRoom;
			}

			var monsters = Globals.Engine.GetMonsterList(() => true, m => monsterFindFunc(m));

			if (artifactFindFunc == null)
			{
				artifactFindFunc = IsArtifactListedInRoom;
			}

			var artifacts = Globals.Engine.GetArtifactList(() => true, a => artifactFindFunc(a));

			buf.AppendFormat("{0}[{1}]", Environment.NewLine, Name);

			if (verboseRoomDesc || Seen == false)
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);

				if (monsters.Any() || artifacts.Any())
				{
					while (buf.Length > 0 && Char.IsWhiteSpace(buf[buf.Length - 1]))
					{
						buf.Length--;
					}

					buf.Append("  ");
				}

				showDesc = true;

				Seen = true;
			}

			var combined = new List<IHaveListedName>();

			if (!verboseMonsterDesc)
			{
				combined.AddRange(monsters.Where(m => m.Seen));
			}

			if (!verboseArtifactDesc)
			{
				combined.AddRange(artifacts.Where(a => a.Seen));
			}

			if (combined.Any())
			{
				buf.AppendFormat("{0}You {1}{2}",
					!showDesc ? Environment.NewLine : "",
					showDesc ? "also " : "",
					showDesc && !monsters.Any() ? "notice " : "see ");

				rc = Globals.Engine.GetRecordNameList(combined, Enums.ArticleType.A, true, true, false, buf);

				if (Globals.Engine.IsFailure(rc))
				{
					// PrintError

					goto Cleanup;
				}

				buf.Append(".");
			}
			else if (!showDesc)
			{
				buf.Append(Environment.NewLine);
			}

			buf.AppendFormat(GetObviousExits());

			rc = GetExitList(buf, s => s.ToLower());

			if (Globals.Engine.IsFailure(rc))
			{
				// PrintError

				goto Cleanup;
			}

			buf.AppendFormat(".{0}", Environment.NewLine);

			combined.Clear();

			combined.AddRange(monsters.Where(m => verboseMonsterDesc || !m.Seen));

			combined.AddRange(artifacts.Where(a => verboseArtifactDesc || !a.Seen));

			foreach (var r in combined)
			{
				rc = r.BuildPrintedFullDesc(buf, true);

				if (Globals.Engine.IsFailure(rc))
				{
					// PrintError

					goto Cleanup;
				}

				r.Seen = true;
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Room

		public Room()
		{
			SetUidIfInvalid = SetRoomUidIfInvalid;

			IsUidRecycled = true;

			Name = "";

			Desc = "";

			Dirs = new long[(long)EnumUtil.GetLastValue<Enums.Direction>() + 1];
		}

		#endregion

		#endregion
	}
}
