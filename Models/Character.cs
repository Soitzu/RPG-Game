using Raylib_cs;
using System.Numerics;

namespace Game.Models
{
  public class Character
  {
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int Strength { get; private set; }
    public Animator Animator { get; private set; }
    public int SoldierSpriteRow { get; private set; }
    public int Rotation { get; private set; }

    public bool IsFacingLeft = false;
    public Vector2 Position { get; private set; }
    
 

    public Character(string name, int health, int strength, Animator animator, Vector2 startPosition)
    {
      Name = name;
      Health = health;
      Strength = strength;
      Animator = animator;
      Position = startPosition;
    }



    public void Attack(Character target)
    {
      target.Health -= Strength;
    }

    public void Update(float deltaTime)
    {
      float speed = 200f;
      SoldierSpriteRow = 0;

      if (Raylib.IsKeyDown(KeyboardKey.Right))
      {
        IsFacingLeft = false;
        SoldierSpriteRow = 1;
        Position += new Vector2(speed * deltaTime, 0);
      }
      if (Raylib.IsKeyDown(KeyboardKey.Left))
      {
        IsFacingLeft = true;
        SoldierSpriteRow = 1;
        Position -= new Vector2(speed * deltaTime, 0);
      }
      Animator.Update(deltaTime);
    }

    public void Draw()
    {
      Animator.Draw(Position, SoldierSpriteRow, IsFacingLeft);
    }


  }
}
