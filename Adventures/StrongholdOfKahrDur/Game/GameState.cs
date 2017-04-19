
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool UsedCauldron { get; set; }

		public virtual long LichState { get; set; }

		protected virtual bool ValidateLichState(IField field, IValidateArgs args)
		{
			return LichState >= 0 && LichState <= 2;
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
						x.Name = "LichState";
						x.Validate = ValidateLichState;
						x.GetValue = () => LichState;
					})
				);
			}

			return Fields;
		}
	}
}
