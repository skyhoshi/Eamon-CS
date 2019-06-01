
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Room : GameBase, IRoom
	{
		#region Public Properties

		#region Interface IRoom

		public virtual LightLevel LightLvl { get; set; }

		public virtual RoomType Type { get; set; }

		public virtual long Zone { get; set; }

		public virtual long[] Dirs { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeRoomUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					Name);
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || !string.IsNullOrWhiteSpace(Desc))
			{
				buf.Append(Environment.NewLine);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IRoom room)
		{
			return this.Uid.CompareTo(room.Uid);
		}

		#endregion

		#region Interface IRoom

		public virtual long GetDirs(long index)
		{
			return Dirs[index];
		}

		public virtual long GetDirs(Direction dir)
		{
			return GetDirs((long)dir);
		}

		public virtual void SetDirs(long index, long value)
		{
			Dirs[index] = value;
		}

		public virtual void SetDirs(Direction dir, long value)
		{
			SetDirs((long)dir, value);
		}

		public virtual bool IsLit()
		{
			var gameState = Globals?.Engine.GetGameState();

			return gameState != null && Uid == gameState.Ro ? gameState.Lt != 0 : LightLvl == LightLevel.Light;
		}

		public virtual bool IsDirectionInvalid(long index)
		{
			return GetDirs(index) == 0;
		}

		public virtual bool IsDirectionInvalid(Direction dir)
		{
			return IsDirectionInvalid((long)dir);
		}

		public virtual bool IsDirectionRoom(long index)
		{
			return GetDirs(index) > 0 && GetDirs(index) < 1001;
		}

		public virtual bool IsDirectionRoom(Direction dir)
		{
			return IsDirectionRoom((long)dir);
		}

		public virtual bool IsDirectionExit(long index)
		{
			return GetDirs(index) == Constants.DirectionExit;
		}

		public virtual bool IsDirectionExit(Direction dir)
		{
			return IsDirectionExit((long)dir);
		}

		public virtual bool IsDirectionDoor(long index)
		{
			return GetDirs(index) > 1000 && GetDirs(index) < 2001;
		}

		public virtual bool IsDirectionDoor(Direction dir)
		{
			return IsDirectionDoor((long)dir);
		}

		public virtual bool IsDirectionSpecial(long index, bool includeExit = true)
		{
			return GetDirs(index) < 0 && (includeExit || !IsDirectionExit(index));
		}

		public virtual bool IsDirectionSpecial(Direction dir, bool includeExit = true)
		{
			return IsDirectionSpecial((long)dir, includeExit);
		}

		public virtual bool IsDirectionInObviousExitsList(long index)
		{
			return IsDirectionRoom(index) || IsDirectionExit(index);
		}

		public virtual bool IsDirectionInObviousExitsList(Direction dir)
		{
			return IsDirectionInObviousExitsList((long)dir);
		}

		public virtual long GetDirectionDoorUid(Direction dir)
		{
			return IsDirectionDoor(dir) ? GetDirs(dir) - 1000 : 0;
		}

		public virtual IArtifact GetDirectionDoor(Direction dir)
		{
			var uid = GetDirectionDoorUid(dir);

			return Globals.ADB[uid];
		}

		public virtual string GetObviousExits()
		{
			return string.Format("{0}Obvious {1}:  ", Environment.NewLine, EvalRoomType("exits", "paths"));
		}

		public virtual bool IsMonsterListedInRoom(IMonster monster)
		{
			if (monster != null && monster.IsInRoom(this))
			{
				if (monster.IsListed == false)
				{
					monster.Seen = true;
				}

				return monster.IsListed;
			}
			else
			{
				return false;
			}
		}

		public virtual bool IsArtifactListedInRoom(IArtifact artifact)
		{
			if (artifact != null && artifact.IsInRoom(this))
			{
				if (artifact.IsListed == false)
				{
					artifact.Seen = true;
				}

				return artifact.IsListed;
			}
			else
			{
				return false;
			}
		}

		public virtual T EvalLightLevel<T>(T darkValue, T lightValue)
		{
			return IsLit() ? lightValue : darkValue;
		}

		public virtual T EvalRoomType<T>(T indoorsValue, T outdoorsValue)
		{
			return Globals.Engine.EvalRoomType(Type, indoorsValue, outdoorsValue);
		}

		public virtual IList<IArtifact> GetTakeableList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = a => a.IsInRoom(this) && a.Weight <= 900 && !a.IsUnmovable01() && (a.DeadBody == null || a.DeadBody.Field1 == 1);
			}

			var list = Globals.Engine.GetArtifactList(a => roomFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.GeneralContainer != null)
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IArtifact> GetEmbeddedList(Func<IArtifact, bool> roomFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = a => a.IsEmbeddedInRoom(this);
			}

			var list = Globals.Engine.GetArtifactList(a => roomFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.GeneralContainer != null)
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IGameBase> GetContainedList(Func<IGameBase, bool> roomFindFunc = null, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (roomFindFunc == null)
			{
				roomFindFunc = x => (x is IMonster m && m.IsInRoom(this)) || (x is IArtifact a && a.IsInRoom(this));        // && !m.IsCharacterMonster()
			}

			var list = new List<IGameBase>();

			list.AddRange(Globals.Engine.GetMonsterList(m => roomFindFunc(m)));

			list.AddRange(Globals.Engine.GetArtifactList(a => roomFindFunc(a)));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var x in list)
				{
					if (x is IMonster m)
					{
						list01.AddRange(m.GetContainedList(monsterFindFunc, artifactFindFunc, recurse));
					}
					else if (x is IArtifact a)
					{
						if (a.GeneralContainer != null)
						{
							list01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
						}
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			RetCode rc;
			long i, j;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			i = 0;

			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				if (IsDirectionInObviousExitsList(dv))
				{
					i++;
				}
			}

			if (i > 0)
			{
				j = 0;

				foreach (var dv in directionValues)
				{
					if (IsDirectionInObviousExitsList(dv))
					{
						var direction = Globals.Engine.GetDirections(dv);

						Debug.Assert(direction != null);

						buf.AppendFormat("{0}{1}",
							j == 0 ? "" : j == i - 1 ? " and " : ", ",
							useNames ? (modFunc != null ? modFunc(direction.Name) : direction.Name) :
							(modFunc != null ? modFunc(direction.Abbr) : direction.Abbr));

						if (++j == i)
						{
							break;
						}
					}
				}
			}
			else
			{
				buf.Append("none");
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false)
		{
			bool showDesc;
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			showDesc = false;

			if (monsterFindFunc == null)
			{
				monsterFindFunc = IsMonsterListedInRoom;
			}

			var monsters = Globals.Engine.GetMonsterList(m => monsterFindFunc(m));

			if (artifactFindFunc == null)
			{
				artifactFindFunc = IsArtifactListedInRoom;
			}

			var artifacts = Globals.Engine.GetArtifactList(a => artifactFindFunc(a));

			buf.AppendFormat("{0}[{1}]", Environment.NewLine, Name);

			if (verboseRoomDesc || Seen == false)
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);

				if (monsters.Any() || artifacts.Any())
				{
					while (buf.Length > 0 && Char.IsWhiteSpace(buf[buf.Length - 1]))
					{
						buf.Length--;
					}

					buf.Append("  ");
				}

				showDesc = true;

				Seen = true;
			}

			var combined = new List<IGameBase>();

			if (!verboseMonsterDesc)
			{
				combined.AddRange(monsters.Where(m => m.Seen));
			}

			if (!verboseArtifactDesc)
			{
				combined.AddRange(artifacts.Where(a => a.Seen));
			}

			if (combined.Any())
			{
				buf.AppendFormat("{0}You {1}{2}",
					!showDesc ? Environment.NewLine : "",
					showDesc ? "also " : "",
					showDesc && !monsters.Any() ? "notice " : "see ");

				rc = Globals.Engine.GetRecordNameList(combined, ArticleType.A, true, StateDescDisplayCode.AllStateDescs, true, false, buf);

				if (Globals.Engine.IsFailure(rc))
				{
					// PrintError

					goto Cleanup;
				}

				buf.Append(".");
			}
			else if (!showDesc)
			{
				buf.Append(Environment.NewLine);
			}

			buf.AppendFormat(GetObviousExits());

			rc = GetExitList(buf, s => s.ToLower());

			if (Globals.Engine.IsFailure(rc))
			{
				// PrintError

				goto Cleanup;
			}

			buf.AppendFormat(".{0}", Environment.NewLine);

			combined.Clear();

			combined.AddRange(monsters.Where(m => verboseMonsterDesc || !m.Seen));

			combined.AddRange(artifacts.Where(a => verboseArtifactDesc || !a.Seen));

			foreach (var r in combined)
			{
				rc = r.BuildPrintedFullDesc(buf, true);

				if (Globals.Engine.IsFailure(rc))
				{
					// PrintError

					goto Cleanup;
				}

				r.Seen = true;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode BuildPrintedTooDarkToSeeDesc(StringBuilder buf)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf.SetPrint("It's too dark to see.");

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Room

		public Room()
		{
			Dirs = new long[(long)EnumUtil.GetLastValue<Direction>() + 1];
		}

		#endregion

		#endregion
	}
}
