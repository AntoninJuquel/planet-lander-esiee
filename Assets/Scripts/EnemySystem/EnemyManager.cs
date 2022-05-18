using System.Collections.Generic;
using System.Linq;
using ReferenceSharing;
using UnityEngine;
using WaveSystem;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour, IWaveHandler
    {
        [SerializeField] private Reference<int> killsRef, waveNumberRef, levelRef;
        [SerializeField] private Reference<bool> wavesFinishedRef;

        private Dictionary<Transform, Enemy> _enemies = new();
        private Transform _player;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void SpawnEnemy(Enemy enemy, Vector3 position)
        {
            var newEnemy = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.SetTarget(_player);
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

            _enemies = new Dictionary<Transform, Enemy>();
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
            SpawnEnemy(toSpawn.GetComponent<Enemy>(), Vector3.zero);
        }

        public void OnWaveFinished()
        {
            wavesFinishedRef.Value = true;
        }
    }
}