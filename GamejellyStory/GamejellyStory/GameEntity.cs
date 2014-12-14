using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{

    public class GameEntity
    {
        private Dictionary<string, Animation> Animations { get; set; }

        public Animation CurrentAnimation { get; private set; }
        private List<IComponent> components;
        public GameEntity(Texture2D texture)
        {
            Texture = texture;
            components = new List<IComponent>();
            Animations = new Dictionary<string, Animation>();
            
            Animations.Add("idleUp", new Animation(Texture, true, 64, 64, 1, new Vector2(0, 0)));
            Animations.Add("idleDown", new Animation(Texture, true, 64, 64, 1, new Vector2(0, 64*2)));
            Animations.Add("idleLeft", new Animation(Texture, true, 64, 64, 1, new Vector2(0, 64)));
            Animations.Add("idleRight", new Animation(Texture, true, 64, 64, 1, new Vector2(0, 64*3)));


            var walkFrameDuration = 75;
            Animations.Add("walkUp", new Animation(Texture, true, 64, 64, walkFrameDuration, new Vector2(0, 0),
                                                                             new Vector2(64, 0),
                                                                             new Vector2(64 * 2, 0),
                                                                             new Vector2(64 * 3, 0),
                                                                             new Vector2(64 * 4, 0),
                                                                             new Vector2(64 * 5, 0),
                                                                             new Vector2(64 * 6, 0),
                                                                             new Vector2(64 * 7, 0),
                                                                             new Vector2(64 * 8, 0)));

            Animations.Add("walkDown", new Animation(Texture, true, 64, 64, walkFrameDuration, new Vector2(0, 64*2),
                                                                               new Vector2(64, 64*2),
                                                                               new Vector2(64 * 2, 64*2),
                                                                               new Vector2(64 * 3, 64*2),
                                                                               new Vector2(64 * 4, 64*2),
                                                                               new Vector2(64 * 5, 64*2),
                                                                               new Vector2(64 * 6, 64*2),
                                                                               new Vector2(64 * 7, 64*2),
                                                                               new Vector2(64 * 8, 64*2)));

            Animations.Add("walkLeft", new Animation(Texture, true, 64, 64, walkFrameDuration, new Vector2(0, 64),
                                                                               new Vector2(64, 64),
                                                                               new Vector2(64 * 2, 64),
                                                                               new Vector2(64 * 3, 64),
                                                                               new Vector2(64 * 4, 64),
                                                                               new Vector2(64 * 5, 64),
                                                                               new Vector2(64 * 6, 64),
                                                                               new Vector2(64 * 7, 64),
                                                                               new Vector2(64 * 8, 64)));
            
            Animations.Add("walkRight", new Animation(Texture, true, 64, 64, walkFrameDuration, new Vector2(0, 64*3),
                                                                                new Vector2(64, 64*3),
                                                                                new Vector2(64 * 2, 64*3),
                                                                                new Vector2(64 * 3, 64*3),
                                                                                new Vector2(64 * 4, 64*3),
                                                                                new Vector2(64 * 5, 64*3),
                                                                                new Vector2(64 * 6, 64*3),
                                                                                new Vector2(64 * 7, 64*3),
                                                                                new Vector2(64 * 8, 64*3)));
            CurrentAnimation = Animations["idleDown"];
        }



        private float walkSpeed = 3.0f;
        private float runSpeed = 5.0f;
        public Texture2D Texture { get; private set; }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(MathHelper.Clamp(value.X, 0, 3200 - 128),
                    MathHelper.Clamp(value.Y, 0, 3200-128));
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();

            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                var thumbsticks = GamePad.GetState(PlayerIndex.One).ThumbSticks;
                var dpad = GamePad.GetState(PlayerIndex.One).DPad;

                if (dpad.Left == ButtonState.Pressed)
                {
                    Position = new Vector2(Position.X - walkSpeed, Position.Y);
                    CurrentAnimation = Animations["walkLeft"];
                }
                if (dpad.Right == ButtonState.Pressed)
                {
                    Position = new Vector2(Position.X + walkSpeed, Position.Y);
                    CurrentAnimation = Animations["walkRight"];
                }
                if (dpad.Up == ButtonState.Pressed)
                {
                    Position = new Vector2(Position.X, Position.Y - walkSpeed);
                    CurrentAnimation = Animations["walkUp"];
                }
                if (dpad.Down == ButtonState.Pressed)
                {
                    Position = new Vector2(Position.X, Position.Y + walkSpeed);
                    CurrentAnimation = Animations["walkDown"];
                }
                    
                Position = new Vector2(Position.X + (thumbsticks.Left.X * walkSpeed), Position.Y + -(thumbsticks.Left.Y * walkSpeed));
            }
            else
            {
                if (pressedKeys.Contains(Keys.Left))
                    Position = new Vector2(Position.X - 5, Position.Y);
                if (pressedKeys.Contains(Keys.Right))
                    Position = new Vector2(Position.X + 5, Position.Y);
                if (pressedKeys.Contains(Keys.Up))
                    Position = new Vector2(Position.X, Position.Y - 5);
                if (pressedKeys.Contains(Keys.Down))
                    Position = new Vector2(Position.X, Position.Y + 5);
            }
            if (position.X <= 0) Position = new Vector2(0, position.Y);
            if (position.Y <= 0) Position = new Vector2(position.X, 0);
            CurrentAnimation.Update(gameTime);
        }
    }
}
