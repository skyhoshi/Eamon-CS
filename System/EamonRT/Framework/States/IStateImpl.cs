
// IStateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	public interface IStateImpl
	{
		IState State { get; set; }

		bool GotoCleanup { get; set; }

		string Name { get; set; }

		IState NextState { get; set; }

		bool PreserveNextState { get; set; }

		bool Discarded { get; set; }

		void Dispose(bool disposing);

		void PrintObjBlocksTheWay(IArtifact artifact);

		void PrintCantGoThatWay();

		void PrintCantVerbThere(string verb);

		void PrintRideOffIntoSunset();

		void PrintEnemiesNearby();

		void ProcessEvents(long eventType);

		string GetDarkName(IGameBase target, Enums.ArticleType articleType, string nameType, bool upshift, bool groupCountOne);

		bool ShouldPreTurnProcess();

		void Execute();
	}
}
