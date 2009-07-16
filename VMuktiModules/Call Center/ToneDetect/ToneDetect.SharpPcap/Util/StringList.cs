/* 
 * StringList.cs
 * 
 * THIS SOURCE CODE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
 * PURPOSE.
 * 
 * Copyright (C) 2004 Tamir Gal, tamirgal@myrealbox.com.
 */

using System;
using System.Collections;

namespace ToneDetect.SharpPcap.Util
{
	public class StringList : CollectionBase
	{
		internal String this[ int index ]  
		{
			get  
			{
				return( (String) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		public int Add( String value )  
		{
			return( List.Add( value ) );
		}

		public int IndexOf( String value )  
		{
			return( List.IndexOf( value ) );
		}

		public void Insert( int index, String value )  
		{
			List.Insert( index, value );
		}

		public void Remove( String value )  
		{
			List.Remove( value );
		}

		public bool Contains( String value )  
		{
			// If value is not of type IpAddressAndMask, this will return false.
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
			if(value!=null)
			{
				if ( value.GetType() != Type.GetType("System.String") )
					throw new ArgumentException( "value must be of type String.", "value" );
			}
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for(int i=0; i<this.Count; i++)
			{
				sb.Append( this[i].ToString() );
				if(i!=this.Count-1)
					sb.Append("\n");
			}
			return sb.ToString();
		}

	}
}
