using System;
using Xunit;
using CGOL.Core;
using System.Collections.Generic;
using System.Linq;

namespace CGOL.Core.Tests
{
    public class GridTests
    {
        Cell[,] _grid3x3 = new Cell[3,3] {
                { new Cell(false), new Cell(false), new Cell(false) },
                { new Cell(false), new Cell(false), new Cell(false) },
                {new Cell(false), new Cell(false), new Cell(false) },
            };

        [Fact]
        public void Grid_GetNeighbors_ForTopLeftCorner_ShouldReturnThreeNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            x 0 0
            0 0 0
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(0,0);

            //Asert
            Assert.Equal(3, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForTopRightCorner_ShouldReturnThreeNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 x
            0 0 0
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(0,2);

            //Asert
            Assert.Equal(3, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForBottomLeftCorner_ShouldReturnThreeNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            0 0 0
            x 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(2,0);

            //Asert
            Assert.Equal(3, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForBottomRightCorner_ShouldReturnThreeNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            0 0 0
            0 0 x
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(2,2);

            //Asert
            Assert.Equal(3, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForTopRow_ShouldReturnFiveNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 x 0
            0 0 0
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(0,1);

            //Asert
            Assert.Equal(5, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForBottomRow_ShouldReturnFiveNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            0 0 0
            0 x 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(2,1);

            //Asert
            Assert.Equal(5, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForFirstColumn_ShouldReturnFiveNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            x 0 0
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(1,0);

            //Asert
            Assert.Equal(5, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForLastColumn_ShouldReturnFiveNodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            0 0 x
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(1,2);

            //Asert
            Assert.Equal(5, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForCenter_ShouldReturn8Nodes()
        {
            //Arrange
            Grid grid = new Grid(_grid3x3);
            /*
            0 0 0
            0 x 0
            0 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(1,1);

            //Asert
            Assert.Equal(8, cells.Count);

        }

        [Fact]
        public void Grid_GetNeighbors_ForCenter_ShouldReturn8NodesWithProperValues()
        {
            //Arrange
            Cell[,] _grid3x3WithLiveCells = new Cell[3,3] {
                { new Cell(true), new Cell(false), new Cell(true) },
                { new Cell(false), new Cell(false), new Cell(false) },
                {new Cell(true), new Cell(false), new Cell(false) },
            };

            Grid grid = new Grid(_grid3x3WithLiveCells);
            /*
            1 0 1
            0 x 0
            1 0 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(1,1);

            //Asert
            Assert.Equal(8, cells.Count); //Returns 8 neighbors
            
            IEnumerable<Cell> liveCells = cells.Where(c => c.IsLive);
            Assert.Equal(3, liveCells.Count()); //The result shall contain 3 live cells

        }

        [Fact]
        public void Grid_GetNeighbors_ForBottomRow_ShouldReturn5NodesWithProperValues()
        {
            //Arrange
            Cell[,] _grid3x3WithLiveCells = new Cell[3,3] {
                { new Cell(true), new Cell(false), new Cell(true) },
                { new Cell(false), new Cell(false), new Cell(false) },
                {new Cell(true), new Cell(false), new Cell(false) },
            };

            Grid grid = new Grid(_grid3x3WithLiveCells);
            /*
            1 0 1
            0 0 0
            1 x 0
            */
            
            //Act
            List<Cell> cells = grid.GetNeighbors(2,1);

            //Asert
            Assert.Equal(5, cells.Count); //Returns 5 neighbors
            
            IEnumerable<Cell> liveCells = cells.Where(c => c.IsLive);
            ///Assert.Equal(1, liveCells.Count()); //The result shall contain 1 live cell
            Assert.Single(liveCells); //Assert.Equal raises a warning if collection only contains one element: "warning xUnit2013: Do not use Assert.Equal() to check for collection size"

        }
    }
}
