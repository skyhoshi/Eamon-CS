
// GameStateHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Helpers
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

		protected virtual bool ValidateQueenGiftEffectUid(IField field, IValidateArgs args)
		{
			return Record.QueenGiftEffectUid >= 5 && Record.QueenGiftEffectUid <= 6;
		}

		protected virtual bool ValidateQueenGiftArtifactUid(IField field, IValidateArgs args)
		{
			return Record.QueenGiftArtifactUid == 7 || Record.QueenGiftArtifactUid == 15;
		}

		protected virtual bool ValidateSpookCounter(IField field, IValidateArgs args)
		{
			return Record.SpookCounter >= 0 && Record.SpookCounter <= 10;
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
						x.Name = "QueenGiftEffectUid";
						x.Validate = ValidateQueenGiftEffectUid;
						x.GetValue = () => Record.QueenGiftEffectUid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "QueenGiftArtifactUid";
						x.Validate = ValidateQueenGiftArtifactUid;
						x.GetValue = () => Record.QueenGiftArtifactUid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SpookCounter";
						x.Validate = ValidateSpookCounter;
						x.GetValue = () => Record.SpookCounter;
					})
				});
			}

			return Fields;
		}
	}
}
