
// HintsCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HintsCommand : Command, IHintsCommand
	{
		protected override void PlayerExecute()
		{
			RetCode rc;
			int i, j;

			if (Globals.Database.GetHintsCount() > 0)
			{
				var hints = Globals.Database.HintTable.Records.Where(h => h.Active).OrderBy(h => h.Uid).ToList();

				if (hints.Count > 0)
				{
					Globals.Out.WriteLine("{0}Your question?", Environment.NewLine);

					for (i = 0; i < hints.Count; i++)
					{
						Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, hints[i].Question);
					}

					Globals.Out.Write("{0}{0}Enter your choice: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize01, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					i = Convert.ToInt32(Globals.Buf.Trim().ToString());

					i--;

					if (i >= 0 && i < hints.Count)
					{
						Globals.Out.Write("{0}{1}{0}", Environment.NewLine, hints[i].Question);

						for (j = 0; j < hints[i].NumAnswers; j++)
						{
							Globals.Out.Write("{0}{1}{0}", Environment.NewLine, hints[i].GetAnswers(j));

							if (j + 1 < hints[i].NumAnswers)
							{
								Globals.Out.Write("{0}Another (Y/N): ", Environment.NewLine);

								Globals.Buf.Clear();

								rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

								Debug.Assert(Globals.Engine.IsSuccess(rc));

								if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
								{
									break;
								}
							}
						}
					}
				}
				else
				{
					Globals.Out.WriteLine("{0}There are no hints available at this point in the adventure.", Environment.NewLine);
				}
			}
			else
			{
				Globals.Out.WriteLine("{0}There are no hints available for this adventure.", Environment.NewLine);
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public HintsCommand()
		{
			SortOrder = 390;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "HintsCommand";

			Verb = "hints";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
