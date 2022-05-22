using System;
using UnityEngine;

namespace WaveSystem
{
    public interface IHandleWave
    {
        void OnNewWave(int delay);
        void OnWaveCleared();
        void OnWaveSpawn(GameObject toSpawn);
        void OnWaveFinished();

        event EventHandler<int> OnStartWave;
        event EventHandler OnStopWave;

        event EventHandler OnKill;
    }
}