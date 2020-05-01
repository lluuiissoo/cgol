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
            SetInitialState(initialState);
        }

        public Universe(int columns, int rows)
        {
            //Initialize universe. All cells are dead.
            Cell[,] initialState = new Cell[columns,rows];
            for (int c = 0; c < columns; c++)
                for (int r = 0; r < rows; r++)
                {
                    initialState.SetValue(new Cell(false), c, r);
                }
        
            //Define initial state of the universe
            this.SetInitialState(initialState);
        }

        private void SetInitialState(Cell[,] initialState)
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
            int rows = _CurrentState.GetUpperBound(0); //TODO: Validate upperbound
            int cols = _CurrentState.GetUpperBound(1); //TODO: Validate upperbound

            // Need a copy of the array to act as buffer so that original stays intact during the processing
            //Cell[,] newState = _CurrentState.Clone() as Cell[,]; //This doesn't work, as it performs a shallow copy, leaving same reference to objects inside array
            Cell[,] newState = CopyArray(_CurrentState);

            Grid grid = new Grid(_CurrentState);

            for (int i = 0; i <= rows; i++) 
                for (int j = 0; j <= cols; j++)
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

        private Cell[,] CopyArray(Cell[,] originalArr)
        {
            int rows = _CurrentState.GetUpperBound(0);
            int cols = _CurrentState.GetUpperBound(1);

            //Cell[,] newState = _CurrentState.Clone() as Cell[,]; //This doesn't work, as it performs a shallow copy, leaving same reference to objects inside array
            Cell[,] newArray = new Cell[rows+1,cols+1];
            for (int i = 0; i <= rows; i++) 
                for (int j = 0; j <= cols; j++)
                {
                    newArray[i,j] = new Cell(_CurrentState[i,j].IsLive);
                }

            return newArray;
        }
    }
}
