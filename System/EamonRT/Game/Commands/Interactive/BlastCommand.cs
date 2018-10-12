
// BlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : Command, IBlastCommand
	{
		/// <summary>
		/// This event fires after a check has been made to resolve the player's spell cast
		/// attempt, and it resolves as successful.
		/// </summary>
		public const long PpeAfterPlayerSpellCastCheck = 1;

		/// <summary>
		/// This event fires after the monster targeted by the Blast spell gets aggravated.
		/// </summary>
		public const long PpeAfterMonsterGetsAggravated = 2;

		public virtual bool CastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!CheckAttack && DobjMonster != null && DobjMonster.Friendliness != Enums.Friendliness.Enemy)
			{
				Globals.Out.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				CheckAttack = true;
			}

			if (CastSpell && !Globals.Engine.CheckPlayerSpellCast(Enums.Spell.Blast, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			PlayerProcessEvents(PpeAfterPlayerSpellCastCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjMonster != null && DobjMonster.Friendliness != Enums.Friendliness.Enemy)
			{
				Globals.Engine.MonsterGetsAggravated(DobjMonster);
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

		public override void PlayerFinishParsing()
		{
			CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom)
			};

			CommandParser.ObjData.ArtifactNotFoundFunc = PrintNobodyHereByThatName;

			PlayerResolveMonster();
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

			Type = Enums.CommandType.Interactive;

			CastSpell = true;
		}
	}
}
