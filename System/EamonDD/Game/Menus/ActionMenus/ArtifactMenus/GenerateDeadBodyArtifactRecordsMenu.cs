
// GenerateDeadBodyArtifactRecordsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
	public class GenerateDeadBodyArtifactRecordsMenu : Menu, IGenerateDeadBodyArtifactRecordsMenu
	{
		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			var artUids = new long[2];

			var monUids = new long[2];

			var nlFlag = false;

			var exited = false;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("GENERATE DEAD BODY ARTIFACT RECORDS", true);

			var maxMonUid = Globals.Database.GetMonsterUid(false);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(43, '\0', 0, "Enter the starting monster uid", "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			monUids[0] = Convert.ToInt64(Buf.Trim().ToString());

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(43, '\0', 0, "Enter the ending monster uid", maxMonUid > 0 ? maxMonUid.ToString() : "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, maxMonUid > 0 ? maxMonUid.ToString() : "1", null, Globals.Engine.IsCharDigit, null);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			monUids[1] = Convert.ToInt64(Buf.Trim().ToString());

			var monsters = Globals.Engine.GetMonsterList(() => true, m => m.Uid >= monUids[0] && m.Uid <= monUids[1]);

			var k = monsters.Count();

			if (k > 0)
			{
				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}

			var j = 0;

			foreach (var monster in monsters)
			{
				if (!exited)
				{
					monster.ListRecord(false, false, false, false, false, false);

					nlFlag = true;

					if (j != 0 && ((j % (Constants.NumRows - 8)) == 0 || j == k - 1))
					{
						nlFlag = false;

						Globals.Out.WriteLine();

						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							exited = true;
						}
						else if (j < k - 1)
						{
							Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
						}
					}
				}

				j++;
			}

			if (nlFlag)
			{
				Globals.Out.WriteLine();
			}

			if (j > 0)
			{
				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Would you like to generate {1} (Y/N): ", Environment.NewLine, j > 1 ? "dead body artifact records" : "a dead body artifact record");

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				for (var i = 0; i < j; i++)
				{
					var monster = monsters.ElementAt(i);

					Debug.Assert(monster != null);

					var lastChar = monster.Name.Length > 0 ? monster.Name[monster.Name.Length - 1] : '\0';

					artifact = Globals.CreateInstance<IArtifact>(x =>
					{
						x.Uid = Globals.Database.GetArtifactUid();
						x.Name = string.Format("{0}{1} body", monster.Name, char.ToUpper(lastChar) != 'S' ? "'s" : "'");
						x.Desc = string.Format("You see {0}.", x.Name);
						x.IsListed = true;
						x.Weight = 150;
						x.GetClasses(0).Type = Enums.ArtifactType.DeadBody;
					});

					if (i == 0)
					{
						artUids[0] = artifact.Uid;
					}

					if (i == j - 1)
					{
						artUids[1] = artifact.Uid;
					}

					rc = Globals.Database.AddArtifact(artifact);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.ArtifactsModified = true;

					if (Globals.Module != null)
					{
						Globals.Module.NumArtifacts++;

						Globals.ModulesModified = true;
					}
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Buf.SetFormat(j > 1 ? "Generated dead body artifacts with uids between {0} and {1}, inclusive." : "Generated a dead body artifact with uid {0}.", artUids[0], artUids[1]);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Buf);
			}

		Cleanup:

			;
		}

		public GenerateDeadBodyArtifactRecordsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
