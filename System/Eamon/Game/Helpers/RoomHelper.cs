
// RoomHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
	[ClassMappings(typeof(IHelper<IRoom>))]
	public class RoomHelper : Helper<IRoom>
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
			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.RmNameLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.RmDescLen;
		}

		protected virtual bool ValidateLightLvl(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), Record.LightLvl);
		}

		protected virtual bool ValidateType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.RoomType), Record.Type);
		}

		protected virtual bool ValidateZone(IField field, IValidateArgs args)
		{
			return Record.Zone > 0;
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

		protected virtual bool ValidateInterdependenciesDirs(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			var i = Convert.ToInt64(field.UserData);

			var dv = (Enums.Direction)i;

			if (Globals.Engine.IsValidDirection(dv))
			{
				if (Record.IsDirectionRoom(dv))
				{
					var roomUid = Record.GetDirs(i);

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
				else if (Record.IsDirectionDoor(dv))
				{
					var artUid = Record.GetDirectionDoorUid(dv);

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
					else if (!artifact.IsInRoom(Record) && !artifact.IsEmbeddedInRoom(Record))
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
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)roomTypeValues[j], Globals.Engine.EvalRoomType(roomTypeValues[j], "Indoors", "Outdoors"));
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
					args.Buf.AppendPrint("{0}", briefDesc);
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

		protected virtual void ListLightLvl(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.LightLvl);
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
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Type, null, Record.EvalRoomType("Indoors", "Outdoors")));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Type);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Zone);
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

					if (Record.IsDirectionRoom(dv))
					{
						var room = Globals.RDB[Record.GetDirs(i)];

						lookupMsg = room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName;
					}
					else if (Record.IsDirectionExit(dv))
					{
						lookupMsg = "Exit";
					}
					else if (Record.IsDirectionDoor(dv))
					{
						var artifact = Record.GetDirectionDoor(dv);

						lookupMsg = artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName;
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.GetDirs(i), null, lookupMsg));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GetDirs(i));
				}
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.RmNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.RmDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
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

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputLightLvl(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var lightLvl = Record.LightLvl;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)lightLvl);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.LightLvl = (Enums.LightLevel)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var type = Record.Type;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)type);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Type = (Enums.RoomType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputZone(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var zone = Record.Zone;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", zone);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Zone = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputDirs(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			var dv = (Enums.Direction)i;

			if (Globals.Engine.IsValidDirection(dv))
			{
				var fieldDesc = args.FieldDesc;

				var value = Record.GetDirs(i);

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
						Record.SetDirs(i, Convert.ToInt64(args.Buf.Trim().ToString()));
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

				Globals.Out.Print("{0}", Globals.LineSep);
			}
			else
			{
				Record.SetDirs(i, 0);
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
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Name";
						x.Validate = ValidateName;
						x.PrintDesc = PrintDescName;
						x.List = ListName;
						x.Input = InputName;
						x.GetPrintedName = () => "Name";
						x.GetValue = () => Record.Name;
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
						x.Name = "LightLvl";
						x.Validate = ValidateLightLvl;
						x.PrintDesc = PrintDescLightLvl;
						x.List = ListLightLvl;
						x.Input = InputLightLvl;
						x.GetPrintedName = () => "Light Level";
						x.GetValue = () => Record.LightLvl;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Type";
						x.Validate = ValidateType;
						x.PrintDesc = PrintDescType;
						x.List = ListType;
						x.Input = InputType;
						x.GetPrintedName = () => "Type";
						x.GetValue = () => Record.Type;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Zone";
						x.Validate = ValidateZone;
						x.PrintDesc = PrintDescZone;
						x.List = ListZone;
						x.Input = InputZone;
						x.GetPrintedName = () => "Zone";
						x.GetValue = () => Record.Zone;
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
							x.GetValue = () => Record.GetDirs(i);
						})
					);
				}
			}

			return Fields;
		}

		#endregion

		#region Class RoomHelper

		protected virtual void SetRoomUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetRoomUid();

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

			if (args.ErrorField.Name.StartsWith("Dirs[", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Assert(args.ErrorField.UserData != null);

				var i = Convert.ToInt64(args.ErrorField.UserData);

				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.GetDirs(i));
			}
		}

		#endregion

		#region Class RoomHelper

		public RoomHelper()
		{
			SetUidIfInvalid = SetRoomUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
