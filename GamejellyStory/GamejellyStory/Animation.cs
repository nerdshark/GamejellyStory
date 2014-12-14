using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class Animation
    {
        private Texture2D texture;
        private List<Sprite> frames;

        public List<Sprite> Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        public int FrameWidth { get; private set; }

        public int FrameHeight { get; private set; }

        /// <summary>
        /// Length of time to display each frame of the animation (in milliseconds)
        /// </summary>
        public int FrameDuration { get; set; }

        public Sprite CurrentFrame { get; private set; }

        private int currentFrameIndex;
        public bool PlaybackLoops { get; private set; }
        public Animation(Texture2D texture, bool playbackLoops, int frameWidth, int frameHeight, int frameDuration, params Vector2[] frameCoordinates)
        {
            if (texture == null) throw new ArgumentNullException("texture", "texture must be valid");
            this.texture = texture;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            PlaybackLoops = playbackLoops;
            FrameDuration = frameDuration;
            Frames = new List<Sprite>();
            for (var idx = 0; idx < frameCoordinates.Length; idx++)
            {
                var sprite = new Sprite(texture, new Rectangle((int) frameCoordinates[idx].X, (int) frameCoordinates[idx].Y, FrameWidth, FrameHeight));
                Frames.Add(sprite);
            }
            CurrentFrame = Frames.FirstOrDefault();
        }

        private int frameTimeAccumulator;
        public void Activate()
        {
            currentFrameIndex = 0;
            CurrentFrame = Frames[currentFrameIndex];
            frameTimeAccumulator = 0;
            AnimationEnded = false;
        }

        public void Update(GameTime gameTime)
        {
            if (frameTimeAccumulator >= FrameDuration)
            {
                if (currentFrameIndex == Frames.Count() - 1)
                {
                    currentFrameIndex = 0;
                    /*if (PlaybackLoops)
                    {
                        currentFrameIndex = 0;
                        frameTimeAccumulator = 0;
                    }
                    else AnimationEnded = true;
                    
                    return;*/
                }
                else currentFrameIndex++;
                
                CurrentFrame = Frames[currentFrameIndex];
                frameTimeAccumulator = 0;
            }
            else
            {
                frameTimeAccumulator += gameTime.ElapsedGameTime.Milliseconds;
            }

        }

        public bool AnimationEnded { get; private set; }
    }
}
