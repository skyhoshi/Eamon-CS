
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : Command, IReadCommand
	{
		/// <summary></summary>
		public const long PpeBeforeArtifactReadTextPrint = 1;

		/// <summary></summary>
		public const long PpeAfterArtifactRead = 2;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.Readable;

			if (ac != null)
			{
				if (!ac.IsOpen())
				{
					PrintMustFirstOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (DobjArtifact.DisguisedMonster != null)
				{
					Globals.Engine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PlayerProcessEvents(PpeBeforeArtifactReadTextPrint);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				for (var i = 1; i <= ac.Field2; i++)
				{
					var effect = Globals.EDB[ac.Field1 + i - 1];

					if (effect != null)
					{
						Globals.Buf.Clear();

						rc = effect.BuildPrintedFullDesc(Globals.Buf);
					}
					else
					{
						Globals.Buf.SetPrint("{0}", "???");

						rc = RetCode.Success;
					}

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.Write("{0}", Globals.Buf);
				}

				PlayerProcessEvents(PpeAfterArtifactRead);

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

		public override void PlayerFinishParsing()
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

			Type = CommandType.Manipulation;
		}
	}
}
