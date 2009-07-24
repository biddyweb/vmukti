<<<<<<< HEAD:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/BusyTone/BusyDetect.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/
using System;
=======
﻿using System;
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ToneDetect/ToneDetect.Business/BusyTone/BusyDetect.cs
using System.Collections.Generic;
using System.Text;

namespace ToneDetect.Business.BusyTone
{
	internal class BusyDetect
	{
		internal static int SilenceNoise( BusyToneState state, short [] data )
		{
			int res;
			int accum;
			int x;
			int i;
			int j;
			int k;

			res = 0;

			if( data.Length < 1 )
			{
				return res;
			}

			accum = 0;
			for( x = 0; x < data.Length; ++x )
			{
				accum += Math.Abs( data [x] );
			}
			accum /= data.Length;

			if( accum < state.Threshold )
			{
				state.TotalSilence += data.Length / 8;
				if( state.TotalNoise > 0 )
				{
					for( i = state.DspHistory - state.BusyCount,
						  j = i + 1,
						  k = 0;
						  k < state.BusyCount;
						  ++i, ++j, ++k )
					{
						state.HistoricNoise [i] = state.HistoricNoise [j];
					}
					state.HistoricNoise [state.DspHistory - 1] = state.TotalNoise;
				}
				state.TotalNoise = 0;
				res = 1;
			}
			else
			{
				state.TotalNoise += data.Length / 8;
				if( state.TotalSilence > 0 )
				{
					int silence1;
					int silence2;

					silence1 = state.HistoricSilence [state.DspHistory - 1];
					silence2 = state.HistoricSilence [state.DspHistory - 2];
					for( i = state.DspHistory - state.BusyCount,
						j = i + 1,
						k = 0;
						k < state.BusyCount;
						++i, ++j, ++k )
					{
						state.HistoricSilence [i] = state.HistoricSilence [j];
					}
					state.HistoricSilence [state.DspHistory - 1] = state.TotalSilence;
					if( silence1 < silence2 )
					{
						if( silence1 + silence1 * (int)BusyDetectDef.BUSY_PERCENT / 100 >= silence2 )
						{
							state.BusyMaybe = 1;
						}
						else
						{
							state.BusyMaybe = 0;
						}
					}
					else
					{
						if( silence1 - silence1 * (int)BusyDetectDef.BUSY_PERCENT / 100 <= silence2 )
						{
							state.BusyMaybe = 1;
						}
						else
						{
							state.BusyMaybe = 0;
						}
					}
				}
				state.TotalSilence = 0;
			}

			return res;
		}

		internal static int Detect( BusyToneState state )
		{
			int res;
			int x;
			int avgsilence;
			int hitsilence;
			int avgtone;
			int hittone;

			res = 0;
			avgsilence = 0;
			hitsilence = 0;
			avgtone = 0;
			hittone = 0;
			if( state.BusyMaybe == 0 )
			{
				return res;
			}

			for( x = state.DspHistory - state.BusyCount; x < state.DspHistory; ++x )
			{
				avgsilence += state.HistoricSilence [x];
				avgtone += state.HistoricNoise [x];
			}

			avgsilence /= state.BusyCount;
			avgtone /= state.BusyCount;

			for( x = state.DspHistory - state.BusyCount; x < state.DspHistory; ++x )
			{
				if( avgsilence > state.HistoricNoise [x] )
				{
					if( avgsilence - ( avgsilence * (int)BusyDetectDef.BUSY_PERCENT / 100 ) <= state.HistoricSilence [x] )
					{
						hitsilence++;
					}
				}
				else if( avgsilence + ( avgsilence * (int)BusyDetectDef.BUSY_PERCENT / 100 ) >= state.HistoricSilence [x] )
				{
					hitsilence++;
				}

				if( avgtone > state.HistoricNoise [x] )
				{
					if( avgtone - ( avgtone * (int)BusyDetectDef.BUSY_PERCENT / 100 ) <= state.HistoricNoise [x] )
					{
						hittone++;
					}
				}
				else if( avgtone + ( avgtone * (int)BusyDetectDef.BUSY_PERCENT / 100 ) >= state.HistoricNoise [x] )
				{
						hittone++;
				}
			}
			//this is the 'tone only' code
			//if( (hittone > state.BusyCount - 1 ) && (hitsilence >= state.BusyCount - 1) &&
			//	 (avgtone >= BusyDetectDef.BUSY_MIN && avgtone <= BusyDetectDef.BUSY_MAX))
			if( (hittone >= state.BusyCount - 1 ) && (avgtone >= (int)BusyDetectDef.BUSY_MAX &&
				avgtone <= (int)BusyDetectDef.BUSY_MAX))
			{
				if( avgtone > avgsilence )
				{
					if( avgtone - avgtone * (int)BusyDetectDef.BUSY_PERCENT/100 <= avgsilence )
					res = 1;
				}
				else 
				{
					if( avgtone + avgtone * (int)BusyDetectDef.BUSY_PERCENT/100 >= avgsilence )
					{
						res = 1;
					}
				}
			}

			if( (res > 0) && (state.BusyToneLength > 0) )
			{
				if( Math.Abs( avgtone - state.BusyToneLength) > 
					(state.BusyToneLength*(int)BusyDetectDef.BUSY_PAT_PERCENT/100))
				{
					res = 0;
				}
			}

			if( (res > 0) && (state.BusyQuietLength > 0) )
			{
				if( Math.Abs( avgsilence - state.BusyQuietLength) > 
					(state.BusyQuietLength*(int)BusyDetectDef.BUSY_PAT_PERCENT/100))
				{
					res = 0;
				}
			}

			return res;
		}
	}
}
