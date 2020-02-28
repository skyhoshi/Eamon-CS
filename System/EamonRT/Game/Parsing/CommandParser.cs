
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
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

		public virtual IPrep Prep { get; set; }

		public virtual IMonster ActorMonster { get; set; }

		public virtual IRoom ActorRoom { get; set; }

		public virtual IParserData DobjData { get; set; }

		public virtual IParserData IobjData { get; set; }

		public virtual IParserData ObjData { get; set; }

		public virtual IState NextState { get; set; }

		public virtual ICommand NextCommand
		{
			get
			{
				return NextState as ICommand;
			}
		}

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

		public virtual StringBuilder ReplacePrepositions(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetFormat(" {0} ", Regex.Replace(buf.ToString(), @"\s+", " ").ToLower().Trim());

			buf = buf.Replace(" in to ", " into ");

			buf = buf.Replace(" inside ", " in ");

			buf = buf.Replace(" from in ", " fromin ");

			buf = buf.Replace(" on to ", " onto ");

			buf = buf.Replace(" on top of ", " on ");

			buf = buf.Replace(" from on ", " fromon ");

			buf = buf.Replace(" below ", " under ").Replace(" beneath ", " under ").Replace(" underneath ", " under ");

			buf = buf.Replace(" from under ", " fromunder ");

			buf = buf.Replace(" in back of ", " behind ");

			buf = buf.Replace(" from behind ", " frombehind ");

			return buf.Trim();
		}

		public virtual void Clear()
		{
			InputBuf.Clear();

			Tokens = null;

			CurrToken = 0;

			PrepTokenIndex = -1;

			Prep = null;

			ActorMonster = null;

			ActorRoom = null;

			DobjData = Globals.CreateInstance<IParserData>();

			IobjData = Globals.CreateInstance<IParserData>();

			ObjData = DobjData;

			NextState = null;
		}

		public virtual void ParseName()
		{
			RetCode rc;

			var command = NextState as ICommand;

			Debug.Assert(command != null);

			if (ObjData.Name == null)
			{
				ObjData.Name = "";

				while (string.IsNullOrWhiteSpace(ObjData.Name))
				{
					if (CurrToken < Tokens.Length && PrepTokenIndex < 0)
					{
						PrepTokenIndex = command.IsDobjPrepEnabled || command.IsIobjEnabled ? gEngine.FindIndex(Tokens, token => gEngine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, token, StringComparison.OrdinalIgnoreCase) && command.IsPrepEnabled(prep)) != null) : -1;

						Prep = PrepTokenIndex >= 0 ? gEngine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, Tokens[PrepTokenIndex], StringComparison.OrdinalIgnoreCase) && command.IsPrepEnabled(prep)) : null;

						if (((ObjData == DobjData && command.IsDobjPrepEnabled) || (ObjData == IobjData && command.IsIobjEnabled)) && PrepTokenIndex == CurrToken)
						{
							command.Prep = Globals.CloneInstance(Prep);

							command.ContainerType = Prep.ContainerType;

							var numTokens = Tokens.Length - CurrToken;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)(CurrToken + 1)).Take((int)(numTokens - 1)));

							CurrToken += numTokens;
						}
						else if ((ObjData == DobjData && command.IsIobjEnabled) && PrepTokenIndex > CurrToken)
						{
							command.Prep = Globals.CloneInstance(Prep);

							command.ContainerType = Prep.ContainerType;

							var numTokens = PrepTokenIndex - CurrToken;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken).Take((int)numTokens));

							CurrToken += numTokens;
						}
						else
						{
							PrepTokenIndex = -1;

							Prep = null;

							ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

							CurrToken = Tokens.Length;
						}
					}

					if (ObjData == IobjData && CurrToken < Tokens.Length && PrepTokenIndex >= 0)
					{
						CurrToken = PrepTokenIndex + 1;
					}

					if (CurrToken < Tokens.Length && string.IsNullOrWhiteSpace(ObjData.Name))
					{
						ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

						CurrToken = Tokens.Length;
					}

					if (string.IsNullOrWhiteSpace(ObjData.Name))
					{
						Debug.Assert(ActorMonster.IsCharacterMonster());

						if (ObjData == DobjData)
						{
							if (ObjData.QueryDescFunc == null)
							{
								ObjData.QueryDescFunc = () => string.Format("{0}{1} {2}who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper(), command.IsDobjPrepEnabled && command.Prep != null && Enum.IsDefined(typeof(ContainerType), command.Prep.ContainerType) ? gEngine.EvalContainerType(command.Prep.ContainerType, "inside ", "on ", "under ", "behind ") : "");
							}
						}
						else
						{
							Debug.Assert(ObjData.QueryDescFunc != null);
						}

						gOut.Write(ObjData.QueryDescFunc());

						Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

						Globals.Buf = ReplacePrepositions(Globals.Buf);

						if (command.ShouldStripTrailingPunctuation())
						{
							Globals.Buf.SetFormat("{0}", Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim());
						}

						Tokens = Tokens.Concat(Globals.Buf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
					}
					else
					{
						var mySeen = false;

						Globals.Buf.SetFormat("{0}", ObjData.Name);

						rc = gEngine.StripPrepsAndArticles(Globals.Buf, ref mySeen);

						Debug.Assert(gEngine.IsSuccess(rc));

						ObjData.Name = Globals.Buf.ToString().Trim();
					}
				}
			}
		}

		public virtual void CheckPlayerCommand(ICommand command, bool afterFinishParsing)
		{
			Debug.Assert(command != null);

			// do nothing
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
					if (Environment.NewLine.Length == 1 && Globals.CursorPosition.Y > -1 && Globals.CursorPosition.Y + 1 >= gOut.GetBufferHeight())
					{
						Globals.CursorPosition.Y--;
					}

					gOut.SetCursorPosition(Globals.CursorPosition);

					if (Globals.LineWrapUserInput)
					{
						gEngine.LineWrap(InputBuf.ToString(), Globals.Buf, Globals.CommandPrompt.Length);
					}
					else
					{
						Globals.Buf.SetFormat("{0}", InputBuf.ToString());
					}

					gOut.WordWrap = false;

					gOut.WriteLine(Globals.Buf);

					gOut.WordWrap = true;
				}
			}

			LastInputStr = InputBuf.ToString();

			InputBuf = ReplacePrepositions(InputBuf);

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (CurrToken < Tokens.Length)
			{
				if (Tokens.Length == 1)
				{
					Globals.Buf.SetFormat("{0}", Tokens[CurrToken]);

					Tokens[CurrToken] = Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim();
				}

				if (Tokens[CurrToken].Length == 0)
				{
					Tokens[CurrToken] = "???";
				}
				else if (string.Equals(Tokens[CurrToken], "at", StringComparison.OrdinalIgnoreCase))
				{
					Tokens[CurrToken] = "a";
				}

				var command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && string.Equals(x.Verb, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) && x.IsEnabled(ActorMonster));

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => string.Equals(s, Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && (x.Verb.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || x.Verb.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) && x.IsEnabled(ActorMonster));
				}

				if (command == null)
				{
					command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.StartsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase) || s.EndsWith(Tokens[CurrToken], StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(ActorMonster));
				}

				if (command != null)
				{
					CurrToken++;

					NextState = Activator.CreateInstance(command.GetType()) as IState;

					command = NextState as ICommand;

					Debug.Assert(command != null);

					command.CommandParser = this;

					command.ActorMonster = ActorMonster;

					command.ActorRoom = ActorRoom;

					command.Dobj = DobjData.Obj;

					command.Iobj = IobjData.Obj;

					if (command.ShouldStripTrailingPunctuation() && Tokens.Length > 1)
					{
						Globals.Buf.SetFormat("{0}", Tokens[Tokens.Length - 1]);

						Tokens[Tokens.Length - 1] = Globals.Buf.TrimEndPunctuationMinusPound().ToString().Trim();
					}

					if (ActorMonster.IsCharacterMonster())
					{
						CheckPlayerCommand(command, false);

						if (NextState == command)
						{
							if (ActorRoom.IsLit() || command.IsDarkEnabled)
							{
								command.FinishParsing();

								if (NextState == command)
								{
									CheckPlayerCommand(command, true);
								}
							}
							else
							{
								NextState = null;
							}
						}
					}
					else
					{
						command.FinishParsing();
					}

					if (NextState == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
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
