
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : Command, IBlastCommand
	{
		/// <summary>
		/// An event that fires after the player's spell cast attempt has resolved as successful.
		/// </summary>
		public const long PpeAfterPlayerSpellCastCheck = 1;

		/// <summary>
		/// An event that fires after the <see cref="IMonster">Monster</see> targeted by the <see cref="Spell.Blast">Blast</see>
		/// spell gets aggravated.
		/// </summary>
		public const long PpeAfterMonsterGetsAggravated = 2;

		public virtual bool CastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!CheckAttack && DobjMonster != null && DobjMonster.Friendliness != Friendliness.Enemy)
			{
				gOut.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				CheckAttack = true;
			}

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Blast, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			PlayerProcessEvents(PpeAfterPlayerSpellCastCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjMonster != null && DobjMonster.Friendliness != Friendliness.Enemy)
			{
				gEngine.MonsterGetsAggravated(DobjMonster);
			}

			PlayerProcessEvents(PpeAfterMonsterGetsAggravated);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.BlastSpell = true;

				x.CheckAttack = CheckAttack;
			});

			CopyCommandData(NextState as ICommand);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			return DobjMonster != null || DobjArtifact.IsAttackable();
		}

		public BlastCommand()
		{
			SortOrder = 260;

			Name = "BlastCommand";

			Verb = "blast";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
