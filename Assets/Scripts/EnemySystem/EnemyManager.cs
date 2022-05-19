using System;
using System.Collections.Generic;
using System.Linq;
using ReferenceSharing;
using UnityEngine;
using WaveSystem;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour, IHandleWave
    {
        [SerializeField] private Reference<int> killsRef, waveNumberRef, levelRef;
        [SerializeField] private Reference<bool> wavesFinishedRef;

        public event EventHandler<int> OnStartEncounter;

        private Dictionary<Transform, GameObject> _enemies = new();
        private Transform _player;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void SpawnEnemy(GameObject enemy, Vector3 position)
        {
            var newEnemy = Instantiate(enemy, position, Quaternion.identity);
            _enemies.Add(newEnemy.transform, newEnemy);
        }

        private void DestroyEnemy(Transform enemy, bool killCount)
        {
            Destroy(_enemies[enemy].gameObject);
            _enemies.Remove(enemy);
            if (killCount)
                killsRef.Value++;
        }

        private void KillAll()
        {
            var enemies = _enemies.Keys.ToArray();

            for (var i = enemies.Length - 1; i >= 0; i--)
            {
                DestroyEnemy(enemies[i], false);
            }

            _enemies = new Dictionary<Transform, GameObject>();
        }

        public void OnNewWave()
        {
            waveNumberRef.Value++;
        }

        public void OnWaveCleared()
        {
        }

        public void OnWaveSpawn(GameObject toSpawn)
        {
            SpawnEnemy(toSpawn, Vector3.zero);
        }

        public void OnWaveFinished()
        {
            wavesFinishedRef.Value = true;
        }
    }
}