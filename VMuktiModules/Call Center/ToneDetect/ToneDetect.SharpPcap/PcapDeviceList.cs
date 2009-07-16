using System;
using System.Collections;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Summary description for PcapDeviceList.
	/// </summary>
	public class PcapDeviceList : CollectionBase
	{
		public PcapDevice this[ int index ]  
		{
			get  
			{
				return( (PcapDevice) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( PcapDevice value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( PcapDevice value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, PcapDevice value )  
		{
			List.Insert( index, value );
		}

		public void Remove( PcapDevice value )  
		{
			List.Remove( value );
		}

		public bool Contains( PcapDevice value )  
		{
			// If value is not of type PcapDevice, this will return false.
			return( List.Contains( value ) );
		}

		//		protected override void OnInsert( int index, Object value )  
		//		{
		//			// Insert additional code to be run only when inserting values.
		//		}
		//
		//		protected override void OnRemove( int index, Object value )  
		//		{
		//			// Insert additional code to be run only when removing values.
		//		}
		//
		//		protected override void OnSet( int index, Object oldValue, Object newValue )  
		//		{
		//			// Insert additional code to be run only when setting values.
		//		}

		protected override void OnValidate( Object value )  
		{
			if ( !(value is PcapDevice) )
				throw new ArgumentException( "value must be of type PcapDevice.", "value" );
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for(int i=0; i<this.Count; i++)
			{
				sb.Append("Device "+i+": ");
				sb.Append( this[i].ToString() );
				if(i!=Count-1)
					sb.Append("\n\n\n");
			}
			return sb.ToString();
		}

	}
}
