using System;

namespace sudoku_solver
{
    public class BoardNumber
    {
        private int number;
        public Tuple<int,int> location; //location of the number in a collection
        public int Number{
            get{return number;}
            set{number = value.Equals(10) ? 0:value;}
        }

        public BoardNumber(int number, Tuple<int,int> location){
            this.Number = number;
            this.location = location;
        }
    }
}