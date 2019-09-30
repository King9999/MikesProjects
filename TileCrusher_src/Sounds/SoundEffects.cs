using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace TileCrusher.Sounds
{
    class SoundEffects
    {
        ContentManager content;
        Dictionary<string, SoundEffect> sounds
                                = new Dictionary<string, SoundEffect>();

        public SoundEffects(ContentManager contentManager)
        {
            content = contentManager;
        }

        public void PlaySound(string sound)
        {
            if (!sounds.ContainsKey(sound))
            {
                sounds.Add(sound, content.Load<SoundEffect>(sound));
            }
            sounds[sound].Play();
        }
    }
}
