//-----------------------------------------------------------------------------
// DictionaryExtensions.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Spooker.Extensions
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Extensions for Dictionary
	/// </summary>
	////////////////////////////////////////////////////////////
	public static class DictionaryExtensions
	{
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Removes all items containing specified key from dictionary
		/// </summary>
		////////////////////////////////////////////////////////////
		public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> match)
		{
			foreach (var cur in dict.Where(match).ToList())
			{
				dict.Remove(cur.Key);
			}
		}
	}
}

