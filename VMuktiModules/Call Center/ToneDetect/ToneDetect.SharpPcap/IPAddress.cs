/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.Collections;

namespace ToneDetect.SharpPcap
{
	/// <summary>
	///Represent an IP Address and a Subnet Mask.
	///This struct hold two parameters:
	///address -  string of the IP Address
	///mask - a string of the subnet mask
	/// </summary>
	public struct IPAddress
	{
		private string _address, _mask;
		internal IPAddress(string address, string mask)
		{
			this._address = address;
			this._mask = mask;
		}

		/// <summary>
		/// IP Address
		/// </summary>
		public string Address
		{
			get
			{
				return _address;
			}
		}

		/// <summary>
		/// Subnet Mask
		/// </summary>
		public string Mask
		{
			get
			{
				return _mask;
			}
		}

		public override string ToString()
		{
			return (Address+"/"+Util.Convert.MaskStringToBits(Mask));
		}

	}

	/// <summary>
	/// Summary description for IPAddressList.
	/// </summary>
	public class IPAddressList : CollectionBase
	{
		public IPAddress this[ int index ]  
		{
			get  
			{
				return( (IPAddress) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( IPAddress value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( IPAddress value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, IPAddress value )  
		{
			List.Insert( index, value );
		}

		public void Remove( IPAddress value )  
		{
			List.Remove( value );
		}

		public bool Contains( IPAddress value )  
		{
			// If value is not of type IPAddress, this will return false.
			return( List.Contains( value ) );
		}

		public bool ContainsIp( string Ip )
		{
			foreach (IPAddress item in this)
			{
				if (item.Address == Ip)
					return true;
			}
			return false;
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
			if ( value.GetType() != Type.GetType("IPAddress") )
				throw new ArgumentException( "value must be of type IPAddress.", "value" );
		}
	}
}
