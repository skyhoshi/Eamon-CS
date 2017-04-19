
// IField.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.Args;

namespace Eamon.Framework
{
	public interface IField
	{
		string Name { get; set; }

		long ListNum { get; set; }

		object UserData { get; set; }

		Func<IField, IValidateArgs, bool> Validate { get; set; }

		Func<IField, IValidateArgs, bool> ValidateInterdependencies { get; set; }

		Action<IField, IPrintDescArgs> PrintDesc { get; set; }

		Action<IField, IListArgs> List { get; set; }

		Action<IField, IInputArgs> Input { get; set; }

		Func<IField, IBuildValueArgs, string> BuildValue { get; set; }

		Func<string> GetPrintedName { get; set; }

		Func<object> GetValue { get; set; }
	};
}
