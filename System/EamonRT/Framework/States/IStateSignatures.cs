
// IStateSignatures.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	public interface IStateSignatures
	{
		#region Properties

		bool GotoCleanup { get; set; }

		string Name { get; set; }

		IState NextState { get; set; }

		bool PreserveNextState { get; set; }

		#endregion

		#region Methods

		void PrintObjBlocksTheWay(IArtifact artifact);

		void PrintCantGoThatWay();

		void PrintCantVerbThere(string verb);

		void PrintRideOffIntoSunset();

		void PrintEnemiesNearby();

		void ProcessEvents(long eventType);

		string GetDarkName(IGameBase target, Enums.ArticleType articleType, string nameType, bool upshift, bool groupCountOne);

		bool ShouldPreTurnProcess();

		void Execute();

		#endregion
	}
}
