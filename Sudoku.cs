using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sudoku_solver
{
    public class Sudoku
    {
        List<List<int>> board;
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
            this.board = boardNumbers;
            return boardNumbers;
        }
        
        public List<List<int>> GetBoard{
            get{
                return ParseBoard();
            }
        }

        //Get and set the locations of what to solve
        public void FindBlanks(){
            foreach(var b in board){
                var temp = new List<BoardNumber>();
                for(int i = 0; i<b.Count;i++){
                    if(b[i] == 0)
                        temp.Add(new BoardNumber(b[i],i));
                }
                this.blanks.Add(new List<BoardNumber>(temp));
            }
        }

        //Check for the same number in column and row
        private bool CheckForSameNumber(int num,int row, int column){
            bool rowCheck = board[row].All(x => x != num); //Check if selected number is equal to anything in row
            bool columnCheck;
            
            List<int> columnNums = new List<int>();
            //Check if seleced number is equal to anything in same column
            for(int i = 0; i < board.Count;i++){
                columnNums.Add(board[i][column]);
            }

            columnCheck = columnNums.All(x => x != num);
            return rowCheck && columnCheck;
        }
        public List<List<int>> BackTrackingAlgothrim(){
            //board[new Random().Next(0,9)][new Random().Next(0,9)] = new Random().Next(0,9);
            return board;
        }
        
    }
}