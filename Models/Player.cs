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




        public new void Update(float deltaTime)
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
                    AttackHitBox = null;
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




            if (IsAttacking)
            {
                currentAnimationType = AnimationType.Attack;
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
                SpriteRow = 0;
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


            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }



        public new void Draw()
        {
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

