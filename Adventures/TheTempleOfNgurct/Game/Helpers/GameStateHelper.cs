
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IGameState>))]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		protected virtual bool ValidateWanderingMonster(IField field, IValidateArgs args)
		{
			return Record.WanderingMonster >= 12 && Record.WanderingMonster <= 27;
		}

		protected virtual bool ValidateDwLoopCounter(IField field, IValidateArgs args)
		{
			return Record.DwLoopCounter >= 0 && Record.DwLoopCounter <= 16;
		}

		protected virtual bool ValidateWandCharges(IField field, IValidateArgs args)
		{
			return Record.WandCharges >= 0 && Record.WandCharges <= 5;
		}

		protected virtual bool ValidateRegenerate(IField field, IValidateArgs args)
		{
			return Record.Regenerate >= 0 && Record.Regenerate <= 5;
		}

		protected virtual bool ValidateKeyRingRoomUid(IField field, IValidateArgs args)
		{
			return Record.KeyRingRoomUid >= 0 && Record.KeyRingRoomUid <= 59;
		}

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				var fields = base.GetFields();

				fields.AddRange(new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WanderingMonster";
						x.Validate = ValidateWanderingMonster;
						x.GetValue = () => Record.WanderingMonster;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DwLoopCounter";
						x.Validate = ValidateDwLoopCounter;
						x.GetValue = () => Record.DwLoopCounter;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WandCharges";
						x.Validate = ValidateWandCharges;
						x.GetValue = () => Record.WandCharges;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Regenerate";
						x.Validate = ValidateRegenerate;
						x.GetValue = () => Record.Regenerate;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "KeyRingRoomUid";
						x.Validate = ValidateKeyRingRoomUid;
						x.GetValue = () => Record.KeyRingRoomUid;
					})
				});
			}

			return Fields;
		}
	}
}
