
// IStateSignatures.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IStateSignatures
	{
		#region Properties

		/// <summary></summary>
		bool GotoCleanup { get; set; }

		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		bool PreserveNextState { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintObjBlocksTheWay(IArtifact artifact);

		/// <summary></summary>
		void PrintCantGoThatWay();

		/// <summary></summary>
		/// <param name="verb"></param>
		void PrintCantVerbThere(string verb);

		/// <summary></summary>
		void PrintRideOffIntoSunset();

		/// <summary></summary>
		void PrintEnemiesNearby();

		/// <summary></summary>
		/// <param name="eventType"></param>
		void ProcessEvents(long eventType);

		/// <summary></summary>
		/// <param name="target"></param>
		/// <param name="articleType"></param>
		/// <param name="nameType"></param>
		/// <param name="upshift"></param>
		/// <param name="groupCountOne"></param>
		/// <returns></returns>
		string GetDarkName(IGameBase target, Enums.ArticleType articleType, string nameType, bool upshift, bool groupCountOne);

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldPreTurnProcess();

		/// <summary></summary>
		void Execute();

		#endregion
	}
}
