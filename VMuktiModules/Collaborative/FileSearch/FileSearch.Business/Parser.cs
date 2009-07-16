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
	/// <summary>
	/// Summary description for Parser.
	/// </summary>
	public class Parser
	{
		public Parser()
		{
		}

		[DllImport("query.dll", CharSet = CharSet.Unicode)] 
		private extern static int LoadIFilter (string pwcsPath, ref IUnknown pUnkOuter, ref IFilter ppIUnk); 

		[ComImport, Guid("00000000-0000-0000-C000-000000000046")] 
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] 
		private interface IUnknown 
		{ 
			[PreserveSig] 
			IntPtr QueryInterface( ref Guid riid, out IntPtr pVoid ); 
 
			[PreserveSig] 
			IntPtr AddRef(); 
 
			[PreserveSig] 
			IntPtr Release(); 
		} 


		private static IFilter loadIFilter(string filename)
		{
			IUnknown iunk = null; 
			IFilter filter = null;
 
			// Try to load the corresponding IFilter 
			int resultLoad = LoadIFilter( filename, ref iunk, ref filter ); 
			if (resultLoad != (int)IFilterReturnCodes.S_OK) 
			{ 
				return null;
			} 
			return filter;
		}

/*
		private static IFilter loadIFilterOffice(string filename)
		{
			IFilter filter = (IFilter)(new CFilter());
			System.Runtime.InteropServices.UCOMIPersistFile ipf = (System.Runtime.InteropServices.UCOMIPersistFile)(filter);
			ipf.Load(filename, 0);

			return filter;
		}
*/

		public static bool IsParseable(string filename)
		{
			return loadIFilter(filename) != null;
		}

		public static string Parse(string filename)
		{
			IFilter filter = null;

			try 
			{
				StringBuilder plainTextResult = new StringBuilder();
				filter = loadIFilter(filename); 

				STAT_CHUNK ps = new STAT_CHUNK();
				IFILTER_INIT mFlags = 0;

				uint i = 0;
				filter.Init( mFlags, 0, null, ref i);

				int resultChunk = 0;

				resultChunk = filter.GetChunk(out ps);
				while (resultChunk == 0)
				{
					if (ps.flags == CHUNKSTATE.CHUNK_TEXT)
					{
						uint sizeBuffer = 60000;
						int resultText = 0;
						while (resultText == Constants.FILTER_S_LAST_TEXT || resultText == 0)
						{
							sizeBuffer = 60000;
							System.Text.StringBuilder sbBuffer = new System.Text.StringBuilder((int)sizeBuffer);
							resultText = filter.GetText(ref sizeBuffer, sbBuffer);

							if (sizeBuffer > 0 && sbBuffer.Length > 0)
							{
								string chunk = sbBuffer.ToString(0, (int)sizeBuffer);
								plainTextResult.Append(chunk);
							}
						}
					}
					resultChunk = filter.GetChunk(out ps);
				}
				return plainTextResult.ToString();
			}
			finally
			{
				if (filter != null)
					Marshal.ReleaseComObject(filter);
			}
		}
	}
}
