using Raylib_cs;
using System.Numerics;

namespace Game.Models
{

    public class Status
    {
        private Character character;
        private int maxHealth;


        public Status(Character character)
        {
            this.character = character;
            this.maxHealth = character.Health;
        }

        public void Draw()
        {
            Vector2 pos = character.Physics.Position;
            float width = 100;
            float height = 10;
            float healthPercent = (float)character.Health / maxHealth;

            // Balkenhintergrund
            Raylib.DrawRectangle((int)pos.X - 50, (int)pos.Y - 80, (int)width, (int)height, new Color(100, 100, 100, 255));

            // Gesundheitsbalken
            Raylib.DrawRectangle((int)pos.X - 50, (int)pos.Y - 80, (int)(width * healthPercent), (int)height, new Color(255, 0, 0, 255));

            // Rahmen (optional)
            Raylib.DrawRectangleLines((int)pos.X - 50, (int)pos.Y - 80, (int)width, (int)height, new Color(0, 0, 0, 255));
        }


    }
}
