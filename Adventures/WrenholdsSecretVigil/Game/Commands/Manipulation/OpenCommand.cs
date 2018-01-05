
// OpenCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IOpenCommand))]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		protected override void PlayerProcessEvents()
		{
			// Try to open running device, all flee

			if (DobjArtifact.Uid == 44)
			{
				Globals.DeviceOpened = true;

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected override void PrintOpened(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				Globals.Out.Write("{0}You try to open the glowing device.{0}", Environment.NewLine);
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		protected override void PrintLocked(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				Globals.Out.Write("{0}The hide is too hard to cut!{0}", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		protected override void PrintOpenObjWithKey(Eamon.Framework.IArtifact artifact, Eamon.Framework.IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				Globals.Out.Write("{0}You try to open the glowing device with {1}.{0}", Environment.NewLine, key.GetDecoratedName03(false, true, false, false, Globals.Buf));
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Large rock

			if (DobjArtifact.Uid == 17)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
