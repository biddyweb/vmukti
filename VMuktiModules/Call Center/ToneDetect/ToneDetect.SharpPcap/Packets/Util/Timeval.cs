// $Id: Timeval.java,v 1.3 2004/09/28 17:31:38 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Util
{
	using System;
	/// <summary> POSIX.4 timeval for Java.
	/// <p>
	/// Container for java equivalent of c's struct timeval.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.3 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2004/09/28 17:31:38 $
	/// 
	/// </version>
	[Serializable]
	public class Timeval/* : System.Runtime.Serialization.ISerializable*/
	{
		/// <summary> Convert this timeval to a java Date.
		/// </summary>
		virtual public System.DateTime Date
		{
			get
			{
//				//UPGRADE_WARNING: Constructor 'java.util.Date' was converted to 'System.DateTime' which may throw an exception. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1101"'
//				return new System.DateTime(seconds * 1000 + microseconds / 1000 * 10000 + 621355968000000000);
				DateTime timeval = new DateTime(1970,1,1); 
				timeval = timeval.AddSeconds(Seconds); 
				timeval = timeval.AddMilliseconds(MicroSeconds / 1000); 
				return timeval.ToLocalTime();
			}			
		}
		virtual public long Seconds
		{
			get
			{
				return seconds;
			}
			
		}
		virtual public int MicroSeconds
		{
			get
			{
				return microseconds;
			}
			
		}
		public Timeval(long seconds, int microseconds)
		{
			this.seconds = seconds;
			this.microseconds = microseconds;
		}
		
		public override System.String ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(seconds);
			sb.Append('.');
			sb.Append(microseconds);
			sb.Append('s');
			
			return sb.ToString();
		}		
		
		internal long seconds;
		internal int microseconds;
		
		private System.String _rcsid = "$Id: Timeval.java,v 1.3 2004/09/28 17:31:38 pcharles Exp $";
	}
}