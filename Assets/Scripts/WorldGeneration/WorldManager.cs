using System.Collections.Generic;
using ChunkSystem;
using ReferenceSharing;
using UnityEngine;

namespace WorldGeneration
{
    public class WorldManager : MonoBehaviour, IHandleChunk
    {
        [SerializeField] private Reference<int> levelRef;
        [SerializeField] private WorldPart worldPartPrefab;
        [SerializeField] private World[] worldPresets;
        private readonly Dictionary<Vector2, WorldPart> _worldParts = new Dictionary<Vector2, WorldPart>();
        private int PresetIndex => levelRef.Value % worldPresets.Length;

        public void ChunkCreatedHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            var start = e.center.x - e.size.x * .5f;
            var end = e.center.x + e.size.y * .5f;
            GenerateWorldPart(start, end, e.center.x);
        }

        public void ChunkEnabledHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            _worldParts[e.center].gameObject.SetActive(true);
        }

        public void ChunkDisabledHandler(Bounds e)
        {
            if (e.center.y != 0) return;
            _worldParts[e.center].gameObject.SetActive(false);
        }

        private void StartGameHandler()
        {
            foreach (var kvp in _worldParts)
            {
                kvp.Value.Generate(worldPresets[PresetIndex]);
            }

            Generate(PresetIndex);
        }

        private void Generate(int index)
        {
            Physics2D.gravity = Vector2.up * worldPresets[index].gravity;
        }

        private void GenerateWorldPart(float startX, float endX, float centerX)
        {
            Vector2 position = Vector3.right * centerX;
            var worldPart = Instantiate(worldPartPrefab, Vector3.zero, Quaternion.identity, transform);
            worldPart.Generate(worldPresets[PresetIndex], startX, endX);
            _worldParts.Add(position, worldPart);
        }
    }
}