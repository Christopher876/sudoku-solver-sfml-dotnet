using System;
using SFML.Window;

namespace sudoku_solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Screen screen = new Screen(900,603,"Sudoku Solver",Styles.Close);            
            screen.Loop();
        }
    }
}
