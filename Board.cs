namespace Checkers
{
    public class Board
    {
        private Cell[,] Cells;
        private int ActiveWhite;
        private int ActiveBlack;
        private bool WhiteTurn;
        private int MovesWithoutAttack;
        public Board()
        {
            this.WhiteTurn = true;
            this.ActiveWhite = 12;
            this.ActiveBlack = 12;
            this.Cells = new Cell[8, 8];
            this.MovesWithoutAttack = 0;
            int c = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3 && (i + j) % 2 == 0)
                    {
                        this.Cells[i, j] = new Cell(true, new WhiteChecker());
                        c++;
                    }
                    else if (i > 4 && (i + j) % 2 == 0)
                    {
                        this.Cells[i, j] = new Cell(true, new BlackChecker());
                        c--;
                    }
                    else
                    {
                        this.Cells[i, j] = new Cell(false);
                    }
                }
            }
            PrintBoard();
        }
        public void Game()
        {
            while (true)
            {
                if (this.CanMove())
                    this.MoveChecker();
                if (this.ActiveWhite == 0)
                {
                    Console.WriteLine("Black won!");
                    break;
                }
                else if (this.ActiveBlack == 0)
                {
                    Console.WriteLine("White won!");
                    break;
                }
                else if (this.MovesWithoutAttack == 30)
                {
                    Console.WriteLine("Draw!");
                    break;
                }
            }
        }
        private void MoveChecker()
        {
            if (this.WhiteTurn) Console.WriteLine("White turn");
            else Console.WriteLine("Black turn");
            while (true)
            {
                Console.WriteLine("Enter coordinates of the checker (x, y): ");
                string surrender = Console.ReadLine();
                if (surrender == "Surrender")
                {
                    if (this.WhiteTurn) this.ActiveWhite = 0;
                    else this.ActiveBlack = 0;
                    break;
                }
                int x1 = int.Parse(surrender);
                int y1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter coordinates where you want to move it (x, y): ");
                int x2 = int.Parse(Console.ReadLine());
                int y2 = int.Parse(Console.ReadLine());
                if (x1 > 0 && x1 < 9 && y1 > 0 && y1 < 9 && x2 > 0 && x2 < 9 && y2 > 0 && y2 < 9)
                {
                    x1--;
                    y1--;
                    x2--;
                    y2--;
                    if (this.Cells[y1, x1].IsOccupied())
                        if (this.Cells[y1, x1].CheckColor(this.WhiteTurn) && !this.Cells[y2, x2].IsOccupied())
                            if (MustAttack())
                            {
                                int dx = x2 - x1;
                                int dy = y2 - y1;
                                if (this.Cells[y1, x1].GetChecker().GetIsDame() && Math.Abs(dx) == Math.Abs(dy))
                                {
                                    int targetX = -1;
                                    int targetY = -1;
                                    int coefX = dx / Math.Abs(dx);
                                    int coefY = dy / Math.Abs(dy);
                                    int x = x1 + coefX;
                                    int y = y1 + coefY;
                                    while (x != x2 && y != y2)
                                    {
                                        if (this.Cells[y, x].IsOccupied())
                                            if (targetX == -1)
                                            {
                                                if (this.Cells[y, x].CheckColor(this.WhiteTurn)) break;
                                                else
                                                {
                                                    targetX = x;
                                                    targetY = y;
                                                }
                                            }
                                            else
                                            {
                                                targetX = -1;
                                                break;
                                            }
                                        x += coefX;
                                        y += coefY;
                                    }
                                    if (targetX > 0)
                                    {
                                        Checker checker = this.Cells[y1, x1].Deoccupy();
                                        this.Cells[y2, x2].Occupy(checker);
                                        this.Cells[targetY, targetX].Deoccupy();
                                        if (this.WhiteTurn) this.ActiveBlack--;
                                        else this.ActiveWhite--;
                                        this.MovesWithoutAttack = 0;
                                        PrintBoard();
                                        if (MustAttack(x2, y2)) continue;
                                        this.WhiteTurn = this.WhiteTurn ? false : true;
                                        break;
                                    }
                                }
                                else if ((dx == 2 || dx == -2) && (dy == 2 || dy == -2) && this.Cells[y1 + dy / 2, x1 + dx / 2].IsOccupied())
                                    if (!this.Cells[y1 + dy / 2, x1 + dx / 2].CheckColor(this.WhiteTurn))
                                    {
                                        Checker checker = this.Cells[y1, x1].Deoccupy();
                                        if ((this.WhiteTurn && y2 == 7) || (!this.WhiteTurn && y2 == 0))
                                            checker.BecomeDame();
                                        this.Cells[y2, x2].Occupy(checker);
                                        this.Cells[y1 + dy / 2, x1 + dx / 2].Deoccupy();
                                        if (this.WhiteTurn) this.ActiveBlack--;
                                        else this.ActiveWhite--;
                                        this.MovesWithoutAttack = 0;
                                        PrintBoard();
                                        if (MustAttack(x2, y2)) continue;
                                        this.WhiteTurn = this.WhiteTurn ? false : true;
                                        break;
                                    }
                            }
                            else if (x2 - x1 == 1 || x1 - x2 == 1)
                                if (this.Cells[y1, x1].CheckDirection(y1, y2))
                                {
                                    Checker checker = this.Cells[y1, x1].Deoccupy();
                                    if ((this.WhiteTurn && y2 == 7) || (!this.WhiteTurn && y2 == 0))
                                        checker.BecomeDame();
                                    this.Cells[y2, x2].Occupy(checker);
                                    this.MovesWithoutAttack++;
                                    PrintBoard();
                                    this.WhiteTurn = this.WhiteTurn ? false : true;
                                    break;
                                }
                }
                Console.WriteLine("Can`t make this move");
            }
        }
        private bool MustAttack()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = y % 2; x < 8; x += 2)
                {
                    if (this.Cells[y, x].IsOccupied())
                    {
                        if (this.Cells[y, x].CheckColor(this.WhiteTurn))
                        {
                            int coefX, coefY;
                            if (this.Cells[y, x].GetChecker().GetIsDame())
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i < 2)
                                        if (x < 2) continue;
                                        else coefX = -1;
                                    else
                                        if (x > 5) continue;
                                    else coefX = 1;
                                    if (i % 2 == 0)
                                        if (y < 2) continue;
                                        else coefY = -1;
                                    else
                                        if (y > 5) continue;
                                    else coefY = 1;
                                    int xd = x + coefX;
                                    int yd = y + coefY;
                                    while (xd >= 0 && xd <= 7 && yd >= 0 && yd <= 7)
                                    {
                                        if (this.Cells[yd, xd].IsOccupied())
                                            if (this.Cells[yd, xd].CheckColor(this.WhiteTurn)) break;
                                            else if (this.Cells[yd + coefY, xd + coefX].IsOccupied()) break;
                                            else return true;
                                        xd += coefX;
                                        yd += coefY;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i < 2)
                                        if (x < 2) continue;
                                        else coefX = -1;
                                    else
                                        if (x > 5) continue;
                                    else coefX = 1;
                                    if (i % 2 == 0)
                                        if (y < 2) continue;
                                        else coefY = -1;
                                    else
                                        if (y > 5) continue;
                                    else coefY = 1;
                                    if (this.Cells[y + coefY, x + coefX].IsOccupied() && !this.Cells[y + coefY, x + coefX].CheckColor(this.WhiteTurn) && !this.Cells[y + 2 * coefY, x + 2 * coefX].IsOccupied())
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool MustAttack(int x, int y)
        {
            int coefX, coefY;
            if (this.Cells[y, x].GetChecker().GetIsDame())
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < 2)
                        if (x < 2) continue;
                        else coefX = -1;
                    else
                        if (x > 5) continue;
                    else coefX = 1;
                    if (i % 2 == 0)
                        if (y < 2) continue;
                        else coefY = -1;
                    else
                        if (y > 5) continue;
                    else coefY = 1;
                    int xd = x + coefX;
                    int yd = y + coefY;
                    while (xd >= 0 && xd <= 7 && yd >= 0 && yd <= 7)
                    {
                        if (this.Cells[yd, xd].IsOccupied())
                            if (this.Cells[yd, xd].CheckColor(this.WhiteTurn)) break;
                            else if (this.Cells[yd + coefY, xd + coefX].IsOccupied()) break;
                            else return true;
                        xd += coefX;
                        yd += coefY;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i < 2)
                        if (x < 2) continue;
                        else coefX = -1;
                    else
                        if (x > 5) continue;
                    else coefX = 1;
                    if (i % 2 == 0)
                        if (y < 2) continue;
                        else coefY = -1;
                    else
                        if (y > 5) continue;
                    else coefY = 1;
                    if (this.Cells[y + coefY, x + coefX].IsOccupied() && !this.Cells[y + coefY, x + coefX].CheckColor(this.WhiteTurn) && !this.Cells[y + 2 * coefY, x + 2 * coefX].IsOccupied())
                        return true;
                }
            }
            return false;
        }
        private bool CanMove()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = y % 2; x < 8; x += 2)
                {
                    if (this.Cells[y, x].IsOccupied())
                    {
                        if (this.Cells[y, x].CheckColor(this.WhiteTurn))
                        {
                            int coefX, coefY;
                            if (this.Cells[y, x].GetChecker().GetIsDame())
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i < 2)
                                        if (x < 2) continue;
                                        else coefX = -1;
                                    else
                                        if (x > 5) continue;
                                    else coefX = 1;
                                    if (i % 2 == 0)
                                        if (y < 2) continue;
                                        else coefY = -1;
                                    else
                                        if (y > 5) continue;
                                    else coefY = 1;
                                    int xd = x + coefX;
                                    int yd = y + coefY;
                                    while (xd >= 0 && xd <= 7 && yd >= 0 && yd <= 7)
                                    {
                                        if (this.Cells[yd, xd].IsOccupied())
                                            if (this.Cells[yd, xd].CheckColor(this.WhiteTurn)) break;
                                            else if (this.Cells[yd + coefY, xd + coefX].IsOccupied()) break;
                                            else return true;
                                        else return true;
                                        xd += coefX;
                                        yd += coefY;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i < 2)
                                        if (x < 2) continue;
                                        else coefX = -1;
                                    else
                                        if (x > 5) continue;
                                    else coefX = 1;
                                    if (i % 2 == 0)
                                        if (y < 2) continue;
                                        else coefY = -1;
                                    else
                                        if (y > 5) continue;
                                    else coefY = 1;
                                    if (!this.Cells[y + coefY, x + coefX].IsOccupied() || (this.Cells[y + coefY, x + coefX].IsOccupied() && !this.Cells[y + coefY, x + coefX].CheckColor(this.WhiteTurn) && !this.Cells[y + 2 * coefY, x + 2 * coefX].IsOccupied()))
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            if (this.WhiteTurn) this.ActiveWhite = 0;
            else this.ActiveBlack = 0;
            return false;
        }
        public void PrintBoard()
        {
            for (int i = 7; i >= 0; i--)
            {
                Console.Write($"{i + 1} ");
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        if (this.Cells[i, j].IsOccupied())
                        {
                            if (this.Cells[i, j].GetChecker().GetColor() == "Black") Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($" {this.Cells[i, j].GetChecker().GetSymbol()} ");
                        }
                        else Console.Write("   ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write("   ");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.WriteLine("   1  2  3  4  5  6  7  8");
        }
    }
}
