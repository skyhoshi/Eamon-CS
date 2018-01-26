
// ShowCharacterStatusSummaryMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ShowCharacterStatusSummaryMenu : Menu, IShowCharacterStatusSummaryMenu
	{
		protected class AdventuringCharacter
		{
			public virtual IModule Module { get; set; }

			public virtual ICharacter Character { get; set; }
		}

		public virtual void PrintPostShowLineSep()
		{
			Globals.Out.WriteLine();

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("SHOW CHARACTER STATUS SUMMARY", true);

			var advCharList = new List<AdventuringCharacter>();

			var adventureDirs = Globals.Directory.GetDirectories(@"..\..\Adventures");

			var j = (long)adventureDirs.Length;

			var i = 0;

			while (i < j)
			{
				var chrfn = Globals.Path.Combine(adventureDirs[i], "FRESHMEAT.XML");

				if (Globals.File.Exists(Globals.GetPrefixedFileName(chrfn)))
				{
					var modfn = Globals.Path.Combine(adventureDirs[i], "MODULE.XML");

					rc = Globals.PushDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = Globals.Database.LoadCharacters(Globals.GetPrefixedFileName(chrfn), printOutput: false);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rc = Globals.Database.LoadModules(modfn, printOutput: false);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					var character = Globals.Database.CharacterTable.Records.FirstOrDefault();

					Debug.Assert(character != null);

					var module = Globals.Database.ModuleTable.Records.FirstOrDefault();

					Debug.Assert(module != null);

					advCharList.Add(new AdventuringCharacter()
					{
						Character = Globals.CloneInstance(character),

						Module = Globals.CloneInstance(module)
					});

					rc = Globals.PopDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				i++;
			}

			var characterTable = Globals.Database.CharacterTable;

			j = characterTable.GetRecordsCount();

			i = 0;

			foreach (var character in characterTable.Records)
			{
				if (character.Status == Enums.Status.Adventuring)
				{
					var advChar = advCharList.FirstOrDefault(ac => ac.Character.Uid == character.Uid);

					Buf.SetFormat("{0,3}. {1}: {2} ({3})", character.Uid, Globals.Engine.Capitalize(character.Name), character.Status, advChar != null ? Globals.Engine.Capitalize(advChar.Module.Name) : "???");
				}
				else
				{
					Buf.SetFormat("{0,3}. {1}: {2}", character.Uid, Globals.Engine.Capitalize(character.Name), character.Status);
				}

				Globals.Out.Write("{0}{1}", Environment.NewLine, Buf.ToString());

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - 8)) == 0) || i == j - 1)
				{
					nlFlag = false;

					PrintPostShowLineSep();

					Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.Print("{0}", Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}
				}

				i++;
			}

			if (nlFlag)
			{
				Globals.Out.WriteLine();
			}

			Globals.Out.Print("Done showing character status summary.");
		}

		public ShowCharacterStatusSummaryMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
