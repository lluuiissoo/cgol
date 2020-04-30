using System;

namespace CGOL.Core
{
    public class Cell
    {
        private bool _alive = false;

        public bool IsLive
        {
             get { return _alive; }
        }

        public Cell(bool state)
        {
            _alive = state;
        }

        public void Kill()
        {
            if (_alive)
            {
                _alive = false;
            }
            else
            {
                throw new InvalidOperationException("Cannot kill a dead cell");
            }
        }

        public void Revive()
        {
            if (!_alive)
            {
                _alive = true;
            }
            else
            {
                throw new InvalidOperationException("Cannot revive a live cell");
            }
        }
    }
}