using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace sudoku_solver
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T: ICloneable
        {
                return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
    public class Sudoku
    {
        private int workingNumberBoard = 0;
        private int workingSpaceRow = 0;
        private int workingSpaceColumn = 0;
        private int recursiveLevel = 0;
        private int numberOfBlanks = 0;
        private int solvedBlanks = 0;
        private static List<List<int>> board;
        private List<List<int>> cells = new List<List<int>>();
        private List<List<BoardNumber>> blanks = new List<List<BoardNumber>>();
        private List<List<int>> ParseBoard(){
            List<List<int>> boardNumbers = new List<List<int>>();
            using (StreamReader streamReader = new StreamReader(@"board.json")){
                var json = JObject.Parse(streamReader.ReadToEnd());
                for(int i = 0; i<json.Count;i++){
                    List<int> tempList = new List<int>();
                    var temp = json["row"+i].ToList();
                    foreach(var number in temp){
                        tempList.Add((int)number);
                    }
                    boardNumbers.Add(tempList);                
                }
            }
            board = boardNumbers;
            return boardNumbers;
        }
        
        public static List<List<int>> Board{
            get{
                lock(board){
                    return board;
                }
            }
            set{
                lock(board){
                    board = value;
                }
            }
        }

        public List<List<int>> GetBoard(){
            return ParseBoard();
        }

        public Sudoku(){
            GetBoard();
            FindBlanks();
            FindCells(); 
        }

        //Get and set the locations of what to solve
        private void FindBlanks(){
            int j = 0;
            foreach(var b in board){
                var temp = new List<BoardNumber>();
                for(int i = 0; i<b.Count;i++){
                    if(b[i] == 0){
                        temp.Add(new BoardNumber(b[i],new Tuple<int, int>(j,i)));
                        numberOfBlanks++;
                    }
                }
                this.blanks.Add(new List<BoardNumber>(temp));
                j++;
            }
        }

        private void FindCells(){
            int x = 0;
            cells.Clear();
            for(int i = 0; i<Board.Count;i++){
                List<int> temp = new List<int>();
                if(i % 3 == 0 && i != 0){
                    i = 0;
                    x+=3;
                }
                //Gets the 3 columns
                for(int j = 3*i; j < 3*(i+1);j++){
                    temp.Add(Board[x][j]);
                }
                for(int j = 3*i; j < 3*(i+1);j++){
                    temp.Add(Board[x+1][j]);
                }
                for(int j = 3*i; j < 3*(i+1);j++){
                    temp.Add(Board[x+2][j]);
                }
                cells.Add(new List<int>(temp));
                if(cells.Count.Equals(9))
                    break;
            }
        }

        //Check for the same number in column and row
        public bool CheckForSameNumber(int num,int row, int column){
            int temp = Board[row][column];
            Board[row][column] = 0;

            bool rowCheck = Board[row].All(x => x != num); //Check if selected number is equal to anything in row
            bool columnCheck;
            bool boxCheck = false;
            
            List<int> columnNums = new List<int>();

            //Check if seleced number is equal to anything in same column
            for(int i = 0; i < Board.Count;i++){
                columnNums.Add(Board[i][column]);
            }
            columnCheck = columnNums.All(x => x != num);
            
            //Check the correct cell if the value exists there
            switch(column){
                case int n when (column>=0 && column<=2 && row >= 0 && row <= 2):
                    if(cells[0].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=3 && column<=5 && row >= 0 && row <= 2):
                    if(cells[1].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=6 && column<=8 && row >= 0 && row <= 2):
                    if(cells[2].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=0 && column<=2 && row >= 3 && row <= 5):
                    if(cells[3].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=3 && column<=5 && row >= 3 && row <= 5):
                    if(cells[4].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=6 && column<=8 && row >= 3 && row <= 5):
                    if(cells[5].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=0 && column<=2 && row >= 6 && row <= 8):
                    if(cells[6].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=3 && column<=5 && row >= 6 && row <= 8):
                    if(cells[7].All(x => x != num))
                        boxCheck = true;
                    break;
                case int n when (column>=6 && column<=8 && row >= 6 && row <= 8):
                    if(cells[8].All(x => x != num))
                        boxCheck = true;
                    break;
            }
            Board[row][column] = temp;
            return rowCheck && columnCheck && boxCheck;
        }

        /*private void SetBoardBlanks(){
            for(int i = 0; i < board.Count;i++){
                for(int j = 0; j < blanks[i].Count;j++){
                    board[blanks[i][j].location.Item1][blanks[i][j].location.Item2] = blanks[i][j].Number; 
                }
            }
        }*/

        private bool IncrementNumber(int row, int column){
            if(CheckForSameNumber(++Board[row][column],row,column)){
                return true;
            }
            return false;       
        }

        public void BackTrackingAlgorithm(){
            int i = 0;
            while(true){
                try{
                    for(int j = 0; j < blanks[i].Count;j++){
                        Thread.Sleep(250);
                        for(int k = 0; k < 20;k++){
                            if(IncrementNumber(blanks[i][j].location.Item1,blanks[i][j].location.Item2)){
                                break;
                            }
                        }
                    }
                    i++;
                    FindCells();
                    if(i >= 45)
                        break;

                //This is when we are done
                }catch (ArgumentOutOfRangeException){
                    Console.WriteLine("We are done!");
                    break;
                }finally{
                    
                }
            }
        }
    }
}