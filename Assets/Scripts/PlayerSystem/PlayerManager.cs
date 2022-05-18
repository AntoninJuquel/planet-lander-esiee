using System;
using ReferenceSharing;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Reference<int> lives, maxLives;

        private GameObject _player;

        private void Start()
        {
            lives.Value = maxLives.Value;
            SpawnPlayer();
        }

        private void Subscribe()
        {
            _player.GetComponent<PlayerCollision>().OnCrash += OnCrash;
            _player.GetComponent<PlayerHealth>().OnDie += OnDie;
        }

        private void Unsubscribe()
        {
            _player.GetComponent<PlayerCollision>().OnCrash -= OnCrash;
            _player.GetComponent<PlayerHealth>().OnDie -= OnDie;
        }

        private void SpawnPlayer()
        {
            var newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            _player = newPlayer;
            Subscribe();
        }

        private void OnCrash(object sender, EventArgs args)
        {
            DestroyPlayer();
        }

        private void OnDie(object sender, EventArgs args)
        {
            DestroyPlayer();
        }

        private void DestroyPlayer()
        {
            Unsubscribe();
            lives.Value--;
            if (lives <= 0)
            {
                //GameOver
                return;
            }

            SpawnPlayer();
        }
    }
}