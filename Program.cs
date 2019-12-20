using System;
using System.Threading;
using SFML.Window;

namespace sudoku_solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku sudoku = new Sudoku();
            Screen screen = new Screen(810,603,"Sudoku Solver",Styles.Close);            
            Thread game = new Thread(screen.Game);
            Thread sudokuGame = new Thread(sudoku.BackTrackingAlgorithm);

            game.Start();
            sudokuGame.Start();                        
        }
    }
}
