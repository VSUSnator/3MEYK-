using mySnake.shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mySnake.snake
{
    internal class SnakeGameLogic : BaseGameLogic
    {
        private int screenWidth; // Определите ширину экрана
        private int screenHeight; // Определите высоту экрана

        private SnakeGamePlayState gameplayState;

        public SnakeGameLogic(int width, int height) // Добавьте конструктор для инициализации размеров
        {
            screenWidth = width;
            screenHeight = height;

            gameplayState = new SnakeGamePlayState(screenWidth, screenHeight); // Создайте объект с параметрами
        }

        public void GotoGameplay()
        {
            gameplayState.fieldWidth = screenWidth;
            gameplayState.fieldHeight = screenHeight;
            ChangeState(gameplayState);
            gameplayState.Reset();
        }

        public override void OnArrowUp()
        {
            if (currentState != gameplayState) return;
            gameplayState.SetDirection(SnakeDir.Up);
        }

        public override void OnArrowDown()
        {
            if (currentState != gameplayState) return;
            gameplayState.SetDirection(SnakeDir.Down);
        }

        public override void OnArrowLeft()
        {
            if (currentState != gameplayState) return;
            gameplayState.SetDirection(SnakeDir.Left);
        }

        public override void OnArrowRight()
        {
            if (currentState != gameplayState) return;
            gameplayState.SetDirection(SnakeDir.Right);
        }

        public override void Update(float deltaTime)
        {
            gameplayState.Update(deltaTime);
        }

        public void CheckExit()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0); // Выход из игры
                }
            }

            if (currentState != gameplayState)
            {
                GotoGameplay();
            }
        }

        public override ConsoleColor[] CreatePalette()
        {
            return new ConsoleColor[]
            {
                ConsoleColor.Green,
                ConsoleColor.Red,
                ConsoleColor.White,
                ConsoleColor.Blue,
            };
        }
    }
}