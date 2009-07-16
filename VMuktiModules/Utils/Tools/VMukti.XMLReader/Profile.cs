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
using System.Data;

namespace VMukti.Profile
{		
	
	public abstract class Profile : IProfile
	{
		// Fields
		private string m_name;
		private bool m_readOnly;
		
		
		public event ProfileChangingHandler Changing;

		public event ProfileChangedHandler Changed;				
		
		protected Profile()
		{			
			m_name = DefaultName;
		}
		
		protected Profile(string name)
		{			
			m_name = name;
		}
		
		protected Profile(Profile profile)
		{			
			m_name = profile.m_name;
			m_readOnly = profile.m_readOnly;			
			Changing = profile.Changing;
			Changed = profile.Changed;
		}
		
		
		public string Name
		{
			get 
			{ 
				return m_name; 
			}
			set 
			{ 
				VerifyNotReadOnly();	
				if (m_name == value.Trim())
					return;
					
				if (!RaiseChangeEvent(true, ProfileChangeType.Name, null, null, value))
					return;
							
				m_name = value.Trim();
				RaiseChangeEvent(false, ProfileChangeType.Name, null, null, value);
			}
		}

		
		public bool ReadOnly
		{
			get 
			{ 
				return m_readOnly; 
			}
			
			set
			{ 
				VerifyNotReadOnly();
				if (m_readOnly == value)
					return;
				
				if (!RaiseChangeEvent(true, ProfileChangeType.ReadOnly, null, null, value))
					return;
							
				m_readOnly = value;
				RaiseChangeEvent(false, ProfileChangeType.ReadOnly, null, null, value);
			}
		}

		
		public abstract string DefaultName
		{
			get;
		}

		
		public abstract object Clone();

		
		public abstract void SetValue(string section, string entry, object value);
		
		
		public abstract object GetValue(string section, string entry);

		
		public virtual string GetValue(string section, string entry, string defaultValue)
		{
			object value = GetValue(section, entry);
			return (value == null ? defaultValue : value.ToString());
		}

		
		public virtual int GetValue(string section, string entry, int defaultValue)
		{
			object value = GetValue(section, entry);
			if (value == null)
				return defaultValue;

			try
			{
				return Convert.ToInt32(value);
			}
			catch 
			{
				return 0;
			}
		}

		
		public virtual double GetValue(string section, string entry, double defaultValue)
		{
			object value = GetValue(section, entry);
			if (value == null)
				return defaultValue;

			try
			{
				return Convert.ToDouble(value);
			}
			catch 
			{
				return 0;
			}
		}

		
		public virtual bool GetValue(string section, string entry, bool defaultValue)
		{
			object value = GetValue(section, entry);
			if (value == null)
				return defaultValue;			

			try
			{
				return Convert.ToBoolean(value);
			}
			catch 
			{
				return false;
			}
		}

		
		public virtual bool HasEntry(string section, string entry)
		{
			string[] entries = GetEntryNames(section);
			
			if (entries == null)
				return false;

			VerifyAndAdjustEntry(ref entry);
			return Array.IndexOf(entries, entry) >= 0;
		}

		
		public virtual bool HasSection(string section)
		{
			string[] sections = GetSectionNames();

			if (sections == null)
				return false;

			VerifyAndAdjustSection(ref section);
			return Array.IndexOf(sections, section) >= 0;
		}

		
		public abstract void RemoveEntry(string section, string entry);

		
		public abstract void RemoveSection(string section);
		
		
		public abstract string[] GetEntryNames(string section);

		
		public abstract string[] GetSectionNames();
		
		
		public virtual IReadOnlyProfile CloneReadOnly()
		{
			Profile profile = (Profile)Clone();
			profile.m_readOnly = true;
			
			return profile;
		}

		
		public virtual DataSet GetDataSet()
		{
			VerifyName();
			
			string[] sections = GetSectionNames();
			if (sections == null)
				return null;
			
			DataSet ds = new DataSet(Name);
			
			// Add a table for each section
			foreach (string section in sections)
			{
				DataTable table = ds.Tables.Add(section);
				
				// Retrieve the column names and values
				string[] entries = GetEntryNames(section);
				DataColumn[] columns = new DataColumn[entries.Length];
				object[] values = new object[entries.Length];								

				int i = 0;
				foreach (string entry in entries)
				{
					object value = GetValue(section, entry);
				
					columns[i] = new DataColumn(entry, value.GetType());
					values[i++] = value;
				}
												
				// Add the columns and values to the table
				table.Columns.AddRange(columns);
				table.Rows.Add(values);								
			}
			
			return ds;
		}
		
		
		public virtual void SetDataSet(DataSet ds)
		{
			if (ds == null)
				throw new ArgumentNullException("ds");
			
			// Create a section for each table
			foreach (DataTable table in ds.Tables)
			{
				string section = table.TableName;
				DataRowCollection rows = table.Rows;				
				if (rows.Count == 0)
					continue;

				// Loop through each column and add it as entry with value of the first row				
				foreach (DataColumn column in table.Columns)
				{
					string entry = column.ColumnName;
					object value = rows[0][column];
					
					SetValue(section, entry, value);
				}
			}
		}

		
		protected string DefaultNameWithoutExtension
		{
			get
			{
				try
				{
					string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
					return file.Substring(0, file.LastIndexOf('.'));
				}
				catch
				{
					return "profile";  // if all else fails
				}
			}
		}

		
		protected virtual void VerifyAndAdjustSection(ref string section)
		{
			if (section == null)
				throw new ArgumentNullException("section");			
			
			section = section.Trim();
		}

		
		protected virtual void VerifyAndAdjustEntry(ref string entry)
		{
			if (entry == null)
				throw new ArgumentNullException("entry");			

			entry = entry.Trim();
		}
		
		
		protected internal virtual void VerifyName()
		{
			if (m_name == null || m_name == "")
				throw new InvalidOperationException("Operation not allowed because Name property is null or empty.");
		}

		
		protected internal virtual void VerifyNotReadOnly()
		{
			if (m_readOnly)
				throw new InvalidOperationException("Operation not allowed because ReadOnly property is true.");			
		}
		
		
		protected bool RaiseChangeEvent(bool changing, ProfileChangeType changeType, string section, string entry, object value)
		{
			if (changing)
			{
				// Don't even bother if there are no handlers.
				if (Changing == null)
					return true;

				ProfileChangingArgs e = new ProfileChangingArgs(changeType, section, entry, value);
				OnChanging(e);
				return !e.Cancel;
			}
			
			// Don't even bother if there are no handlers.
			if (Changed != null)
				OnChanged(new ProfileChangedArgs(changeType, section, entry, value));
			return true;
		}
		                          
		
		protected virtual void OnChanging(ProfileChangingArgs e)
		{
			if (Changing == null)
				return;

			foreach (ProfileChangingHandler handler in Changing.GetInvocationList())
			{
				handler(this, e);
				
				// If a particular handler cancels the event, stop
				if (e.Cancel)
					break;
			}
		}

		
		protected virtual void OnChanged(ProfileChangedArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}
		
		
		public virtual void Test(bool cleanup)
		{
			string task = ""; 
			try
			{
				string section = "Profile Test";
				
				task = "initializing the profile -- cleaning up the '" + section + "' section";
				
					RemoveSection(section);
				
				task = "getting the sections and their count";
				
					string[] sections = GetSectionNames();
					int sectionCount = (sections == null ? 0 : sections.Length);
					bool haveSections = sectionCount > 1;
				
				task = "adding some valid entries to the '" + section + "' section";
				
					SetValue(section, "Text entry", "123 abc"); 
					SetValue(section, "Blank entry", ""); 
					SetValue(section, "Null entry", null);  // nothing will be added
					SetValue(section, "  Entry with leading and trailing spaces  ", "The spaces should be trimmed from the entry"); 
					SetValue(section, "Integer entry", 2 * 8 + 1); 
					SetValue(section, "Long entry", 1234567890123456789); 
					SetValue(section, "Double entry", 2 * 8 + 1.95); 
					SetValue(section, "DateTime entry", DateTime.Today); 
					SetValue(section, "Boolean entry", haveSections); 
				
				task = "adding a null entry to the '" + section + "' section";

					try
					{
						SetValue(section, null, "123 abc"); 
						throw new Exception("Passing a null entry was allowed for SetValue");
					}
					catch (ArgumentNullException)
					{						
					}
						
				task = "retrieving a null section";

					try
					{
						GetValue(null, "Test"); 
						throw new Exception("Passing a null section was allowed for GetValue");
					}
					catch (ArgumentNullException)
					{						
					}

				task = "getting the number of entries and their count";
				
					int expectedEntries = 8;
					string[] entries = GetEntryNames(section);

				task = "verifying the number of entries is " + expectedEntries;
				
					if (entries.Length != expectedEntries)
						throw new Exception("Incorrect number of entries found: " + entries.Length);

				task = "checking the values for the entries added";
								
					string strValue = GetValue(section, "Text entry", "");
					if (strValue != "123 abc")
						throw new Exception("Incorrect string value found for the Text entry: '" + strValue + "'");
						
					int nValue = GetValue(section, "Text entry", 321);
					if (nValue != 0)
						throw new Exception("Incorrect integer value found for the Text entry: " + nValue);

					strValue = GetValue(section, "Blank entry", "invalid");
					if (strValue != "")
						throw new Exception("Incorrect string value found for the Blank entry: '" + strValue + "'");
				
					object value = GetValue(section, "Blank entry");
					if (value == null)
						throw new Exception("Incorrect null value found for the Blank entry");

					nValue = GetValue(section, "Blank entry", 321);
					if (nValue != 0)
						throw new Exception("Incorrect integer value found for the Blank entry: " + nValue);

					bool bValue = GetValue(section, "Blank entry", true);
					if (bValue != false)
						throw new Exception("Incorrect bool value found for the Blank entry: " + bValue);

					strValue = GetValue(section, "Null entry", "");
					if (strValue != "")
						throw new Exception("Incorrect string value found for the Null entry: '" + strValue + "'");
				
					value = GetValue(section, "Null entry");
					if (value != null)
						throw new Exception("Incorrect object value found for the Blank entry: '" + value + "'");

					strValue = GetValue(section, "  Entry with leading and trailing spaces  ", "");
					if (strValue != "The spaces should be trimmed from the entry")
						throw new Exception("Incorrect string value found for the Entry with leading and trailing spaces: '" + strValue + "'");

					if (!HasEntry(section, "Entry with leading and trailing spaces"))
						throw new Exception("The Entry with leading and trailing spaces (trimmed) was not found");

					nValue = GetValue(section, "Integer entry", 0);
					if (nValue != 17)
						throw new Exception("Incorrect integer value found for the Integer entry: " + nValue);
					
					double dValue = GetValue(section, "Integer entry", 0.0);
					if (dValue != 17)
						throw new Exception("Incorrect double value found for the Integer entry: " + dValue);

					long lValue = Convert.ToInt64(GetValue(section, "Long entry"));
					if (lValue != 1234567890123456789)
						throw new Exception("Incorrect long value found for the Long entry: " + lValue);
					
					strValue = GetValue(section, "Long entry", "");
					if (strValue != "1234567890123456789")
						throw new Exception("Incorrect string value found for the Long entry: '" + strValue + "'");

					dValue = GetValue(section, "Double entry", 0.0);
					if (dValue != 17.95)
						throw new Exception("Incorrect double value found for the Double entry: " + dValue);

					nValue = GetValue(section, "Double entry", 321);
					if (nValue != 0)
						throw new Exception("Incorrect integer value found for the Double entry: " + nValue);
				
					strValue = GetValue(section, "DateTime entry", "");
					if (strValue != DateTime.Today.ToString())
						throw new Exception("Incorrect string value found for the DateTime entry: '" + strValue + "'");

					DateTime today = DateTime.Parse(strValue);
					if (today != DateTime.Today)
						throw new Exception("The DateTime value is not today's date: '" + strValue + "'");
				
					bValue = GetValue(section, "Boolean entry", !haveSections);
					if (bValue != haveSections)
						throw new Exception("Incorrect bool value found for the Boolean entry: " + bValue);
					
					strValue = GetValue(section, "Boolean entry", "");
					if (strValue != haveSections.ToString())
						throw new Exception("Incorrect string value found for the Boolean entry: '" + strValue + "'");

					value = GetValue(section, "Nonexistent entry");
					if (value != null)
						throw new Exception("Incorrect value found for the Nonexistent entry: '" + value + "'");

					strValue = GetValue(section, "Nonexistent entry", "Some Default");
					if (strValue != "Some Default")
						throw new Exception("Incorrect default value found for the Nonexistent entry: '" + strValue + "'");

				task = "creating a ReadOnly clone of the object";
				
					IReadOnlyProfile roProfile = CloneReadOnly();
					
					if (!roProfile.HasSection(section))
						throw new Exception("The section is missing from the cloned read-only profile");

					dValue = roProfile.GetValue(section, "Double entry", 0.0);
					if (dValue != 17.95)
						throw new Exception("Incorrect double value in the cloned object: " + dValue);
				
				task = "checking if ReadOnly clone can be hacked to allow writing";

					try
					{
						((IProfile)roProfile).ReadOnly = false;
						throw new Exception("Changing of the ReadOnly flag was allowed on the cloned read-only profile");
					}
					catch (InvalidOperationException)
					{						
					}

					try
					{
						// Test if a read-only profile can be hacked by casting
						((IProfile)roProfile).SetValue(section, "Entry which should not be written", "This should not happen");
						throw new Exception("SetValue did not throw an InvalidOperationException when writing to the cloned read-only profile");
					}
					catch (InvalidOperationException)
					{						
					}
						
			//	task = "checking the DataSet methods";

				//	DataSet ds = GetDataSet();
				//	Profile copy = (Profile)Clone();
				//	copy.Name = Name + "2";
				//	copy.SetDataSet(ds);					
					       
				if (!cleanup)
					return;
					
				task = "deleting the entries just added";

					RemoveEntry(section, "Text entry"); 
					RemoveEntry(section, "Blank entry"); 
					RemoveEntry(section, "  Entry with leading and trailing spaces  "); 
					RemoveEntry(section, "Integer entry"); 
					RemoveEntry(section, "Long entry"); 
					RemoveEntry(section, "Double entry"); 
					RemoveEntry(section, "DateTime entry"); 
					RemoveEntry(section, "Boolean entry"); 													

				task = "deleting a nonexistent entry";

					RemoveEntry(section, "Null entry"); 

				task = "verifying all entries were deleted";

					entries = GetEntryNames(section);
				
					if (entries.Length != 0)
						throw new Exception("Incorrect number of entries still found: " + entries.Length);

				task = "deleting the section";

					RemoveSection(section);

				task = "verifying the section was deleted";

					int sectionCount2 = GetSectionNames().Length;
				
					if (sectionCount != sectionCount2)
						throw new Exception("Incorrect number of sections found after deleting: " + sectionCount2);

					entries = GetEntryNames(section);				
				
					if (entries != null)
						throw new Exception("The section was apparently not deleted since GetEntryNames did not return null");
			}
			catch (Exception ex)
			{
				throw new Exception("Test Failed while " + task, ex);
			}
		}
	}	
}
