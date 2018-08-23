
// ErrorState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ErrorState : State, IErrorState
	{
		public virtual long ErrorCode { get; set; }

		public virtual string ErrorMessage { get; set; }

		public override void Execute()
		{
			Debug.Assert(false, ErrorMessage);
		}

		public ErrorState()
		{
			Name = "ErrorState";

			ErrorCode = 1;

			ErrorMessage = "ErrorState: Unknown message";
		}
	}
}

/* EamonCsCodeTemplate

// ErrorState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class ErrorState : EamonRT.Game.States.ErrorState, IErrorState
	{

	}
}
EamonCsCodeTemplate */
