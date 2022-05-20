﻿using System;
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

        public event EventHandler<int> OnStartWave;
        public event EventHandler OnStopWave;

        private Dictionary<Transform, GameObject> _enemies = new();
        private Transform _player;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void SpawnEnemy(GameObject enemy)
        {
            var position = (Vector2) _mainCamera.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(1.5f, 2f)));
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
            OnStopWave?.Invoke(this, null);
        }

        public void StartEnemyManager()
        {
            OnStartWave?.Invoke(this, levelRef.Value);
        }

        public void StopEnemyManager()
        {
            KillAll();
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
            SpawnEnemy(toSpawn);
        }

        public void OnWaveFinished()
        {
            wavesFinishedRef.Value = true;
        }
    }
}