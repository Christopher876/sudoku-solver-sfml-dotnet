using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.Http;
using System.Net;
using System;

namespace sudoku_solver
{
    public class Sudoku
    {
        #region Variables
        private int SPEED = 1; //speed of how fast it will be solving
        private bool DELAY = false;
        public static bool isSolving = false;
        private const int SIZE = 9;
        #pragma warning disable CS0414
        private static List<List<int>> board;
        private List<List<int>> cells = new List<List<int>>();
        #endregion

        public Sudoku(int speed = 0, string difficulty = ""){
            DELAY = speed.Equals(0) ? false:true; //No delay if speed is 0
            SPEED = speed;

            if(difficulty != String.Empty && difficulty != null){
                GetBoard(difficulty);
            }else{
                GetBoard();
            }
            FindCells();
        }
        private List<List<int>> ParseWebBoard(JObject webBoard){
            List<List<int>> boardNumbers = new List<List<int>>();
            for(int i = 0; i<webBoard["board"].Count();i++){
                List<int> tempList = new List<int>();
                var temp = webBoard["board"][i].ToList();
                foreach(var number in temp){
                    tempList.Add((int)number);
                }
                boardNumbers.Add(tempList);                
            }
            
            board = boardNumbers;
            return boardNumbers;
        }

        //Get a Sudoku Board from public api
        public List<List<int>> GetBoard(string difficulty){
            string url = $"https://sugoku.herokuapp.com/board?difficulty={difficulty}";
            string html = ""; //sudoku board from internet

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();             
            }
            return ParseWebBoard(JObject.Parse(html));
        } 
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

        //Find one blank spot at a time
        private List<int> FindUnassigned(int row, int col)
        {
            int numunassign = 0;
            //Search within our rows
            for(int i=0;i<SIZE;i++)
            {
                //Search within our columns
                for(int j=0;j<SIZE;j++)
                {
                    //cell is unassigned
                    if(Board[i][j] == 0)
                    {
                        //there is one or more unassigned cells
                        numunassign = 1;
                        List<int> unassigned = new List<int>(){numunassign, i, j};
                        return unassigned;
                    }
                }
            }
            //If there are no zeroes found from the loop then we are done
            List<int> noUnassigned = new List<int>(){numunassign, -1, -1};
            return noUnassigned;
        }

        //Check for the same number in column and row and matrix
        private bool CheckForSameNumber(int num,int row, int column){
            //Console.WriteLine($"row={row},column={column},value={num}");
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
            return rowCheck && columnCheck && boxCheck;
        }
        public bool BackTrackingAlgorithm(){
            int row=0;
            int col=0;
            List<int> unassigned = FindUnassigned(row, col);
            //if all cells are assigned then the sudoku is already solved
            if(unassigned[0] == 0)
                return true;
            
            //number between 1 to 9
            row = unassigned[1]; //Get the row of the unassigned
            col = unassigned[2]; //Get the column of the unassigned
            
            //Assign between numbers 1 and 9 to find a correct number
            //When an answer is false then the program goes back up and keeps on incrementing i until it hits 9.
            //If it hits 9 then it exits the loop and then returns false.
            //It does this until it back tracks to a value that it can increment and keep, then it calls itself to get the next value
            for(int i=1;i<=SIZE;i++)
            {
                if(DELAY)
                    Thread.Sleep(SPEED);
                if(CheckForSameNumber(i, row, col))
                {
                    FindCells();
                    Board[row][col] = i;
                    if(BackTrackingAlgorithm())
                        return true;
                    //if we can't proceed with this solution
                    //reassign the cell to 0
                    Board[row][col]=0; //Recursive, if one recursive loop is false, then all before that can't increment to another value becomes false
                    FindCells();
                }
            }
        return false;
        }
    }   
}