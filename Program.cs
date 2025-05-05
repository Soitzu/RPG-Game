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




            Vector2 startPosition = new Vector2(100 + 50, 100 + 50);
            Raylib.InitWindow(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, "Mini Adventurer");
            Texture2D map1 = Raylib.LoadTexture("Background/Map1/5.png");
            Texture2D soldier = Raylib.LoadTexture("Sprites/Characters/Soldier/Soldier/Soldier.png");
            Texture2D orc = Raylib.LoadTexture("Sprites/Characters/Orc/Orc/Orc.png");
            List<Enemy> enemies = new List<Enemy>
            {
               new Enemy("Orc", 50, 50, new Animator(orc), new Vector2(500, 500), Constants.CHARACTER_SIZE),
            };
            Character hero = new Character("Nikita", 10, 10, new Animator(soldier), startPosition, Constants.CHARACTER_SIZE);
            //Enemy enemy = new Enemy("Orc", 50, 50, new Animator(orc), new Vector2(500, 500), Constants.CHARACTER_SIZE);


            // Map1
            Rectangle mapSourceRect = new Rectangle(0, 0, map1.Width, map1.Height);
            Rectangle mapDestRect = new Rectangle(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            Vector2 origin = new Vector2(0, 0);


            Raylib.InitAudioDevice();
            Music music = Raylib.LoadMusicStream("Music/Wav/Pixel 1.wav");
            Raylib.PlayMusicStream(music);
            SoundManager.LoadSounds();

            GameManager gameManager = new GameManager(hero, enemies);




            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                Raylib.UpdateMusicStream(music);

                Raylib.BeginDrawing();
                Raylib.DrawTexturePro(map1, mapSourceRect, mapDestRect, origin, 0, new Color(255, 255, 255, 255));


                Raylib.ClearBackground(Color.White);

                Raylib.DrawRectangle(0, 500, Constants.SCREEN_WIDTH, 50, new Color(0, 0, 0, 255));



                //Raylib.DrawText($"Name: {hero.Name}, Health: {hero.Health}, Strength: {hero.Strength}", 12, 12, 20, Color.Black);
                gameManager.Update(deltaTime);
                hero.Draw();
                foreach (var enemy in enemies)
                {
                    enemy.Draw();
                }




                Raylib.EndDrawing();
            }

            SoundManager.UnloadAll();

            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }
    }
}
