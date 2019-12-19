using System;
using SFML.Window;

namespace sudoku_solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku sudoku = new Sudoku();
            //Screen screen = new Screen(810,603,"Sudoku Solver",Styles.Close);            
            //screen.Game(sudoku.GetBoard,sudoku);
            var board = sudoku.GetBoard;
            sudoku.FindBlanks();
        }
    }
}
