using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
    public class GameManager
    {
        private Character hero;
        private List<Enemy> enemies;


        public GameManager(Character hero, List<Enemy> enemies)
        {
            this.hero = hero;
            this.enemies = enemies;
        }




        public void Update(float deltaTime)
        {
            CheckCollisions();
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
                    if (!enemy.IsDead && !hero.alreadyHitEnemies.Contains(enemy))
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
                    if (!hero.IsInvincible)
                    {
                        hero.TakeDamage(enemy.Strength);
                    }

                }
            }
        }


    }
}

