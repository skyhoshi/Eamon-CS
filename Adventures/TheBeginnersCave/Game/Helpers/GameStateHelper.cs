
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Helpers
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

		protected virtual bool ValidateTrollsfire(IField field, IValidateArgs args)
		{
			return Record.Trollsfire >= 0 && Record.Trollsfire <= 1;
		}

		protected virtual bool ValidateBookWarning(IField field, IValidateArgs args)
		{
			return Record.BookWarning >= 0 && Record.BookWarning <= 1;
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
						x.Name = "Trollsfire";
						x.Validate = ValidateTrollsfire;
						x.GetValue = () => Record.Trollsfire;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "BookWarning";
						x.Validate = ValidateBookWarning;
						x.GetValue = () => Record.BookWarning;
					})
				});
			}

			return Fields;
		}
	}
}
