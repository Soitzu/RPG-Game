using Raylib_cs;
namespace Game.Models
{
    public class SoundManager
    {
        private static Dictionary<string, Sound> sounds = new();




        // Load all Sounds
        public static void LoadSounds()
        {
            sounds["attack_swing"] = Raylib.LoadSound("SoundEffects/Soldier/Attack/swing.wav");
        }

        // Play the Sound
        public static void PlaySound(string key)
        {
            if (sounds.ContainsKey(key))
            {
                Raylib.PlaySound(sounds[key]);
            }
        }

        // Unload all Sounds at the end of the program
        public static void UnloadAll()
        {
            foreach (var sound in sounds.Values)
            {
                Raylib.UnloadSound(sound);
            }
            sounds.Clear();
        }



    }
}
