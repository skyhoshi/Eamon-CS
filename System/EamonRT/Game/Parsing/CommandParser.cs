
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
			var command = NextState as ICommand;

			Debug.Assert(command != null);

			if (ObjData == DobjData)
			{
				ObjData.Name = "";

				if (CurrToken < Tokens.Length)
				{
					PrepTokenIndex = command.IsDobjPrepEnabled || command.IsIobjEnabled ? Globals.Engine.FindIndex(Tokens, token => Globals.Engine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, token, StringComparison.OrdinalIgnoreCase) && command.IsPrepEnabled(prep)) != null) : -1;

					Prep = PrepTokenIndex >= 0 ? Globals.Engine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, Tokens[PrepTokenIndex], StringComparison.OrdinalIgnoreCase) && command.IsPrepEnabled(prep)) : null;

					if (command.IsDobjPrepEnabled && PrepTokenIndex == CurrToken)
					{
						command.Prep = Globals.CloneInstance(Prep);

						var numTokens = Tokens.Length - CurrToken;

						ObjData.Name = string.Join(" ", Tokens.Skip((int)(CurrToken + 1)).Take((int)(numTokens - 1)));

						CurrToken += numTokens;
					}
					else if (command.IsIobjEnabled && PrepTokenIndex > CurrToken)
					{
						command.Prep = Globals.CloneInstance(Prep);

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

				Globals.Buf.SetFormat("{0}", ObjData.Name);

				if (string.IsNullOrWhiteSpace(ObjData.QueryDesc))
				{
					ObjData.QueryDesc = string.Format("{0}{1} {2}who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper(), command.IsDobjPrepEnabled && command.Prep != null && Enum.IsDefined(typeof(ContainerType), command.Prep.ContainerType) ? Globals.Engine.EvalContainerType(command.Prep.ContainerType, "inside ", "on ", "under ", "behind ") : "");
				}

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

						if (command.ShouldStripTrailingPunctuation())
						{
							Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString().Trim(), @"\p{P}*$", "").Trim());
						}
						else
						{
							Globals.Buf.Trim();
						}
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				ObjData.Name = "";

				if (CurrToken < Tokens.Length && PrepTokenIndex >= 0)
				{
					CurrToken = PrepTokenIndex + 1;

					ObjData.Name = string.Join(" ", Tokens.Skip((int)CurrToken));

					CurrToken = Tokens.Length;
				}

				Globals.Buf.SetFormat("{0}", ObjData.Name);

				Debug.Assert(!string.IsNullOrWhiteSpace(ObjData.QueryDesc));

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

						if (command.ShouldStripTrailingPunctuation())
						{
							Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString().Trim(), @"\p{P}*$", "").Trim());
						}
						else
						{
							Globals.Buf.Trim();
						}
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

			InputBuf.SetFormat(" {0} ", Regex.Replace(InputBuf.ToString(), @"\s+", " ").ToLower().Trim());

			InputBuf = InputBuf.Replace(" in to ", " into ");

			InputBuf = InputBuf.Replace(" inside ", " in ");

			InputBuf = InputBuf.Replace(" from in ", " fromin ");

			InputBuf = InputBuf.Replace(" on to ", " onto ");

			InputBuf = InputBuf.Replace(" on top of ", " on ");

			InputBuf = InputBuf.Replace(" from on ", " fromon ");

			InputBuf = InputBuf.Replace(" below ", " under ").Replace(" beneath ", " under ").Replace(" underneath ", " under ");

			InputBuf = InputBuf.Replace(" from under ", " fromunder ");

			InputBuf = InputBuf.Replace(" in back of ", " behind ");

			InputBuf = InputBuf.Replace(" from behind ", " frombehind ");

			InputBuf = InputBuf.Trim();

			Tokens = InputBuf.ToString().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			if (CurrToken < Tokens.Length)
			{
				if (Tokens.Length == 1)
				{
					Tokens[CurrToken] = Regex.Replace(Tokens[CurrToken], @"\p{P}*$", "").Trim();
				}

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
						Tokens[Tokens.Length - 1] = Regex.Replace(Tokens[Tokens.Length - 1], @"\p{P}*$", "").Trim();
					}

					if (ActorMonster.IsCharacterMonster())
					{
						Globals.Engine.CheckPlayerCommand(command, false);

						if (NextState == command)
						{
							if (ActorRoom.IsLit() || command.IsDarkEnabled)
							{
								command.FinishParsing();

								if (NextState == command)
								{
									Globals.Engine.CheckPlayerCommand(command, true);
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
