using mySnake.shared;
using mySnake.snake;
using Shared;
using System.Diagnostics;

internal class Program 
{
    const float targetFrameTime = 1f / 60f;

    static void Main()
    {
        var gameLogic = new SnakeGameLogic();

        var palette = gameLogic.CreatePalette();

        var renderer0 = new ConsoleRenderer(palette);
        var renderer1 = new ConsoleRenderer(palette);

        var input = new ConsoleInput();
        gameLogic.InitializeInput(input);

        var prevRenderer = renderer0;
        var currRenderer = renderer1;
        var lastFrameTime = DateTime.Now;
        while (true)
        {
            var frameStartTime = DateTime.Now;
            float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;
            input.Update();

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
