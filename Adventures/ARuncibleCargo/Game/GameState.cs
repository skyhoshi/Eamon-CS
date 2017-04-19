
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long DreamCounter { get; set; }

		public virtual long SwarmyCounter { get; set; }

		public virtual long CargoOpenCounter { get; set; }

		public virtual long CargoInRoom { get; set; }

		public virtual long GiveAmazonMoney { get; set; }

		public virtual bool[] PookaMet { get; set; }

		public virtual bool AmazonMet { get; set; }

		public virtual bool BillAndAmazonMeet { get; set; }

		public virtual bool PrinceMet { get; set; }

		public virtual bool AmazonLilWarning { get; set; }

		public virtual bool BillLilWarning { get; set; }

		public virtual bool FireEscaped { get; set; }

		public virtual bool CampEntered { get; set; }

		public virtual bool PaperRead { get; set; }

		public virtual bool Explosive { get; set; }

		protected virtual bool ValidateDreamCounter(IField field, IValidateArgs args)
		{
			return DreamCounter >= 0 && DreamCounter <= 13;
		}

		protected virtual bool ValidateSwarmyCounter(IField field, IValidateArgs args)
		{
			return SwarmyCounter >= 1 && SwarmyCounter <= 3;
		}

		protected virtual bool ValidateCargoOpenCounter(IField field, IValidateArgs args)
		{
			return CargoOpenCounter >= 0 && CargoOpenCounter <= 3;
		}

		protected virtual bool ValidateCargoInRoom(IField field, IValidateArgs args)
		{
			return CargoInRoom >= 0 && CargoInRoom <= 1;
		}

		protected virtual bool ValidateGiveAmazonMoney(IField field, IValidateArgs args)
		{
			return GiveAmazonMoney >= 0 && GiveAmazonMoney <= 1;
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
						x.Name = "DreamCounter";
						x.Validate = ValidateDreamCounter;
						x.GetValue = () => DreamCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SwarmyCounter";
						x.Validate = ValidateSwarmyCounter;
						x.GetValue = () => SwarmyCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CargoOpenCounter";
						x.Validate = ValidateCargoOpenCounter;
						x.GetValue = () => CargoOpenCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CargoInRoom";
						x.Validate = ValidateCargoInRoom;
						x.GetValue = () => CargoInRoom;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GiveAmazonMoney";
						x.Validate = ValidateGiveAmazonMoney;
						x.GetValue = () => GiveAmazonMoney;
					})
				);
			}

			return Fields;
		}

		public virtual bool GetPookaMet(long index)
		{
			return PookaMet[index];
		}

		public virtual void SetPookaMet(long index, bool value)
		{
			PookaMet[index] = value;
		}

		public GameState()
		{
			DreamCounter = 1;

			SwarmyCounter = 1;

			PookaMet = new bool[3];
		}
	}
}
