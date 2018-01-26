
// DocumentationViewModel.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Linq;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Mobile.ViewModels
{
	public class DocumentationViewModel : BaseViewModel
	{
		public List<BatchFile> BatchFiles { get; set; }

		public DocumentationViewModel()
		{
			Title = "Documentation";

			BatchFiles = new List<BatchFile>();

			var docFiles = App.GetDocumentationFiles();

			foreach (var docFile in docFiles)
			{
				BatchFiles.Add(new BatchFile()
				{
					Name = ClassMappings.Path.GetFileNameWithoutExtension(docFile),

					FileName = docFile
				});
			}

			BatchFiles = BatchFiles.OrderBy(bf => bf.Name).ToList();
		}

	}
}