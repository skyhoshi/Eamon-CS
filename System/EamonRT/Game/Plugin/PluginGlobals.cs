
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using EamonRT.Framework;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Plugin;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Plugin
{
	public class PluginGlobals : EamonDD.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public virtual StringBuilder Buf01 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual StringBuilder Buf02 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual IList<ICommand> CommandList { get; set; }

		public virtual IList<ICommand> LastCommandList { get; set; }

		public virtual IList<IArtifact> LoopArtifactList { get; set; }

		public virtual long LoopArtifactListIndex { get; set; }

		public virtual long LoopMonsterUid { get; set; }

		public virtual long LoopMemberNumber { get; set; }

		public virtual long LoopAttackNumber { get; set; }

		public virtual long LoopGroupCount { get; set; }

		public virtual IMonster LoopLastDfMonster { get; set; }

		public virtual new Framework.IEngine Engine
		{
			get
			{
				return (Framework.IEngine)base.Engine;
			}

			set
			{
				if (base.Engine != value)
				{
					base.Engine = value;
				}
			}
		}

		public virtual IIntroStory IntroStory { get; set; }

		public virtual IMainLoop MainLoop { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IState CurrState { get; set; }

		public virtual IState NextState { get; set; }

		public virtual IGameState GameState { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual Enums.ExitType ExitType { get; set; }

		public virtual string CommandPrompt { get; set; }

		public virtual ICommand LastCommand
		{
			get
			{
				return LastCommandList.Count > 0 ? LastCommandList[LastCommandList.Count - 1] : null;
			}
		}

		public virtual bool GameRunning
		{
			get
			{
				return ExitType == Enums.ExitType.None;
			}
		}

		public virtual bool DeleteGameStateAfterLoop
		{
			get
			{
				return ExitType == Enums.ExitType.GoToMainHall || ExitType == Enums.ExitType.StartOver || ExitType == Enums.ExitType.FinishAdventure || ExitType == Enums.ExitType.DeleteCharacter;
			}
		}

		public virtual bool StartOver
		{
			get
			{
				return ExitType == Enums.ExitType.StartOver;
			}
		}

		public virtual bool ErrorExit
		{
			get
			{
				return ExitType == Enums.ExitType.Error;
			}
		}

		public virtual bool ExportCharacterGoToMainHall
		{
			get
			{
				return ExitType == Enums.ExitType.GoToMainHall || ExitType == Enums.ExitType.FinishAdventure;
			}
		}

		public virtual bool ExportCharacter
		{
			get
			{
				return ExitType == Enums.ExitType.FinishAdventure;
			}
		}

		public virtual bool DeleteCharacter
		{
			get
			{
				return ExitType == Enums.ExitType.DeleteCharacter;
			}
		}

		public override bool EnableGameOverrides
		{
			get
			{
				return base.EnableGameOverrides && GameState != null;
			}
		}

		public override void InitSystem()
		{
			base.InitSystem();

			CommandList = new List<ICommand>();

			LastCommandList = new List<ICommand>();

			IntroStory = CreateInstance<IIntroStory>();

			MainLoop = CreateInstance<IMainLoop>();

			CommandParser = CreateInstance<ICommandParser>();

			CommandPrompt = Constants.CommandPrompt;
		}
	}
}
