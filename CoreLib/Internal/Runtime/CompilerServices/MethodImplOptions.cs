﻿using System;

namespace Internal.Runtime.CompilerServices
{
	// This Enum matchs the miImpl flags defined in corhdr.h. It is used to specify
	// certain method properties.
	[Flags]
	public enum MethodImplOptions
	{
		Unmanaged = 0x0004,
		NoInlining = 0x0008,
		ForwardRef = 0x0010,
		Synchronized = 0x0020,
		NoOptimization = 0x0040,
		PreserveSig = 0x0080,
		AggressiveInlining = 0x0100,
		AggressiveOptimization = 0x0200,
		InternalCall = 0x1000
	}
}
