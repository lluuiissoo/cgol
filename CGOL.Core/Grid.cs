using System;
using System.Collections.Generic;

namespace CGOL.Core
{
    public class Grid
    {
        private Cell[,] _Grid;

        private int _RowCount, _ColCount;
        
        public Grid(Cell[,] grid)
        {
            this._Grid = grid;

            _RowCount = grid.GetUpperBound(0) + 1;
            _ColCount = grid.GetUpperBound(1) + 1;
        }

        public List<Cell> GetNeighbors(int i, int j)
        {
            Cell topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight = null;
            List<Cell> neighbors = new List<Cell>();
            
            topLeft = ((i==0) || (j==0)) ? null : _Grid[i-1,j-1]; //Exclude first row and first column
            if (topLeft != null)
                neighbors.Add(topLeft);
            
            top = (i==0) ? null : _Grid[i-1,j]; //Exclude first row
            if (top != null)
                neighbors.Add(top);

            topRight = ((i==0) || (j==_ColCount-1)) ? null : _Grid[i-1,j+1]; //Exclude first row and last column
            if (topRight != null)
                neighbors.Add(topRight);

            left = (j==0) ? null : _Grid[i,j-1]; // Exclude first column
            if (left != null)
                neighbors.Add(left);

            right = (j==_ColCount-1) ? null : _Grid[i,j+1]; //Exclude last column
            if (right != null)
                neighbors.Add(right);

            bottomLeft = ((i==_RowCount-1) || (j==0)) ? null : _Grid[i+1,j-1]; //Exclude last row and first column
            if (bottomLeft != null)
                neighbors.Add(bottomLeft);

            bottom = (i==_RowCount-1) ? null : _Grid[i+1,j]; //Exclude last row
            if (bottom != null)
                neighbors.Add(bottom);

            bottomRight = ((i==_RowCount-1) || (j==_ColCount-1)) ? null : _Grid[i+1,j+1]; //Exclude last row and last column
            if (bottomRight != null)
                neighbors.Add(bottomRight);

            return neighbors;
        }
    }
}
