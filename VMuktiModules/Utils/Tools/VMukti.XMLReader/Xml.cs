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

using System.IO;
using System.Xml;

namespace VMukti.Profile
{
	
	public class Xml : XmlBased
	{
		// Fields
		private string m_rootName = "profile";

		/// <summary>
		///   Initializes a new instance of the Xml class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
		public Xml()
		{
		}

		/// <summary>
		///   Initializes a new instance of the Xml class by setting the <see cref="Profile.Name" /> to the given file name. </summary>
		/// <param name="fileName">
		///   The name of the XML file to initialize the <see cref="Profile.Name" /> property with. </param>
		public Xml(string fileName) :
			base(fileName)
		{
		}

		/// <summary>
		///   Initializes a new instance of the Xml class based on another Xml object. </summary>
		/// <param name="xml">
		///   The Xml object whose properties and events are used to initialize the object being constructed. </param>
		public Xml(Xml xml) :
			base(xml)
		{
			m_rootName = xml.m_rootName;
		}

		/// <summary>
		///   Gets the default name for the XML file. </summary>
		/// <remarks>
		///   For Windows apps, this property returns the name of the executable plus .xml ("program.exe.xml").
		///   For Web apps, this property returns the full path of <i>web.xml</i> based on the root folder.
		///   This property is used to set the <see cref="Profile.Name" /> property inside the default constructor.</remarks>
		public override string DefaultName
		{
			get
			{
				return DefaultNameWithoutExtension + ".xml";
			}
		}

		/// <summary>
		///   Retrieves a copy of itself. </summary>
		/// <returns>
		///   The return value is a copy of itself as an object. </returns>
		/// <seealso cref="Profile.CloneReadOnly" />
		public override object Clone()
		{
			return new Xml(this);
		}

		/// <summary>
		///   Retrieves the XPath string used for retrieving a section from the XML file. </summary>
		/// <returns>
		///   An XPath string. </returns>
		/// <seealso cref="GetEntryPath" />
		private string GetSectionsPath(string section)
		{
			return "section[@name=\"" + section + "\"]";
		}
		                              
		/// <summary>
		///   Retrieves the XPath string used for retrieving an entry from the XML file. </summary>
		/// <returns>
		///   An XPath string. </returns>
		/// <seealso cref="GetSectionsPath" />
		private string GetEntryPath(string entry)
		{
			return "entry[@name=\"" + entry + "\"]";
		}

		public string RootName
		{
			get 
			{ 
				return m_rootName; 
			}
			set 
			{ 
				VerifyNotReadOnly();
				if (m_rootName == value.Trim())
					return;
					
				if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "RootName", value))
					return;

				m_rootName = value.Trim(); 				
				RaiseChangeEvent(false, ProfileChangeType.Other, null, "RootName", value);				
			}
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

			string valueString = value.ToString();

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
				
	            writer.WriteStartElement(m_rootName);			
					writer.WriteStartElement("section");
					writer.WriteAttributeString("name", null, section);				
						writer.WriteStartElement("entry");
						writer.WriteAttributeString("name", null, entry);				
	            			writer.WriteString(valueString);
	            		writer.WriteEndElement();
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
			
			// Get the section element and add it if it's not there
			XmlNode sectionNode = root.SelectSingleNode(GetSectionsPath(section));
			if (sectionNode == null)
			{
				XmlElement element = doc.CreateElement("section");
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = section;
				element.Attributes.Append(attribute);			
				sectionNode = root.AppendChild(element);			
			}

			// Get the entry element and add it if it's not there
			XmlNode entryNode = sectionNode.SelectSingleNode(GetEntryPath(entry));
			if (entryNode == null)
			{
				XmlElement element = doc.CreateElement("entry");
				XmlAttribute attribute = doc.CreateAttribute("name");
				attribute.Value = entry;
				element.Attributes.Append(attribute);			
				entryNode = sectionNode.AppendChild(element);			
			}

			// Add the value and save the file
			entryNode.InnerText = valueString;
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
				
				XmlNode entryNode = root.SelectSingleNode(GetSectionsPath(section) + "/" + GetEntryPath(entry));
				return entryNode.InnerText;
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
			XmlNode entryNode = root.SelectSingleNode(GetSectionsPath(section) + "/" + GetEntryPath(entry));
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
			XmlNode sectionNode = root.SelectSingleNode(GetSectionsPath(section));
			if (sectionNode == null)
				return;
			
			if (!RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, null, null))
				return;

			root.RemoveChild(sectionNode);
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
			XmlNodeList entryNodes = root.SelectNodes(GetSectionsPath(section) + "/entry[@name]");
			if (entryNodes == null)
				return null;

			// Add all entry names to the string array			
			string[] entries = new string[entryNodes.Count];
			int i = 0;

			foreach (XmlNode node in entryNodes)
				entries[i++] = node.Attributes["name"].Value;
			
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

			// Get the section nodes
			XmlNodeList sectionNodes = root.SelectNodes("section[@name]");
			if (sectionNodes == null)
				return null;

			// Add all section names to the string array			
			string[] sections = new string[sectionNodes.Count];			
			int i = 0;

			foreach (XmlNode node in sectionNodes)
				sections[i++] = node.Attributes["name"].Value;
			
			return sections;
		}		
	}
}
