using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class Enemy : Character
    {

        private AnimationType currentAnimationType = AnimationType.Idle;


        // AI-Parameter
        public float AggroRange = 400f;      // Aggro distance
        public float MoveSpeed = 120f;       // Movement Speed
        public float AttackCooldown = 1.5f;  // Time between Attacks
        private float attackCooldownTimer = 1f;
        private Random rnd = new Random();

        private float _cooldownBetweenActions = 3f;
        private float _moveTimer = 0.0f;  // 2 Sekunden nach links gehen

        // Target (Player)
        public Player Target { get; set; }

        public bool HasHitPlayerThisAttack = false;





        public Enemy(string name, int health, int strength, Animator animator, Vector2 startPosition, float characterSize, float mass = 1.0f) : base(name, health, strength, animator, startPosition, characterSize, mass)
        {
            AttackRange = 100f;


        }

        public override void Attack()
        {
            base.Attack();
            HasHitPlayerThisAttack = false;
        }


        public void Seek(float deltaTime)
        {

            // Cooldown between Actions is shit and currently not working. But it still looks nice the way it is
            if (_cooldownBetweenActions > 0)
            {
                _cooldownBetweenActions -= deltaTime;
                currentAnimationType = AnimationType.Idle;
                Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                return;
            }
            else
            {



                if (_moveTimer <= 0f)
                {

                    int movement = rnd.Next(1, 4);
                    _moveTimer = 0.5f + (float)rnd.NextDouble() * 1.5f;


                    switch (movement)
                    {
                        case 1:
                            Physics.Velocity = new Vector2(-MoveSpeed, Physics.Velocity.Y);
                            currentAnimationType = AnimationType.Move;
                            IsFacingLeft = true;
                            Console.WriteLine("Action: Move Left");
                            break;
                        case 2:
                            Physics.Velocity = new Vector2(MoveSpeed, Physics.Velocity.Y);
                            currentAnimationType = AnimationType.Move;
                            IsFacingLeft = false;
                            Console.WriteLine("Action: Move Right");
                            break;
                        case 3:
                            Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                            currentAnimationType = AnimationType.Idle;
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

        public void SeekTarget(Player Target, float deltaTime)
        {

        }





        public void UpdateAI(float deltaTime)
        {
            // Death function
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
            // Death function end 

            /*
                        // --- KI-Logik ---
                        if (Target != null && !Target.IsDead)
                        {
                            Vector2 myPos = Physics.Position;
                            Vector2 targetPos = Target.Physics.Position;
                            float distance = Vector2.Distance(myPos, targetPos);

                            // Richtung zum Spieler
                            float dx = targetPos.X - myPos.X;
                            IsFacingLeft = dx < 0;

                            // Angriff nur, wenn Cooldown abgelaufen
                            attackCooldownTimer -= deltaTime;

                            // <<< NEU: Angriff NUR starten, wenn NICHT IsAttacking!
                            if (distance <= AttackRange)
                            {
                                if (!IsAttacking && attackCooldownTimer <= 0f)
                                {
                                    Attack();
                                    attackCooldownTimer = AttackCooldown;
                                }
                            }
                            else if (distance <= AggroRange)
                            {
                                float moveDir = MathF.Sign(dx);
                                Physics.Velocity = new Vector2(moveDir * MoveSpeed, Physics.Velocity.Y);
                                currentAnimationType = AnimationType.Move;
                            }
                            else
                            {
                                Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                                currentAnimationType = AnimationType.Idle;
                            }
                        }
                        else
                        {
                            Physics.Velocity = new Vector2(0, Physics.Velocity.Y);
                            currentAnimationType = AnimationType.Idle;
                        }

                        // --- KI-End ---
            */

            Vector2 myPos = Physics.Position;
            Vector2 targetPos = Target.Physics.Position;
            float distance = Vector2.Distance(myPos, targetPos);

            // Phase 1, "seek" the enemy if he's not in range
            Seek(deltaTime);


            //currentAnimationType = AnimationType.Idle;
            Animator.SetAnimation(currentAnimationType);
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
                    float xOffset = IsFacingLeft ? -AttackRange : CharacterSize;
                    Vector2 pos = Physics.Position;
                    AttackHitBox = new Rectangle(pos.X + xOffset, pos.Y, AttackWidth, AttackHeight);
                }
                else
                {
                    AttackHitBox = null;
                }

                if (AttackTimer <= 0)
                {
                    IsAttacking = false;
                    AttackHitBox = null;
                }
            }
            else
            {
                AttackHitBox = null;
                HasHitPlayerThisAttack = false;
            }
        }


    }
}

