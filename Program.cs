using Raylib_cs;
using System.Numerics;
using Game.Models;

namespace HelloWorld;

public class Constants
{
    public const int SCREEN_WIDTH = 1280;
    public const int SCREEN_HEIGHT = 1080;
    public const int CHARACTER_SIZE = 100;





    class Program
    {
        public static void Main()
        {

            Raylib.SetWindowState(ConfigFlags.ResizableWindow);

            int userMonitor = Raylib.GetCurrentMonitor();
            int userMonitorWidth = Raylib.GetMonitorWidth(userMonitor);
            int userMonitorHeight = Raylib.GetMonitorHeight(userMonitor);



            Vector2 startPosition = new Vector2(100 + 50, 100 + 50);
            Raylib.InitWindow(userMonitorWidth, userMonitorHeight, "Mini Adventurer");
            var orcAnimations = new Dictionary<AnimationType, AnimationInfo>
            {
              { AnimationType.Idle,   new AnimationInfo(0, 6, 0.12f) },
              { AnimationType.Move,   new AnimationInfo(1, 8, 0.1f)  },
              { AnimationType.Attack, new AnimationInfo(2, 6, 0.09f) },
              { AnimationType.Hurt,   new AnimationInfo(4, 4, 0.1f)  },
              { AnimationType.Death,  new AnimationInfo(5, 4, 0.13f) }
            };

            var soldierAnimations = new Dictionary<AnimationType, AnimationInfo>
            {
              { AnimationType.Idle,   new AnimationInfo(0, 6, 0.12f) },
              { AnimationType.Move,   new AnimationInfo(1, 8, 0.1f) },
              { AnimationType.Attack, new AnimationInfo(2, 6, 0.1f) },
              { AnimationType.Hurt, new AnimationInfo(5, 4, 0.1f)   },
              { AnimationType.Death,  new AnimationInfo(6, 4, 0.1f) }
            };


            Texture2D map1 = Raylib.LoadTexture("Background/Map1/5.png");
            Texture2D soldier = Raylib.LoadTexture("Sprites/Characters/Soldier/Soldier/Soldier.png");
            Texture2D orc = Raylib.LoadTexture("Sprites/Characters/Orc/Orc/Orc.png");





            Player hero = new Player("Nikita", 50000000, 5, new Animator(soldier, soldierAnimations), startPosition, Constants.CHARACTER_SIZE);
            List<Enemy> enemies = new List<Enemy>
            {
               new Enemy("Orc", 50, 10, new Animator(orc, orcAnimations), new Vector2(700, 500), Constants.CHARACTER_SIZE, hero),
            };
            //Enemy enemy = new Enemy("Orc", 50, 50, new Animator(orc), new Vector2(500, 500), Constants.CHARACTER_SIZE);


            // Map1
            Rectangle mapSourceRect = new Rectangle(0, 0, map1.Width, map1.Height);

            Vector2 origin = new Vector2(0, 0);


            Raylib.InitAudioDevice();
            Music music = Raylib.LoadMusicStream("Music/Wav/Pixel 1.wav");
            //Raylib.PlayMusicStream(music);
            SoundManager.LoadSounds();

            GameManager gameManager = new GameManager(hero, enemies);

            Console.WriteLine("SoldierAnimations:");
            foreach (var kv in soldierAnimations)
                Console.WriteLine($"{kv.Key}: Row={kv.Value.SpriteRow}, Frames={kv.Value.FrameCount}, Interval={kv.Value.Interval}");

            Console.WriteLine("OrcAnimations:");
            foreach (var kv in orcAnimations)
                Console.WriteLine($"{kv.Key}: Row={kv.Value.SpriteRow}, Frames={kv.Value.FrameCount}, Interval={kv.Value.Interval}");


            Random rnd = new Random();
            int movement = rnd.Next(500, 1000);




            while (!Raylib.WindowShouldClose())
            {
                int currentWidth = Raylib.GetScreenWidth();
                int currentHeight = Raylib.GetScreenHeight();
                float deltaTime = Raylib.GetFrameTime();
                Rectangle mapDestRect = new Rectangle(0, 0, currentWidth, currentHeight);


                Raylib.UpdateMusicStream(music);

                gameManager.Update(deltaTime);



                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                Raylib.DrawTexturePro(map1, mapSourceRect, mapDestRect, origin, 0, new Color(255, 255, 255, 255));


                gameManager.Draw();

                if (Raylib.IsKeyPressed(KeyboardKey.S))
                {
                    enemies.Add(new Enemy("Orc", 50, 10, new Animator(orc, orcAnimations), new Vector2(rnd.Next(500, 800), 500), Constants.CHARACTER_SIZE, hero));
                }




                Raylib.EndDrawing();
            }

            SoundManager.UnloadAll();

            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
    }
}
