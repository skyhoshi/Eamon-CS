
// WearCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WearCommand : Command, IWearCommand
	{
		protected virtual void PlayerProcessEvents()
		{

		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var dobjAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Wearable);

			if (dobjAc != null)
			{
				if (DobjArtifact.IsWornByCharacter())
				{
					Globals.Out.WriteLine("{0}You're already wearing {1}!", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					PrintTakingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>();

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<IWearCommand>();

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				if (dobjAc.Field5 > 0)
				{
					var arArtifact = Globals.ADB[Globals.GameState.Ar];

					var shArtifact = Globals.ADB[Globals.GameState.Sh];

					var arAc = arArtifact != null ? arArtifact.GetArtifactClass(Enums.ArtifactType.Wearable) : null;

					var shAc = shArtifact != null ? shArtifact.GetArtifactClass(Enums.ArtifactType.Wearable) : null;

					if (dobjAc.Field5 > 1)
					{
						if (dobjAc.Field5 > 14)
						{
							dobjAc.Field5 = 14;
						}

						if (arAc != null)
						{
							Globals.Out.WriteLine("{0}You're already wearing armor!", Environment.NewLine);

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						Globals.GameState.Ar = DobjArtifact.Uid;

						ActorMonster.Armor = (dobjAc.Field5 / 2) + (shAc != null ? shAc.Field5 : 0);
					}
					else
					{
						if (shAc != null)
						{
							Globals.Out.WriteLine("{0}You're already wearing a shield!", Environment.NewLine);

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						Globals.GameState.Sh = DobjArtifact.Uid;

						ActorMonster.Armor = (arAc != null ? arAc.Field5 / 2 : 0) + dobjAc.Field5;
					}
				}

				DobjArtifact.SetWornByCharacter();

				Globals.Out.Write("{0}{1} worn.{0}", Environment.NewLine, DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));

				PlayerProcessEvents();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{ 
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsWornByCharacter()
			};

			PlayerResolveArtifact();
		}

		public WearCommand()
		{
			SortOrder = 240;

			Name = "WearCommand";

			Verb = "wear";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
