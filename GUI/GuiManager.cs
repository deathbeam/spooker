/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System.Collections.Generic;

namespace SFGL.GUI
{
    public static class GuiManager
    {
		private static Dictionary<string, object> assets = new Dictionary<string, object>();

        public static T Get<T>(string name) where T : class
        {
            object result;
            if (assets.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return (T)result;
        }

        public static T Set<T>(string name, object obj) where T : class
        {
            if (!assets.ContainsKey(name))
                assets.Add(name, (T)obj);
			return (T)obj;
        }

		public static void Clear()
		{
			assets.Clear();
		}
    }
}
