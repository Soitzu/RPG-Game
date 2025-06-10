using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Player : Character
    {

        private AnimationType currentAnimationType = AnimationType.Idle;


        private AnimationType lastAnimationType = AnimationType.Idle;

        private bool isInventoryOpen = false;
        public Inventory Inventory { get; private set; }





        public Player(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {
            Inventory = new Inventory();

        }




        public void Update(float deltaTime)
        {
            float speed = 200f;
            bool moved = false;
            currentAnimationType = AnimationType.Idle;

            if (IsDead)
            {
                if (!isDeadHandled)
                {
                    isDeadHandled = true;
                    IsAttacking = false;
                    AttackHitbox = null;
                }

                TimeSinceDeath += deltaTime;
                if (TimeSinceDeath > 3f)
                {
                    IsMarkedForRemoval = true;
                }

                Animator.SetAnimation(AnimationType.Death);
                Animator.Update(deltaTime);
                return;
            }

            if (IsHurt || Animator.isPlayingOnce)
            {
                Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                currentAnimationType = AnimationType.Hurt;
                Animator.Update(deltaTime);
                return;
            }

            if (IsInvincible)
            {
                InvincibilityTimer -= deltaTime;
                if (InvincibilityTimer <= 0f)
                {
                    IsInvincible = false;
                    InvincibilityTimer = 0f;
                }

            }




            if (IsAttacking)
            {
                currentAnimationType = AnimationType.Attack;
                AttackTimer -= deltaTime;
                int currentFrame = Animator.GetCurrentFrame();

                float xOffset = IsFacingLeft ? -AttackWidth : CharacterSize;
                Vector2 pos = Physics.Position;
                AttackHitbox = new Rectangle(pos.X - CharacterSize / 2 + xOffset, pos.Y - CharacterSize / 2, AttackWidth, AttackHeight);


                if (AttackTimer <= 0)
                {
                    IsAttacking = false;
                }
            }
            else
            {
                currentAnimationType = AnimationType.Idle;
                AttackHitbox = null;
            }
            if (!IsAttacking)
            {
                if (Raylib.IsKeyDown(KeyboardKey.Right))
                {
                    MoveRight(speed);
                    moved = true;
                    currentAnimationType = AnimationType.Move;

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
                    currentAnimationType = AnimationType.Move;

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
            if (currentAnimationType != lastAnimationType)
            {
                Animator.SetAnimation(currentAnimationType);
                lastAnimationType = currentAnimationType;
            }


            UpdateSpriteHitbox();
            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }



        public void Draw()
        {
            Color drawColor = IsInvincible && (int)(TimeSinceDamage * 10) % 2 == 0
         ? new Color(255, 255, 255, 150) // Halb-transparent
         : Color.White;

            Animator.Draw(Physics.Position, IsFacingLeft);
            Status.Draw();
            Status.DrawCharacterWindow();
        }

        protected override void OnDeath()
        {
            currentAnimationType = AnimationType.Death;
            Animator.Reset();
            Console.WriteLine($"{Name} ist gestorben");

        }




    }
}

