using mySnake.shared;
using Shared;
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
        private bool newGamePending = false;
        private int currLevel = 0;
        private ShowTextState showTextState = new(2f);


        public void GotoGameplay()
        {
            gameplayState.level = currLevel;
            gameplayState.fieldWidth = screenWidth;
            gameplayState.fieldHeight = screenHeight;
            ChangeState(gameplayState);
            gameplayState.Reset();
        }
        private void GotoGameOver()
        {
            currLevel = 0;
            newGamePending = true;
            showTextState.text = $"Game Over!";
            ChangeState(showTextState);
        }
        private void GotoNextLevel()
        {
            currLevel++;
            newGamePending = false;
            showTextState.text = $"Level {currLevel}";
            ChangeState(showTextState);
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
            if (currentState != null && !currentState.IsDone())
                return;
            if (currentState == null || currentState == gameplayState && !gameplayState.gameOver)
            {
                GotoNextLevel();
            }
            else if (currentState == gameplayState && gameplayState.gameOver)
            {
                GotoGameOver();
            }
            else if (currentState != gameplayState && newGamePending)
            {
                GotoNextLevel();
            }
            else if (currentState != gameplayState && !newGamePending)
            {
                GotoGameplay();
            }
        }

        public override ConsoleColor[] CreatePalette()
        {
            return
            [
                ConsoleColor.Green,
                ConsoleColor.Red,
                ConsoleColor.White,
                ConsoleColor.Blue,
            ];
        }
    }
}
