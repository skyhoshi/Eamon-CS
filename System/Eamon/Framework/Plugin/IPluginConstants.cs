
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Plugin
{
	public interface IPluginConstants
	{
		string ToughDesc { get; }

		string CourageDesc { get; }

		int ArtNameLen { get; }

		int ArtStateDescLen { get; }

		int ArtDescLen { get; }

		int CharNameLen { get; }

		int CharWpnNameLen { get; }

		int EffDescLen { get; }

		int FsNameLen { get; }

		int FsFileNameLen { get; }

		int HntQuestionLen { get; }

		int HntAnswerLen { get; }

		int ModNameLen { get; }

		int ModDescLen { get; }

		int ModAuthorLen { get; }

		int ModVolLabelLen { get; }

		int ModSerialNumLen { get; }

		int MonNameLen { get; }

		int MonStateDescLen { get; }

		int MonDescLen { get; }

		int RmNameLen { get; }

		int RmDescLen { get; }

		long AxePrice { get; }

		long BowPrice { get; }

		long MacePrice { get; }

		long SpearPrice { get; }

		long SwordPrice { get; }

		long ShieldPrice { get; }

		long LeatherArmorPrice { get; }

		long ChainMailPrice { get; }

		long PlateMailPrice { get; }

		long BlastPrice { get; }

		long HealPrice { get; }

		long SpeedPrice { get; }

		long PowerPrice { get; }

		long RecallPrice { get; }

		long StatGainPrice { get; }

		long WeaponTrainingPrice { get; }

		long ArmorTrainingPrice { get; }

		long SpellTrainingPrice { get; }

		long InfoBoothPrice { get; }

		long FountainPrice { get; }

		long NumCacheItems { get; }

		long NumDatabases { get; }

		long NumArtifactCategories { get; }

		int BufSize { get; }

		int BufSize01 { get; }

		int BufSize02 { get; }

		int BufSize03 { get; }

		string ResolveEffectRegexPattern { get; }

		string ResolveUidMacroRegexPattern { get; }

		string RecIdepErrorFmtStr { get; }

		string AndroidAdventuresDir { get; }

		string AdventuresDir { get; }

		string QuickLaunchDir { get; }

		string DefaultWorkDir { get; }

		string ProcessMutexName { get; }

		string EamonAdventuresSlnFile { get; }

		string StackTraceFile { get; }

		string ProgVersion { get; }

		long InfiniteDrinkableEdible { get; }

		long DirectionExit { get; }

		long MinGoldValue { get; }

		long MaxGoldValue { get; }

		long MaxPathLen { get; }

		long MaxRecursionLevel { get; }

		int WindowWidth { get; }

		int WindowHeight { get; }

		int BufferWidth { get; }

		int BufferHeight { get; }

		long RightMargin { get; }

		long NumRows { get; }
	}
}
