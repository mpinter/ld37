using System.Collections;
using System.Collections.Generic;

namespace flyyoufools {

    public static class Helpers {
        public static bool inBounds(int x, int y, int width, int height) {
            if (x<0 || y<0) return false;
            if (x>(width-1) || y>(height-1)) return false;
            return true;
        }
    }
}