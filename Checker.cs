namespace Checkers
{
    public abstract class Checker
    {
        public string Color;
        public bool IsDame;
        public char Symbol;
        public string GetColor() 
        { 
            return Color;
        }
        public void BecomeDame()
        {
            this.IsDame = true;
            this.Symbol = '@';
        }
        public bool GetIsDame()
        { 
            return this.IsDame; 
        }
        public char GetSymbol()
        {
            return this.Symbol;
        }
        public abstract bool CheckColor(bool color);
        public abstract bool CheckDirection(int y1, int y2);
    }
}
