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
        private SnakeGamePlayState gameplayState = new SnakeGamePlayState();

        public void GotoGameplay()
        {
            gameplayState.Reset();
        }

        public override void OnArrowUp()
        {
            gameplayState.SetDirection(SnakeDir.Up);
        }

        public override void OnArrowDown()
        {
            gameplayState.SetDirection(SnakeDir.Down);
        }

        public override void OnArrowLeft()
        {
            gameplayState.SetDirection(SnakeDir.Left);
        }

        public override void OnArrowRight()
        {
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
        }
    }
}