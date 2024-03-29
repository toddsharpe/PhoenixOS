﻿using Internal.Runtime.CompilerServices;
using System;
using System.Runtime;

namespace Internal.Runtime.CompilerHelpers
{
	internal class StartupCodeHelpers
	{
		[RuntimeExport("RhpReversePInvoke")]
		static void RhpReversePInvoke(IntPtr frame) { }
		[RuntimeExport("RhpReversePInvokeReturn")]
		static void RhpReversePInvokeReturn(IntPtr frame) { }
		[RuntimeExport("RhpPInvoke")]
		static void RhpPInvoke(IntPtr frame) { }
		[RuntimeExport("RhpPInvokeReturn")]
		static void RhpPInvokeReturn(IntPtr frame) { }

		[RuntimeExport("RhpFallbackFailFast")]
		static void RhpFallbackFailFast() { while (true) ; }

		[RuntimeExport("RhpNewArray")]
		internal static unsafe object RhpNewArray(EEType* pEEType, int length)
		{
			var size = pEEType->BaseSize + (ulong)length * pEEType->ComponentSize;

			// Round to next power of 8
			if (size % 8 > 0)
				size = ((size / 8) + 1) * 8;

			var data = Platform.Allocate(size);
			var obj = Unsafe.As<IntPtr, object>(ref data);
			Platform.ZeroMemory(data, size);
			SetEEType(data, pEEType);

			var b = (byte*)data;
			b += sizeof(IntPtr);
			Platform.CopyMemory((IntPtr)b, (IntPtr)(&length), sizeof(int));

			return obj;
		}

		internal static unsafe void SetEEType(IntPtr obj, EEType* type)
		{
			Platform.CopyMemory(obj, (IntPtr)(&type), (ulong)sizeof(IntPtr));
		}

		[RuntimeExport("RhpAssignRef")]
		static unsafe void RhpAssignRef(void** address, void* obj)
		{
			*address = obj;
		}

		[RuntimeExport("RhpStelemRef")]
		static unsafe void RhpStelemRef(Array array, int index, object obj)
		{
			fixed (int* n = &array._numComponents)
			{
				var ptr = (byte*)n;
				ptr += 8;   // Array length is padded to 8 bytes on 64-bit
				ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
				var pp = (IntPtr*)ptr;
				*pp = Unsafe.As<object, IntPtr>(ref obj);
			}
		}


		[RuntimeExport("RhpNewFast")]
		static unsafe object RhpNewFast(EEType* pEEType)
		{
			var size = pEEType->BaseSize;

			// Round to next power of 8
			if (size % 8 > 0)
				size = ((size / 8) + 1) * 8;

			var data = Platform.Allocate(size);
			var obj = Unsafe.As<IntPtr, object>(ref data);
			Platform.ZeroMemory(data, size);
			SetEEType(data, pEEType);

			return obj;
		}
	}
}
