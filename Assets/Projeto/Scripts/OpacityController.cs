using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpacityController : MonoBehaviour
{
    public Image targetImage;
    public float fadeSpeed = 2f;
    private Coroutine fadeCoroutine;

    void Start()
    {
        Color color = targetImage.color;
        color.a = 0f;
        targetImage.color = color;
    }

    public void FadeInAndOut()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeTo(1f)); 
        yield return new WaitForSeconds(0.5f);    
        yield return StartCoroutine(FadeTo(0f));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        while (!Mathf.Approximately(targetImage.color.a, targetAlpha))
        {
            Color color = targetImage.color;
            color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            targetImage.color = color;
            yield return null;
        }
    }
}