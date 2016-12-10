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
    }

    public class IntPair {
        public int first;
        public int second;

        public IntPair(int f, int s) {
            first = f;
            second = s;
        }
    }
}