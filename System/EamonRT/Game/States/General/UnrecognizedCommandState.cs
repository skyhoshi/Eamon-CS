
// UnrecognizedCommandState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

			var charMonster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(charMonster != null);

			var commandList = Globals.CommandList.Where(x => x.IsEnabled(charMonster) && x.IsListed).ToList();

			Globals.Out.Print("Movement Commands:");

			Globals.Buf.Clear();

			var rc = Globals.Engine.BuildCommandList(commandList, CommandType.Movement, Globals.Buf, ref newSeen);
			
			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Write("{0}", Globals.Buf);

			Globals.Out.Print("Artifact Manipulation:");

			Globals.Buf.Clear();

			rc = Globals.Engine.BuildCommandList(commandList, CommandType.Manipulation, Globals.Buf, ref newSeen);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Write("{0}", Globals.Buf);

			Globals.Out.Print("Interactive:");

			Globals.Buf.Clear();

			rc = Globals.Engine.BuildCommandList(commandList, CommandType.Interactive, Globals.Buf, ref newSeen);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Write("{0}", Globals.Buf);

			Globals.Out.Print("Miscellaneous:");

			Globals.Buf.Clear();

			rc = Globals.Engine.BuildCommandList(commandList, CommandType.Miscellaneous, Globals.Buf, ref newSeen);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Write("{0}", Globals.Buf);

			if (newSeen)
			{
				Globals.Out.Print("(*) New Command");
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
