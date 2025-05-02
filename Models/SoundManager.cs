using Raylib_cs;
using System.Collections.Generic;

namespace Game.Models
{
    public class SoundManager
    {
        private static Dictionary<string, Sound> sounds = new();




        // 
        public static void LoadSounds()
        {
            sounds["attack_swing"] = Raylib.LoadSound("SoundEffects/Soldier/Attack/swing.wav");
        }

        public static void PlaySound(string key)
        {
            if (sounds.ContainsKey(key))
            {
                Raylib.PlaySound(sounds[key]);
            }
        }

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
