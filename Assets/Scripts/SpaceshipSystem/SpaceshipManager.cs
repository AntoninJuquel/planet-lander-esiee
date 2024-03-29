﻿using System;
using ReferenceSharing;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceshipSystem
{
    public class SpaceshipManager : MonoBehaviour
    {
        [SerializeField] private GameObject spaceshipPrefab;

        [SerializeField] private Reference<int> livesRef, maxLivesRef, shotsRef, hitsRef, deathsRef;
        [SerializeField] private Reference<float> fuelRef, maxFuelRef, fuelBurntRef;
        [SerializeField] private Reference<bool> landRef;
        [SerializeField] private Spaceship[] data;
        [SerializeField] private float maxFuel;
        [SerializeField] private UnityEvent<GameObject> onSpaceshipSpawn;
        [SerializeField] private UnityEvent onLivesEmpty, onSpaceshipDestroy;
        private GameObject _spaceship;
        public static Spaceship Data;

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

        private void SpawnSpaceship()
        {
            Data = data[maxLivesRef.Value - livesRef.Value];
            var newPlayer = Instantiate(spaceshipPrefab, Vector3.up * 50f, Quaternion.identity);
            _spaceship = newPlayer;
            onSpaceshipSpawn?.Invoke(_spaceship);
            Subscribe();
        }

        private void OnCrash(object sender, EventArgs args)
        {
            DestroySpaceship();
            CheckRespawn();
        }

        private void OnDie(object sender, EventArgs args)
        {
            DestroySpaceship();
            CheckRespawn();
        }

        private void DestroySpaceship(bool withInvoke = true)
        {
            if (withInvoke)
                onSpaceshipDestroy?.Invoke();
            Unsubscribe();
            Destroy(_spaceship);
        }

        private void CheckRespawn()
        {
            livesRef.Value--;
            deathsRef.Value++;
            if (livesRef.Value <= 0)
            {
                onLivesEmpty?.Invoke();
                return;
            }

            SpawnSpaceship();
        }

        private void ResetRefs()
        {
            livesRef.Value = maxLivesRef.Value = data.Length;
            fuelRef.Value = maxFuelRef.Value = maxFuel;

            hitsRef.Value = 0;
            fuelBurntRef.Value = 0;
            shotsRef.Value = 0;
            deathsRef.Value = 0;
            landRef.Value = false;
        }

        public void StartSpaceshipManager()
        {
            ResetRefs();
            SpawnSpaceship();
        }

        public void StopSpaceshipManager()
        {
            if (_spaceship)
                DestroySpaceship(false);
        }
    }
}