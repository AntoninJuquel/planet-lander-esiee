using System;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceshipSystem
{
    public class SpaceshipManager : MonoBehaviour
    {
        [SerializeField] private GameObject spaceshipPrefab;

        [SerializeField] private Reference<int> lives, maxLives;
        [SerializeField] private Spaceship[] data;
        private GameObject _spaceship;
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
            _spaceship.GetComponent<SpaceshipCollision>().OnCrash += OnCrash;
            _spaceship.GetComponent<SpaceshipHealth>().OnDie += OnDie;
        }

        private void Unsubscribe()
        {
            _spaceship.GetComponent<SpaceshipCollision>().OnCrash -= OnCrash;
            _spaceship.GetComponent<SpaceshipHealth>().OnDie -= OnDie;
        }

        private void SpawnPlayer()
        {
            Data = data[maxLives.Value - lives.Value];
            var newPlayer = Instantiate(spaceshipPrefab, Vector3.up * 50f, Quaternion.identity);
            _spaceship = newPlayer;
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
            Destroy(_spaceship);
            if (lives <= 0)
            {
                //GameOver
                return;
            }

            SpawnPlayer();
        }
    }
}