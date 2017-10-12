
// IRoom.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IRoom : IGameBase, IComparable<IRoom>
	{
		#region Properties

		Enums.LightLevel LightLvl { get; set; }

		Enums.RoomType Type { get; set; }

		long Zone { get; set; }

		long[] Dirs { get; set; }

		#endregion

		#region Methods

		long GetDirs(long index);

		long GetDirs(Enums.Direction dir);

		void SetDirs(long index, long value);

		void SetDirs(Enums.Direction dir, long value);

		bool IsLit();

		bool IsDirectionInvalid(Enums.Direction dir);

		bool IsDirectionRoom(Enums.Direction dir);

		bool IsDirectionExit(Enums.Direction dir);

		bool IsDirectionDoor(Enums.Direction dir);

		bool IsDirectionSpecial(Enums.Direction dir, bool includeExit = true);

		long GetDirectionDoorUid(Enums.Direction dir);

		IArtifact GetDirectionDoor(Enums.Direction dir);

		bool IsMonsterListedInRoom(IMonster monster);

		bool IsArtifactListedInRoom(IArtifact artifact);

		T EvalLightLevel<T>(T darkValue, T lightValue);

		T EvalRoomType<T>(T indoorsValue, T outdoorsValue);

		IList<IArtifact> GetTakeableList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		IList<IArtifact> GetEmbeddedList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		IList<IGameBase> GetContainedList(Func<IGameBase, bool> roomFindFunc = null, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true);

		RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false);

		#endregion
	}
}
