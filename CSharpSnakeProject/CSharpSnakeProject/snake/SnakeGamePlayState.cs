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
        private float _timeToMove = 0f;
        private const float MoveInterval = 0.25f; // Интервал движения
        private float _lastInputTime = 0f; // Время последнего ввода
        private const float InputTimeout = 0.5f; // Таймаут для сброса

        public SnakeGamePlayState()
        {
            // Запускаем поток для обработки ввода
            Thread inputThread = new Thread(HandleInput);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private void HandleInput()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    // Управление WASD
                    case ConsoleKey.W:
                        SetDirection(SnakeDir.Up);
                        break;
                    case ConsoleKey.S:
                        SetDirection(SnakeDir.Down);
                        break;
                    case ConsoleKey.A:
                        SetDirection(SnakeDir.Left);
                        break;
                    case ConsoleKey.D:
                        SetDirection(SnakeDir.Right);
                        break;

                    // Управление стрелочными клавишами
                    case ConsoleKey.UpArrow:
                        SetDirection(SnakeDir.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        SetDirection(SnakeDir.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        SetDirection(SnakeDir.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        SetDirection(SnakeDir.Right);
                        break;
                }
            }
        }

        public void SetDirection(SnakeDir dir)
        {
            _lastInputTime = 0f; // Обновляем время последнего ввода
            // Проверка на противоположное направление
            /*
            if (!(currentDir == SnakeDir.Up && dir == SnakeDir.Down) &&
                !(currentDir == SnakeDir.Down && dir == SnakeDir.Up) &&
                !(currentDir == SnakeDir.Left && dir == SnakeDir.Right) &&
                !(currentDir == SnakeDir.Right && dir == SnakeDir.Left))
            {
                currentDir = dir; // Обновляем текущее направление
            }
            */
            currentDir = dir; // Убрали проверку, чтобы обновлять направление сразу
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
            _lastInputTime += deltaTime; // Увеличиваем время последнего ввода
            if (_lastInputTime >= InputTimeout)
            {
                
            }

            if (_timeToMove <= 0f)
            {
                _timeToMove = MoveInterval;

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