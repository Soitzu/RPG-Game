using Raylib_cs;
using System.Numerics;


namespace Game.Models
{
    public class Enemy : Character
    {







        public Enemy(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {

        }



        public void UpdateAI(float deltaTime)
        {
            if (Health <= 0 && !IsDead)
            {
                SoldierSpriteRow = 5;
                IsDead = true;

            }

            if (!IsDead)
            {
                Physics.Update(deltaTime);

            }

            Animator.Update(deltaTime, SoldierSpriteRow);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(Physics.Position.X, Physics.Position.Y, CharacterSize, CharacterSize);
        }

    }
}

