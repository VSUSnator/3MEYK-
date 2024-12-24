using mySnake.shared;
using mySnake.snake;
using Shared;
using System.Diagnostics;

internal class Program
{
    const float targetFrameTime = 1f / 60f;

    static void Main()
    {
        int screenWidth = 80; // Задайте ширину экрана
        int screenHeight = 20; // Задайте высоту экрана

        var gameLogic = new SnakeGameLogic(screenWidth, screenHeight); // Передайте параметры в конструктор

        var palette = gameLogic.CreatePalette();

        var renderer0 = new ConsoleRenderer(palette);
        var renderer1 = new ConsoleRenderer(palette);

        var input = new ConsoleInput();
        gameLogic.InitializeInput(input);
        gameLogic.GotoGameplay();

        var prevRenderer = renderer0;
        var currRenderer = renderer1;

        var lastFrameTime = DateTime.Now;
        while (true)
        {
            input.Update();
            gameLogic.CheckExit(); // Проверка на выход из игры

            var frameStartTime = DateTime.Now;
            float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;
            gameLogic.Update(deltaTime);
            lastFrameTime = frameStartTime;

            gameLogic.DrawNewState(deltaTime, currRenderer);
            lastFrameTime = frameStartTime;

            if (!currRenderer.Equals(prevRenderer)) currRenderer.Render();

            var tmp = prevRenderer;
            prevRenderer = currRenderer;
            currRenderer = tmp;
            currRenderer.Clear();

            var nextFrameTime = frameStartTime + TimeSpan.FromSeconds(targetFrameTime);
            var endFrameTime = DateTime.Now;
            if (nextFrameTime > endFrameTime)
            {
                Thread.Sleep((int)(nextFrameTime - endFrameTime).TotalMilliseconds);
            }
        }
    }
}