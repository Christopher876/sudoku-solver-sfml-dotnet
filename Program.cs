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
        public static Thread sudokuGame;
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

            Sudoku sudoku = new Sudoku(options.speed,options.difficulty);
            Screen screen = new Screen(810,603,"Sudoku Solver",Styles.Close);            
            Thread game = new Thread(screen.Game);
            sudokuGame = new Thread(() => sudoku.BackTrackingAlgorithm());
            
            game.Start();                      
        }
    }
}
