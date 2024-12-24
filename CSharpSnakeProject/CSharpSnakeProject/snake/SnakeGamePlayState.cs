using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; // Добавлено
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
        private class Cell
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

        private List<Cell> _body = new();
        private SnakeDir currentDir = SnakeDir.Left;

        private float _timeToMove = 0f;

        private const float MoveInterval = 0.25f; // Интервал движения
        private float _lastInputTime = 0f; // Время последнего ввода
        private const float InputTimeout = 0.5f; // Таймаут для сброса

        public SnakeGamePlayState(int width, int height) // Изменено: принимаем размеры поля
        {
            fieldWidth = width;
            fieldHeight = height;

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

                    // Выход из игры
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void SetDirection(SnakeDir dir)
        {
            _lastInputTime = 0f; // Обновляем время последнего ввода
            // Проверка на противоположное направление
            //if (!(currentDir == SnakeDir.Up && dir == SnakeDir.Down) &&
            //    !(currentDir == SnakeDir.Down && dir == SnakeDir.Up) &&
            //    !(currentDir == SnakeDir.Left && dir == SnakeDir.Right) &&
            //    !(currentDir == SnakeDir.Right && dir == SnakeDir.Left))
            {
                currentDir = dir; // Обновляем текущее направление
            }
        }

        public override void Reset()
        {
            _body.Clear();
            var middleY = fieldHeight / 2;
            var middleX = fieldWidth / 2;
            currentDir = SnakeDir.Left;
            _body.Add(new Cell(middleX + 3, middleY));
            _timeToMove = 0f;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;
            _lastInputTime += deltaTime; // Увеличиваем время последнего ввода
            if (_timeToMove > 0f)
                return;

            _timeToMove = MoveInterval;
            var head = _body[0];
            var nextCell = ShiftTo(head, currentDir);

            _body.Insert(0, nextCell); // Добавляем новую ячейку в тело змеи
            //Console.WriteLine($"{nextCell.x}, {nextCell.y}"); // координат змейки

            if (_lastInputTime >= InputTimeout)
            {
                // Логика сброса или обработки таймаута
            }

            _body.RemoveAt(_body.Count - 1); // Удаляем последнюю ячейку
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
            foreach (Cell cell in _body)
            {
                renderer.SetPixel(cell.x, cell.y, squareSymbol, 3);
            }
        }
    }
}