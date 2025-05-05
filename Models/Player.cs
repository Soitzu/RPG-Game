using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Player : Character
    {








        public Player(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {

        }



        public new void Update(float deltaTime)
        {

            Animator.Update(deltaTime, SoldierSpriteRow);

            Physics.Update(deltaTime);
            Animator.Update(deltaTime, SoldierSpriteRow);
        }

        public new void Draw()
        {

        }



    }
}

