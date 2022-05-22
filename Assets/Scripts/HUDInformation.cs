using System.Collections;
using TMPro;
using UnityEngine;


public class HUDInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI land, wave;

    public void OnNewWave(int delay)
    {
        if (delay == 0) return;
        var values = new string[delay + 1];

        for (var i = 0; i < delay + 1; i++)
        {
            values[i] = $"PROCHAINE VAGUE DANS\n{delay - i}";
        }

        StartCoroutine(wave.AnimateText(1f, values, false));
    }

    public void OnWaveFinished()
    {
        StartCoroutine(land.AnimateText(2f, new[] {"SE POSER", ">SE POSER<", ">>SE POSER<<", ">>>SE POSER<<<"}));
    }
}

public static class TextMeshProExtension
{
    public static IEnumerator AnimateText(this TextMeshProUGUI text, float rate, string[] values, bool loop = true)
    {
        text.enabled = true;
        var waiteForSeconds = new WaitForSeconds(1f / rate);
        for (var i = 0; i < values.Length && text.enabled; i = loop ? (i + 1) % values.Length : i + 1)
        {
            text.text = values[i];
            yield return waiteForSeconds;
        }

        text.enabled = false;
    }
}