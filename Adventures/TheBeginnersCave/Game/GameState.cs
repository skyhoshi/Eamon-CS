
// GameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		protected long _trollsfire = 0;

		public virtual long Trollsfire
		{
			get
			{
				return _trollsfire;
			}

			set
			{
				RetCode rc;

				Debug.Assert(value >= 0 && value <= 1);

				if (_trollsfire != value)
				{
					var artifact = Globals.ADB[10];

					Debug.Assert(artifact != null);

					var ac = artifact.GetArtifactClass(Enums.ArtifactType.MagicWeapon);

					Debug.Assert(ac != null);

					if (value != 0)
					{
						rc = artifact.AddStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						ac.Field8 = 10;
					}
					else
					{
						rc = artifact.RemoveStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						ac.Field8 = 6;
					}

					_trollsfire = value;
				}
			}
		}

		public virtual long BookWarning { get; set; }

		public virtual long UsedWpnIdx { get; set; }

		public virtual long[] HeldWpnUids { get; set; }

		protected virtual bool ValidateTrollsfire(IField field, IValidateArgs args)
		{
			return Trollsfire >= 0 && Trollsfire <= 1;
		}

		protected virtual bool ValidateBookWarning(IField field, IValidateArgs args)
		{
			return BookWarning >= 0 && BookWarning <= 1;
		}

		protected virtual bool ValidateUsedWpnIdx(IField field, IValidateArgs args)
		{
			return UsedWpnIdx >= 0 && UsedWpnIdx < HeldWpnUids.Length;
		}

		protected virtual bool ValidateHeldWpnUids(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < HeldWpnUids.Length);

			return GetHeldWpnUids(i) >= 0;
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
						x.Name = "Trollsfire";
						x.Validate = ValidateTrollsfire;
						x.GetValue = () => Trollsfire;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "BookWarning";
						x.Validate = ValidateBookWarning;
						x.GetValue = () => BookWarning;
					})
				);

				fields.Add
				(
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "UsedWpnIdx";
						x.Validate = ValidateUsedWpnIdx;
						x.GetValue = () => UsedWpnIdx;
					})
				);

				for (var i = 0; i < HeldWpnUids.Length; i++)
				{
					var j = i;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("HeldWpnUids[{0}]", j);
							x.UserData = j;
							x.Validate = ValidateHeldWpnUids;
							x.GetValue = () => GetHeldWpnUids(j);
						})
					);
				}
			}

			return Fields;
		}

		public virtual long GetHeldWpnUids(long index)
		{
			return HeldWpnUids[index];
		}

		public virtual void SetHeldWpnUids(long index, long value)
		{
			HeldWpnUids[index] = value;
		}

		public GameState()
		{
			var character = Globals.CreateInstance<ICharacter>();

			HeldWpnUids = new long[character.Weapons.Length];
		}
	}
}
