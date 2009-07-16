using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ToneDetect.Business.SIP
{
	public class SipParser
	{
		#region Fields
		/// <summary>
		/// The payload from the packet
		/// </summary>
		private string payload;

		/// <summary>
		/// Set of media attributes found in the packet.
		/// </summary>
		private List<string> mediaAttributes;

		/// <summary>
		/// Set of media descriptions found in the packet...will there be only one?
		/// </summary>
		private List<string> mediaDescriptions;

		private bool localSource;

		#endregion

		#region Constructor
		public SipParser( string payload, string localAddress, string destinationAddress )
		{
			if( localAddress.Equals( destinationAddress ) )
			{
				localSource = false;
			}
			else
			{
				localSource = true;
			}
			mediaAttributes = null;
			mediaDescriptions = null;
			this.payload = payload;
			Parse();
		}
		#endregion

		#region Properties

		/// <summary>
		/// The purpose of this property is to return the fact whether or not the
		/// packet is a local source.  Meaning THIS machine running the software
		/// generated the packet.
		/// </summary>
		public bool LocalSource
		{
			get
			{
				return localSource;
			}
		}

		/// <summary>
		/// The purpose of this property is to return the fact whether or not the
		/// packet is a local destination.
		/// </summary>
		public bool LocalDestination
		{
			get
			{
				return !localSource;
			}
		}

		/// <summary>
		/// The SIP Status String
		/// </summary>
		public string SipStatusString
		{
			get;
			set;
		}

		/// <summary>
		/// The status: (200/403 etc)
		/// </summary>
		public int SipStatus
		{
			get;
			set;
		}

		/// <summary>
		/// Indicates who the message is to, so it is a way to bind
		/// channels with packets.  There can be 1 and only 1 call on a particular
		/// channel.
		/// </summary>
		public string ToField
		{
			get;
			set;
		}

		/// <summary>
		/// Get the media attributes for the packet.
		/// </summary>
		public List<string> MediaAttributes
		{
			get
			{
				return mediaAttributes;
			}
		}
		/// <summary>
		/// Get the media descriptions for the packet.
		/// </summary>
		public List<string> MediaDescriptions
		{
			get
			{
				return mediaDescriptions;
			}
		}
		#endregion

		enum ParseState
		{
			HEADER,
			BODY
		}

		#region Functions
		/// <summary>
		/// The parse method...take the payload and parse it.
		/// </summary>
		private void Parse()
		{
			string line;
			string [] lineFields;
			StringReader sr=null;
			StringBuilder sb;
			ParseState state;

			state = ParseState.HEADER;
			try
			{
				sr = new StringReader( payload );
				while( ( line = sr.ReadLine() ) != null )
				{
					if( state == ParseState.HEADER )
					{
						// if the length of the line is zero, then we are
						// done with the headers section and can be moving to the body.
						if( line.Length == 0 )
						{
							state = ParseState.BODY;
							continue;
						}

						// will typically look like: SIP/2.0 200 SOMETHING
						if( line.StartsWith( "SIP/" ) )
						{
							lineFields = line.Split( ' ' );
							if( lineFields.Length >= 3 )
							{
								SipStatus = int.Parse( lineFields [1] );
								sb = new StringBuilder();
								for( int i = 2; i < lineFields.Length; ++i )
								{
									sb.Append( lineFields [i] );
									sb.Append( " " );
								}
								SipStatusString = sb.ToString();
							}
							else
							{
								Exception e;
								e = new Exception();
								e.Data ["Custom"] = "Incorrect Number Of Fields";
								throw e;
							}
						}
						// will typically look like To: <sip:####@server>
						else if( line.StartsWith( "To:" ) )
						{
							lineFields = line.Split( ' ' );
							if( lineFields.Length >= 2 )
							{
								int start;
								int end;
								// so here, grab everything between the : and the @ to consider it the
								// phone number.
								start = lineFields [1].IndexOf( ':' ) + 1;
								end = lineFields [1].IndexOf( '@' );
								ToField = lineFields [1].Substring( start, end - start );

								// kinda works.
								//sb = new StringBuilder();
								//for( int i = 1; i < lineFields.Length; ++i )
								//{
								//	sb.Append( lineFields [i] );
								//	sb.Append( " " );
								//}
								//ToField = sb.ToString();
							}
							else
							{
								Exception e;
								e = new Exception();
								e.Data ["Custom"] = "Incorrect Number Of Fields";
								throw e;
							}
						}
					}
					else // ParseState.Body
					{
						// look for the media attribute -- simply store
						// the list of strings for the time being.
						if( line.StartsWith( "a=" ) )
						{
							if( mediaAttributes == null )
							{
								mediaAttributes = new List<string>();
							}
							mediaAttributes.Add( line );
						}
						else if( line.StartsWith( "m=" ) )
						{
							if( mediaDescriptions == null )
							{
								mediaDescriptions = new List<string>();
							}
							mediaDescriptions.Add( line );
						}
					}
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
			finally
			{
				if( sr != null )
				{
					sr.Close();
					sr = null;
				}
			}
		}
		#endregion
	}
}
