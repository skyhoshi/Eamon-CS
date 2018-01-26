
// Field.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;

namespace Eamon.Game
{
	[ClassMappings]
	public class Field : IField
	{
		public virtual string Name { get; set; }

		public virtual long ListNum { get; set; }

		public virtual object UserData { get; set; }

		public virtual Func<IField, IValidateArgs, bool> Validate { get; set; }

		public virtual Func<IField, IValidateArgs, bool> ValidateInterdependencies { get; set; }

		public virtual Action<IField, IPrintDescArgs> PrintDesc { get; set; }

		public virtual Action<IField, IListArgs> List { get; set; }

		public virtual Action<IField, IInputArgs> Input { get; set; }

		public virtual Func<IField, IBuildValueArgs, string> BuildValue { get; set; }

		public virtual Func<string> GetPrintedName { get; set; }

		public virtual Func<object> GetValue { get; set; }
	};
}
