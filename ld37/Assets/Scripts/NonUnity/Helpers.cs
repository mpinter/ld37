using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flyyoufools {

    public static class Helpers {
        public static bool inBounds(int row, int col, int height, int width) {
            if (row<0 || col<0) return false;
            if ((row>(height-1)) || (col>(width-1))) return false;
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