
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long WanderingMonster { get; set; }

		public virtual long DwLoopCounter { get; set; }

		public virtual long WandCharges { get; set; }

		public virtual long Regenerate { get; set; }

		public virtual long KeyRingRoomUid { get; set; }

		public virtual bool AlkandaKilled { get; set; }

		public virtual bool AlignmentConflict { get; set; }

		public virtual bool CobraAppeared { get; set; }

		protected virtual bool ValidateWanderingMonster(IField field, IValidateArgs args)
		{
			return WanderingMonster >= 12 && WanderingMonster <= 27;
		}

		protected virtual bool ValidateDwLoopCounter(IField field, IValidateArgs args)
		{
			return DwLoopCounter >= 0 && DwLoopCounter <= 16;
		}

		protected virtual bool ValidateWandCharges(IField field, IValidateArgs args)
		{
			return WandCharges >= 0 && WandCharges <= 5;
		}

		protected virtual bool ValidateRegenerate(IField field, IValidateArgs args)
		{
			return Regenerate >= 0 && Regenerate <= 5;
		}

		protected virtual bool ValidateKeyRingRoomUid(IField field, IValidateArgs args)
		{
			return KeyRingRoomUid >= 0 && KeyRingRoomUid <= 59;
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
						x.Name = "WanderingMonster";
						x.Validate = ValidateWanderingMonster;
						x.GetValue = () => WanderingMonster;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DwLoopCounter";
						x.Validate = ValidateDwLoopCounter;
						x.GetValue = () => DwLoopCounter;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WandCharges";
						x.Validate = ValidateWandCharges;
						x.GetValue = () => WandCharges;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Regenerate";
						x.Validate = ValidateRegenerate;
						x.GetValue = () => Regenerate;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "KeyRingRoomUid";
						x.Validate = ValidateKeyRingRoomUid;
						x.GetValue = () => KeyRingRoomUid;
					})
				);
			}

			return Fields;
		}

		public GameState()
		{
			// Sets up wandering monsters and fireball wand charges

			WanderingMonster = Globals.Engine.RollDice01(1, 14, 11);

			WandCharges = Globals.Engine.RollDice01(1, 4, 1);
		}
	}
}
