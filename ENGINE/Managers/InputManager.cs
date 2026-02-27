using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.ENGINE.Managers
{
    abstract class InputManager
    {
        protected KeyboardState keyboardState;
        protected KeyboardState previousKeyboardState;
        protected MouseState mouseState;

        public void ProcessInput(KeyboardState keyboardState, MouseState mouseState)
        {
            previousKeyboardState = this.keyboardState;
            this.keyboardState = keyboardState;
            this.mouseState = mouseState;
            ProcessInput();
        }

        protected abstract void ProcessInput();

        public bool IsKeyPressed(Keys key)
        {
            return keyboardState != null && keyboardState.IsKeyDown(key);
        }

        public bool IsKeyTapped(Keys key)
        {
            return keyboardState != null && previousKeyboardState != null && keyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }
    }
}