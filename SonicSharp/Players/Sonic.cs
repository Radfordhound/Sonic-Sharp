using Microsoft.Xna.Framework;
using System.IO;

namespace SonicSharp.Players
{
    public class Sonic : Player
    {
        // Constructors
        public Sonic(Input.Devices inputDevice, Color color) : this()
        {
            Color = color;
            InputDevice = inputDevice;
        }

        public Sonic()
        {
            // Load Sprites
            var origin = new Vector2(OriginX, OriginY);
            var spinOrigin = new Vector2(SpinOriginX, SpinOriginY);
            var pth = Path.Combine(Sprite.SpritesDir,
                "Players", "Sonic");

            IdleSprite = new Sprite(Path.Combine(pth, "idle"),
                Width, Height, origin, 6);

            WalkingSprite = new Sprite(Path.Combine(pth, "walking"),
                Width, Height, origin);

            RunningSprite = new Sprite(Path.Combine(pth, "running"),
                Width, Height, origin);

            BrakingSprite = new Sprite(Path.Combine(pth, "braking"),
                Width, Height, origin, doesLoop: false);

            DeadSprite = new Sprite(Path.Combine(pth, "death"),
                Width, Height, origin);

            JumpingSprite = new Sprite(Path.Combine(pth, "jumping"),
                SpinWidth, SpinHeight, spinOrigin);

            TurnAroundSprite = new Sprite(Path.Combine(pth, "turnaround"),
                Width, Height, origin, doesLoop: false);

            PushingSprite = new Sprite(Path.Combine(pth, "pushing"),
                Width, Height, origin, 32);

            Balancing1Sprite = new Sprite(Path.Combine(pth, "balancing1"),
                Width, Height, origin, 8);

            Balancing2Sprite = new Sprite(Path.Combine(pth, "balancing2"),
                Width, Height + 2, new Vector2(OriginX, OriginY + 2), 6);
        }
    }
}