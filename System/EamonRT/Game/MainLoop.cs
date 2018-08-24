
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings]
	public class MainLoop : IMainLoop
	{
		public virtual StringBuilder Buf { get; set; }

		public virtual bool ShouldStartup { get; set; }

		public virtual bool ShouldShutdown { get; set; }

		public virtual bool ShouldExecute { get; set; }

		public virtual void Startup()
		{
			Globals.Engine.InitWtValueAndEnforceLimits();

			var monster = Globals.Engine.ConvertCharacterToMonster();

			Debug.Assert(monster != null);

			Globals.GameState.Cm = monster.Uid;

			Debug.Assert(Globals.GameState.Cm > 0);

			Globals.Engine.AddPoundCharsToArtifactNames();

			Globals.Engine.AddMissingDescs();

			Globals.GameState.Ro = Globals.Engine.StartRoom;

			Globals.GameState.R2 = Globals.Engine.StartRoom;

			Globals.GameState.R3 = Globals.Engine.StartRoom;

			Globals.Engine.InitSaArray();

			Globals.Engine.CreateCommands();

			Globals.Engine.InitArtifacts();

			Globals.Engine.InitMonsters();

			Globals.Module.NumArtifacts = Globals.Database.GetArtifactsCount();

			Globals.Module.NumMonsters = Globals.Database.GetMonstersCount();

			Globals.Engine.CreateInitialState(false);
		}

		public virtual void Shutdown()
		{
			var weaponList = new List<IArtifact>();

			Globals.Engine.SetArmorClass();

			Globals.Engine.ConvertToCarriedInventory(weaponList);

			Globals.Engine.SellExcessWeapons(weaponList);

			var monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(monster != null);

			Globals.Engine.ConvertMonsterToCharacter(monster, weaponList);

			Globals.Engine.SellInventoryToMerchant();
		}

		public virtual void Execute()
		{
			Globals.Out.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			Globals.Out.WriteLine("Please wait a short while (waking up the monsters...)");

			Globals.Thread.Sleep(3000);

			Globals.Out.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			Globals.Out.WriteLine("[Base Program {0}]", Constants.RtProgVersion);

			Globals.Out.Print("Welcome to the Eamon CS fantasy gaming system!");

			Globals.Out.Print("{0}", Globals.LineSep);

			while (Globals.GameRunning)
			{
				Debug.Assert(Globals.CurrState != null);

				var command = Globals.CurrState as ICommand;

				if (command != null && command.ActorMonster.IsCharacterMonster())
				{
					Globals.LastCommandList.Add(command);

					Globals.CurrState.Execute();
				}
				else
				{
					Globals.CurrState.Execute();

					Globals.CurrState.Dispose();
				}

				Globals.CurrState = Globals.NextState;

				Globals.NextState = null;
			}
		}

		public MainLoop()
		{
			Buf = Globals.Buf;

			ShouldStartup = true;

			ShouldShutdown = true;

			ShouldExecute = true;
		}
	}
}
