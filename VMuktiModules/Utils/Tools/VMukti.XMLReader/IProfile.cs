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
                       
[assembly:CLSCompliant(true)] 
namespace VMukti.Profile
{	
	
	public interface IReadOnlyProfile : ICloneable
	{
		/// <summary>
		///   Gets the name associated with the profile. </summary>
		/// <remarks>
		///   This should be the name of the file where the data is stored, or something equivalent. </remarks>
		string Name
		{
			get; 
		}

		
		object GetValue(string section, string entry);
		
		
		string GetValue(string section, string entry, string defaultValue);
		
		
		int GetValue(string section, string entry, int defaultValue);

		
		double GetValue(string section, string entry, double defaultValue);

		
		bool GetValue(string section, string entry, bool defaultValue);

		
		bool HasEntry(string section, string entry);

		
		bool HasSection(string section);

		
		string[] GetEntryNames(string section);

		
		string[] GetSectionNames();

		DataSet GetDataSet();
	}

	
	public interface IProfile : IReadOnlyProfile
	{
		
		new string Name
		{
			get; 
			set;
		}

		
		string DefaultName
		{
			get;
		}

		
		bool ReadOnly
		{
			get; 
			set;
		}		
	
		
		void SetValue(string section, string entry, object value);
		
		
		void RemoveEntry(string section, string entry);

		
		void RemoveSection(string section);
		
		
		void SetDataSet(DataSet ds);
		
		
		IReadOnlyProfile CloneReadOnly();
		
		
		event ProfileChangingHandler Changing;

		
		event ProfileChangedHandler Changed;				
	}
}

