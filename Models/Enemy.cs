using Raylib_cs;
using System.Numerics;


namespace Game.Models
{
    public class Enemy : Character
    {






        public Enemy(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize = 100f, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {

        }



        public void UpdateAI(float deltaTime)
        {


            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }

    }
}

