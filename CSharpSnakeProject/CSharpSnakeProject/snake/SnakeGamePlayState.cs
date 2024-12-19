using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mySnake.shared;

namespace mySnake.snake
{
    internal enum SnakeDir
    {
        Up, Down, Left, Right
    }

    internal class SnakeGamePlayState : BaseGameState
    {
        private class Cell
        {
            public int X { get; }
            public int Y { get; }

            public Cell(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private List<Cell> _body = new();
        private SnakeDir currentDir = SnakeDir.Left;
        private Queue<SnakeDir> directionQueue = new(); // Используем очередь для направлений
        private float _timeToMove = 0f;
        private const float MoveInterval = 0.25f; // Интервал движения

        public void SetDirection(SnakeDir dir)
        {
            // Проверка на противоположное направление
            if (!(currentDir == SnakeDir.Up && dir == SnakeDir.Down) &&
                !(currentDir == SnakeDir.Down && dir == SnakeDir.Up) &&
                !(currentDir == SnakeDir.Left && dir == SnakeDir.Right) &&
                !(currentDir == SnakeDir.Right && dir == SnakeDir.Left))
            {
                directionQueue.Enqueue(dir); // Добавляем направление в очередь
            }
        }

        public override void Reset()
        {
            _body.Clear();
            currentDir = SnakeDir.Left;
            _body.Add(new Cell(0, 0));
            _timeToMove = 0f;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            if (_timeToMove <= 0f)
            {
                _timeToMove = MoveInterval;

                // Обновляем текущее направление, если есть новые команды
                if (directionQueue.Count > 0)
                {
                    currentDir = directionQueue.Dequeue();
                }

                var head = _body[0];
                var nextCell = ShiftTo(head, currentDir);

                _body.Insert(0, nextCell);
                Console.WriteLine($"{nextCell.X}, {nextCell.Y}");
                _body.RemoveAt(_body.Count - 1);
            }
        }

        private Cell ShiftTo(Cell from, SnakeDir toDir)
        {
            return toDir switch
            {
                SnakeDir.Up => new Cell(from.X, from.Y + 1),
                SnakeDir.Down => new Cell(from.X, from.Y - 1),
                SnakeDir.Left => new Cell(from.X - 1, from.Y),
                SnakeDir.Right => new Cell(from.X + 1, from.Y),
                _ => from,
            };
        }
    }
}