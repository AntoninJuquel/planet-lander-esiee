using UnityEngine;

namespace AudioSystem
{
    public class SoundEffectPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        private AudioSource _source;

        private void Awake()
        {
            _source = gameObject.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.Stop();
        }

        private void OnEnable()
        {
            AudioManager.OnSoundEffectVolumeChange += AdjustVolume;
        }

        private void OnDisable()
        {
            AudioManager.OnSoundEffectVolumeChange -= AdjustVolume;
        }

        private void AdjustVolume(object sender, float value)
        {
            _source.volume = value;
        }

        public void Play(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }

        public void Play(int index)
        {
            Play(clips[index]);
        }

        public void PlayRandom()
        {
            Play(Random.Range(0, clips.Length));
        }
    }
}