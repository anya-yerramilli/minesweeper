using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperProgrammingProject
{
    class Cell
    {
        public bool Revealed { get; set; }
        public bool HasBomb { get; set; }
        public bool Flagged { get; set; }
        public int surroundingBombs { get; set; }
        public Button Button { get; set; }

        public Cell(bool revealed, bool hasBomb, bool isFlagged, int numBombsAround, Button button)
        {
            Revealed = revealed;
            HasBomb = hasBomb;
            Flagged = isFlagged;
            surroundingBombs = numBombsAround;
            Button = button;
        }

    }
}
