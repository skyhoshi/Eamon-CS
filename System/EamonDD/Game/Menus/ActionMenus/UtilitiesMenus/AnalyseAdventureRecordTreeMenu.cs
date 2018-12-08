
// AnalyseAdventureRecordTreeMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseAdventureRecordTreeMenu : Menu, IAnalyseAdventureRecordTreeMenu
	{
		public virtual IList<string> RecordTreeStrings { get; set; }

		protected virtual void AnalyseArtifactRecordTree(IArtifact artifact, string tag, long indentLevel)
		{
			Debug.Assert(artifact != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStrings.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, artifact.Uid, artifact.GetDecoratedName02(true, true, false, false, Buf)));

			var containedList = artifact.GetContainedList();

			foreach (var containedArtifact in containedList)
			{
				AnalyseArtifactRecordTree(containedArtifact, "CA", indentLevel + 1);
			}

			RecordTreeStrings.Add(string.Format("{0}]", containedList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		protected virtual void AnalyseMonsterRecordTree(IMonster monster, string tag, long indentLevel)
		{
			Debug.Assert(monster != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStrings.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, monster.Uid, monster.GetDecoratedName02(true, true, false, false, Buf)));

			var wornList = monster.GetWornList();

			foreach (var wornArtifact in wornList)
			{
				AnalyseArtifactRecordTree(wornArtifact, "WA", indentLevel + 1);
			}

			var carriedList = monster.GetCarriedList();

			foreach (var carriedArtifact in carriedList)
			{
				AnalyseArtifactRecordTree(carriedArtifact, "CA", indentLevel + 1);
			}

			RecordTreeStrings.Add(string.Format("{0}]", wornList.Count > 0 || carriedList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		protected virtual void AnalyseRoomRecordTree(IRoom room, string tag, long indentLevel)
		{
			Debug.Assert(room != null && tag != null && indentLevel > 0);

			var indentString = new string('\t', (int)indentLevel);

			RecordTreeStrings.Add(string.Format("{0}{1}[{2}{3}: {4}", Environment.NewLine, indentString, tag, room.Uid, room.Name));

			var monsterList = Globals.Engine.GetMonsterList(m => m.IsInRoom(room));

			foreach (var monster in monsterList)
			{
				AnalyseMonsterRecordTree(monster, "M", indentLevel + 1);
			}

			var artifactList = Globals.Engine.GetArtifactList(a => a.IsInRoom(room) || a.IsEmbeddedInRoom(room));

			foreach (var artifact in artifactList)
			{
				AnalyseArtifactRecordTree(artifact, artifact.IsEmbeddedInRoom() ? "EA" : "A", indentLevel + 1);
			}

			RecordTreeStrings.Add(string.Format("{0}]", monsterList.Count > 0 || artifactList.Count > 0 ? Environment.NewLine + indentString : ""));
		}

		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("ANALYSE ADVENTURE RECORD TREE", true);

			Debug.Assert(Globals.Engine.IsAdventureFilesetLoaded());

			RecordTreeStrings.Clear();

			RecordTreeStrings.Add(string.Format("{0}[{1}",
				Environment.NewLine,
				Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName));

			var roomList = Globals.Database.RoomTable.Records;

			foreach (var room in roomList)
			{
				AnalyseRoomRecordTree(room, "R", 1);
			}

			var monsterList = Globals.Engine.GetMonsterList(m => !m.IsInRoom());

			foreach (var monster in monsterList)
			{
				AnalyseMonsterRecordTree(monster, "M", 1);
			}

			var artifactList = Globals.Engine.GetArtifactList(a => !a.IsInRoom() && !a.IsEmbeddedInRoom() && !a.IsCarriedByMonster() && !a.IsWornByMonster() && !a.IsCarriedByContainer());

			foreach (var artifact in artifactList)
			{
				AnalyseArtifactRecordTree(artifact, "A", 1);
			}

			RecordTreeStrings.Add(string.Format("{0}]", roomList.Count > 0 || monsterList.Count > 0 || artifactList.Count > 0 ? Environment.NewLine : ""));

			Globals.Out.Write("{0}Would you like to use page breaks (Y/N) [Y]: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "Y", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Out.Print("{0}", Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] != 'N')
			{
				var i = 0;

				var j = 0;

				var k = RecordTreeStrings.Count;

				while (i < k)
				{
					Globals.Out.Write(RecordTreeStrings[i]);

					if (RecordTreeStrings[i].Contains(Environment.NewLine))
					{
						j++;
					}

					nlFlag = true;

					if (i == k - 1 || (j >= Constants.NumRows - 10 && (RecordTreeStrings[i + 1].StartsWith(Environment.NewLine + "\t[R") || RecordTreeStrings[i + 1].StartsWith(Environment.NewLine + "\t[M") || RecordTreeStrings[i + 1].StartsWith(Environment.NewLine + "\t[A"))))
					{
						nlFlag = false;

						Globals.Out.WriteLine();

						Globals.Out.Print("{0}", Globals.LineSep);

						Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Out.Print("{0}", Globals.LineSep);

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							break;
						}

						j = 0;
					}

					i++;
				}
			}
			else
			{
				for (var i = 0; i < RecordTreeStrings.Count; i++)
				{
					Globals.Out.Write(RecordTreeStrings[i]);
				}

				Globals.Out.WriteLine();

				Globals.In.KeyPress(Buf);

				Globals.Out.Print("{0}", Globals.LineSep);
			}

			if (nlFlag)
			{
				Globals.Out.WriteLine();
			}

			Globals.Out.Print("Done analysing adventure record tree.");
		}

		public AnalyseAdventureRecordTreeMenu()
		{
			Buf = Globals.Buf;

			RecordTreeStrings = new List<string>();
		}
	}
}
