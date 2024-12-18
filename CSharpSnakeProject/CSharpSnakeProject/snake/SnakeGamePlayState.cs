using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using mySnake.shared;
using Shared;

namespace mySnake.snake
{
    internal enum SnakeDir
    {
        Up, Down, Left, Right
    }
    internal class SnakeGamePlayState : BaseGameState
    {
        const char squareSymbol = '■';
        const char circleSymbol = '0';

        private struct Cell
        {
            public int x; public int y;

            public Cell(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }
        public bool gameOver { get; private set; }
        public bool hasWon { get; private set; }
        public int level { get; set; }


        private List<Cell> _body = new();
        private SnakeDir currentDir = SnakeDir.Left;      
        private float _timeToMove = 0f;
        private Cell _apple = new();
        private Random _random = new();


        public void SetDirection(SnakeDir dir)
        {
            currentDir = dir;
        }        

        public override void Reset()
        {
            _body.Clear();
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            gameOver = false;
            hasWon = false;
            currentDir = SnakeDir.Left;
            _body.Add(new(middleX + 3, middleY));
            _apple = new(middleX - 3, middleY);
            _timeToMove = 0f;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            if (_timeToMove > 0f || gameOver)
                return;

            _timeToMove = 1f / (4f + level);
            var head = _body[0];
            var nextCell = ShiftTo(head, currentDir);
            if (nextCell.Equals(_apple))
            {
                _body.Insert(0, _apple);
                hasWon = _body.Count >= level + 3;
                GenerateApple();
                return;
            }
            if (nextCell.x < 0 || nextCell.y < 0 || nextCell.x >= fieldWidth || nextCell.y >= fieldHeight)
            {
                gameOver = true;
                return;
            }
            _body.RemoveAt(_body.Count - 1);
            _body.Insert(0, nextCell);
        }
       
        private Cell ShiftTo(Cell from, SnakeDir toDir)
        {
            switch (toDir)
            {
                case SnakeDir.Up:
                    return new Cell(from.x, from.y - 1);
                case SnakeDir.Down:
                    return new Cell(from.x, from.y + 1);
                case SnakeDir.Left:
                    return new Cell(from.x - 1, from.y);
                case SnakeDir.Right:
                    return new Cell(from.x + 1, from.y);
            }

            return from;
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            renderer.DrawString($"Level: {level}", 3, 1, ConsoleColor.White);
            renderer.DrawString($"Score: {_body.Count - 1}", 3, 2, ConsoleColor.White);

            foreach (Cell cell in _body)
            {
                renderer.SetPixel(cell.x, cell.y, squareSymbol, 3);
            }
            renderer.SetPixel(_apple.x, _apple.y, circleSymbol, 1);
        }
        private void GenerateApple()
        {
            Cell cell;
            cell.x = _random.Next(fieldWidth);
            cell.y = _random.Next(fieldHeight);

            if (_body[0].Equals(cell))
            {
                if (cell.y > fieldHeight / 2)
                    cell.y--;
                else
                    cell.y++;
            }

            _apple = cell;
        }

        public override bool IsDone()
        {
            return gameOver || hasWon;
        }
    }
}
