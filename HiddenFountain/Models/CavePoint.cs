﻿namespace HiddenFountain.Models {
    internal struct CavePoint {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public CavePoint(int row, int col) {
            Row = row;
            Col = col;
        }

        public override readonly bool Equals(object obj) {
            if (obj is CavePoint other) {
                return Row == other.Row && Col == other.Col;
            }
            return false;
        }

        public override readonly int GetHashCode() {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(CavePoint point1, CavePoint point2) {
            return point1.Equals(point2);
        }

        public static bool operator !=(CavePoint point1, CavePoint point2) {
            return !point1.Equals(point2);
        }

    }
}
