/*
 * AMS.Profile Class Library
 * 
 * Written by Alvaro Mendez
 * Copyright (c) 2005. All Rights Reserved.
 * 
 * The AMS.Profile namespace contains interfaces and classes that 
 * allow reading and writing of user-profile data.
 * This file contains the event-related types.
 * 
 * Last Updated: Feb. 15, 2005
 */


using System;
                       
namespace VMukti.Profile
{	
	public enum ProfileChangeType
	{
		Name,
		ReadOnly,
		SetValue,
		RemoveEntry,
		RemoveSection,
		Other
	}
	
	public class ProfileChangedArgs : EventArgs
	{   
		// Fields
		private readonly ProfileChangeType m_changeType;
		private readonly string m_section;
		private readonly string m_entry;
		private readonly object m_value;

		public ProfileChangedArgs(ProfileChangeType changeType, string section, string entry, object value) 
		{
			m_changeType = changeType;
			m_section = section;
			m_entry = entry;
			m_value = value;
		}
		
		/// <summary>
		///   Gets the type of change that raised the event. </summary>
		public ProfileChangeType ChangeType
		{
			get 
			{
				return m_changeType;
			}
		}
		
		/// <summary>
		///   Gets the name of the section involved in the change, or null if not applicable. </summary>
		public string Section
		{
			get 
			{
				return m_section;
			}
		}
		
		public string Entry
		{
			get 
			{
				return m_entry;
			}
		}
		
		/// <summary>
		///   Gets the new value for the entry or method/property, based on the value of <see cref="ChangeType" />. </summary>
		public object Value
		{
			get 
			{
				return m_value;
			}
		}
	}

	public class ProfileChangingArgs : ProfileChangedArgs
	{   
		private bool m_cancel;
		
		public ProfileChangingArgs(ProfileChangeType changeType, string section, string entry, object value) :
			base(changeType, section, entry, value)
		{
		}
		                    
		public bool Cancel
		{
			get 
			{
				return m_cancel;
			}
			set
			{
				m_cancel = value;
			}
		}
	}
   
	public delegate void ProfileChangingHandler(object sender, ProfileChangingArgs e);

	public delegate void ProfileChangedHandler(object sender, ProfileChangedArgs e);
}

