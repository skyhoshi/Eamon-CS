
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long FoodButtonPushes { get; set; }

		public virtual bool Sterilize { get; set; }

		public virtual long Flood { get; set; }

		public virtual long FloodLevel { get; set; }

		public virtual long Elevation { get; set; }

		public virtual bool Energize { get; set; }

		public virtual long EnergyMaceCharge { get; set; }

		public virtual long LaserScalpelCharge { get; set; }

		public virtual bool CabinetOpen { get; set; }

		public virtual bool LockerOpen { get; set; }

		public virtual bool Shark { get; set; }

		public virtual bool FloorAttack { get; set; }

		public virtual long QuestValue { get; set; }

		public virtual bool ReadPlaque { get; set; }

		public virtual bool ReadTerminals { get; set; }

		public virtual long FakeWallExamines { get; set; }

		public virtual bool AlphabetDial { get; set; }

		public virtual bool ReadDisplayScreen { get; set; }

		public virtual long LabRoomsSeen { get; set; }

		protected virtual bool ValidateFoodButtonPushes(IField field, IValidateArgs args)
		{
			return FoodButtonPushes >= 0 && FoodButtonPushes <= 2;
		}

		protected virtual bool ValidateFlood(IField field, IValidateArgs args)
		{
			return Flood >= 0 && Flood <= 2;
		}

		protected virtual bool ValidateFloodLevel(IField field, IValidateArgs args)
		{
			return FloodLevel >= 0 && FloodLevel <= 11;
		}

		protected virtual bool ValidateElevation(IField field, IValidateArgs args)
		{
			return Elevation >= 0 && Elevation <= 4;
		}

		protected virtual bool ValidateEnergyMaceCharge(IField field, IValidateArgs args)
		{
			return EnergyMaceCharge >= 0 && EnergyMaceCharge <= 120;
		}

		protected virtual bool ValidateLaserScalpelCharge(IField field, IValidateArgs args)
		{
			return LaserScalpelCharge >= 0 && LaserScalpelCharge <= 40;
		}

		protected virtual bool ValidateQuestValue(IField field, IValidateArgs args)
		{
			return QuestValue >= 0 && QuestValue <= 1250;
		}

		protected virtual bool ValidateFakeWallExamines(IField field, IValidateArgs args)
		{
			return FakeWallExamines >= 0 && FakeWallExamines <= 2;
		}

		protected virtual bool ValidateLabRoomsSeen(IField field, IValidateArgs args)
		{
			return LabRoomsSeen >= 0 && LabRoomsSeen <= 45;
		}

		public override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				var fields = base.GetFields();

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FoodButtonPushes";
						x.Validate = ValidateFoodButtonPushes;
						x.GetValue = () => FoodButtonPushes;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Flood";
						x.Validate = ValidateFlood;
						x.GetValue = () => Flood;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FloodLevel";
						x.Validate = ValidateFloodLevel;
						x.GetValue = () => FloodLevel;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Elevation";
						x.Validate = ValidateElevation;
						x.GetValue = () => Elevation;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "EnergyMaceCharge";
						x.Validate = ValidateEnergyMaceCharge;
						x.GetValue = () => EnergyMaceCharge;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LaserScalpelCharge";
						x.Validate = ValidateLaserScalpelCharge;
						x.GetValue = () => LaserScalpelCharge;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "QuestValue";
						x.Validate = ValidateQuestValue;
						x.GetValue = () => QuestValue;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FakeWallExamines";
						x.Validate = ValidateFakeWallExamines;
						x.GetValue = () => FakeWallExamines;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LabRoomsSeen";
						x.Validate = ValidateLabRoomsSeen;
						x.GetValue = () => LabRoomsSeen;
					})
				);
			}

			return Fields;
		}

		public GameState()
		{
			EnergyMaceCharge = 120;

			LaserScalpelCharge = 40;
		}
	}
}
