
// GameStateHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Helpers
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

		protected virtual bool ValidateFoodButtonPushes(IField field, IValidateArgs args)
		{
			return Record.FoodButtonPushes >= 0 && Record.FoodButtonPushes <= 2;
		}

		protected virtual bool ValidateFlood(IField field, IValidateArgs args)
		{
			return Record.Flood >= 0 && Record.Flood <= 2;
		}

		protected virtual bool ValidateFloodLevel(IField field, IValidateArgs args)
		{
			return Record.FloodLevel >= 0 && Record.FloodLevel <= 11;
		}

		protected virtual bool ValidateElevation(IField field, IValidateArgs args)
		{
			return Record.Elevation >= 0 && Record.Elevation <= 4;
		}

		protected virtual bool ValidateEnergyMaceCharge(IField field, IValidateArgs args)
		{
			return Record.EnergyMaceCharge >= 0 && Record.EnergyMaceCharge <= 120;
		}

		protected virtual bool ValidateLaserScalpelCharge(IField field, IValidateArgs args)
		{
			return Record.LaserScalpelCharge >= 0 && Record.LaserScalpelCharge <= 40;
		}

		protected virtual bool ValidateQuestValue(IField field, IValidateArgs args)
		{
			return Record.QuestValue >= 0 && Record.QuestValue <= 1250;
		}

		protected virtual bool ValidateFakeWallExamines(IField field, IValidateArgs args)
		{
			return Record.FakeWallExamines >= 0 && Record.FakeWallExamines <= 2;
		}

		protected virtual bool ValidateLabRoomsSeen(IField field, IValidateArgs args)
		{
			return Record.LabRoomsSeen >= 0 && Record.LabRoomsSeen <= 45;
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
						x.Name = "FoodButtonPushes";
						x.Validate = ValidateFoodButtonPushes;
						x.GetValue = () => Record.FoodButtonPushes;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Flood";
						x.Validate = ValidateFlood;
						x.GetValue = () => Record.Flood;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FloodLevel";
						x.Validate = ValidateFloodLevel;
						x.GetValue = () => Record.FloodLevel;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Elevation";
						x.Validate = ValidateElevation;
						x.GetValue = () => Record.Elevation;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "EnergyMaceCharge";
						x.Validate = ValidateEnergyMaceCharge;
						x.GetValue = () => Record.EnergyMaceCharge;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LaserScalpelCharge";
						x.Validate = ValidateLaserScalpelCharge;
						x.GetValue = () => Record.LaserScalpelCharge;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "QuestValue";
						x.Validate = ValidateQuestValue;
						x.GetValue = () => Record.QuestValue;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FakeWallExamines";
						x.Validate = ValidateFakeWallExamines;
						x.GetValue = () => Record.FakeWallExamines;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LabRoomsSeen";
						x.Validate = ValidateLabRoomsSeen;
						x.GetValue = () => Record.LabRoomsSeen;
					})
				});
			}

			return Fields;
		}
	}
}
