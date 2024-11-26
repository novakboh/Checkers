namespace Checkers
{
    public class Cell
    {
        private bool Occupied;
        private Checker Checker;
        public Cell(bool occupied, Checker checker = null)
        { 
            this.Occupied = occupied;
            this.Checker = checker;

        }
        public void Occupy(Checker checker)
        {
            this.Occupied = true;
            this.Checker = checker;
        }
        public Checker Deoccupy()
        {
            Checker checker = this.Checker;
            this.Occupied = false;
            this.Checker = null;
            return checker;
        }
        public bool IsOccupied() 
        { 
            return this.Occupied; 
        }
        public Checker GetChecker()
        {
            return this.Checker;
        }
        public bool CheckColor(bool color)
        {
            return this.Checker.CheckColor(color);
        }
        public bool CheckDirection(int y1, int y2)
        {
            return this.Checker.CheckDirection(y1, y2);
        }
    }
}
