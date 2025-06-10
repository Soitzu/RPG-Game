using Raylib_cs;

namespace Game.Models
{
    public class GameManager
    {
        private Player hero;
        private List<Enemy> enemies;
        private bool showHitboxes = false;


        public GameManager(Player hero, List<Enemy> enemies)
        {
            this.hero = hero;
            this.enemies = enemies;
            foreach (var enemy in enemies)
            {
                enemy.Target = hero;
            }
        }




        public void Update(float deltaTime)
        {
            CheckCollisions();
            CheckEnemyAttacks();
            enemies.RemoveAll(enemy => enemy.IsMarkedForRemoval);

            hero.Update(deltaTime);

            foreach (var enemy in enemies)
            {
                enemy.UpdateAI(deltaTime);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.H))
            {
                showHitboxes = !showHitboxes;
                Console.WriteLine("Hitboxes: " + (showHitboxes ? "ON" : "OFF"));
            }


        }

        public void Draw()
        {
            hero.Draw();
            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }

            if (showHitboxes)
            {
                hero.DrawAttackHitbox(Color.Red);
                hero.DrawSpriteHitbox(Color.Green);
                foreach (var enemy in enemies)
                {
                    enemy.DrawAttackHitbox(Color.Red);
                    enemy.DrawSpriteHitbox(Color.Green);
                }
            }

        }





        private void CheckCollisions()
        {
            foreach (var enemy in enemies)
            {
                if (hero.AttackHitbox.HasValue && Raylib.CheckCollisionRecs(hero.AttackHitbox.Value, enemy.GetSpriteHitbox()))
                {

                    int currentFrame = hero.Animator.GetCurrentFrame();

                    if ((currentFrame == 3 || currentFrame == 4) && !enemy.IsDead && !hero.alreadyHitEnemies.Contains(enemy))
                    {
                        Console.WriteLine("Treffer!");

                        hero.alreadyHitEnemies.Add(enemy);
                        hero.OnHitEnemy(enemy);
                    }
                }
            }
        }

        private void CheckEnemyAttacks()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.AttackHitbox.HasValue && Raylib.CheckCollisionRecs(enemy.AttackHitbox.Value, hero.GetSpriteHitbox()))
                {
                    int currentFrame = enemy.Animator.GetCurrentFrame();
                    if ((currentFrame == 3 || currentFrame == 4) && !hero.IsInvincible && !enemy.HasHitPlayerThisAttack)
                    {
                        hero.TakeDamage(enemy.Strength);
                        enemy.HasHitPlayerThisAttack = true;
                        // Optional: hero.IsInvincible = true; // für kurze Unverwundbarkeit
                    }
                }
            }
        }
    }
}

