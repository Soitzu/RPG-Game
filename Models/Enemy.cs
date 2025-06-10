using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Enemy : Character
    {

        private AnimationType currentAnimationType = AnimationType.Idle;


        // AI-Parameter
        public float AggroRange = 400f;      // Aggro distance
        public float StoppingDistance;
        public float MoveSpeed = 120f;       // Movement Speed
        public float AttackCooldown = 1.0f;  // Time between Attacks
        private float attackCooldownTimer = 1f;
        private Random rnd = new Random();

        private float _cooldownBetweenActions = 3f;
        private float _moveTimer = 0.0f;  // 2 Sekunden nach links gehen

        // Target (Player)
        public Player Target { get; set; }

        public bool HasHitPlayerThisAttack = false;





        public Enemy(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, Player target, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {
            AttackRange = 100f;
            Target = target;


        }

        public override void Attack()
        {
            base.Attack();
            HasHitPlayerThisAttack = false;
        }


        public void Seek(float deltaTime)
        {
            // Cooldown between Actions is shit and currently not working. But it still looks okayish the way it is

            if (_cooldownBetweenActions > 0)
            {
                _cooldownBetweenActions -= deltaTime;
                Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                currentAnimationType = AnimationType.Idle;
                return;
            }
            else
            {
                if (_moveTimer <= 0f)
                {

                    int movement = rnd.Next(1, 4);
                    _moveTimer = 0.5f + (float)rnd.NextDouble() * 1.5f;
                    currentAnimationType = AnimationType.Idle;

                    switch (movement)
                    {
                        case 1:
                            currentAnimationType = AnimationType.Move;
                            Physics.Velocity = new Vector2(-MoveSpeed, Physics.Velocity.Y);
                            IsFacingLeft = true;
                            Console.WriteLine("Action: Move Left");
                            break;
                        case 2:
                            currentAnimationType = AnimationType.Move;
                            Physics.Velocity = new Vector2(MoveSpeed, Physics.Velocity.Y);
                            IsFacingLeft = false;
                            Console.WriteLine("Action: Move Right");
                            break;
                        case 3:
                            currentAnimationType = AnimationType.Idle;
                            Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                            Console.WriteLine("Action: Idle");
                            break;
                    }
                }
                else
                {
                    _moveTimer -= deltaTime;

                }
            }
        }

        public void UpdateAI(float deltaTime)
        {
            // Death check
            if (IsDead)
            {
                HandleDeath(isDeadHandled, deltaTime);

                TimeSinceDeath += deltaTime;
                if (TimeSinceDeath > 3f)
                {
                    IsMarkedForRemoval = true;
                }

                Animator.SetAnimation(AnimationType.Death);
                Animator.Update(deltaTime);
                return;
            }
            // Death check end


            // Get the Positions and the distaance
            Vector2 enemyPosition = Physics.Position;
            Vector2 playerPosition = Target.Physics.Position;
            float distance = Vector2.Distance(enemyPosition, playerPosition);
            float dx = playerPosition.X - enemyPosition.X;

            if (Target != null && !Target.IsDead)
            {

                if (distance <= AggroRange)
                {
                    IsFacingLeft = dx < 0;

                    if (distance <= AttackRange + StoppingDistance)
                    {
                        Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                        attackCooldownTimer -= deltaTime;

                        if (!IsAttacking && attackCooldownTimer <= 0f)
                        {
                            Attack();
                            attackCooldownTimer = AttackCooldown;
                            Animator.Reset();
                        }
                        else if (!IsAttacking)
                        {
                            currentAnimationType = AnimationType.Idle;
                        }


                    }
                    else
                    {
                        float moveDir = MathF.Sign(dx);
                        Physics.Velocity = new Vector2(moveDir * MoveSpeed, Physics.Velocity.Y);
                        currentAnimationType = AnimationType.Move;
                    }

                }
                else
                {
                    // Phase 1, "seek" the enemy if he's not in range
                    Seek(deltaTime);
                }


            }
            else
            {
                Seek(deltaTime);

            }
            // Enemy AI End


            //currentAnimationType = AnimationType.Idle;
            UpdateSpriteHitbox();
            if (!Animator.isPlayingOnce)
            {
                Animator.SetAnimation(currentAnimationType);
            }

            UpdateAttack(deltaTime);
            Physics.Update(deltaTime);
            Animator.Update(deltaTime);
        }

        protected override void OnDeath()
        {
            currentAnimationType = AnimationType.Death;
            Animator.Reset();
            Console.WriteLine($"{Name} ist gestorben");

        }

        public void UpdateAttack(float deltaTime)
        {
            if (IsAttacking)
            {
                currentAnimationType = AnimationType.Attack;
                AttackTimer -= deltaTime;
                int currentFrame = Animator.GetCurrentFrame();

                // Hitbox nur in Trefferframes setzen
                if (currentFrame == 3 || currentFrame == 4)
                {
                    float xOffset = IsFacingLeft ? -AttackWidth : CharacterSize;
                    Vector2 pos = Physics.Position;
                    AttackHitbox = new Rectangle(pos.X - CharacterSize / 2 + xOffset, pos.Y - CharacterSize / 2, AttackWidth, AttackHeight);

                }
                else
                {
                    AttackHitbox = null;
                }

                if (AttackTimer <= 0)
                {
                    IsAttacking = false;
                    AttackHitbox = null;
                    currentAnimationType = AnimationType.Idle;
                }
            }
            else
            {
                AttackHitbox = null;
                HasHitPlayerThisAttack = false;
            }
        }

        public void Draw()
        {
            Animator.Draw(Physics.Position, IsFacingLeft);
            Status.Draw();
        }


    }
}

