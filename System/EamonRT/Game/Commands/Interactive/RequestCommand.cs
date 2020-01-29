
// RequestCommand.cs

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
	public class RequestCommand : Command, IRequestCommand
	{
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null && gIobjMonster != null);

			if (gIobjMonster.Friendliness < Friendliness.Friend)
			{
				gEngine.MonsterSmiles(gIobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			if (!GetCommandCalled)
			{
				RedirectToGetCommand<IRequestCommand>(gDobjArtifact, false);

				goto Cleanup;
			}

			if (!gDobjArtifact.IsCarriedByCharacter())
			{
				if (gDobjArtifact.DisguisedMonster == null)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (gIobjMonster.Weapon == gDobjArtifact.Uid)
			{
				Debug.Assert(gDobjArtifact.GeneralWeapon != null);

				rc = gDobjArtifact.RemoveStateDesc(gDobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gIobjMonster.Weapon = -1;
			}

			if (gActorMonster.Weapon <= 0 && gDobjArtifact.IsReadyableByCharacter() && NextState == null)
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

		public override void PlayerFinishParsing()
		{
			gCommandParser.ParseName();

			gCommandParser.ObjData = gCommandParser.IobjData;

			gCommandParser.ObjData.QueryDesc = string.Format("{0}From whom? ", Environment.NewLine);

			PlayerResolveMonster();

			if (gIobjMonster != null)
			{
				gCommandParser.ObjData = gCommandParser.DobjData;

				gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByMonster(gIobjMonster),
					a => a.IsWornByMonster(gIobjMonster)
				};

				gCommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch02;

				gCommandParser.ObjData.ArtifactNotFoundFunc = () =>
				{
					gOut.Print("{0}{1} have it.",
						gIobjMonster.GetTheName(true),
						gIobjMonster.EvalPlural(" doesn't", " don't"));
				};

				PlayerResolveArtifact();
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return false;
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { "from" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RequestCommand()
		{
			SortOrder = 300;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "RequestCommand";

			Verb = "request";

			Type = CommandType.Interactive;
		}
	}
}
