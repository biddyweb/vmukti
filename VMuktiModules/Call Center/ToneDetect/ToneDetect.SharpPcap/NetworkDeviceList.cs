using System;
using System.Collections;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	/// Summary description for NetworkDeviceList.
	/// </summary>
	public class NetworkDeviceList : CollectionBase
	{
		public NetworkDevice this[ int index ]  
		{
			get  
			{
				return( (NetworkDevice) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( NetworkDevice value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( NetworkDevice value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, NetworkDevice value )  
		{
			List.Insert( index, value );
		}

		public void Remove( NetworkDevice value )  
		{
			List.Remove( value );
		}

		public bool Contains( NetworkDevice value )  
		{
			// If value is not of type NetworkDevice, this will return false.
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
			if ( value.GetType() != Type.GetType("NetworkDevice") )
				throw new ArgumentException( "value must be of type NetworkDevice.", "value" );
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for(int i=0; i<this.Count; i++)
			{
				sb.Append("Device "+i+": ");
				sb.Append( this[i].ToStringDetailed() );
				if(i!=Count-1)
					sb.Append("\n\n\n");
			}
			return sb.ToString();
		}

	}
}
