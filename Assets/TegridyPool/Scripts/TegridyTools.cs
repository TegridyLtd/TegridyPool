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