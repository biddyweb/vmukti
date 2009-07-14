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
 * Last Updated: Feb. 17, 2005
 */


using System.IO;
using System.Xml;

namespace VMukti.Profile
{
	
	public class Config : XmlBased
	{
		// Fields
		private string m_groupName = "profile";
		private const string SECTION_TYPE = "System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null";

		/// <summary>
		///   Initializes a new instance of the Config class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
		public Config()
		{
		}

		/// <summary>
		///   Initializes a new instance of the Config class by setting the <see cref="Profile.Name" /> to the given file name. </summary>
		/// <param name="fileName">
		///   The name of the Config file to initialize the <see cref="Profile.Name" /> property with. </param>
		public Config(string fileName) :
			base(fileName)
		{
		}

		/// <summary>
		///   Initializes a new instance of the Config class based on another Config object. </summary>
		/// <param name="config">
		///   The Config object whose properties and events are used to initialize the object being constructed. </param>
		public Config(Config config) :
			base(config)
		{
			m_groupName = config.m_groupName;
		}

		/// <summary>
		///   Gets the default name for the Config file. </summary>
		/// <remarks>
		///   For Windows apps, this property returns the name of the executable plus .config ("program.exe.config").
		///   For Web apps, this property returns the full path of the <i>web.config</i> file.
		///   This property is used to set the <see cref="Profile.Name" /> property inside the default constructor.</remarks>
		public override string DefaultName
		{
			get
			{
				return DefaultNameWithoutExtension + ".config";
			}
		}

		/// <summary>
		///   Retrieves a copy of itself. </summary>
		/// <returns>
		///   The return value is a copy of itself as an object. </returns>
		/// <seealso cref="Profile.CloneReadOnly" />
		public override object Clone()
		{
			return new Config(this);
		}

		public string GroupName
		{
			get 
			{ 
				return m_groupName; 
			}
			set 
			{ 
				VerifyNotReadOnly();
				if (m_groupName == value)
					return;

				if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "GroupName", value))
					return;

				m_groupName = value; 
				if (m_groupName != null)
				{
					m_groupName = m_groupName.Replace(' ', '_');

					if (m_groupName.IndexOf(':') >= 0)
						throw new XmlException("GroupName may not contain a namespace prefix.");
				}

