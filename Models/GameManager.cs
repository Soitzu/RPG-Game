using Raylib_cs;

namespace Game.Models
{
    public class GameManager
    {
        private Player hero;
        private List<Enemy> enemies;


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


        }

        public void Draw()
        {
            hero.Draw();
            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }
        }





        private void CheckCollisions()
        {
            foreach (var enemy in enemies)
            {
                if (hero.AttackHitBox.HasValue && Raylib.CheckCollisionRecs(hero.AttackHitBox.Value, enemy.GetHitbox()))
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
                if (enemy.AttackHitBox.HasValue && Raylib.CheckCollisionRecs(enemy.AttackHitBox.Value, hero.GetHitbox()))
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

