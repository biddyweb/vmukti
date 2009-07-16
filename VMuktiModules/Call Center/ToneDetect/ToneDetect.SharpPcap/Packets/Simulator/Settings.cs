// $Id: Settings.java,v 1.1 2001/06/26 22:53:52 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Simulator
{
	using System;
	using PropertyHelper = Packets.Util.PropertyHelper;
	/// <summary> Simulator settings.
	/// *
	/// </summary>
	/// <author>  Patrick Charles and Jonas Lehmann
	/// </author>
	/// <version>  $Revision: 1.1 $
	/// @lastModifiedBy $Author: pcharles $
	/// @lastModifiedAt $Date: 2001/06/26 22:53:52 $
	/// 
	/// </version>
	public class Settings
	{
		public static System.String PROPERTY_PKG = "SharpPcap.Packets.Simulator";
		public static System.String PROPERTY_FILE = "simulator.properties";
		
		public static int SIM_NETWORK;
		public static int SIM_NETMASK;
		public static float PROB_ETH_IP;
		public static float PROB_ETH_ARP;
		public static float PROB_ETH_RARP;
		public static float PROB_ETH_OTHER;
		public static float PROB_IP_TCP;
		public static float PROB_IP_UDP;
		public static float PROB_IP_ICMP;
		public static float PROB_IP_OTHER;
		public static float PROB_ARP_REQUEST;
		public static float PROB_ARP_REPLY;
		
		
		// default search paths for property file location
		//UPGRADE_WARNING: Method 'java.util.Properties.getProperty' was converted to 'System.Configuration.AppSettingsReader.GetValue' which may throw an exception. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1101"'
		//UPGRADE_ISSUE: Method 'java.lang.System.getProperties' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javalangSystemgetProperties"'
		//UPGRADE_NOTE: The initialization of  'PATH_DEFAULTS' was moved to static method 'SharpPcap.Packets.Simulator.Settings'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		public static System.String[] PATH_DEFAULTS;
		
		
		
		private static System.String _rcsId = "$Id: Settings.java,v 1.1 2001/06/26 22:53:52 pcharles Exp $";
		static Settings()
		{
			PATH_DEFAULTS = new System.String[]{".", Environment.SpecialFolder.ApplicationData + "/properties"};
			{
				System.Configuration.AppSettingsReader properties = PropertyHelper.load(PATH_DEFAULTS, PROPERTY_FILE);
				if (properties == null /*|| properties.Count == 0*/)
				{
					System.Console.Error.WriteLine("FATAL: simulator cannot start without properties!");
					System.Environment.Exit(1);
				}
				
				SIM_NETWORK = PropertyHelper.getIpProperty(properties, PROPERTY_PKG + ".network");
				SIM_NETMASK = PropertyHelper.getIpProperty(properties, PROPERTY_PKG + ".mask");
				PROB_ETH_IP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.eth.ip");
				PROB_ETH_ARP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.eth.arp");
				PROB_ETH_RARP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.eth.rarp");
				PROB_ETH_OTHER = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.eth.other");
				PROB_ARP_REQUEST = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.arp.request");
				PROB_ARP_REPLY = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.arp.reply");
				PROB_IP_TCP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.ip.tcp");
				PROB_IP_UDP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.ip.udp");
				PROB_IP_ICMP = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.ip.icmp");
				PROB_IP_OTHER = PropertyHelper.getFloatProperty(properties, PROPERTY_PKG + ".prob.ip.other");
			}
		}
	}
}