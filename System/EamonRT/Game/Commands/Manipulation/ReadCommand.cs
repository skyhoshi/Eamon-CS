
// ReadCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : Command, IReadCommand
	{
		protected virtual void PlayerProcessEvents()
		{

		}

		protected virtual void PlayerProcessEvents01()
		{

		}

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Readable);

			if (ac != null)
			{
				if (!ac.IsOpen())
				{
					PrintMustFirstOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (DobjArtifact.IsDisguisedMonster())
				{
					Globals.RtEngine.RevealDisguisedMonster(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PlayerProcessEvents();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				for (var i = 1; i <= ac.Field6; i++)
				{
					var effect = Globals.EDB[ac.Field5 + i - 1];

					if (effect != null)
					{
						Globals.Buf.Clear();

						rc = effect.BuildPrintedFullDesc(Globals.Buf);
					}
					else
					{
						Globals.Buf.SetFormat("{0}???{0}", Environment.NewLine);

						rc = RetCode.Success;
					}

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);
				}

				PlayerProcessEvents01();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{ 
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public ReadCommand()
		{
			SortOrder = 200;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "ReadCommand";

			Verb = "read";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
