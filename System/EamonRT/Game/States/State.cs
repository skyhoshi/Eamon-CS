
// State.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.States;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	public abstract class State : IState
	{
		#region Protected Properties

		protected virtual bool GotoCleanup { get; set; }

		#endregion

		#region Public Properties

		#region Interface IState

		public virtual string Name { get; set; }

		public virtual IState NextState { get; set; }

		public virtual bool PreserveNextState { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			// get rid of unmanaged resources
		}

		#endregion

		protected virtual void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} block{2} the way!{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("s", ""));
		}

		protected virtual void PrintCantGoThatWay()
		{
			Globals.Out.WriteLine("{0}You can't go that way!", Environment.NewLine);
		}

		protected virtual void PrintCantVerbThere(string verb)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(verb));

			Globals.Out.WriteLine("{0}You can't {1} there.", Environment.NewLine, verb);
		}

		protected virtual void PrintRideOffIntoSunset()
		{
			Globals.Out.WriteLine("{0}You successfully ride off into the sunset.", Environment.NewLine);
		}

		protected virtual void PrintEnemiesNearby()
		{
			Globals.Out.WriteLine("{0}You can't do that with unfriendlies about!", Environment.NewLine);
		}

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public void Dispose()      // virtual intentionally omitted
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Interface IState

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

		public abstract void Execute();

		#endregion

		#region Class State

		public State()
		{
			Name = "";
		}

		#endregion

		#endregion
	}
}
