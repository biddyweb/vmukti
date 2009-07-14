namespace ToneDetect.Business.FaxInfo
{
	/// <summary>
	/// The purpose of this enumeration is to provide specifications for the
	/// fax frequencies to search for.
	/// </summary>
	internal class FaxSpecifiecations
	{
		/* The information in this class was taken frmo dsp.c in asterisk. */

		/* The CNG signal is made up of the transmission of 1100Hz for .5 second,
		 * followed by a 3 second silent period).  It is sent by the calling fax.
		 */
		internal static int CNG_Freq = 1100; // Hz
		internal static int CNG_Dur = 500; // Ms
		internal static int CNG_DB = 16; // decibals

		/* This signal may be sent by the Terminating FAX machine anywhere
		 * between 1.8 to 2.5 seonds after answering the call.  The CED signal is
		 * made up of a 2100 Hz tone that is from 2.6 to 4 seconds in duration.
		 * It is sent by the terminating fax.
		 */
		internal static int CED_Freq = 2100; // Hz
		internal static int CED_Dur = 2600; // Ms
		internal static int CED_DB = 16; // decibals

		// Through analysis of signals recorded, I was able
		// to find signals coming from the terminatnig fax that did not
		// match the other two specifications.  I don't know exactly what it
		// is yet, but it seems to light around 1700 hz.
		internal static int OTH_Freq = 1700; // hz;
		internal static int OTH_Dur = 1800; // Ms
		internal static int OTH_DB = 16; // decibals
	}
}
