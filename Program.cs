using Raylib_cs;
using System.Numerics;
using Game.Models;


namespace HelloWorld;

public class Constants
{
    public const int SCREEN_WIDTH = 1280;
    public const int SCREEN_HEIGHT = 1080;
    public const int CHARACTER_SIZE = 100;
}




class Program
{
    public static void Main()
    {
        Vector2 startPosition = new Vector2(100 + 50, 100 + 50);
        Raylib.InitWindow(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, "Mini Adventurer");
        Texture2D soldier = Raylib.LoadTexture("Sprites/Characters/Soldier/Soldier/Soldier.png");
        Texture2D orc = Raylib.LoadTexture("Sprites/Characters/Orc/Orc/Orc.png");
        Character hero = new Character("Nikita", 50, 50, new Animator(soldier), startPosition, Constants.CHARACTER_SIZE);
        Enemy enemy = new Enemy("Orc", 50, 50, new Animator(orc), new Vector2(500, 500), Constants.CHARACTER_SIZE);
        Raylib.InitAudioDevice();
        Music music = Raylib.LoadMusicStream("Music/Wav/Pixel 1.wav");
        Raylib.PlayMusicStream(music);

        while (!Raylib.WindowShouldClose())
        {
            float deltaTime = Raylib.GetFrameTime();
            Raylib.UpdateMusicStream(music);



            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            Raylib.DrawRectangle(0, 500, Constants.SCREEN_WIDTH, 50, new Color(0, 0, 0, 255));

            //Raylib.DrawText($"Name: {hero.Name}, Health: {hero.Health}, Strength: {hero.Strength}", 12, 12, 20, Color.Black);
            hero.Update(deltaTime);
            hero.Draw();
            enemy.UpdateAI(deltaTime);
            enemy.Draw();



            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
