
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool RedSunSpeaks { get; set; }

		public virtual bool JacquesShouts { get; set; }

		public virtual bool JacquesRecoversRapier { get; set; }

		public virtual bool KoboldsAppear { get; set; }

		public virtual bool SylvaniSpeaks { get; set; }

		public virtual bool ThorsHammerAppears { get; set; }

		public virtual bool LibrarySecretPassageFound { get; set; }

		public virtual bool ScuffleSoundsHeard { get; set; }

		public virtual bool CharismaBoosted { get; set; }

		public virtual long GenderChangeCounter { get; set; }

		protected virtual bool ValidateGenderChangeCounter(IField field, IValidateArgs args)
		{
			return GenderChangeCounter >= 0 && GenderChangeCounter <= 2;
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
						x.Name = "GenderChangeCounter";
						x.Validate = ValidateGenderChangeCounter;
						x.GetValue = () => GenderChangeCounter;
					})
				);
			}

			return Fields;
		}

		public GameState()
		{

		}
	}
}
