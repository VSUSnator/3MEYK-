using mySnake.shared;
using mySnake.snake;
using System.Diagnostics;


internal class Program
{
    static void Main()
    {
        var gameLogic = new SnakeGameLogic();
        var input = new ConsoleInput();
        gameLogic.InitializeInput(input);
        gameLogic.GotoGameplay();

        var lastFrameTime = DateTime.Now;
        while (true)
        {
            input.Update();
            gameLogic.CheckExit(); // Проверка на выход из игры

            var frameStartTime = DateTime.Now;
            float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;
            gameLogic.Update(deltaTime);
            lastFrameTime = frameStartTime;
        }
    }
}
