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


        public Animator(Texture2D sprite)
        {
            Sprite = sprite;
        }


        public void Update(float deltaTime)
        {

            animationTimer += deltaTime;

            if (animationTimer >= animationInterval)
            {
                characterIndex++;
                if (characterIndex >= soldierIdleCount)
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
