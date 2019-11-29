
// StateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StateImpl : IStateImpl
	{
		public virtual IState State { get; set; }

		public virtual bool GotoCleanup { get; set; }

		public virtual string Name { get; set; }

		public virtual IState NextState { get; set; }

		public virtual bool PreserveNextState { get; set; }

		public virtual void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} block{1} the way!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("s", ""));
		}

		public virtual void PrintCantGoThatWay()
		{
			Globals.Out.Print("You can't go that way!");
		}

		public virtual void PrintCantVerbThere(string verb)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(verb));

			Globals.Out.Print("You can't {0} there.", verb);
		}

		public virtual void PrintRideOffIntoSunset()
		{
			Globals.Out.Print("You successfully ride off into the sunset.");
		}

		public virtual void PrintEnemiesNearby()
		{
			Globals.Out.Print("You can't do that with unfriendlies about!");
		}

		public virtual void ProcessEvents(long eventType)
		{

		}

		public virtual void ProcessRevealContentArtifacts(bool printOutput = true)
		{
			Globals.EnableRevealContentOverrides = false;

			var containerTypes = new ContainerType[] { ContainerType.In, ContainerType.On, ContainerType.Under, ContainerType.Behind };

			var containerContentsList = new List<string>();

			var monster = Globals.RevealContentMonster;

			var room = monster != null ? monster.GetInRoom() : Globals.RevealContentRoom;

			if (room != null)
			{
				Debug.Assert(Globals.RevealContentArtifacts.Count == Globals.RevealContentLocations.Count);

				for (var i = 0; i < Globals.RevealContentArtifacts.Count; i++)
				{
					var artifact = Globals.RevealContentArtifacts[i];

					var location = Globals.RevealContentLocations[i];

					if (artifact.IsInLimbo())
					{
						if (artifact.ShouldRevealContentsWhenMovedIntoLimbo())
						{
							Globals.Engine.RevealContainerContents(room, i, containerTypes, printOutput && room.IsLit() && monster != null && monster.IsCharacterMonster() ? containerContentsList : null);
						}
					}
					else if (location != Constants.LimboLocation)
					{
						Globals.Engine.RevealContainerContents(room, i, null, printOutput && room.IsLit() && monster != null && monster.IsCharacterMonster() ? containerContentsList : null);
					}
				}
			}

			foreach (var containerContentsDesc in containerContentsList)
			{
				Globals.Out.Write("{0}", containerContentsDesc);
			}

			Globals.ResetRevealContentProperties();

			Globals.EnableRevealContentOverrides = true;
		}

		public virtual string GetDarkName(IGameBase target, ArticleType articleType, string nameType, bool upshift, bool groupCountOne)
		{
			string result = null;

			Debug.Assert(target != null);

			Debug.Assert(Enum.IsDefined(typeof(ArticleType), articleType));

			var artifact = target as IArtifact;

			if (artifact != null)
			{
				switch (articleType)
				{
					case ArticleType.None:

						result = artifact.EvalPlural("unseen object", "unseen objects");

						break;

					case ArticleType.A:
					case ArticleType.An:

						result = artifact.EvalPlural("an unseen object", "unseen objects");

						break;

					case ArticleType.Some:

						result = artifact.EvalPlural("an unseen object", "some unseen objects");

						break;

					case ArticleType.The:

						result = artifact.EvalPlural("the unseen object", "the unseen objects");

						break;
				}
			}
			else
			{
				var monster = target as IMonster;

				Debug.Assert(monster != null);

				switch (articleType)
				{
					case ArticleType.None:

						result = groupCountOne ? "unseen entity" : monster.EvalPlural("unseen entity", "unseen entities");

						break;

					case ArticleType.A:
					case ArticleType.An:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "unseen entities");

						break;

					case ArticleType.Some:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "some unseen entities");

						break;

					case ArticleType.The:

						result = groupCountOne ? "the unseen entity" : monster.EvalPlural("the unseen entity", "the unseen entities");

						break;
				}
			}

			if (upshift && result != null)
			{
				result = result.FirstCharToUpper();
			}

			return result;
		}

		public virtual bool ShouldPreTurnProcess()
		{
			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			return room.IsLit() || Globals.LastCommandList.Count > 0 ? Globals.LastCommandList.FirstOrDefault(x => x.ShouldPreTurnProcess()) != null : true;
		}

		public virtual void Execute()
		{

		}

		public StateImpl()
		{
			// Here we make an exception to the "always use State" rule

			Name = "";
		}
	}
}
