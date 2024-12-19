using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mySnake.shared
{
    internal abstract class BaseGameLogic : ConsoleInput.IArrowListener
    {
        public abstract void OnArrowUp();
        public abstract void OnArrowDown();
        public abstract void OnArrowLeft();
        public abstract void OnArrowRight();

        public abstract void Update(float deltaTime);

        public void InitializeInput(ConsoleInput input)
        {
            input.Subscribe(this);
        }
    }
}