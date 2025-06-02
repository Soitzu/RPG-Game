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
        public int SpriteRow { get; set; }
        public int Rotation { get; private set; }
        public float CharacterSize { get; set; }
        public float Mass { get; set; }
        public Status Status { get; private set; }

        public bool IsFacingLeft = false;
        public Vector2 Position { get; protected set; }
        protected bool IsAttacking = false;
        protected float AttackTimer = 0f;
        protected float AttackDuration = 0.5f;
        public Rectangle? AttackHitBox { get; protected set; }
        public Rectangle SpriteHitBox { get; protected set; }

        // Attack Stats
        public float AttackRange = 0;

        public float AttackWidth = 30;
        public float AttackHeight = 50;
        protected readonly int[] attackHitFrames = { 3, 4 };
        protected bool hasHitInCurrentAttack = false;
        public bool hasHitEnemy = false;
        public HashSet<Enemy> alreadyHitEnemies = new HashSet<Enemy>();


        // returns true, if Health <= 0
        public bool IsDead => Health <= 0;
        protected bool isDeadHandled = false;

        public bool IsInvincible = false;

        public float TimeSinceDeath { get; protected set; } = 0f;
        public bool IsMarkedForRemoval { get; protected set; } = false;

        private AnimationType currentAnimationType = AnimationType.Idle;






        public Character(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f)
        {
            Name = name;
            Health = health;
            Strength = strength;
            Animator = animator;
            Physics = new PhysicsBody { Position = startPosition, Size = characterSize, Mass = mass };
            CharacterSize = characterSize;
            Status = new Status(this);

            currentAnimationType = AnimationType.Idle;
            Animator.SetAnimation(currentAnimationType);

        }


        public virtual void Attack()
        {
            IsAttacking = true;
            AttackTimer = Animator.GetAttackDuration();
            currentAnimationType = AnimationType.Attack;
            Animator.Reset();
            SoundManager.PlaySound("attack_swing");
            alreadyHitEnemies.Clear();
            Animator.SetAnimation(currentAnimationType);
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
            SpriteRow = 1;
            Physics.Velocity = new Vector2(speed, Physics.Velocity.Y);


        }

        public void MoveLeft(float speed)
        {
            IsFacingLeft = true;
            SpriteRow = 1;
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
            Animator.SetAnimation(AnimationType.Death);
        }

        public Rectangle GetHitbox()
        {
            return new Rectangle(Physics.Position.X, Physics.Position.Y, CharacterSize, CharacterSize);
        }

        public void HandleDeath(bool isDeadHandled, float deltaTime)
        {
            if (!isDeadHandled)
            {
                isDeadHandled = true;
                IsAttacking = false;
                AttackHitBox = null;
            }

            Animator.SetAnimation(AnimationType.Death);
            Animator.Update(deltaTime);
            return;

        }

        public void DrawHitBox(Color color, float thickness = 2f)
        {
            if (AttackHitBox.HasValue)
            {
                Raylib_cs.Rectangle hitbox = AttackHitBox.Value;
                Raylib.DrawRectangleLinesEx(hitbox, thickness, color);
            }
        }

        public void UpdateSpriteHitBox()
        {
            SpriteHitBox = new Rectangle(
                Position.X,
                Position.Y,
                CharacterSize, // Breite des Sprites
                CharacterSize  // Höhe des Sprites
            );
        }

        public void DrawSpriteHitBox(Color color, float thickness = 2f)
        {
            Raylib.DrawRectangleLinesEx(SpriteHitBox, thickness, color);
        }







        public void Update(float deltaTime)
        {
            float speed = 200f;
            bool moved = false;

            if (IsDead)
            {
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
                currentAnimationType = AnimationType.Idle;
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
                    currentAnimationType = AnimationType.Attack;
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
            Animator.SetAnimation(currentAnimationType);
            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }

        public void Draw()
        {
            Animator.Draw(Physics.Position, IsFacingLeft);
            Status.Draw();
        }

    }
}
