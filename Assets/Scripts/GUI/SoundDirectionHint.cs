using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the "hint" breathing animation shown in the headphone check screen
/// </summary>
public class SoundDirectionHint : MonoBehaviour
{
    [SerializeField]
    private Image hint;

    [SerializeField]
    private float fadeTime = 1.0f;

    private float alphaTarget = 0.0f;

    private Coroutine currentCoroutine;

    private bool couroutineRunning = false;

    private void Awake()
    {
        currentCoroutine = StartCoroutine(Fade(alphaTarget));
    }

    private void Update()
    {
        if (couroutineRunning)
        {
            return;
        }

        if (hint.color.a >= 1.0)
        {
            alphaTarget = 0.0f;
        }

        if (hint.color.a <= 0.0)
        {
            alphaTarget = 1.0f;
        }

        currentCoroutine = StartCoroutine(Fade(alphaTarget));
    }

    private IEnumerator Fade(float target)
    {
        float start = hint.color.a;
        float counter = 0f;

        couroutineRunning = true;
        while (counter < fadeTime)
        {
            counter += Time.deltaTime;

            float newAlpha = Mathf.Lerp(start, target, counter / fadeTime);
            hint.color = new Color(hint.color.r, hint.color.g, hint.color.b, newAlpha);

            yield return null;
        }
        couroutineRunning = false;
    }

}
