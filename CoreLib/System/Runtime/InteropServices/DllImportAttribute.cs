﻿namespace System.Runtime.InteropServices
{
	public sealed class DllImportAttribute : Attribute
	{
		public string EntryPoint;
		public CharSet CharSet;
		public bool SetLastError;
		public bool ExactSpelling;
		public CallingConvention CallingConvention;
		public bool BestFitMapping;
		public bool PreserveSig;
		public bool ThrowOnUnmappableChar;

		public string Value { get; }

		public DllImportAttribute(string dllName)
		{
			Value = dllName;
		}
	}
}