namespace sudoku_solver
{
    public class BoardNumber
    {
        public int location; //location of the number in a collection
        public int number;

        public BoardNumber(int number, int location){
            this.number = number;
            this.location = location;
        }
    }
}