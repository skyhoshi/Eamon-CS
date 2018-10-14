
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : ICommandParser
	{
		public virtual StringBuilder InputBuf { get; set; }

		public virtual string LastInputStr { get; set; }

		public virtual string[] Tokens { get; set; }

		public virtual long CurrToken { get; set; }

		public virtual long PrepTokenIndex { get; set; }

		public virtual IMonster ActorMonster { get; set; }

		public virtual IRoom ActorRoom { get; set; }

		public virtual IParserData DobjData { get; set; }

		public virtual IParserData IobjData { get; set; }

		public virtual IParserData ObjData { get; set; }

		public virtual IState NextState { get; set; }

		public virtual string GetActiveObjData()
		{
			return ObjData == DobjData ? "DobjData" : "IobjData";
		}

		public virtual void SetArtifact(IArtifact artifact)
		{
			var command = NextState as ICommand;

			Debug.Assert(command != null);

			if (ObjData == DobjData)
			{
				command.Dobj = artifact;
			}
			else
			{
				command.Iobj = artifact;
			}

			ObjData.Obj = artifact;
		}

		public virtual void SetMonster(IMonster monster)
		{
			var command = NextState as ICommand;

			Debug.Assert(command != null);

			if (ObjData == DobjData)
			{
				command.Dobj = monster;
			}
			else
			{
				command.Iobj = monster;
			}

			ObjData.Obj = monster;
		}

		public virtual IArtifact GetArtifact()
		{
			var command = NextState as ICommand;

			Debug.Assert(command != null);

			return ObjData == DobjData ? command.DobjArtifact : command.IobjArtifact;
		}

		public virtual IMonster GetMonster()
		{
			var command = NextState as ICommand;

			Debug.Assert(command != null);

			return ObjData == DobjData ? command.DobjMonster : command.IobjMonster;
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			Tokens = null;

			CurrToken = 0;

			PrepTokenIndex = -1;

			ActorMonster = null;

			ActorRoom = null;

			DobjData = Globals.CreateInstance<IParserData>();

			IobjData = Globals.CreateInstance<IParserData>();

			ObjData = DobjData;

			NextState = null;
		}

		public virtual void ParseName()
		{
			if (ObjData == DobjData)
			{
				var command = NextState as ICommand;

				Debug.Assert(command != null);

				if (string.IsNullOrWhiteSpace(ObjData.QueryDesc))
				{
					ObjData.QueryDesc = string.Format("{0}{1} who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper());
				}

				ObjData.Name = "";

				if (CurrToken < Tokens.Length)
				{
					PrepTokenIndex = command.IsIobjEnabled ? Globals.Engine.FindIndex(Tokens, token => Globals.Engine.Preps.FirstOrDefault(prep => string.Equals(prep, token, StringComparison.OrdinalIgnoreCase)) != null) : -1;

					if (PrepTokenIndex > CurrToken)
					{
						command.Prep = Globals.CloneInstance(Tokens[PrepTokenIndex]);

						var numTokens = PrepTokenIndex - CurrToken;

						ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)numTokens));

						CurrToken += numTokens;
					}
					else
					{
						PrepTokenIndex = -1;

						ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

						CurrToken = Tokens.Length;
					}
				}

				Globals.Buf.SetFormat("{0}", ObjData.Name);

				while (true)
				{
					var mySeen = false;

					var rc = Globals.Engine.StripPrepsAndArticles(Globals.Buf, ref mySeen);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					ObjData.Name = Globals.Buf.ToString().Trim();

					if (string.IsNullOrWhiteSpace(ObjData.Name) && ActorMonster.IsCharacterMonster())
					{
						Globals.Out.Write(ObjData.QueryDesc);

						Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

						Globals.Buf.Trim();
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				Debug.Assert(!string.IsNullOrWhiteSpace(ObjData.QueryDesc));

				ObjData.Name = "";

				if (CurrToken < Tokens.Length && PrepTokenIndex >= 0)
				{
					CurrToken = PrepTokenIndex + 1;

					ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

					CurrToken = Tokens.Length;
				}

				Globals.Buf.SetFormat("{0}", ObjData.Name);

				while (true)
				{
					var mySeen = false;

					var rc = Globals.Engine.StripPrepsAndArticles(Globals.Buf, ref mySeen);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					ObjData.Name = Globals.Buf.ToString().Trim();

					if (string.IsNullOrWhiteSpace(ObjData.Name) && ActorMonster.IsCharacterMonster())
					{
						Globals.Out.Write(ObjData.QueryDesc);

						Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

						Globals.Buf.Trim();
					}
					else
					{
						break;
					}
				}
			}
		}

		public virtual void Execute()
		{
			Debug.Assert(ActorMonster != null);

			ActorRoom = ActorMonster.GetInRoom();

			Debug.Assert(ActorRoom != null);

			if (InputBuf.Length == 0)
			{
				InputBuf.SetFormat("{0}", LastInputStr);

				if (InputBuf.Length > 0 && ActorMonster.IsCharacterMonster())
				{
					if (Environment.NewLine.Length == 1 && Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= Globals.Out.GetBufferHeight())
					{
						Globals.CursorPosition.Y--;
					}

					Globals.Out.SetCursorPosition(Globals.CursorPosition);

					if (Globals.LineWrapUserInput)
					{
						Globals.Engine.LineWrap(InputBuf.ToString(), Globals.Buf, Globals.CommandPrompt.Length);
					}
					else
					{
						Globals.Buf.SetFormat("{0}", InputBuf.ToString());
					}

					Globals.Out.WordWrap = false;

					Globals.Out.WriteLine(Globals.Buf);

					Globals.Out.WordWrap = true;
				}
			}

			LastInputStr = InputBuf.ToString();

			Tokens = LastInputStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (CurrToken < Tokens.Length)
			{
				if (string.Equals(Tokens[CurrToken], "at", StringComparison.OrdinalIgnoreCase))
				{
					Tokens[CurrToken] = "a";
				}

				var command = Globals.CommandList.FirstOrDefault(x => string.Equals(x.Verb, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) && x.IsEnabled(ActorMonster));

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => string.Equals(s, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => (x.Verb.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || x.Verb.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || s.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command != null)
				{
					CurrToken++;

					NextState = Globals.CloneInstance(command);

					command = NextState as ICommand;

					command.CommandParser = this;

					command.ActorMonster = ActorMonster;

					command.ActorRoom = ActorRoom;

					command.Dobj = DobjData.Obj;

					command.Iobj = IobjData.Obj;

					if (ActorMonster.IsCharacterMonster())
					{
						Globals.Engine.CheckPlayerCommand(command);
					}

					if (command.Discarded)
					{
						NextState = command.NextState ?? Globals.CreateInstance<IStartState>();
					}
					else if (!ActorMonster.IsCharacterMonster() || ActorRoom.IsLit() || command.IsDarkEnabled)
					{
						command.FinishParsing();

						if (!command.Discarded && ActorMonster.IsCharacterMonster())
						{
							Globals.Engine.CheckPlayerCommand(command);

							if (command.Discarded)
							{
								NextState = command.NextState ?? Globals.CreateInstance<IStartState>();
							}
						}
					}
					else
					{
						command.Discarded = true;

						NextState = Globals.CreateInstance<IStartState>();
					}

					if (command.Discarded)
					{
						command.Dispose();
					}
				}
			}
		}

		public CommandParser()
		{
			InputBuf = new StringBuilder(Constants.BufSize);

			LastInputStr = "";
		}
	}
}
