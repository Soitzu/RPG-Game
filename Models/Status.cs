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


        public void DrawCharacterWindow()
        {
            int windowX = 20;
            int windowY = 20;
            int windowWidth = 220;
            int windowHeight = 80;

            // Fenster-Hintergrund (hellgrau, leicht transparent)
            Raylib.DrawRectangle(windowX, windowY, windowWidth, windowHeight, new Color(230, 230, 230, 220));
            // Fenster-Rahmen (schwarz)
            Raylib.DrawRectangleLines(windowX, windowY, windowWidth, windowHeight, new Color(0, 0, 0, 255));

            string nameText = $"Name: {character.Name}";
            string healthText = $"Leben: {character.Health} / {maxHealth}";
            string strengthText = $"Stärke: {character.Strength}";

            int fontSize = 22;
            int lineSpacing = 28;

            // Name (schwarz)
            Raylib.DrawText(nameText, windowX + 12, windowY + 8, fontSize, new Color(0, 0, 0, 255));
            // Leben (dunkelgrün)
            Raylib.DrawText(healthText, windowX + 12, windowY + 8 + lineSpacing, fontSize, new Color(0, 120, 0, 255));
            // Stärke (dunkelblau)
            Raylib.DrawText(strengthText, windowX + 12, windowY + 8 + 2 * lineSpacing, fontSize, new Color(0, 0, 160, 255));
        }

    }
}
