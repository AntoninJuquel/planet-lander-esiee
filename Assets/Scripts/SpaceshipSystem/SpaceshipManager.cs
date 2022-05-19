using System;
using ReferenceSharing;
using UnityEngine;

namespace SpaceshipSystem
{
    public class SpaceshipManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Reference<int> lives, maxLives;
        [SerializeField] private Spaceship[] data;
        private GameObject _player;
        public static Spaceship Data;

        private void Awake()
        {
            lives.Value = maxLives.Value = data.Length;
        }

        private void Start()
        {
            SpawnPlayer();
        }

        private void Subscribe()
        {
            _player.GetComponent<SpaceshipCollision>().OnCrash += OnCrash;
            _player.GetComponent<SpaceshipHealth>().OnDie += OnDie;
        }

        private void Unsubscribe()
        {
            _player.GetComponent<SpaceshipCollision>().OnCrash -= OnCrash;
            _player.GetComponent<SpaceshipHealth>().OnDie -= OnDie;
        }

        private void SpawnPlayer()
        {
            Data = data[maxLives.Value - lives.Value];
            var newPlayer = Instantiate(playerPrefab, Vector3.up * 50f, Quaternion.identity);
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
            Destroy(_player);
            if (lives <= 0)
            {
                //GameOver
                return;
            }

            SpawnPlayer();
        }
    }
}