
// GameStateHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Helpers
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

		protected virtual bool ValidateDreamCounter(IField field, IValidateArgs args)
		{
			return Record.DreamCounter >= 0 && Record.DreamCounter <= 13;
		}

		protected virtual bool ValidateSwarmyCounter(IField field, IValidateArgs args)
		{
			return Record.SwarmyCounter >= 1 && Record.SwarmyCounter <= 3;
		}

		protected virtual bool ValidateCargoOpenCounter(IField field, IValidateArgs args)
		{
			return Record.CargoOpenCounter >= 0 && Record.CargoOpenCounter <= 3;
		}

		protected virtual bool ValidateCargoInRoom(IField field, IValidateArgs args)
		{
			return Record.CargoInRoom >= 0 && Record.CargoInRoom <= 1;
		}

		protected virtual bool ValidateGiveAmazonMoney(IField field, IValidateArgs args)
		{
			return Record.GiveAmazonMoney >= 0 && Record.GiveAmazonMoney <= 1;
		}

		protected override IList<IField> GetFields()
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
						x.GetValue = () => Record.DreamCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SwarmyCounter";
						x.Validate = ValidateSwarmyCounter;
						x.GetValue = () => Record.SwarmyCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CargoOpenCounter";
						x.Validate = ValidateCargoOpenCounter;
						x.GetValue = () => Record.CargoOpenCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CargoInRoom";
						x.Validate = ValidateCargoInRoom;
						x.GetValue = () => Record.CargoInRoom;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GiveAmazonMoney";
						x.Validate = ValidateGiveAmazonMoney;
						x.GetValue = () => Record.GiveAmazonMoney;
					})
				);
			}

			return Fields;
		}
	}
}
