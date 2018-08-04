
// EamonDDViewModel.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Linq;
using Eamon.Mobile.Models;

namespace Eamon.Mobile.ViewModels
{
	public class EamonDDViewModel : BaseViewModel
	{
		public List<BatchFile> BatchFiles { get; set; }

		public EamonDDViewModel()
		{
			Title = "EamonDD";

			BatchFiles = new List<BatchFile>()
			{
				new BatchFile()
				{
					Name = "EditAdventures",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "ADVENTURES.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditCatalog",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "CATALOG.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditCharacters",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-chrfn", "CHARACTERS.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditContemporary",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "CONTEMPORARY.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditFantasy",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "FANTASY.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditSciFi",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "SCIFI.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditTest",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "TEST.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditWorkbench",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "WORKBENCH.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "EditWorkInProgress",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-fsfn", "WIP.XML", "-rge" }
				},
				new BatchFile()
				{
					Name = "LoadAdventureSupportMenu",

					PluginArgs = new string[] { "-pfn", "EamonRT.dll", "-rge" }
				}
			};

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				BatchFiles.Add(new BatchFile()
				{
					Name = string.Format("Edit{0}", dir),

					PluginArgs = new string[] { "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", string.Format(@"..\..\Adventures\{0}", dir), "-la", "-rge" }
				});
			}

			BatchFiles = BatchFiles.OrderBy(bf => bf.Name).ToList();
		}
	}
}