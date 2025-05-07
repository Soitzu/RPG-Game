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
            float speed = 200f;
            bool moved = false;


            if (IsDead)
            {
                if (!isDeadHandled)
                {
                    OnDeath();
                    isDeadHandled = true;
                }

                TimeSinceDeath += deltaTime;
                if (TimeSinceDeath > 3f)
                {
                    IsMarkedForRemoval = true;
                }
                return;
            }


            SoldierSpriteRow = 0;


            if (IsAttacking)
            {
                SoldierSpriteRow = 2;
                AttackTimer -= deltaTime;
                int currentFrame = Animator.GetCurrentFrame();

                float xOffset = IsFacingLeft ? -AttackRange : CharacterSize;
                Vector2 pos = Physics.Position;

                AttackHitBox = new Rectangle(pos.X + xOffset, pos.Y, AttackWidth, AttackHeight);

                if (AttackTimer <= 0)
                {
                    IsAttacking = false;
                }
            }
            else
            {
                SoldierSpriteRow = 0;
                AttackHitBox = null;
            }

            if (AttackTimer <= 0)
            {
                IsAttacking = false;
                AttackHitBox = null;
            }

            if (!IsAttacking)
            {
                if (Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    MoveRight(speed);
                    moved = true;
                    if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                    {
                        speed += 100;
                        moved = true;
                        MoveRight(speed);
                    }
                }

                if (Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    MoveLeft(speed);
                    moved = true;
                    if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                    {
                        speed += 100;
                        moved = true;
                        MoveLeft(speed);
                    }

                }

                if (Raylib.IsKeyPressed(KeyboardKey.Z))
                {
                    Attack();
                    Console.WriteLine("Attack!");
                }

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



        public new void Draw()
        {
            Animator.Draw(Physics.Position, SoldierSpriteRow, IsFacingLeft);
            Status.Draw();
            Status.DrawCharacterWindow();
        }

        protected override void OnDeath()
        {
            SoldierSpriteRow = 5;
            Animator.Reset();
            Console.WriteLine($"{Name} ist gestorben");

        }




    }
}

