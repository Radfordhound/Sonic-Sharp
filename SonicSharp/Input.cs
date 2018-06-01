using Microsoft.Xna.Framework.Input;

namespace SonicSharp
{
    public class Input
    {
        // Variables/Constants
        public Keys Key;
        public Buttons Button;

        public enum Devices
        {
            Keyboard, GamePad1, GamePad2, GamePad3, GamePad4
        }

        // Constructors
        public Input() { }
        public Input(Keys key, Buttons button)
        {
            Key = key;
            Button = button;
        }

        // Methods
        public virtual bool IsDown(Devices device)
        {
            if (device == Devices.GamePad1)
                return GameWindow.PadStates[0].IsButtonDown(Button);
            else if (device == Devices.GamePad2)
                return GameWindow.PadStates[1].IsButtonDown(Button);
            else if (device == Devices.GamePad3)
                return GameWindow.PadStates[2].IsButtonDown(Button);
            else if (device == Devices.GamePad4)
                return GameWindow.PadStates[3].IsButtonDown(Button);
            else
                return GameWindow.KeyState.IsKeyDown(Key);
        }

        public virtual bool IsUp(Devices device)
        {
            if (device == Devices.GamePad1)
                return GameWindow.PadStates[0].IsButtonUp(Button);
            else if (device == Devices.GamePad2)
                return GameWindow.PadStates[1].IsButtonUp(Button);
            else if (device == Devices.GamePad3)
                return GameWindow.PadStates[2].IsButtonUp(Button);
            else if (device == Devices.GamePad4)
                return GameWindow.PadStates[3].IsButtonUp(Button);
            else
                return GameWindow.KeyState.IsKeyUp(Key);
        }

        public virtual bool JustPressed(Devices device)
        {
            if (device == Devices.GamePad1)
            {
                return GameWindow.PadStates[0].IsButtonDown(Button) &&
                    !GameWindow.PrevPadStates[0].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad2)
            {
                return GameWindow.PadStates[1].IsButtonDown(Button) &&
                    !GameWindow.PrevPadStates[1].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad3)
            {
                return GameWindow.PadStates[2].IsButtonDown(Button) &&
                    !GameWindow.PrevPadStates[2].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad4)
            {
                return GameWindow.PadStates[3].IsButtonDown(Button) &&
                    !GameWindow.PrevPadStates[3].IsButtonDown(Button);
            }
            else
            {
                return GameWindow.KeyState.IsKeyDown(Key) &&
                    !GameWindow.PrevKeyState.IsKeyDown(Key);
            }
        }

        public virtual bool JustReleased(Devices device)
        {
            if (device == Devices.GamePad1)
            {
                return !GameWindow.PadStates[0].IsButtonDown(Button) &&
                    GameWindow.PrevPadStates[0].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad2)
            {
                return !GameWindow.PadStates[1].IsButtonDown(Button) &&
                    GameWindow.PrevPadStates[1].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad3)
            {
                return !GameWindow.PadStates[2].IsButtonDown(Button) &&
                    GameWindow.PrevPadStates[2].IsButtonDown(Button);
            }
            else if (device == Devices.GamePad4)
            {
                return !GameWindow.PadStates[3].IsButtonDown(Button) &&
                    GameWindow.PrevPadStates[3].IsButtonDown(Button);
            }
            else
            {
                return !GameWindow.KeyState.IsKeyDown(Key) &&
                    GameWindow.PrevKeyState.IsKeyDown(Key);
            }
        }

        // TODO
    }
}