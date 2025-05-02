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
        private bool IsAttacking = false;
        private float AttackTimer = 0f;
        private float AttackDuration = 0.5f;




        public Character(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Animator = animator;
            Physics = new PhysicsBody { Position = startPosition, Size = characterSize, Mass = mass };
            CharacterSize = characterSize;
        }



        public void Attack()
        {
            IsAttacking = true;
            AttackTimer = Animator.GetAttackDuration();
            SoldierSpriteRow = 2;
            Animator.Reset();
            SoundManager.PlaySound("attack_swing");
        }

        public void Jump(float jumpStrength)
        {
            if (Physics.IsOnGround)
            {
                Physics.Velocity = new Vector2(Physics.Velocity.X, -jumpStrength);

            }
        }

        public void MoveRight(float speed)
        {

            IsFacingLeft = false;
            SoldierSpriteRow = 1;
            Physics.Velocity = new Vector2(speed, Physics.Velocity.Y);

        }

        public void MoveLeft(float speed)
        {
            IsFacingLeft = true;
            SoldierSpriteRow = 1;
            Physics.Velocity = new Vector2(-speed, Physics.Velocity.Y);
        }




        public void Update(float deltaTime)
        {
            float speed = 200f;
            SoldierSpriteRow = 0;
            bool moved = false;

            if (IsAttacking)
            {
                SoldierSpriteRow = 2;
                AttackTimer -= deltaTime;
                if (AttackTimer <= 0)
                {
                    IsAttacking = false;
                }
            }
            else
            {
                SoldierSpriteRow = 0;
            }


            if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                MoveRight(speed);
                moved = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                MoveLeft(speed);
                moved = true;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Up) && !IsAttacking)
            {
                Attack();
                Console.WriteLine("Attack!");

            }

            if (!moved)
            {
                Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Space) && Physics.IsOnGround)
            {
                Jump(400f);
            }
            Physics.Update(deltaTime);
            Animator.Update(deltaTime, SoldierSpriteRow);
        }

        public void Draw()
        {
            Animator.Draw(Physics.Position, SoldierSpriteRow, IsFacingLeft);
        }

    }
}
