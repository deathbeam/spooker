using System;
using System.Collections.Generic;
using System.Linq;

namespace SFGL.Utils
{
	public static class Extensions
	{
		public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> match)
		{
			foreach (var cur in dict.Where(match).ToList())
			{
				dict.Remove(cur.Key);
			}
		}
	}
}