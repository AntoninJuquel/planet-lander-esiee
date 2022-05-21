using System;
using UnityEngine;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static EventHandler<float> OnMusicVolumeChange, OnSoundEffectVolumeChange;

        private void Start()
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicSlider", .5f));
            SetSoundEffectVolume(PlayerPrefs.GetFloat("EffectsSlider", .5f));
        }

        public void SetMusicVolume(float value)
        {
            OnMusicVolumeChange?.Invoke(this, value);
        }

        public void SetSoundEffectVolume(float value)
        {
            OnSoundEffectVolumeChange?.Invoke(this, value);
        }
    }
}