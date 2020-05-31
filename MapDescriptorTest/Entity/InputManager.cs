using Microsoft.Xna.Framework.Input;
namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// Manager that controls and updates all user input
    /// </summary>
    public static class InputManager
    {
        /// <summary>
        /// Current Keyboard State, updates each frame
        /// </summary>
        public static KeyboardState KeyboardState { get; private set; }
        /// <summary>
        /// Current Mouse State, updates each frame
        /// </summary>
        public static MouseState MouseState { get; private set; }
        /// <summary>
        /// Stores whether the window is active or not, updated each frame
        /// </summary>
        public static bool IsActive { get; private set; }
        /// <summary>
        /// Last key pressed on keyboard
        /// </summary>
        public static KeyboardState LastKeyboardState { get; private set; }

        /// <summary>
        /// Updates Mouse, Keyboard, and IsActive states each frame
        /// </summary>
        /// <param name="IsActive">IsActive state of the window</param>
        public static void Update(bool IsActive)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            InputManager.IsActive = IsActive;
        }
    }
}
