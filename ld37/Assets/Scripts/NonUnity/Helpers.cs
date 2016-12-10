using System.Collections;
using System.Collections.Generic;

namespace flyyoufools {

    public static class Helpers {
        public static bool inBounds(int x, int y, int width, int height) {
            if (x<0 || y<0) return false;
            if (x>(width-1) || y>(height-1)) return false;
            return true;
        }

        public class IntPos {
            public IntPos(int _row, int _col) {
                row = _row;
                col = _col;
            }
            public IntPos(BfsPos bfsPos) {
                row = bfsPos.row;
                col = bfsPos.col;
            }
            public int row;
            public int col;
        }
        public class BfsPos {
            public BfsPos(int _row, int _col, BfsPos _prev = null) {
                row = _row;
                col = _col;
                prev = _prev;
            } 
            public int row;
            public int col;
            public BfsPos prev;
        }
    }
}