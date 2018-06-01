using Microsoft.Xna.Framework;

namespace SonicSharp
{
    public class GameObject
    {
        // Variables/Constants
        public Sprite Sprite
        {
            get => sprite;
            set
            {
                if (sprite != value)
                {
                    value.Reset();
                    sprite = value;
                }
            }
        }

        public Vector2 Position;
        private Sprite sprite;

        // Methods
        /// <summary>
        /// Called when the object is initialized, usually upon (re)starting a stage.
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Called every loop while the object is active.
        /// </summary>
        public virtual void Update()
        {
            if (sprite != null)
                sprite.Animate();
        }

        /// <summary>
        /// Called every frame while the object is active.
        /// Used for drawing the object to the screen. 
        /// </summary>
        public virtual void Draw()
        {
            if (sprite != null)
                sprite.Draw(Position);
        }
    }
}