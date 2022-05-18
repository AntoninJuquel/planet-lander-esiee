using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private World[] worldPresets;
        [SerializeField] private float scrollSpeed;
        private Material _material;
        private Vector3 _offset;
        private World CurrentWorld => worldPresets[levelRef.Value % worldPresets.Length];

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            var offset = _offset + transform.position * scrollSpeed;

            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                _material.SetVector($"_{noiseSettings.name}Offset", offset);
            }
        }

        private void MainMenuHandler()
        {
            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                _material.SetColor($"_{noiseSettings.name}Color", Color.black);
            }
        }

        private void StartGameHandler()
        {
            _offset = new Vector3(Random.Range(-999f, 999f), Random.Range(-999f, 999f));

            foreach (var noiseSettings in CurrentWorld.noiseSettings)
            {
                _material.SetColor($"_{noiseSettings.name}Color", CurrentWorld.backgroundColor);
                _material.SetFloat($"_{noiseSettings.name}Strength", noiseSettings.strength);
                _material.SetFloat($"_{noiseSettings.name}Roughness", noiseSettings.roughness);
            }
        }
    }
}