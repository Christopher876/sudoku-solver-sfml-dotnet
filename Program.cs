using System;
using SFML.Window;

namespace sudoku_solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Screen screen = new Screen(800,600,"Sudoku Solver");            
            screen.Loop();
        }
    }
}
