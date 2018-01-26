
// RequestCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RequestCommand : Command, IRequestCommand
	{
		public virtual bool GetCommandCalled { get; set; }

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjMonster != null);

			if (IobjMonster.Friendliness < Enums.Friendliness.Friend)
			{
				Globals.Engine.MonsterSmiles(IobjMonster);

				Globals.Out.WriteLine();

				goto Cleanup;
			}

			if (!GetCommandCalled)
			{
				NextState = Globals.CreateInstance<IGetCommand>(x =>
				{
					x.PreserveNextState = true;
				});

				CopyCommandData(NextState as ICommand, false);

				NextState.NextState = Globals.CreateInstance<IRequestCommand>(x =>
				{
					x.GetCommandCalled = true;
				});

				CopyCommandData(NextState.NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!DobjArtifact.IsDisguisedMonster())
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (IobjMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.IsWeapon01());

				rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IobjMonster.Weapon = -1;
			}

			if (!DobjArtifact.Seen)
			{
				Globals.Buf.Clear();

				rc = DobjArtifact.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				DobjArtifact.Seen = true;
			}

			if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByCharacter() && NextState == null)
			{
				NextState = Globals.CreateInstance<IReadyCommand>();

				CopyCommandData(NextState as ICommand, false);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			CommandParser.ObjData = CommandParser.IobjData;

			CommandParser.ObjData.QueryDesc = string.Format("{0}From whom? ", Environment.NewLine);

			PlayerResolveMonster();

			if (IobjMonster != null)
			{
				CommandParser.ObjData = CommandParser.DobjData;

				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByMonster(IobjMonster),
					a => a.IsWornByMonster(IobjMonster)
				};

				CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch02;

				CommandParser.ObjData.ArtifactNotFoundFunc = () =>
				{
					Globals.Out.Print("{0}{1} have it.",
						IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf),
						IobjMonster.EvalPlural(" doesn't", " don't"));
				};

				PlayerResolveArtifact();
			}
		}

		public RequestCommand()
		{
			SortOrder = 300;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "RequestCommand";

			Verb = "request";

			Type = Enums.CommandType.Interactive;
		}
	}
}
