
// CommandImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("With great effort, {0}'s door slides closed.", artifact.GetTheName());

				artifact.DoorGate.SetKeyUid(-1);
			}
			else
			{
				base.PrintClosed(artifact);
			}
		}

		public override void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5 || artifact.Uid == 13 || artifact.Uid == 35)
			{
				gOut.Print("You don't have the right tools for that.");
			}
			else
			{
				base.PrintWontOpen(artifact);
			}
		}

		public override void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("You can't seem to get enough leverage to pry open {0}'s door.", artifact.GetTheName());

				artifact.DoorGate.SetKeyUid(-1);
			}
			else if (artifact.Uid == 13)
			{
				gOut.Print("You work for a while on {0}'s lock but can't break it.", artifact.GetTheName());

				artifact.InContainer.SetKeyUid(-1);
			}
			else if (artifact.Uid == 35)
			{
				gOut.Print("You work for a while on {0}'s lid but can't pry it open.", artifact.GetTheName());

				artifact.InContainer.SetKeyUid(-1);
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		public override void PrintLightObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("A magical flame bursts from {0}.", artifact.GetTheName());
			}
			else
			{
				base.PrintLightObj(artifact);
			}
		}

		public override void PrintLightExtinguished(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("The fire is violently extinguished.");
			}
			else
			{
				base.PrintLightExtinguished(artifact);
			}
		}

		public override void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("With great effort, {0}'s door slides open.", artifact.GetTheName());
			}
			else if (artifact.Uid == 13)
			{
				gOut.Print("You manage to break open {0}, and find several items inside!", artifact.GetTheName());
			}
			else if (artifact.Uid == 35)
			{
				gOut.Print("You manage to pry open {0}'s lid, and find something inside!", artifact.GetTheName());
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		public override void PrintNotEnoughGold()
		{
			gOut.Print("You aren't carrying that much gold of your own!");
		}

		public override void PrintMustFirstReadyWeapon()
		{
			gOut.Print("You have no weapon ready!");
		}

		public override void PrintDontHaveItNotHere()
		{
			if (gCommandParser.DecorationId > 0)
			{
				PrintCantDoThat();
			}
			else
			{
				base.PrintDontHaveItNotHere();
			}
		}

		public override void PrintNothingHereByThatName()
		{
			if (gCommandParser.DecorationId > 0)
			{
				PrintCantDoThat();
			}
			else
			{
				base.PrintNothingHereByThatName();
			}
		}

		public override void PrintCalmDown()
		{
			gOut.Print("There's nothing to flee from!");
		}
	}
}
