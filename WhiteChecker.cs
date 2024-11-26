namespace Checkers
{
    public class WhiteChecker : Checker
    {
        public WhiteChecker()
        {
            Color = "White";
            IsDame = false;
            Symbol = '#';
        }
        public override bool CheckColor(bool color)
        {
            return color;
        }
        public override bool CheckDirection(int y1, int y2)
        {
            return y2 - y1 == 1;
        }

    }
}
