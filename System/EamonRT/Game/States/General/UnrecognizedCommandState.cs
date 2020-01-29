
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class UnrecognizedCommandState : State, IUnrecognizedCommandState
	{
		public override void Execute()
		{
			var newSeen = false;

			var charMonster = gMDB[gGameState.Cm];

			Debug.Assert(charMonster != null);

			var commandList = Globals.CommandList.Where(x => x.IsEnabled(charMonster) && x.IsListed).ToList();

			gOut.Print("Movement Commands:");

			Globals.Buf.Clear();

			var rc = gEngine.BuildCommandList(commandList, CommandType.Movement, Globals.Buf, ref newSeen);
			
			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Artifact Manipulation:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(commandList, CommandType.Manipulation, Globals.Buf, ref newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Interactive:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(commandList, CommandType.Interactive, Globals.Buf, ref newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			gOut.Print("Miscellaneous:");

			Globals.Buf.Clear();

			rc = gEngine.BuildCommandList(commandList, CommandType.Miscellaneous, Globals.Buf, ref newSeen);

			Debug.Assert(gEngine.IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);

			if (newSeen)
			{
				gOut.Print("(*) New Command");
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public UnrecognizedCommandState()
		{
			Name = "UnrecognizedCommandState";
		}
	}
}
