
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long QueenGiftEffectUid { get; set; }

		public virtual long QueenGiftArtifactUid { get; set; }

		public virtual long SpookCounter { get; set; }

		protected virtual bool ValidateQueenGiftEffectUid(IField field, IValidateArgs args)
		{
			return QueenGiftEffectUid >= 5 && QueenGiftEffectUid <= 6;
		}

		protected virtual bool ValidateQueenGiftArtifactUid(IField field, IValidateArgs args)
		{
			return QueenGiftArtifactUid == 7 || QueenGiftArtifactUid == 15;
		}

		protected virtual bool ValidateSpookCounter(IField field, IValidateArgs args)
		{
			return SpookCounter >= 0 && SpookCounter <= 10;
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
						x.Name = "QueenGiftEffectUid";
						x.Validate = ValidateQueenGiftEffectUid;
						x.GetValue = () => QueenGiftEffectUid;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "QueenGiftArtifactUid";
						x.Validate = ValidateQueenGiftArtifactUid;
						x.GetValue = () => QueenGiftArtifactUid;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SpookCounter";
						x.Validate = ValidateSpookCounter;
						x.GetValue = () => SpookCounter;
					})
				);
			}

			return Fields;
		}

		public GameState()
		{
			// Queen's gift

			QueenGiftEffectUid = 5;

			QueenGiftArtifactUid = 7;
		}
	}
}