				RaiseChangeEvent(false, ProfileChangeType.Other, null, "GroupName", value);				
			}
		}

		/// <summary>
		///   Gets whether we have a valid GroupName. </summary>
		private bool HasGroupName
		{
			get
			{
				return m_groupName != null && m_groupName != "";
			}
		}
		
		/// <summary>
		///   Gets the name of the GroupName plus a slash or an empty string is HasGroupName is false. </summary>
		/// <remarks>
		///   This property helps us when retrieving sections. </remarks>
		private string GroupNameSlash
		{
			get 
			{ 
				return (HasGroupName ? (m_groupName + "/") : "");
			}
		}

		/// <summary>
		///   Retrieves whether we don't have a valid GroupName and a given section is 
		///   equal to "appSettings". </summary>
		/// <remarks>
		///   This method helps us determine whether we need to deal with the "configuration\configSections" element. </remarks>
		private bool IsAppSettings(string section)
		{
			return !HasGroupName && section != null && section == "appSettings";
		}

		protected override void VerifyAndAdjustSection(ref string section)
		{
			base.VerifyAndAdjustSection(ref section);
			if (section.IndexOf(' ') >= 0)
				section = section.Replace(' ', '_');
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
			VerifyName();
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);

			if (!RaiseChangeEvent(true, ProfileChangeType.SetValue, section, entry, value))
				return;
			
			bool hasGroupName = HasGroupName;
			bool isAppSettings = IsAppSettings(section);
			
			// If the file does not exist, use the writer to quickly create it
			if ((m_buffer == null || m_buffer.IsEmpty) && !File.Exists(Name))
			{				
				XmlTextWriter writer = null;
				
				// If there's a buffer, write to it without creating the file
				if (m_buffer == null)
					writer = new XmlTextWriter(Name, Encoding);			
				else
					writer = new XmlTextWriter(new MemoryStream(), Encoding);			

				writer.Formatting = Formatting.Indented;
	            
	            writer.WriteStartDocument();
				
	            writer.WriteStartElement("configuration");			
				if (!isAppSettings)
				{
					writer.WriteStartElement("configSections");
					if (hasGroupName)
					{
						writer.WriteStartElement("sectionGroup");
						writer.WriteAttributeString("name", null, m_groupName);				
					}
					writer.WriteStartElement("section");
					writer.WriteAttributeString("name", null, section);				
					writer.WriteAttributeString("type", null, SECTION_TYPE);
        			writer.WriteEndElement();

					if (hasGroupName)
            			writer.WriteEndElement();
           			writer.WriteEndElement();
				}
				if (hasGroupName)
					writer.WriteStartElement(m_groupName);
				writer.WriteStartElement(section);
				writer.WriteStartElement("add");
				writer.WriteAttributeString("key", null, entry);				
				writer.WriteAttributeString("value", null, value.ToString());
    			writer.WriteEndElement();
    			writer.WriteEndElement();
				if (hasGroupName)
           			writer.WriteEndElement();
       			writer.WriteEndElement();
			
				if (m_buffer != null)
					m_buffer.Load(writer);
				writer.Close();   				

				RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
				return;
			}
			
			// The file exists, edit it
			
			XmlDocument doc = GetXmlDocument();
			XmlElement root = doc.DocumentElement;
			
			XmlAttribute attribute = null;
			XmlNode sectionNode = null;
			
			// Check if we need to deal with the configSections element
			if (!isAppSettings)
			{
				// Get the configSections element and add it if it's not there
				XmlNode sectionsNode = root.SelectSingleNode("configSections");
				if (sectionsNode == null)
					sectionsNode = root.AppendChild(doc.CreateElement("configSections"));			
	
				XmlNode sectionGroupNode = sectionsNode;
				if (hasGroupName)
				{
					// Get the sectionGroup element and add it if it's not there
					sectionGroupNode = sectionsNode.SelectSingleNode("sectionGroup[@name=\"" + m_groupName + "\"]");
					if (sectionGroupNode == null)
					{
						XmlElement element = doc.CreateElement("sectionGroup");
						attribute = doc.CreateAttribute("name");
						attribute.Value = m_groupName;
						element.Attributes.Append(attribute);			
						sectionGroupNode = sectionsNode.AppendChild(element);			
					}
				}
	
				// Get the section element and add it if it's not there
				sectionNode = sectionGroupNode.SelectSingleNode("section[@name=\"" + section + "\"]");
				if (sectionNode == null)
				{
					XmlElement element = doc.CreateElement("section");
					attribute = doc.CreateAttribute("name");
					attribute.Value = section;
					element.Attributes.Append(attribute);			
	
					sectionNode = sectionGroupNode.AppendChild(element);			
				}
	
				// Update the type attribute
				attribute = doc.CreateAttribute("type");
				attribute.Value = SECTION_TYPE;
				sectionNode.Attributes.Append(attribute);			
			}

			// Get the element with the sectionGroup name and add it if it's not there
			XmlNode groupNode = root;
			if (hasGroupName)
			{
				groupNode = root.SelectSingleNode(m_groupName);
				if (groupNode == null)
					groupNode = root.AppendChild(doc.CreateElement(m_groupName));			
			}

			// Get the element with the section name and add it if it's not there
			sectionNode = groupNode.SelectSingleNode(section);
			if (sectionNode == null)
				sectionNode = groupNode.AppendChild(doc.CreateElement(section));			

			// Get the 'add' element and add it if it's not there
			XmlNode entryNode = sectionNode.SelectSingleNode("add[@key=\"" + entry + "\"]");
			if (entryNode == null)
			{
				XmlElement element = doc.CreateElement("add");
				attribute = doc.CreateAttribute("key");
				attribute.Value = entry;
				element.Attributes.Append(attribute);			

				entryNode = sectionNode.AppendChild(element);			
			}

			// Update the value attribute
			attribute = doc.CreateAttribute("value");
			attribute.Value = value.ToString();
			entryNode.Attributes.Append(attribute);			

			// Save the file
			Save(doc);
			RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
		}

		public override object GetValue(string section, string entry)
		{
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);

			try
			{
				XmlDocument doc = GetXmlDocument();
				XmlElement root = doc.DocumentElement;				
				
				XmlNode entryNode = root.SelectSingleNode(GroupNameSlash + section + "/add[@key=\"" + entry + "\"]");
				return entryNode.Attributes["value"].Value;
			}
			catch
			{				
				return null;
			}
		}

		public override void RemoveEntry(string section, string entry)
		{
			VerifyNotReadOnly();
			VerifyAndAdjustSection(ref section);
			VerifyAndAdjustEntry(ref entry);

			// Verify the document exists
			XmlDocument doc = GetXmlDocument();
			if (doc == null)
				return;

			// Get the entry's node, if it exists
			XmlElement root = doc.DocumentElement;			
			XmlNode entryNode = root.SelectSingleNode(GroupNameSlash + section + "/add[@key=\"" + entry + "\"]");
			if (entryNode == null)
				return;

			if (!RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, null))
				return;
			
			entryNode.ParentNode.RemoveChild(entryNode);			
			Save(doc);
			RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, null);
		}

		public override void RemoveSection(string section)
		{
			VerifyNotReadOnly();
			VerifyAndAdjustSection(ref section);

			// Verify the document exists
			XmlDocument doc = GetXmlDocument();
			if (doc == null)
				return;

			// Get the root node, if it exists
			XmlElement root = doc.DocumentElement;
			if (root == null)
				return;

			// Get the section's node, if it exists
			XmlNode sectionNode = root.SelectSingleNode(GroupNameSlash + section);
			if (sectionNode == null)
				return;
			
			if (!RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, null, null))
				return;
			
			sectionNode.ParentNode.RemoveChild(sectionNode);

			// Delete the configSections entry also			
			if (!IsAppSettings(section))
			{											
				sectionNode = root.SelectSingleNode("configSections/" + (HasGroupName ? ("sectionGroup[@name=\"" + m_groupName + "\"]") : "") + "/section[@name=\"" + section + "\"]");
				if (sectionNode == null)
					return;
			
				sectionNode.ParentNode.RemoveChild(sectionNode);
			}
			
			Save(doc);
			RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, null, null);
		}
		public override string[] GetEntryNames(string section)
		{
			// Verify the section exists
			if (!HasSection(section))
				return null;
			    			
			VerifyAndAdjustSection(ref section);
			XmlDocument doc = GetXmlDocument();
			XmlElement root = doc.DocumentElement;
			
			// Get the entry nodes
			XmlNodeList entryNodes = root.SelectNodes(GroupNameSlash + section + "/add[@key]");
			if (entryNodes == null)
				return null;

			// Add all entry names to the string array			
			string[] entries = new string[entryNodes.Count];
			int i = 0;
			
			foreach (XmlNode node in entryNodes)
				entries[i++] = node.Attributes["key"].Value;
			
			return entries;
		}
		
		public override string[] GetSectionNames()
		{
			// Verify the document exists
			XmlDocument doc = GetXmlDocument();
			if (doc == null)
				return null;

			// Get the root node, if it exists
			XmlElement root = doc.DocumentElement;
			if (root == null)
				return null;

			// Get the group node
			XmlNode groupNode = (HasGroupName ? root.SelectSingleNode(m_groupName) : root);
			if (groupNode == null)
				return null;

			// Get the section nodes
			XmlNodeList sectionNodes = groupNode.ChildNodes;
			if (sectionNodes == null)
				return null;

			// Add all section names to the string array			
			string[] sections = new string[sectionNodes.Count];			
			int i = 0;

			foreach (XmlNode node in sectionNodes)
				sections[i++] = node.Name;
			
			return sections;
		}		
	}
}
