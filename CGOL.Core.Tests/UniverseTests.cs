using System;
using Xunit;
using CGOL.Core;

namespace CGOL.Core.Tests
{
    public class UniverseTests
    {
        private bool MatchState(Cell[,] state1, Cell[,] state2)
        {
            for (int i = 0; i <= state1.GetUpperBound(0); i++)
                for (int j = 0; j <= state1.GetUpperBound(1); j++)
                    if (state1[i,j].IsLive != state2[i,j].IsLive)
                        return false;

            return true;
        }

        [Fact]
        public void Universe_Tick_OneTime_ShouldRecordTwoGenerations()
        {
            //Arrange
            Cell[,] initialState = new Cell[3,3] {
                { new Cell(true), new Cell(false), new Cell(true) },
                { new Cell(false), new Cell(false), new Cell(false) },
                {new Cell(true), new Cell(false), new Cell(false) },
            };
            /*
            1 0 1
            0 0 0
            1 0 0
            */
            Cell[,] expectedStateTick1 = new Cell[3,3] {
                { new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(true), new Cell(false) },
                {new Cell(false), new Cell(false), new Cell(false) },
            };
            /*
            0 0 0
            0 1 0
            0 0 0
            */
            
            Universe universe = new Universe(initialState);

            //Assert.Equal(1, universe.Generations.Count); //Ensure initial state has only one generation
            Assert.Single(universe.Generations); //Assert.Equal raises a warning if collection only contains one element: "warning xUnit2013: Do not use Assert.Equal() to check for collection size"
            Assert.Equal(0, universe.Generations[0].TickNumber); //Ensure initial tick number is zero (generation 0)
            Assert.True(MatchState(initialState, universe.Generations[0].StateOfUniverse)); //Ensure initial state is identical to generation zero
                        
            //Act
            universe.Tick();

            //Asert
            Assert.Equal(2, universe.Generations.Count); //Ensure new generation was generated
            Assert.True(MatchState(expectedStateTick1, universe.Generations[1].StateOfUniverse));
        }

        [Fact]
        public void Universe_Tick_TwoTimes_ShouldRecordProperGenerations()
        {
            //Arrange
            Cell[,] initialState = new Cell[3,3] {
                { new Cell(true), new Cell(true), new Cell(true) },
                { new Cell(true), new Cell(false), new Cell(false) },
                {new Cell(true), new Cell(false), new Cell(false) },
            };
            /*
            1 1 1
            1 0 0
            1 0 0
            */
            Cell[,] expectedStateTick1 = new Cell[3,3] {
                { new Cell(true), new Cell(true), new Cell(false) },
                { new Cell(true), new Cell(false), new Cell(false) },
                {new Cell(false), new Cell(false), new Cell(false) },
            };
            /*
            1 1 0
            1 0 0
            0 0 0
            */
            Cell[,] expectedStateTick2 = new Cell[3,3] {
                { new Cell(true), new Cell(true), new Cell(false) },
                { new Cell(true), new Cell(true), new Cell(false) },
                {new Cell(false), new Cell(false), new Cell(false) },
            };
            /*
            1 1 0
            1 1 0
            0 0 0
            */
            
            Universe universe = new Universe(initialState);

            Assert.True(MatchState(initialState, universe.Generations[0].StateOfUniverse)); //Ensure initial state is identical to generation zero
                        
            //Act
            universe.Tick();
            
            //Asert
            Assert.True(MatchState(expectedStateTick1, universe.Generations[1].StateOfUniverse));

            //Act
            universe.Tick();

            //Asert
            Assert.True(MatchState(expectedStateTick2, universe.Generations[2].StateOfUniverse));
        }

        [Fact]
        public void Universe_Tick_DiagonalWith5Cells_ShouldIterate3Times()
        {
            //Arrange
            /*
            1 0 0 0 0
            0 1 0 0 0
            0 0 1 0 0
            0 0 0 1 0
            0 0 0 0 1
            */
            Cell[,] initialState = new Cell[5,5] {
                { new Cell(true), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(true), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(true), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(true), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(true) }
            };

            /*
            0 0 0 0 0
            0 1 0 0 0
            0 0 1 0 0
            0 0 0 1 0
            0 0 0 0 0
            */
            Cell[,] expectedStateTick1 = new Cell[5,5] {
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(true), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(true), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(true), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) }
            };

            /*
            0 0 0 0 0
            0 0 0 0 0
            0 0 1 0 0
            0 0 0 0 0
            0 0 0 0 0
            */
            Cell[,] expectedStateTick2 = new Cell[5,5] {
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(true), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) }
            };

            /*
            0 0 0 0 0
            0 0 0 0 0
            0 0 0 0 0
            0 0 0 0 0
            0 0 0 0 0
            */
            Cell[,] expectedStateTick3 = new Cell[5,5] {
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false), new Cell(false), new Cell(false) }
            };
            
            Universe universe = new Universe(initialState);

            Assert.True(MatchState(initialState, universe.Generations[0].StateOfUniverse)); //Ensure initial state is identical to generation zero
                        
            //Act
            universe.Tick();
            
            //Asert
            Assert.True(MatchState(expectedStateTick1, universe.Generations[1].StateOfUniverse));

            //Act
            universe.Tick();

            //Asert
            Assert.True(MatchState(expectedStateTick2, universe.Generations[2].StateOfUniverse));

            //Act
            universe.Tick();

            //Asert
            Assert.True(MatchState(expectedStateTick3, universe.Generations[3].StateOfUniverse));
        }
    }
}
