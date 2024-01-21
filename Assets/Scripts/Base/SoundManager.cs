using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<Sound> _audioClips;

        public void Init()
        {
            
        }

        public void PlaySound(SoundName soundName)
        {
            var sound = _audioClips.Find(s => s.Name == soundName);
            if (sound != null)
            {
                _audioSource.PlayOneShot(sound.Clip);
            }
        }

        public void Dispose()
        {
            
        }
    }

    [Serializable]
    public class Sound
    {
        public SoundName Name;
        public AudioClip Clip;
    }
}