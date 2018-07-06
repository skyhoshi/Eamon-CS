﻿
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IGameState>))]
	public class GameStateHelper : Helper<IGameState>
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		// do nothing

		#endregion

		#region GetName Methods

		protected virtual string GetNameNBTL(bool addToNamesList)
		{
			var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Index = (long)fv;

				GetName("NBTLElement", addToNamesList);
			}

			return "NBTL";
		}

		protected virtual string GetNameNBTLElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("NBTL[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameDTTL(bool addToNamesList)
		{
			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					Index = (long)fv;

					GetName("DTTLElement", addToNamesList);
				}
			}

			return "DTTL";
		}

		protected virtual string GetNameDTTLElement(bool addToNamesList)
		{
			string result = string.Empty;

			if (Globals.IsRulesetVersion(5))
			{
				var i = Index;

				result = string.Format("DTTL[{0}].Element", i);

				if (addToNamesList)
				{
					Names.Add(result);
				}
			}

			return result;
		}

		protected virtual string GetNameSa(bool addToNamesList)
		{
			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SaElement", addToNamesList);
			}

			return "Sa";
		}

		protected virtual string GetNameSaElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("Sa[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		protected virtual string GetNameHeldWpnUids(bool addToNamesList)
		{
			for (Index = 0; Index < Record.HeldWpnUids.Length; Index++)
			{
				GetName("HeldWpnUidsElement", addToNamesList);
			}

			return "HeldWpnUids";
		}

		protected virtual string GetNameHeldWpnUidsElement(bool addToNamesList)
		{
			var i = Index;

			var result = string.Format("HeldWpnUids[{0}].Element", i);

			if (addToNamesList)
			{
				Names.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		protected virtual object GetValueNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i);
		}

		protected virtual object GetValueDTTLElement()
		{
			var i = Index;

			return Record.GetDTTL(i);
		}

		protected virtual object GetValueSaElement()
		{
			var i = Index;

			return Record.GetSa(i);
		}

		protected virtual object GetValueHeldWpnUidsElement()
		{
			var i = Index;

			return Record.GetHeldWpnUids(i);
		}

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateAr()
		{
			return Record.Ar >= 0;
		}

		protected virtual bool ValidateCm()
		{
			return Record.Cm > 0;
		}

		protected virtual bool ValidateLs()
		{
			return Record.Ls >= 0;
		}

		protected virtual bool ValidateSh()
		{
			return Record.Sh >= 0;
		}

		protected virtual bool ValidateLt()
		{
			return Enum.IsDefined(typeof(Enums.LightLevel), Record.Lt);
		}

		protected virtual bool ValidateSpeed()
		{
			return Record.Speed >= 0;
		}

		protected virtual bool ValidateWt()
		{
			return Record.Wt >= 0;
		}

		protected virtual bool ValidateCurrTurn()
		{
			return Record.CurrTurn >= 0;
		}

		protected virtual bool ValidateUsedWpnIdx()
		{
			return Record.UsedWpnIdx >= 0 && Record.UsedWpnIdx < Record.HeldWpnUids.Length;
		}

		protected virtual bool ValidateNBTL()
		{
			var result = true;

			var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Index = (long)fv;

				result = ValidateField("NBTLElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i) >= 0;
		}

		protected virtual bool ValidateDTTL()
		{
			var result = true;

			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					Index = (long)fv;

					result = ValidateField("DTTLElement");

					if (result == false)
					{
						break;
					}
				}
			}

			return result;
		}

		protected virtual bool ValidateDTTLElement()
		{
			var i = Index;

			return !Globals.IsRulesetVersion(5) || Record.GetDTTL(i) >= 0;
		}

		protected virtual bool ValidateSa()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				result = ValidateField("SaElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateSaElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSa(i) >= spell.MinValue && Record.GetSa(i) <= spell.MaxValue;
		}

		protected virtual bool ValidateHeldWpnUids()
		{
			var result = true;

			for (Index = 0; Index < Record.HeldWpnUids.Length; Index++)
			{
				result = ValidateField("HeldWpnUidsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		protected virtual bool ValidateHeldWpnUidsElement()
		{
			var i = Index;

			return Record.GetHeldWpnUids(i) >= 0;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		// do nothing

		#endregion

		#region List Methods

		// do nothing

		#endregion

		#region Input Methods

		// do nothing

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class GameStateHelper

		protected virtual void SetGameStateUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetGameStateUid();

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

		// do nothing

		#endregion

		#region Class GameStateHelper

		public GameStateHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Ar",
				"Cm",
				"Ls",
				"Ro",
				"R2",
				"R3",
				"Sh",
				"Af",
				"Die",
				"Lt",
				"Speed",
				"Wt",
				"Vr",
				"Vm",
				"Va",
				"CurrTurn",
				"UsedWpnIdx",
				"NBTL",
				"DTTL",
				"Sa",
				"HeldWpnUids",
			};

			SetUidIfInvalid = SetGameStateUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
