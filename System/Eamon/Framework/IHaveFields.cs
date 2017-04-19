
// IHaveFields.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;

namespace Eamon.Framework
{
	public interface IHaveFields
	{
		void FreeFields();

		IList<IField> GetFields();

		IField GetField(string name);

		IField GetField(long listNum);
	}
}
