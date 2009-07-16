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
using System.Windows.Forms;
using Microsoft.Win32;

namespace VMukti.Profile
{
	public class Registry : Profile
	{
		// Fields
		private RegistryKey m_rootKey = Microsoft.Win32.Registry.CurrentUser;

		/// <summary>
		///   Initializes a new instance of the Registry class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
		public Registry()
		{
		}

		public Registry(RegistryKey rootKey, string subKeyName) :
			base("")
		{
			if (rootKey != null)
				m_rootKey = rootKey;
			if (subKeyName != null)
				Name = subKeyName;
		}

		/// <summary>
		///   Initializes a new instance of the Registry class based on another Registry object. </summary>
		/// <param name="reg">
		///   The Registry object whose properties and events are used to initialize the object being constructed. </param>
		public Registry(Registry reg) :
			base(reg)
		{
			m_rootKey = reg.m_rootKey;
		}

		/// <summary>
		///   Gets the default name sub-key registry path. </summary>
		/// <exception cref="InvalidOperationException">
		///   Application.CompanyName or Application.ProductName are empty.</exception>
		/// <remarks>
		///   This is set to "Software\\" + Application.CompanyName + "\\" + Application.ProductName. </remarks>
		public override string DefaultName
		{
			get
			{
				if (Application.CompanyName == "" || Application.ProductName == "")
					throw new InvalidOperationException("Application.CompanyName and/or Application.ProductName are empty and they're needed for the DefaultName.");
				
				return "Software\\" + Application.CompanyName + "\\" + Application.ProductName;			
			}
		}

		/// <summary>
		///   Retrieves a copy of itself. </summary>
		/// <returns>
		///   The return value is a copy of itself as an object. </returns>
		/// <seealso cref="Profile.CloneReadOnly" />
		public override object Clone()
		{
			return new Registry(this);
		}

		public RegistryKey RootKey
		{
			get 
			{ 
				return m_rootKey; 
			}
			set 
			{ 
				VerifyNotReadOnly();
				if (m_rootKey == value)
					return;
				
				if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "RootKey", value))
					return;
				
				m_rootKey = value; 
				RaiseChangeEvent(false, ProfileChangeType.Other, null, "RootKey", value);
			}
		}

		protected RegistryKey GetSubKey(string section, bool create, bool writable)		
		{
			VerifyName();
			
			string keyName = Name + "\\" + section;

			if (create)
				return m_rootKey.CreateSubKey(keyName);
			return m_rootKey.OpenSubKey(keyName, writable);
		}

		public override void SetValue(string section, string entry, object value)
		{
			// If the value is null, remove the entry
			if (value == null)
			{
				RemoveEntry(section, entry);
				return;
			}
			
			VerifyNotReadOnly();
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);
			
			if (!RaiseChangeEvent(true, ProfileChangeType.SetValue, section, entry, value))
				return;
			
			using (RegistryKey subKey = GetSubKey(section, true, true))
				subKey.SetValue(entry, value);
			
			RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
		}

		
		public override object GetValue(string section, string entry)
		{
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);

			using (RegistryKey subKey = GetSubKey(section, false, false))
				return (subKey == null ? null : subKey.GetValue(entry));
		}

		
		public override void RemoveEntry(string section, string entry)
		{
			VerifyNotReadOnly();
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);
			
			using (RegistryKey subKey = GetSubKey(section, false, true))
			{
				if (subKey != null && subKey.GetValue(entry) != null)
				{
					if (!RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, null))
						return;
			
					subKey.DeleteValue(entry, false);
					RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, null);
				}
			}	
		}

		
		public override void RemoveSection(string section)
		{
			VerifyNotReadOnly();
			VerifyName();
			VerifyAndAdjustSection(ref section);
			
			using (RegistryKey key = m_rootKey.OpenSubKey(Name, true))
			{
				if (key != null && HasSection(section))
				{
					if (!RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, null, null))
						return;
					
					key.DeleteSubKeyTree(section);
					RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, null, null);
				}
			}	
		}
		
		
		public override string[] GetEntryNames(string section)
		{
			VerifyAndAdjustSection(ref section);

			using (RegistryKey subKey = GetSubKey(section, false, false))
			{
				if (subKey == null)
					return null;
				
				return subKey.GetValueNames();
			}				
		}		

		
		public override string[] GetSectionNames()
		{
			VerifyName();
			
			using (RegistryKey key = m_rootKey.OpenSubKey(Name))
			{
				if (key == null)
					return null;				
				return key.GetSubKeyNames();
			}				
		}		
	}
}
