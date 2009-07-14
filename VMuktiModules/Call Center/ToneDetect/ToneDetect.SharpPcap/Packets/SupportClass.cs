using System;

namespace ToneDetect.SharpPcap.Packets
{
	public class SupportClass
	{
//		/// <summary>
//		/// This method is used as a dummy method to simulate VJ++ behavior
//		/// </summary>
//		/// <param name="literal">The literal to return</param>
//		/// <returns>The received value</returns>
//		public static long Identity(long literal)
//		{
//			return literal;
//		}
//
//		/// <summary>
//		/// This method is used as a dummy method to simulate VJ++ behavior
//		/// </summary>
//		/// <param name="literal">The literal to return</param>
//		/// <returns>The received value</returns>
//		public static ulong Identity(ulong literal)
//		{
//			return literal;
//		}
//
//		/// <summary>
//		/// This method is used as a dummy method to simulate VJ++ behavior
//		/// </summary>
//		/// <param name="literal">The literal to return</param>
//		/// <returns>The received value</returns>
//		public static float Identity(float literal)
//		{
//			return literal;
//		}
//
//		/// <summary>
//		/// This method is used as a dummy method to simulate VJ++ behavior
//		/// </summary>
//		/// <param name="literal">The literal to return</param>
//		/// <returns>The received value</returns>
//		public static double Identity(double literal)
//		{
//			return literal;
//		}
//
//		/*******************************/
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
		static public System.Random Random = new System.Random();

		public static long GetConst(long val)
		{
			return val;
		}

	}
}
