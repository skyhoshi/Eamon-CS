
// StateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.States;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	/// <summary>
	/// An implementation of the State class that can be subclassed and overridden at the game level.
	/// </summary>
	/// <remarks>
	/// This class was introduced to allow the logic in the abstract base class State to be overridden
	/// by the game designer.  This can already be done on a State by State basis, that is, you have to
	/// override each State individually to create derived behavior.  But now you can override StateImpl
	/// and every class derived from State will inherit the modified behavior automatically.
	/// <para>
	/// Note that when subclassing and writing code in StateImpl, you should never access its methods or
	/// properties directly; always access them indirectly using the State property.  For example, use
	/// State.PrintCantGoThatWay() or State.NextState, etc.  The reason for this is if a subclass of
	/// State has overridden the property or method you don't want to use StateImpl's version, you want
	/// that classes' override (if there is no override in the subclass, then State will just thunk back
	/// over to StateImpl's version).
	/// </para>
	/// </remarks>
	/// <seealso cref="EamonRT.Framework.States.IStateImpl" />
	/// <seealso cref="Eamon.Framework.States.IState" />
	/// <seealso cref="EamonRT.Game.States.State" />
	[ClassMappings]
	public class StateImpl : IStateImpl
	{
		public virtual IState State { get; set; }

		public virtual bool GotoCleanup { get; set; }

		public virtual string Name { get; set; }

		public virtual IState NextState { get; set; }

		public virtual bool PreserveNextState { get; set; }

		public virtual bool Discarded { get; set; }

		public virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			// get rid of unmanaged resources
		}

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

		public virtual string GetDarkName(IGameBase target, Enums.ArticleType articleType, string nameType, bool upshift, bool groupCountOne)
		{
			string result = null;

			Debug.Assert(target != null);

			Debug.Assert(Enum.IsDefined(typeof(Enums.ArticleType), articleType));

			var artifact = target as IArtifact;

			if (artifact != null)
			{
				switch (articleType)
				{
					case Enums.ArticleType.None:

						result = artifact.EvalPlural("unseen object", "unseen objects");

						break;

					case Enums.ArticleType.A:
					case Enums.ArticleType.An:

						result = artifact.EvalPlural("an unseen object", "unseen objects");

						break;

					case Enums.ArticleType.Some:

						result = artifact.EvalPlural("an unseen object", "some unseen objects");

						break;

					case Enums.ArticleType.The:

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
					case Enums.ArticleType.None:

						result = groupCountOne ? "unseen entity" : monster.EvalPlural("unseen entity", "unseen entities");

						break;

					case Enums.ArticleType.A:
					case Enums.ArticleType.An:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "unseen entities");

						break;

					case Enums.ArticleType.Some:

						result = groupCountOne ? "an unseen entity" : monster.EvalPlural("an unseen entity", "some unseen entities");

						break;

					case Enums.ArticleType.The:

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
