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
        public float CharacterSize { get; set; }
        public float Mass { get; set; }
        public Status Status { get; private set; }

        public bool IsFacingLeft = false;
        public Vector2 Position { get; protected set; }
        protected bool IsAttacking = false;
        protected float AttackTimer = 0f;
        protected float AttackDuration = 0.5f;
        public Rectangle? AttackHitbox { get; protected set; }
        public Rectangle SpriteHitbox { get; protected set; }

        // Attacking Attributes
        public float AttackRange = 0;
        public float AttackWidth = 50;
        public float AttackHeight = 100;
        protected readonly int[] attackHitFrames = { 3, 4 };
        protected bool hasHitInCurrentAttack = false;
        public bool hasHitEnemy = false;
        public HashSet<Enemy> alreadyHitEnemies = new HashSet<Enemy>();


        // Invincibility Attributes
        public bool IsInvincible = false;
        public float InvincibilityTimer = 0f;
        public float InvincibilityDuration = 0.3f;
        public float TimeSinceDamage = 0f;

        // Death Attributes
        public bool IsDead => Health <= 0;
        public bool IsHurt = false;
        public bool IsHurtHandled = false;
        protected bool isDeadHandled = false;

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


        // Move Methods
        public void MoveRight(float speed)
        {
            IsFacingLeft = false;
            currentAnimationType = AnimationType.Move;
            Physics.Velocity = new Vector2(speed, Physics.Velocity.Y);
        }

        public void MoveLeft(float speed)
        {
            IsFacingLeft = true;
            currentAnimationType = AnimationType.Move;
            Physics.Velocity = new Vector2(-speed, Physics.Velocity.Y);
        }

        public void Jump(float jumpStrength)
        {
            if (Physics.IsOnGround)
            {
                Physics.Velocity = new Vector2(Physics.Velocity.X, -jumpStrength);

            }
        }
        // Move Methods end

        // Attack Methods
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

        public virtual void TakeDamage(int amount)
        {
            if (IsDead || IsInvincible) return;

            Health -= amount;
            IsHurt = true;

            if (!Animator.isPlayingOnce)
            {
                Animator.PlayOnce(AnimationType.Hurt, () =>
                    {
                        IsHurt = false;
                        IsHurtHandled = false;

                    });
                IsInvincible = true;
                InvincibilityTimer = InvincibilityDuration;
            }
        }
        // Damage Methods end


        // Death Methods
        protected virtual void OnDeath()
        {
            Animator.SetAnimation(AnimationType.Death);
        }

        public void HandleDeath(bool isDeadHandled, float deltaTime)
        {
            if (!isDeadHandled)
            {
                isDeadHandled = true;
                IsAttacking = false;
                AttackHitbox = null;
            }

            Animator.SetAnimation(AnimationType.Death);
            Animator.Update(deltaTime);
            return;

        }
        // Death Methods end


        // Hitbox Methods
        public Rectangle GetSpriteHitbox()
        {
            return new Rectangle(Physics.Position.X - CharacterSize / 2, Physics.Position.Y - CharacterSize / 2, CharacterSize, CharacterSize);
        }

        public void DrawAttackHitbox(Color color, float thickness = 2f)
        {
            if (AttackHitbox.HasValue)
            {
                Rectangle hitbox = AttackHitbox.Value;
                Raylib.DrawRectangleLinesEx(hitbox, thickness, color);
            }
        }

        public void UpdateSpriteHitbox()
        {
            SpriteHitbox = new Rectangle(
                Physics.Position.X - CharacterSize / 2,
                Physics.Position.Y - CharacterSize / 2,
                CharacterSize,
                CharacterSize
            );
        }

        public void DrawSpriteHitbox(Color color, float thickness = 2f)
        {
            Raylib.DrawRectangleLinesEx(SpriteHitbox, thickness, color);
        }
        // Hitbox Method end





    }
}
