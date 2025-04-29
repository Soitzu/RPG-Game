using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Character
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Strength { get; private set; }
        public Animator Animator { get; private set; }
        public PhysicsBody Physics { get; private set; }
        public int SoldierSpriteRow { get; private set; }
        public int Rotation { get; private set; }
        public float CharacterSize { get; set; }
        public float Mass { get; set; }

        public bool IsFacingLeft = false;
        public Vector2 Position { get; private set; }



        public Character(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize = 100f, float mass = 1.0f)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Animator = animator;
            Physics = new PhysicsBody { Position = startPosition, Size = characterSize, Mass = mass };
            CharacterSize = characterSize;
        }



        public void Attack(Character target)
        {
            target.Health -= Strength;
        }

        public void Update(float deltaTime)
        {
            float speed = 200f;
            SoldierSpriteRow = 0;
            var velocity = Physics.Velocity;
            velocity.X = 0;

            if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                IsFacingLeft = false;
                SoldierSpriteRow = 1;
                velocity.X = speed;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                IsFacingLeft = true;
                SoldierSpriteRow = 1;
                velocity.X = -speed;
            }

            Physics.Velocity = velocity;
            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }

        public void Draw()
        {
            Animator.Draw(Physics.Position, SoldierSpriteRow, IsFacingLeft);
        }

    }
}
