using System.Collections;
using UnityEngine;

namespace AudioSystem
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private AnimationCurve fading;
        private AudioSource[] _sources = new AudioSource[2];
        private Coroutine _fade;
        private float _volume;

        private void Awake()
        {
            for (var i = 0; i < _sources.Length; i++)
            {
                _sources[i] = gameObject.AddComponent<AudioSource>();
                _sources[i].loop = true;
                _sources[i].playOnAwake = false;
                _sources[i].Stop();
            }
        }

        private void OnEnable()
        {
            AudioManager.OnMusicVolumeChange += AdjustVolume;
        }

        private void OnDisable()
        {
            AudioManager.OnMusicVolumeChange -= AdjustVolume;
        }

        private void AdjustVolume(object sender, float value)
        {
            _volume = value;
            _sources[1].volume = value;
        }

        private IEnumerator Fade(int to)
        {
            (_sources[0], _sources[1]) = (_sources[1], _sources[0]);
            _sources[1].volume = 0f;
            _sources[1].clip = clips[to];
            _sources[1].Play();
            var time = 0f;
            var maxTime = fading.keys[^1].time;
            while (time < maxTime)
            {
                time += Time.deltaTime;
                _sources[1].volume = fading.Evaluate(time) * _volume;
                _sources[0].volume = fading.Evaluate(maxTime - time) * _volume;
                yield return null;
            }

            _sources[0].Stop();
            _sources[0].volume = 0f;
        }

        public void Play(int index)
        {
            index %= clips.Length;
            if (_fade != null) StopCoroutine(_fade);
            _fade = StartCoroutine(Fade(index));
        }

        public void Stop()
        {
            _sources[1].Stop();
            _sources[0].Stop();
        }
    }
}