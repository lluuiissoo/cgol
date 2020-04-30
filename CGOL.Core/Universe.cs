using System;
using System.Collections.Generic;
using System.Linq;

namespace CGOL.Core
{
    public class Universe
    {
        private Cell[,] _CurrentState;
        
        private int _TickCount;

        public List<Generation> Generations { get; private set; }

        public Universe(Cell[,] initialState)
        {
            //Define initial state of the universe
            _CurrentState = initialState;
            _TickCount = 0;
            this.Generations = new List<Generation>(){ new Generation(_CurrentState, _TickCount) }; //Generation 0
        }

        public void Tick()
        {
            // Evolve to a new generation by applying rules to all cells in the current universe
            _CurrentState = this.ApplyRules();
            _TickCount++;

            this.Generations.Add(new Generation(_CurrentState, _TickCount));
        }

        private Cell[,] ApplyRules()
        {
            Cell[,] newState = _CurrentState.Clone() as Cell[,];

            Grid grid = new Grid(_CurrentState);

            for (int i = 0; i <= _CurrentState.GetUpperBound(0); i++) //TODO: Validate upperbound
                for (int j = 0; j <= _CurrentState.GetUpperBound(1); j++) //TODO: Validate upperbound
                {
                   List<Cell> neighbors = grid.GetNeighbors(i,j);
                   IEnumerable<Cell> liveCells = neighbors.Where(c => c.IsLive);

                   //Any live cell with two or three live neighbors survives.
                   if (_CurrentState[i,j].IsLive)
                   {
                        if ( (liveCells.Count() == 2) || (liveCells.Count() == 3) )
                        {
                            //Survive
                        }                            
                        else
                        {
                            newState[i,j].Kill();
                        }
                   }
                   else
                   {
                        //Any dead cell with three live neighbors becomes a live cell.
                        if (liveCells.Count() == 3)
                            newState[i,j].Revive();
                   }

                   //All other live cells die in the next generation. Similarly, all other dead cells stay dead.
                }

            return newState;
        }
    }
}
