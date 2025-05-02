using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Animator
    {

        public Texture2D Sprite;


        public float characterSize = 100f;
        private int characterIndex = 0;
        private int soldierIdleCount = 6;
        private float animationTimer = 0f;
        private float animationInterval = 0.1f;
        //  Idle, Move, Attack
        private int[] framesPerRow = new int[] { 6, 8, 6 };
        //  Idle, Move, Attack
        private float[] animationIntervals = new float[] { 0.1f, 0.1f, 0.1f };
        private int lastSpriteRow = -1;


        public Animator(Texture2D sprite)
        {
            Sprite = sprite;
        }


        public void Reset()
        {
            characterIndex = 0;
            animationTimer = 0f;
        }

        public void SetAnimation(int row)
        {
            soldierIdleCount = framesPerRow[row];
            Reset();
        }

        public float GetAttackDuration()
        {
            return framesPerRow[2] * animationIntervals[2];
        }


        public void Update(float deltaTime, int spriteRow)
        {

            float interval = animationIntervals[spriteRow];

            animationTimer += deltaTime;

            if (animationTimer >= interval)
            {
                characterIndex++;
                if (characterIndex >= framesPerRow[spriteRow])
                {
                    characterIndex = 0;

                }
                animationTimer = 0f;
            }


        }

        public void Draw(Vector2 position, int spriteRow, bool isFacingLeft)
        {
            float width = characterSize * 5;
            float height = characterSize * 5;

            float direction = 1;

            if (spriteRow != lastSpriteRow)
            {
                SetAnimation(spriteRow);
                lastSpriteRow = spriteRow;
            }

            if (isFacingLeft == true)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            Rectangle source = new Rectangle(characterIndex * characterSize, spriteRow * characterSize, direction * characterSize, characterSize);
            Rectangle dest = new Rectangle(position.X, position.Y, width, height);
            Vector2 origin = new Vector2(width / 2, height / 2);




            Raylib.DrawTexturePro(Sprite, source, dest, origin, 0f, new Color(255, 255, 255, 255));

        }
    }
}
