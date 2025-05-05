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

            if (IsDead)
            {
                if (!isDeadHandled)
                {
                    OnDeath();
                    isDeadHandled = true;
                }
                Animator.Update(deltaTime, SoldierSpriteRow);
                return;
            }

            Physics.Update(deltaTime);
            Animator.Update(deltaTime, SoldierSpriteRow);
        }

        protected override void OnDeath()
        {
            SoldierSpriteRow = 5;
            Animator.Reset();
            Console.WriteLine($"{Name} ist gestorben");
        }

    }
}

