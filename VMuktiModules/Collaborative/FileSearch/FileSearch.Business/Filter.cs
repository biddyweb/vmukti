/* VMukti 1.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FileSearch.Business
{
	[Flags]
	public enum IFILTER_INIT : uint
	{
		NONE                   = 0,
		CANON_PARAGRAPHS       = 1,
		HARD_LINE_BREAKS       = 2,
		CANON_HYPHENS          = 4,
		CANON_SPACES           = 8,
		APPLY_INDEX_ATTRIBUTES = 16,
		APPLY_CRAWL_ATTRIBUTES = 256,
		APPLY_OTHER_ATTRIBUTES = 32,
		INDEXING_ONLY          = 64,
		SEARCH_LINKS           = 128,       
		FILTER_OWNED_VALUE_OK  = 512
	}

	public enum CHUNK_BREAKTYPE
	{
		CHUNK_NO_BREAK = 0,
		CHUNK_EOW      = 1,
		CHUNK_EOS      = 2,
		CHUNK_EOP      = 3,
		CHUNK_EOC      = 4
	}

	[Flags]
	public enum CHUNKSTATE
	{
		CHUNK_TEXT               = 0x1,
		CHUNK_VALUE              = 0x2,
		CHUNK_FILTER_OWNED_VALUE = 0x4
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PROPSPEC
	{
		public uint ulKind;
		public uint propid;
		public IntPtr lpwstr;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FULLPROPSPEC
	{
		public Guid guidPropSet;
		public PROPSPEC psProperty;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct STAT_CHUNK
	{
		public uint  idChunk;
		[MarshalAs(UnmanagedType.U4)]
		public CHUNK_BREAKTYPE breakType;
		[MarshalAs(UnmanagedType.U4)]
		public CHUNKSTATE flags;
		public uint locale;
		[MarshalAs(UnmanagedType.Struct)] public FULLPROPSPEC attribute;
		public uint idChunkSource;
		public uint cwcStartSource;
		public uint cwcLenSource;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FILTERREGION
	{
		public uint idChunk;
		public uint cwcStart;
		public uint cwcExtent;
	}

	[ComImport]
	[Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilter
	{
		void Init([MarshalAs(UnmanagedType.U4)] IFILTER_INIT grfFlags, uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] FULLPROPSPEC[] aAttributes, ref uint pdwFlags);
		[PreserveSig] int GetChunk(out STAT_CHUNK pStat);
		[PreserveSig] int GetText(ref uint pcwcBuffer, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer);
		void GetValue(ref UIntPtr ppPropValue);
		void BindRegion([MarshalAs(UnmanagedType.Struct)]FILTERREGION origPos, ref Guid riid, ref UIntPtr ppunk);
	}

	[ComImport]
	[Guid("f07f3920-7b8c-11cf-9be8-00aa004b9986")]
	public class CFilter
	{
	}

	public class Constants
	{
		public const uint PID_STG_DIRECTORY               =0x00000002;
		public const uint PID_STG_CLASSID                 =0x00000003;
		public const uint PID_STG_STORAGETYPE             =0x00000004;
		public const uint PID_STG_VOLUME_ID               =0x00000005;
		public const uint PID_STG_PARENT_WORKID           =0x00000006;
		public const uint PID_STG_SECONDARYSTORE          =0x00000007;
		public const uint PID_STG_FILEINDEX               =0x00000008;
		public const uint PID_STG_LASTCHANGEUSN           =0x00000009;
		public const uint PID_STG_NAME                    =0x0000000a;
		public const uint PID_STG_PATH                    =0x0000000b;
		public const uint PID_STG_SIZE                    =0x0000000c;
		public const uint PID_STG_ATTRIBUTES              =0x0000000d;
		public const uint PID_STG_WRITETIME               =0x0000000e;
		public const uint PID_STG_CREATETIME              =0x0000000f;
		public const uint PID_STG_ACCESSTIME              =0x00000010;
		public const uint PID_STG_CHANGETIME              =0x00000011;
		public const uint PID_STG_CONTENTS                =0x00000013;
		public const uint PID_STG_SHORTNAME               =0x00000014;
		public const int  FILTER_E_END_OF_CHUNKS          =(unchecked((int)0x80041700));
		public const int  FILTER_E_NO_MORE_TEXT           =(unchecked((int)0x80041701));
		public const int  FILTER_E_NO_MORE_VALUES         =(unchecked((int)0x80041702));
		public const int  FILTER_E_NO_TEXT                =(unchecked((int)0x80041705));
		public const int  FILTER_E_NO_VALUES              =(unchecked((int)0x80041706));
		public const int  FILTER_S_LAST_TEXT              =(unchecked((int)0x00041709));
	}

	/// <summary> 
	/// IFilter return codes 
	/// </summary> 
	public enum IFilterReturnCodes : uint 
	{ 
		/// <summary> 
		/// Success 
		/// </summary> 
		S_OK = 0, 
		/// <summary> 
		/// The function was denied access to the filter file.  
		/// </summary> 
		E_ACCESSDENIED = 0x80070005, 
		/// <summary> 
		/// The function encountered an invalid handle, probably due to a low-memory situation.  
		/// </summary> 
		E_HANDLE = 0x80070006, 
		/// <summary> 
		/// The function received an invalid parameter. 
		/// </summary> 
		E_INVALIDARG = 0x80070057, 
		/// <summary> 
		/// Out of memory 
		/// </summary> 
		E_OUTOFMEMORY = 0x8007000E, 
		/// <summary> 
		/// Not implemented 
		/// </summary> 
		E_NOTIMPL = 0x80004001, 
		/// <summary> 
		/// Unknown error 
		/// </summary> 
		E_FAIL = 0x80000008, 
		/// <summary> 
		/// File not filtered due to password protection 
		/// </summary> 
		FILTER_E_PASSWORD = 0x8004170B, 
		/// <summary> 
		/// The document format is not recognised by the filter 
		/// </summary> 
		FILTER_E_UNKNOWNFORMAT = 0x8004170C, 
		/// <summary> 
		/// No text in current chunk 
		/// </summary> 
		FILTER_E_NO_TEXT = 0x80041705, 
		/// <summary> 
		/// No more chunks of text available in object 
		/// </summary> 
		FILTER_E_END_OF_CHUNKS = 0x80041700, 
		/// <summary> 
		/// No more text available in chunk 
		/// </summary> 
		FILTER_E_NO_MORE_TEXT = 0x80041701, 
		/// <summary> 
		/// No more property values available in chunk 
		/// </summary> 
		FILTER_E_NO_MORE_VALUES = 0x80041702, 
		/// <summary> 
		/// Unable to access object 
		/// </summary> 
		FILTER_E_ACCESS = 0x80041703, 
		/// <summary> 
		/// Moniker doesn't cover entire region 
		/// </summary> 
		FILTER_W_MONIKER_CLIPPED = 0x00041704, 
		/// <summary> 
		/// Unable to bind IFilter for embedded object 
		/// </summary> 
		FILTER_E_EMBEDDING_UNAVAILABLE = 0x80041707, 
		/// <summary> 
		/// Unable to bind IFilter for linked object 
		/// </summary> 
		FILTER_E_LINK_UNAVAILABLE = 0x80041708, 
		/// <summary> 
		/// This is the last text in the current chunk 
		/// </summary> 
		FILTER_S_LAST_TEXT = 0x00041709, 
		/// <summary> 
		/// This is the last value in the current chunk 
		/// </summary> 
		FILTER_S_LAST_VALUES = 0x0004170A 
	} 
}
