// $Id: AnsiEscapeSequences.java,v 1.1 2001/06/01 06:24:44 pcharles Exp $

/// <summary>************************************************************************
/// Copyright (C) 2001, Patrick Charles and Jonas Lehmann                   *
/// Distributed under the Mozilla Public License                            *
/// http://www.mozilla.org/NPL/MPL-1.1.txt                                *
/// *************************************************************************
/// </summary>
namespace ToneDetect.SharpPcap.Packets.Util
{
	using System;
	
	/// <summary> String constants for color console output.
	/// <p>
	/// This file contains control sequences to print color text on a text 
	/// console capable of interpreting and displaying control sequences.
	/// <p>
	/// A capable console would be 
	/// unix bash, os/2 shell, or command.com w/ ansi.sys loaded
	/// *
	/// </summary>
	/// <author>  Chris Cheetham
	/// 
	/// </author>
	public struct AnsiEscapeSequences_Fields{
		public readonly static System.String ESCAPE_BEGIN;
		public readonly static System.String ESCAPE_END = "m";
		public readonly static System.String RESET;
		public readonly static System.String BOLD;
		public readonly static System.String UNDERLINE;
		public readonly static System.String INVERSE;
		public readonly static System.String BLACK;
		public readonly static System.String BLUE;
		public readonly static System.String GREEN;
		public readonly static System.String CYAN;
		public readonly static System.String RED;
		public readonly static System.String PURPLE;
		public readonly static System.String BROWN;
		public readonly static System.String LIGHT_GRAY;
		public readonly static System.String DARK_GRAY;
		public readonly static System.String LIGHT_BLUE;
		public readonly static System.String LIGHT_GREEN;
		public readonly static System.String LIGHT_CYAN;
		public readonly static System.String LIGHT_RED;
		public readonly static System.String LIGHT_PURPLE;
		public readonly static System.String YELLOW;
		public readonly static System.String WHITE;
		public readonly static System.String RED_BACKGROUND;
		public readonly static System.String GREEN_BACKGROUND;
		public readonly static System.String YELLOW_BACKGROUND;
		public readonly static System.String BLUE_BACKGROUND;
		public readonly static System.String PURPLE_BACKGROUND;
		public readonly static System.String CYAN_BACKGROUND;
		public readonly static System.String LIGHT_GRAY_BACKGROUND;
		static AnsiEscapeSequences_Fields()
		{
			ESCAPE_BEGIN = "" + (char) 27 + "[";
			RESET = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			BOLD = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;1" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			UNDERLINE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;4" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			INVERSE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;7" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			BLACK = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;30" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			BLUE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;34" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			GREEN = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;32" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			CYAN = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;36" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			RED = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;31" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			PURPLE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;35" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			BROWN = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;33" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_GRAY = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;37" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			DARK_GRAY = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;30" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_BLUE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;34" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_GREEN = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;32" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_CYAN = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;36" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_RED = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;31" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_PURPLE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;35" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			YELLOW = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;33" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			WHITE = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "1;37" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			RED_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;41" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			GREEN_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;42" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			YELLOW_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;43" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			BLUE_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;44" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			PURPLE_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;45" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			CYAN_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;46" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
			LIGHT_GRAY_BACKGROUND = Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_BEGIN + "0;47" + Packets.Util.AnsiEscapeSequences_Fields.ESCAPE_END;
		}
	}
	public interface AnsiEscapeSequences
		{
			//UPGRADE_NOTE: Members of interface 'AnsiEscapeSequences' were extracted into structure 'AnsiEscapeSequences_Fields'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1045"'
		}
}