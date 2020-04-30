using System;

namespace CGOL.Core
{
    public class Generation
    {
        public Cell[,] StateOfUniverse{get;set;}

        public int TickNumber {get;set;}

        public Generation(Cell[,] state, int tick)
        {
            this.StateOfUniverse = state;
            this.TickNumber = tick;
        }
    }
}