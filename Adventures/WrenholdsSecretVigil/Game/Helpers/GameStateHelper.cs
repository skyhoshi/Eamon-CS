
// GameStateHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Helpers
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

		protected virtual bool ValidateMedallionCharges(IField field, IValidateArgs args)
		{
			return Record.MedallionCharges >= 0 && Record.MedallionCharges <= 15;
		}

		protected virtual bool ValidateSlimeBlasts(IField field, IValidateArgs args)
		{
			return Record.SlimeBlasts >= 0 && Record.SlimeBlasts <= 3;
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
						x.Name = "MedallionCharges";
						x.Validate = ValidateMedallionCharges;
						x.GetValue = () => Record.MedallionCharges;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SlimeBlasts";
						x.Validate = ValidateSlimeBlasts;
						x.GetValue = () => Record.SlimeBlasts;
					}),
				});
			}

			return Fields;
		}
	}
}
