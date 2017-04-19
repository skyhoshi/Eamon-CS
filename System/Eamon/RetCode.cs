
// RetCode.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon
{
	public enum RetCode : long
	{
		CycleFound = -21,
		InvalidObj,
		Expired,
		NotAllocated,
		Unsupported,
		AlreadyExists,
		Unimplemented,
		InvalidFmt,
		IsFull,
		IsEmpty,
		NotFound02,
		NotFound01,
		NotFound,
		OutOfMemory,
		InvalidArg,
		TimeOut,
		Failure04,
		Failure03,
		Failure02,
		Failure01,
		Failure,
		Success,
		Aborted_S
	}
}
