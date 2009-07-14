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
using System.IO;
using System.Text;
using System.Xml;

namespace VMukti.Profile
{
	public abstract class XmlBased : Profile
	{
		private Encoding m_encoding = Encoding.UTF8;
		internal XmlBuffer m_buffer;

		protected XmlBased()
		{
		}

		protected XmlBased(string fileName) :
			base(fileName)
		{
		}

		protected XmlBased(XmlBased profile) :
			base(profile)
		{
			m_encoding = profile.Encoding;
		}

		protected XmlDocument GetXmlDocument()
		{
			if (m_buffer != null)
				return m_buffer.XmlDocument;

			VerifyName();
			if (!File.Exists(Name))
				return null;

			XmlDocument doc = new XmlDocument();
			doc.Load(Name);
			return doc;
		}

		protected void Save(XmlDocument doc)
		{
			if (m_buffer != null)
				m_buffer.m_needsFlushing = true;
			else
				doc.Save(Name);

		}

		
		public XmlBuffer Buffer(bool lockFile)
		{
			if (m_buffer == null)
				m_buffer = new XmlBuffer(this, lockFile);
			return m_buffer; 
		}

		
		public XmlBuffer Buffer()
		{
			return Buffer(true);
		}

	
		public bool Buffering
		{
			get 
			{
				return m_buffer != null;
			}
		}

		public Encoding Encoding
		{
			get 
			{ 
				return m_encoding; 
			}
			set 
			{ 
				VerifyNotReadOnly();
				if (m_encoding == value)
					return;
						
				if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "Encoding", value))
					return;

				m_encoding = value; 				
				RaiseChangeEvent(false, ProfileChangeType.Other, null, "Encoding", value);				
			}
		}
	}

	
	public class XmlBuffer : IDisposable
	{
		private XmlBased m_profile;
		private XmlDocument m_doc;
		private FileStream m_file;
		internal bool m_needsFlushing;

		internal XmlBuffer(XmlBased profile, bool lockFile)
		{
			m_profile = profile;

			if (lockFile)
			{
				m_profile.VerifyName();
				if (File.Exists(m_profile.Name))
					m_file = new FileStream(m_profile.Name, FileMode.Open, m_profile.ReadOnly ? FileAccess.Read : FileAccess.ReadWrite, FileShare.Read);
			}
		}

		internal void Load(XmlTextWriter writer)
		{
			writer.Flush();
			writer.BaseStream.Position = 0;
			m_doc.Load(writer.BaseStream);

			m_needsFlushing = true;
		}

		internal XmlDocument XmlDocument
		{
			get
			{
				if (m_doc == null)
				{
					m_doc = new XmlDocument();

					if (m_file != null)
					{
						m_file.Position = 0;
						m_doc.Load(m_file);
					}
					else
					{
						m_profile.VerifyName();
						if (File.Exists(m_profile.Name))
							m_doc.Load(m_profile.Name);
					}
				}
				return m_doc;
			}
		}

		internal bool IsEmpty
		{
			get
			{
				return XmlDocument.InnerXml == String.Empty;
			}
		}

		public bool NeedsFlushing
		{
			get
			{
				return m_needsFlushing;
			}
		}

		
		public bool Locked
		{
			get
			{
				return m_file != null;
			}
		}

		
		public void Flush()
		{
			if (m_profile == null)
				throw new InvalidOperationException("Cannot flush an XmlBuffer object that has been closed.");

			if (m_doc == null)
				return;

			if (m_file == null)
				m_doc.Save(m_profile.Name);
			else
			{
				m_file.SetLength(0);
				m_doc.Save(m_file);
			}

			m_needsFlushing = false;
		}

		
		public void Reset()
		{
			if (m_profile == null)
				throw new InvalidOperationException("Cannot reset an XmlBuffer object that has been closed.");

			m_doc = null;
			m_needsFlushing = false;
		}

		
		public void Close()
		{
			if (m_profile == null)
				return;
				
			if (m_needsFlushing)
				Flush();

			m_doc = null;
		
			if (m_file != null)
			{
				m_file.Close();
				m_file = null;
			}

			if (m_profile != null)
				m_profile.m_buffer = null;
			m_profile = null;
		}

		
		public void Dispose()
		{
			Close();
		}
	}
}
