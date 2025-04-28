using Raylib_cs;
using System.Numerics;
using Game.Models;


namespace HelloWorld;

public class Constants
{
  public const int SCREEN_WIDTH = 1280;
  public const int SCREEN_HEIGHT = 1080;
}




class Program
{
    public static void Main()
    {
        Vector2 startPosition = new Vector2(100 + 50, 100 + 50);
        Raylib.InitWindow(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, "Mini Adventurer");
        Texture2D soldier = Raylib.LoadTexture("Sprites/Characters/Soldier/Soldier/Soldier.png");
        Character hero = new Character("Nikita", 50, 50, new Animator(soldier), startPosition);
        

        while (!Raylib.WindowShouldClose())
        {
            float deltaTime = Raylib.GetFrameTime();



            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            //Raylib.DrawText($"Name: {hero.Name}, Health: {hero.Health}, Strength: {hero.Strength}", 12, 12, 20, Color.Black);
            hero.Update(deltaTime);
            hero.Draw();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
