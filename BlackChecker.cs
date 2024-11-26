namespace Checkers
{
    public class BlackChecker : Checker
    {
        public BlackChecker()
        {
            Color = "Black";
            IsDame = false;
            Symbol = '#';
        }
        public override bool CheckColor(bool color)
        {
            return !color;
        }
        public override bool CheckDirection(int y1, int y2)
        {
            return y1 - y2 == 1;
        }
    }
}
