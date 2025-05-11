using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Animator
    {

        public Texture2D Sprite;


        public float characterSize = 100f;
        private int characterIndex = 0;
        private float animationTimer = 0f;
        private float animationInterval = 0.1f;
        private int lastSpriteRow = -1;


        private AnimationType currentType;
        private AnimationInfo currentAnimation;

        private Dictionary<AnimationType, AnimationInfo> animations;



        public Animator(Texture2D sprite, Dictionary<AnimationType, AnimationInfo> animationMap)
        {
            Sprite = sprite;
            animations = animationMap;
            SetAnimation(AnimationType.Idle);
            characterIndex = 0;
            animationTimer = 0f;


        }


        public void Reset()
        {
            characterIndex = 0;
            animationTimer = 0f;
        }

        public void SetAnimation(AnimationType type)
        {
            Console.WriteLine($"SetAnimation: {type}");
            if (currentType != type)
            {
                if (!animations.TryGetValue(type, out var animInfo))
                {
                    Console.WriteLine($"WARNUNG: AnimationType {type} Nicht im Mapping! Fallback auf Idle");
                    animInfo = animations[AnimationType.Idle];
                    type = AnimationType.Idle;
                }
                currentType = type;
                currentAnimation = animInfo;
                Reset();
            }
        }

        public float GetAttackDuration()
        {
            return animations[AnimationType.Attack].FrameCount * animations[AnimationType.Attack].Interval;
        }

        public int GetCurrentFrame()
        {
            return characterIndex;
        }


        public void Update(float deltaTime)
        {
            if (currentAnimation == null)
            {
                Console.WriteLine("FEHLER: currentAnimation ist null! Wurde SetAnimation() korrekt aufgerufen?");
                return;
            }

            animationTimer += deltaTime;

            if (animationTimer >= currentAnimation.Interval)
            {
                characterIndex++;
                if (characterIndex >= currentAnimation.FrameCount)
                {
                    if (currentType == AnimationType.Death)
                    {
                        characterIndex = currentAnimation.FrameCount - 1;
                    }
                    else
                    {
                        characterIndex = 0;
                    }
                }
                animationTimer = 0f;
            }
        }






        public void Draw(Vector2 position, bool isFacingLeft)
        {
            if (currentAnimation == null)
            {
                Console.WriteLine("FEHLER: currentAnimation ist null! Draw() abgebrochen.");
                return;
            }
            Console.WriteLine($"Draw: Row={currentAnimation.SpriteRow}, Index={characterIndex}, Size={characterSize}");

            Console.WriteLine($"Draw: SpriteRow={currentAnimation.SpriteRow}, Frame={characterIndex}");

            float width = characterSize * 5;
            float height = characterSize * 5;
            float direction = isFacingLeft ? -1 : 1;


            Rectangle source = new Rectangle(characterIndex * characterSize, currentAnimation.SpriteRow * characterSize, direction * characterSize, characterSize);
            Rectangle dest = new Rectangle(position.X, position.Y, width, height);
            Vector2 origin = new Vector2(width / 2, height / 2);




            Raylib.DrawTexturePro(Sprite, source, dest, origin, 0f, new Color(255, 255, 255, 255));

        }
    }
}
