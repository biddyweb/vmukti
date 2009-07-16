// $Id: FileUtility.java,v 1.1 2004/09/28 17:31:38 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2004, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Util
{
	using System;
	/// <summary> Writes data in tcpdump format
	/// *
	/// </summary>
	/// <author>  Joyce Lin
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/09/28 17:31:38 $
	/// *
	/// 
	/// </version>
	public class FileUtility
	{
		public static System.String readFile(System.String filename)
		{
			System.String readString = "";
			System.String tmp;
			
			System.IO.FileInfo f = new System.IO.FileInfo(filename);
			char[] readIn = new char[(int) (SupportClass.FileLength(f))];
			
			//UPGRADE_TODO: Expected value of parameters of constructor 'java.io.BufferedReader.BufferedReader' are different in the equivalent in .NET. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1092"'
			System.IO.StreamReader in_Renamed = new System.IO.StreamReader(new System.IO.StreamReader(f.FullName).BaseStream, System.Text.Encoding.UTF7);
			
			//UPGRADE_TODO: Method 'java.io.Reader.read' was converted to 'System.IO.StreamReader.Read' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073_javaioReaderread_char[]"'
			in_Renamed.Read((System.Char[]) readIn, 0, readIn.Length);
			readString = new System.String(readIn);
			
			in_Renamed.Close();
			
			return readString;
		}
		
		public static void  writeFile(System.String str, System.String filename, bool append)
		{
			
			int length = str.Length;
			System.IO.StreamWriter out_Renamed = new System.IO.StreamWriter(filename, append, System.Text.Encoding.Default);
			out_Renamed.Write(str, 0, length);
			out_Renamed.Close();
		}
		
		public static void  writeFile(byte[] bytes, System.String filename, bool append)
		{
			
			//UPGRADE_ISSUE: Constructor 'java.io.FileOutputStream.FileOutputStream' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaioFileOutputStreamFileOutputStream_javalangString_boolean"'
			System.IO.FileStream out_Renamed = new System.IO.FileStream(filename, System.IO.FileMode.Append);
			out_Renamed.Write((bytes), 0, bytes.Length);
			out_Renamed.Close();
		}
		
		public static void  writeFile(byte[][] bytes, System.String filename, bool append)
		{
			
			writeFile(bytes[0], filename, append);
			for (int i = 1; i < bytes.Length; i++)
				writeFile(bytes[i], filename, true);
		}
		
		public static void  writeFile(byte[][] bytes, int beginIndex, int endIndex, System.String filename, bool append)
		{
			writeFile(bytes[beginIndex], filename, append);
			for (int i = beginIndex + 1; i <= endIndex; i++)
				writeFile(bytes[i], filename, true);
		}
		
		
		internal const System.String _rcsid = "$Id: FileUtility.java,v 1.1 2004/09/28 17:31:38 pcharles Exp $";
	}
}