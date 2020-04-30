using System;
using Xunit;
using CGOL.Core;

namespace CGOL.Core.Tests
{
    public class CellTests
    {
        [Fact]
        public void Cell_Kill_ForDeadCell_ShouldThrowException()
        {
            //Arrange
            Cell cell = new Cell(false);
            
            //Asert
            Assert.Throws<InvalidOperationException>(() => cell.Kill());
        }

        [Fact]
        public void Cell_Kill_ForLiveCell_ShouldMakeCellDead()
        {
            //Arrange
            Cell cell = new Cell(true);
            
            //Act
            cell.Kill();

            //Asert
            Assert.False(cell.IsLive);
        }

        [Fact]
        public void Cell_Revive_ForLiveCell_ShouldThrowException()
        {
            //Arrange
            Cell cell = new Cell(true);
            
            //Asert
            Assert.Throws<InvalidOperationException>(() => cell.Revive());
        }

        [Fact]
        public void Cell_Revive_ForDeadCell_ShouldMakeCellLive()
        {
            //Arrange
            Cell cell = new Cell(false);
            
            //Act
            cell.Revive();

            //Asert
            Assert.True(cell.IsLive);
        }
    }
}
