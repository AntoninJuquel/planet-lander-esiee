using UnityEngine;

namespace WaveSystem
{
    public interface IWaveHandler
    {
        void OnNewWave();
        void OnWaveCleared();
        void OnWaveSpawn(GameObject toSpawn);
        void OnWaveFinished();
    }
}