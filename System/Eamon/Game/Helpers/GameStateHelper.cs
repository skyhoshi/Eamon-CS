
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Helper<IGameState>, IGameStateHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		// do nothing

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameNBTL(bool addToNamesList)
		{
			var friendlinessValues = EnumUtil.GetValues<Friendliness>();

			foreach (var fv in friendlinessValues)
			{
				Index = (long)fv;

				GetName("NBTLElement", addToNamesList);
			}

			return "NBTL";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
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

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameDTTL(bool addToNamesList)
		{
			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Friendliness>();

				foreach (var fv in friendlinessValues)
				{
					Index = (long)fv;

					GetName("DTTLElement", addToNamesList);
				}
			}

			return "DTTL";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
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

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameSa(bool addToNamesList)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SaElement", addToNamesList);
			}

			return "Sa";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
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

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
		protected virtual string GetNameHeldWpnUids(bool addToNamesList)
		{
			for (Index = 0; Index < Record.HeldWpnUids.Length; Index++)
			{
				GetName("HeldWpnUidsElement", addToNamesList);
			}

			return "HeldWpnUids";
		}

		/// <summary></summary>
		/// <param name="addToNamesList"></param>
		/// <returns></returns>
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

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueDTTLElement()
		{
			var i = Index;

			return Record.GetDTTL(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueSaElement()
		{
			var i = Index;

			return Record.GetSa(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual object GetValueHeldWpnUidsElement()
		{
			var i = Index;

			return Record.GetHeldWpnUids(i);
		}

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateAr()
		{
			return Record.Ar >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateCm()
		{
			return Record.Cm > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLs()
		{
			return Record.Ls >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSh()
		{
			return Record.Sh >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLt()
		{
			return Enum.IsDefined(typeof(LightLevel), Record.Lt);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSpeed()
		{
			return Record.Speed >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWt()
		{
			return Record.Wt >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateCurrTurn()
		{
			return Record.CurrTurn >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidatePauseCombatMs()
		{
			return Record.PauseCombatMs >= 0 && Record.PauseCombatMs <= 10000;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateUsedWpnIdx()
		{
			return Record.UsedWpnIdx >= 0 && Record.UsedWpnIdx < Record.HeldWpnUids.Length;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNBTL()
		{
			var result = true;

			var friendlinessValues = EnumUtil.GetValues<Friendliness>();

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

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNBTLElement()
		{
			var i = Index;

			return Record.GetNBTL(i) >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateDTTL()
		{
			var result = true;

			if (Globals.IsRulesetVersion(5))
			{
				var friendlinessValues = EnumUtil.GetValues<Friendliness>();

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

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateDTTLElement()
		{
			var i = Index;

			return !Globals.IsRulesetVersion(5) || Record.GetDTTL(i) >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSa()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Spell>();

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

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSaElement()
		{
			var i = Index;

			var spell = Globals.Engine.GetSpells((Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSa(i) >= spell.MinValue && Record.GetSa(i) <= spell.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
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

		/// <summary></summary>
		/// <returns></returns>
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

		protected override void SetUidIfInvalid()
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

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

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
				"PauseCombatMs",
				"UsedWpnIdx",
				"NBTL",
				"DTTL",
				"Sa",
				"HeldWpnUids",
			};
		}

		#endregion

		#endregion
	}
}
