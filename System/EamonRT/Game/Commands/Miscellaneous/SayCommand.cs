
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SayCommand : Command, ISayCommand
	{
		public const long PpeBeforePlayerSayTextPrint = 1;

		public const long PpeAfterPlayerSay = 2;

		public virtual string OriginalPhrase { get; set; }

		public virtual string PrintedPhrase { get; set; }

		public virtual string ProcessedPhrase { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(OriginalPhrase));

			PrintedPhrase = OriginalPhrase.Trim(new char[] { ' ', '"', '\'' }).FirstCharToUpper();

			if (!Char.IsPunctuation(PrintedPhrase[PrintedPhrase.Length - 1]))
			{
				PrintedPhrase += ".";
			}

			ProcessedPhrase = PrintedPhrase.Trim(new char[] { '.', '?', '!' }).ToLower();

			PlayerProcessEvents(PpeBeforePlayerSayTextPrint);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			Globals.Out.Print("Okay, \"{0}\"", PrintedPhrase);

			PlayerProcessEvents(PpeAfterPlayerSay);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				OriginalPhrase = CommandParser.InputBuf.ToString().Replace(CommandParser.Tokens[0] + " ", "");

				CommandParser.CurrToken += (CommandParser.Tokens.Length - CommandParser.CurrToken);
			}

			while (true)
			{
				if (string.IsNullOrWhiteSpace(OriginalPhrase))
				{
					Globals.Out.Write("{0}{1} who or what? ", Environment.NewLine, Verb.FirstCharToUpper());

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					OriginalPhrase = Globals.Buf.ToString();
				}
				else
				{
					break;
				}
			}
		}

		public SayCommand()
		{
			SortOrder = 340;

			Name = "SayCommand";

			Verb = "say";

			Type = CommandType.Miscellaneous;
		}
	}
}
