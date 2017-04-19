
// IConfig.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.DataEntry;
using Eamon.Framework.Validation;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IConfig : IHaveUid, IHaveFields, IValidator, IEditable, IComparable<IConfig>
	{
		#region Properties

		bool ShowDesc { get; set; }

		bool ResolveEffects { get; set; }

		bool GenerateUids { get; set; }

		Enums.FieldDesc FieldDesc { get; set; }

		long WordWrapMargin { get; set; }

		string DdFilesetFileName { get; set; }

		string DdCharacterFileName { get; set; }

		string DdModuleFileName { get; set; }

		string DdRoomFileName { get; set; }

		string DdArtifactFileName { get; set; }

		string DdEffectFileName { get; set; }

		string DdMonsterFileName { get; set; }

		string DdHintFileName { get; set; }

		string MhWorkDir { get; set; }

		string MhFilesetFileName { get; set; }

		string MhCharacterFileName { get; set; }

		string MhEffectFileName { get; set; }

		string RtFilesetFileName { get; set; }

		string RtCharacterFileName { get; set; }

		string RtModuleFileName { get; set; }

		string RtRoomFileName { get; set; }

		string RtArtifactFileName { get; set; }

		string RtEffectFileName { get; set; }

		string RtMonsterFileName { get; set; }

		string RtHintFileName { get; set; }

		string RtGameStateFileName { get; set; }

		bool DdEditingFilesets { get; set; }

		bool DdEditingCharacters { get; set; }

		bool DdEditingModules { get; set; }

		bool DdEditingRooms { get; set; }

		bool DdEditingArtifacts { get; set; }

		bool DdEditingEffects { get; set; }

		bool DdEditingMonsters { get; set; }

		bool DdEditingHints { get; set; }

		#endregion

		#region Methods

		RetCode LoadGameDatabase(bool validate = true, bool printOutput = true);

		RetCode SaveGameDatabase(bool printOutput = true);

		RetCode DeleteGameState(string configFileName, bool startOver);

		RetCode CopyProperties(IConfig config);

		#endregion
	}
}
