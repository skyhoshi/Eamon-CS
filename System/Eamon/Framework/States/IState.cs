
// IState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.States
{
	public interface IState : IDisposable
	{
		#region Properties

		string Name { get; set; }

		IState NextState { get; set; }

		bool PreserveNextState { get; set; }

		#endregion

		#region Methods

		string GetDarkName(IGameBase target, Enums.ArticleType articleType, string nameType, bool upshift, bool groupCountOne);

		bool ShouldPreTurnProcess();

		void Execute();

		#endregion
	}
}
