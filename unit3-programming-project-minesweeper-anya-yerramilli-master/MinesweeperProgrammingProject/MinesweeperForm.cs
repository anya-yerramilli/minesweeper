using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperProgrammingProject
{
    public partial class MinesweeperForm : Form
    {
        //////////////////////////////////////////
        // class constants
        private const int ROWS = 15;
        private const int COLS = 24;
        private const int BUTTON_SIZE = 25;
        private const string BOMB = "\uD83D\uDCA3";
        private const string FLAG = "\uD83D\uDEA9";

        //////////////////////////////////////////
        // fields and properties
        private int Rows { get; set; }
        private int Cols { get; set; }

        private Cell[,] board = new Cell[ROWS, COLS];

        //public Image[] surroundingNumbers = new Image[9];

        //////////////////////////////////////////
        // constructor
        public MinesweeperForm()
        {
            InitializeComponent();
            this.Rows = ROWS;
            this.Cols = COLS;
        }

        //////////////////////////////////////////
        // event handlers
        private void MinesweeperForm_Load(object sender, EventArgs e)
        {
            
            // resize the form
            this.Width = BUTTON_SIZE * this.Cols + this.Cols;
            int titleHeight = this.Height - this.ClientRectangle.Height;
            this.Height = BUTTON_SIZE * this.Rows + this.Rows + titleHeight;

            // create the buttons on the form
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    // create a new button control
                    Button b = new Button();
                    // set the button width and height
                    b.Width = BUTTON_SIZE;
                    b.Height = BUTTON_SIZE;

                    b.BackColor = System.Drawing.Color.Tan;

                    // position the button on the form
                    b.Top = i * BUTTON_SIZE;
                    b.Left = j * BUTTON_SIZE;
                    // no text
                    b.Text = String.Empty;
                    // set the button style
                    b.FlatStyle = FlatStyle.Popup;
                    // add a MouseDown event handler
                    b.MouseDown += new MouseEventHandler(MinesweeperForm_MouseDown);
                    // give the button a name in "row_col" format 
                    b.Name = i + "_" + j;
                    // add the button control to the form
                    this.Controls.Add(b);

                    // do other stuff here?
                    board[i, j] = new Cell(false, false, false, 0, b);

                }

            }

            makeBombs();
            setSurroundingBombCount();


        }

        private void makeBombs()
        {
            Random random = new Random();
            //Random randomRow = new Random();

            for (int i = 0; i < random.Next(30,40); i++)
            {
                int col = random.Next(0, 24);
                int row = random.Next(0, 15);

                if (board[row, col].HasBomb)
                {
                    i--;
                }
                else
                {
                    board[row, col].HasBomb = true;

                }

            }
        }


        private void setSurroundingBombCount()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 24; j++)
                {

                    for (int a = -1; a < 2; a++)
                    {
                        for (int b = -1; b < 2; b++)
                        {
                            if(a == 0 && b == 0) // skip own cell 
                            {
                                continue;
                            }


                            try
                            {
                                Console.WriteLine(board[i + a, j + b]);
                            }
                            catch  // error if oob
                            {
                                continue; 
                            }

                            if (board[i + a, j + b].HasBomb)
                            {
                                board[i, j].surroundingBombs++; 
                            }
                        }
                    }
                }
            }
        }

        private void MinesweeperForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (Button)sender;
                // extract the row and column from the button name
                int index = b.Name.IndexOf("_");
                int i = int.Parse(b.Name.Substring(0, index));
                int j = int.Parse(b.Name.Substring(index + 1));

                // handle mousebuttons left and right differently
                if (e.Button == MouseButtons.Left)
                {
                    // dig the position to reveal the contents 
                    RevealCell(i, j);
                }
                else
                {
                    // flag the position as a possible mine
                    if (board[i, j].Flagged)
                    {
                        board[i, j].Flagged = false;
                        b.Text = " ";
                        if(board[i,j].Revealed == true && board[i, j].surroundingBombs <= 0)
                        {
                            b.BackColor = System.Drawing.Color.Green;
                        } 
                        else if (board[i, j].Revealed == true)
                        {
                            b.BackColor = System.Drawing.Color.GreenYellow; 
                            b.Text = board[i, j].surroundingBombs.ToString();
                        } else
                        {
                            b.BackColor = System.Drawing.Color.Tan;
                        }
                        

                    }
                    else
                    {
                        board[i, j].Flagged = true;
                        b.Text = FLAG;
                        b.BackColor = System.Drawing.Color.Red;
                    }
                }
                if (checkIfWin())
                {
                    endGame();
                    MessageBox.Show("You Win!");
                }
            }
        }



        private bool checkIfWin()
        {
            for (int i = 0; i < 15; i ++)
            {
                for (int j = 0; j < 24; j++)
                { 
                    if (board[i,j].HasBomb == true && board[i,j].Flagged == false)
                    {
                        return false;
                    }

                    if (board[i, j].HasBomb == false && board[i, j].Revealed == false)
                    {
                        return false;
                    }

                }
            }
            return true; 

        }


        private void endGame()
        {
            //show all of the bombs

            //end game
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    if (board[i, j].HasBomb)
                    {
                        board[i,j].Button.Text = BOMB;
                        board[i, j].Button.BackColor = System.Drawing.Color.LightGray; 
                    } 
                    else
                    {

                        if (( i % 2 == 0 && j % 2 == 0 ) || (i % 2 == 1&& j % 2 == 1))
                        {
                            board[i, j].Button.BackColor = System.Drawing.Color.ForestGreen;
                        } else
                        {
                            board[i, j].Button.BackColor = System.Drawing.Color.Green;
                        }
                        
                        board[i, j].Button.Text = " ";
                    }
                }
            }
              
        }


        

        public void RevealCell(int row, int col)
        {
            int count = 0; 

            try
            {
                Button b = board[row, col].Button;
                if (board[row, col].HasBomb && !board[row, col].Revealed  && count <= 10)
                {
                    endGame();
                    MessageBox.Show("Game Over!");
                }
                else if (board[row, col].surroundingBombs > 0 && count <= 10 )
                {
                    b.Text = board[row, col].surroundingBombs.ToString();
                    b.BackColor = System.Drawing.Color.GreenYellow;
                    board[row, col].Revealed = true;
                    count++; 
                }
                else if (board[row, col].surroundingBombs == 0 && !board[row, col].Revealed && count <= 10 )
                {
                    b.BackColor = System.Drawing.Color.Green;
                    board[row, col].Revealed = true;
                    count++; 



                    for (int a = -1; a < 2; a++)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            if (a == 0 && k == 0)
                            {
                                continue;
                            }

                            RevealCell(row + a, col + k);

                        }
                    }




                        }
            } 
            catch
            {
                return;
            }






        }

    }
}
