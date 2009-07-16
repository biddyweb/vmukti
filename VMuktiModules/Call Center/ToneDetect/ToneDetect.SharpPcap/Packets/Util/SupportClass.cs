using System;

namespace ToneDetect.SharpPcap.Packets.Util
{
	public class SupportClass
	{
//		/// <summary>
//		/// Converts an array of bytes to an array of bytes
//		/// </summary>
//		/// <param name="byteArray">The array of bytes to be converted</param>
//		/// <returns>The new array of bytes</returns>
//		public static byte[] ToByteArray(byte[] byteArray)
//		{
//			byte[] byteArray = new byte[byteArray.Length];
//			for(int index=0; index < byteArray.Length; index++)
//				byteArray[index] = (byte) byteArray[index];
//			return byteArray;
//		}
//
//		/// <summary>
//		/// Converts a string to an array of bytes
//		/// </summary>
//		/// <param name="sourceString">The string to be converted</param>
//		/// <returns>The new array of bytes</returns>
//		public static byte[] ToByteArray(string sourceString)
//		{
//			byte[] byteArray = new byte[sourceString.Length];
//			for (int index=0; index < sourceString.Length; index++)
//				byteArray[index] = (byte) sourceString[index];
//			return byteArray;
//		}

		/*******************************/
		public static long FileLength(System.IO.FileInfo file)
		{
			if (System.IO.Directory.Exists(file.FullName))
				return 0;
			else 
				return file.Length;
		}

		/*******************************/
		public class Tokenizer
		{
			private System.Collections.ArrayList elements;
			private string source;
			//The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character
			private string delimiters = " \t\n\r";		

			public Tokenizer(string source)
			{			
				this.elements = new System.Collections.ArrayList();
				this.elements.AddRange(source.Split(this.delimiters.ToCharArray()));
				this.RemoveEmptyStrings();
				this.source = source;
			}

			public Tokenizer(string source, string delimiters)
			{
				this.elements = new System.Collections.ArrayList();
				this.delimiters = delimiters;
				this.elements.AddRange(source.Split(this.delimiters.ToCharArray()));
				this.RemoveEmptyStrings();
				this.source = source;
			}

			public int Count
			{
				get
				{
					return (this.elements.Count);
				}
			}

			public bool HasMoreTokens()
			{
				return (this.elements.Count > 0);			
			}

			public string NextToken()
			{			
				string result;
				if (source == "") throw new System.Exception();
				else
				{
					this.elements = new System.Collections.ArrayList();
					this.elements.AddRange(this.source.Split(delimiters.ToCharArray()));
					RemoveEmptyStrings();		
					result = (string) this.elements[0];
					this.elements.RemoveAt(0);				
					this.source = this.source.Remove(this.source.IndexOf(result),result.Length);
					this.source = this.source.TrimStart(this.delimiters.ToCharArray());
					return result;					
				}			
			}

			public string NextToken(string delimiters)
			{
				this.delimiters = delimiters;
				return NextToken();
			}

			private void RemoveEmptyStrings()
			{
				//VJ++ does not treat empty strings as tokens
				for (int index=0; index < this.elements.Count; index++)
					if ((string)this.elements[index]== "")
					{
						this.elements.RemoveAt(index);
						index--;
					}
			}
		}

	}
}
