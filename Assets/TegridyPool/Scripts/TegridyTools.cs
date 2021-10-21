/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace Tegridy.Tools
{
	public static class TegridyTools
	{
		public static void SetActive(this Component obj, bool value)
		{
			obj.gameObject.SetActive(value);
		}

		public static void SetGroupActive(this Component[] obj, bool value)
		{
			for (int i = 0; i < obj.Length; i++) obj[i].SetActive(value);
		}

		public static string ToStringTime(this float time)
		{
			time = Mathf.Abs(time);
			int minutes = (int)time / 60;
			int seconds = (int)time - 60 * minutes;
			int milliseconds = (int)(100 * (time - minutes * 60 - seconds));
			return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
		}

		public static string ToStringTime(this float time, string format)
		{
			int minutes = (int)time / 60;
			int seconds = (int)time - 60 * minutes;
			int milliseconds = (int)(1000 * (time - minutes * 60 - seconds));
			return string.Format(format, minutes, seconds, milliseconds);
		}
	}
}