
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Framework.Parsing;
using Eamon.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Plugin
{
	public interface IPluginGlobals : EamonDD.Framework.Plugin.IPluginGlobals
	{
		StringBuilder Buf01 { get; set; }

		StringBuilder Buf02 { get; set; }

		IList<ICommand> CommandList { get; set; }

		IList<ICommand> LastCommandList { get; set; }

		IList<IArtifact> LoopArtifactList { get; set; }

		long LoopArtifactListIndex { get; set; }

		long LoopMonsterUid { get; set; }

		long LoopMemberNumber { get; set; }

		long LoopAttackNumber { get; set; }

		long LoopGroupCount { get; set; }

		IMonster LoopLastDfMonster { get; set; }

		new IEngine Engine { get; set; }

		IIntroStory IntroStory { get; set; }

		IMainLoop MainLoop { get; set; }

		ICommandParser CommandParser { get; set; }

		IState CurrState { get; set; }

		IState NextState { get; set; }

		IGameState GameState { get; set; }

		ICharacter Character { get; set; }

		Enums.ExitType ExitType { get; set; }

		string CommandPrompt { get; set; }

		ICommand LastCommand { get; }

		bool GameRunning { get; }

		bool DeleteGameStateAfterLoop { get; }

		bool StartOver { get; }

		bool ErrorExit { get; }

		bool ExportCharacterGoToMainHall { get; }

		bool ExportCharacter { get; }

		bool DeleteCharacter { get; }
	}
}

/* EamonCsCodeTemplate

// IPluginGlobals.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

namespace YourAdventureName.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{

	}
}
EamonCsCodeTemplate */
