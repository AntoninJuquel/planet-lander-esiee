﻿using System;
using System.Collections.Generic;
using System.Linq;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.Events;
using WaveSystem;

namespace EnemySystem
{
    public class EnemyManager : MonoBehaviour, IHandleWave
    {
        [SerializeField] private Reference<int> killsRef, waveNumberRef, levelRef;
        [SerializeField] private Reference<bool> wavesFinishedRef;

        [SerializeField] private UnityEvent<Vector2> onEnemyDestroy;

        public event EventHandler<int> OnStartWave;
        public event EventHandler OnStopWave;
        public event EventHandler OnKill;

        private Dictionary<Transform, Enemy> _enemies = new();
        private Transform _player;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void SpawnEnemy(GameObject enemy)
        {
            var position = (Vector2) _mainCamera.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(1.5f, 2f)));
            var newEnemy = Instantiate(enemy, position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.GetComponent<EnemyHealth>().OnDie += OnEnemyDie;
            newEnemy.SetTarget(_player);
            _enemies.Add(newEnemy.transform, newEnemy);
        }

        private void OnEnemyDie(object sender, EventArgs args)
        {
            if (sender is EnemyHealth enemyHealth)
            {
                onEnemyDestroy?.Invoke(enemyHealth.transform.position);
                DestroyEnemy(enemyHealth.transform);
            }
        }

        private void DestroyEnemy(Transform enemy)
        {
            enemy.GetComponent<EnemyHealth>().OnDie -= OnEnemyDie;
            OnKill?.Invoke(this, null);
            Destroy(_enemies[enemy].gameObject);
            _enemies.Remove(enemy);
            killsRef.Value++;
        }

        private void KillAll()
        {
            var enemies = _enemies.Keys.ToArray();

            for (var i = enemies.Length - 1; i >= 0; i--)
            {
                DestroyEnemy(enemies[i]);
            }

            killsRef.Value = 0;

            _enemies = new Dictionary<Transform, Enemy>();
            OnStopWave?.Invoke(this, null);
        }

        public void StartEnemyManager()
        {
            OnStartWave?.Invoke(this, levelRef.Value);
            wavesFinishedRef.Value = false;
            waveNumberRef.Value = 0;
        }

        public void StopEnemyManager()
        {
            KillAll();
        }

        public void OnNewWave(int delay)
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

        public void OnPlayerSpawn(GameObject player)
        {
            _player = player.transform;
            foreach (var enemy in _enemies)
            {
                enemy.Value.SetTarget(_player);
            }
        }
    }
}