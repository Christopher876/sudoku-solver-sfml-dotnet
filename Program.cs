using System.Threading;
using SFML.Window;
using CommandLine;

class Options{
    [Option('s',"speed",
        Default = 1,
        HelpText = "Speed that the solver runs at")]
    public int speed{get;set;}

    [Option('n',"new",HelpText="Get a new puzzle from API")]
    public string difficulty{get;set;}
}

namespace sudoku_solver
{
    class Program
    {
        public static Thread sudokuGame{get; private set;}
        static void Main(string[] args)
        {
            Options options = new Options();

            //CommandLine Arguments
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opt =>
                {
                    options.difficulty = opt.difficulty;
                    options.speed = opt.speed;
                });

            //Setup Sudoku Instance and Thread
            Sudoku sudoku = new Sudoku(options.speed,options.difficulty);
            sudokuGame = new Thread(() => sudoku.BackTrackingAlgorithm());
            
            //Setup Screen and Start on Main Thread
            Screen screen = new Screen(810,603,"Sudoku Solver",Styles.Close);            
            screen.Game();
        }
    }
}
