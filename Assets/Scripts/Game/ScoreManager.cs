using ReferenceSharing;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    internal struct Score<T>
    {
        public Reference<T> valueRef;
        public float coefficient;
    }

    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Reference<int> scoreRef, bestScoreRef, levelRef;
        [SerializeField] private Score<float> timer, fuel, fuelBurnt, accuracy;
        [SerializeField] private Score<int> deaths;

        private void LoadBestScore()
        {
            bestScoreRef.Value = PlayerPrefs.GetInt($"best_score_{levelRef.Value}", 0);
        }

        private void SaveBestScore()
        {
            bestScoreRef.Value = scoreRef.Value;
            PlayerPrefs.SetInt($"best_score_{levelRef.Value}", bestScoreRef.Value);
        }

        private void SetScoreRef()
        {
            var timeScore = timer.valueRef / (timer.coefficient + timer.valueRef);
            var fuelScore = (fuel.coefficient + fuel.valueRef) / (fuelBurnt.coefficient + fuelBurnt.valueRef);
            var deathsScore = deaths.coefficient * deaths.valueRef;
            scoreRef.Value = (int) ((timeScore + fuelScore + deathsScore) * accuracy.valueRef);

            if (scoreRef.Value < bestScoreRef.Value) return;

            SaveBestScore();
        }

        public void OnStart()
        {
            LoadBestScore();
        }

        public void OnWin()
        {
            SetScoreRef();
        }
    }
}