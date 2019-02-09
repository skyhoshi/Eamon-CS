
// IRoom.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IRoom : IGameBase, IComparable<IRoom>
	{
		#region Properties

		/// <summary></summary>
		Enums.LightLevel LightLvl { get; set; }

		/// <summary></summary>
		Enums.RoomType Type { get; set; }

		/// <summary></summary>
		long Zone { get; set; }

		/// <summary></summary>
		long[] Dirs { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetDirs(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDirs(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetDirs(long index, long value);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="value"></param>
		void SetDirs(Enums.Direction dir, long value);

		/// <summary></summary>
		/// <returns></returns>
		bool IsLit();

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionInvalid(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionInvalid(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionRoom(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionRoom(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionExit(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionExit(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsDirectionDoor(long index);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsDirectionDoor(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="includeExit"></param>
		/// <returns></returns>
		bool IsDirectionSpecial(long index, bool includeExit = true);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <param name="includeExit"></param>
		/// <returns></returns>
		bool IsDirectionSpecial(Enums.Direction dir, bool includeExit = true);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		long GetDirectionDoorUid(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		IArtifact GetDirectionDoor(Enums.Direction dir);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsMonsterListedInRoom(IMonster monster);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool IsArtifactListedInRoom(IArtifact artifact);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="indoorsValue"></param>
		/// <param name="outdoorsValue"></param>
		/// <returns></returns>
		T EvalRoomType<T>(T indoorsValue, T outdoorsValue);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetTakeableList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetEmbeddedList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="roomFindFunc"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IGameBase> GetContainedList(Func<IGameBase, bool> roomFindFunc = null, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="modFunc"></param>
		/// <param name="useNames"></param>
		/// <returns></returns>
		RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="verboseRoomDesc"></param>
		/// <param name="verboseMonsterDesc"></param>
		/// <param name="verboseArtifactDesc"></param>
		/// <returns></returns>
		RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false);

		#endregion
	}
}
