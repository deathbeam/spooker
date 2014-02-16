/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

namespace SFGL.Utils
{
    public static class AddonMath
    {
        public static int SnapTo(int loc, int size, int max)
        {
            if (loc <= 0) return 0;
            do { loc--; }
            while (!isDivisible(loc, size));
            if (loc + size >= max) return max - size;
            return loc;
        }

        public static bool isDivisible(int x, int d)
        {
            return x % d == 0;
        }
    }
}
