using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Character
    {
        public string Name { get; private set; }
        public int Health { get; set; }
        public int Strength { get; private set; }
        public Animator Animator { get; private set; }
        public PhysicsBody Physics { get; private set; }
        public int SoldierSpriteRow { get; set; }
        public int Rotation { get; private set; }
        public float CharacterSize { get; set; }
        public float Mass { get; set; }
        public Status Status { get; private set; }

        public bool IsFacingLeft = false;
        public Vector2 Position { get; private set; }
        private bool IsAttacking = false;
        private float AttackTimer = 0f;
        private float AttackDuration = 0.5f;
        public Rectangle? AttackHitBox { get; private set; }

        public float AttackRange = 0;
        public float AttackWidth = 30;
        public float AttackHeight = 50;
        public bool hasHitEnemy = false;
        public HashSet<Enemy> alreadyHitEnemies = new HashSet<Enemy>();


        // returns true, if Health <= 0
        public bool IsDead => Health <= 0;
        protected bool isDeadHandled = false;

        public bool IsInvincible = false;







        public Character(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Animator = animator;
            Physics = new PhysicsBody { Position = startPosition, Size = characterSize, Mass = mass };
            CharacterSize = characterSize;
            Status = new Status(this);
        }



        public void Attack()
        {
            IsAttacking = true;
            AttackTimer = Animator.GetAttackDuration();
            SoldierSpriteRow = 2;
            Animator.Reset();
            SoundManager.PlaySound("attack_swing");
            alreadyHitEnemies.Clear(); // <--- NEU!
        }

        public void OnHitEnemy(Enemy enemy)
        {
            if (enemy.IsDead) return;
            Console.WriteLine($"{Name} hat {enemy.Name} getroffen!");
            enemy.TakeDamage(Strength);
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


        public virtual void TakeDamage(int amount)
        {

            if (IsDead) return;

            Health -= amount;

            if (IsDead && !isDeadHandled)
            {
                isDeadHandled = true;
                OnDeath();
            }
        }

        protected virtual void OnDeath()
        {
            SoldierSpriteRow = 5;
            Animator.SetAnimation(SoldierSpriteRow);
        }


        public Rectangle GetHitbox()
        {
            return new Rectangle(Physics.Position.X, Physics.Position.Y, CharacterSize, CharacterSize);
        }









        public void Update(float deltaTime)
        {
            float speed = 200f;
            bool moved = false;

            if (IsDead)
            {
                Animator.Update(deltaTime, SoldierSpriteRow);
                return;
            }

            SoldierSpriteRow = 0;


            if (IsAttacking)
            {
                SoldierSpriteRow = 2;
                AttackTimer -= deltaTime;
                int currentFrame = Animator.GetCurrentFrame();
                if (currentFrame == 3 || currentFrame == 4)
                {

                }
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
                }
                if (Raylib.IsKeyDown(KeyboardKey.Left))
                {
                    MoveLeft(speed);
                    moved = true;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Up))
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

        public void Draw()
        {
            Animator.Draw(Physics.Position, SoldierSpriteRow, IsFacingLeft);
            Status.Draw();
        }

    }
}
