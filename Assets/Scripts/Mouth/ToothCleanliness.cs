using System.Collections;
using UnityEngine;

public class ToothCleanliness : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float cleanliness;
    [SerializeField] private float fadeOutTime = 1;
    public Material mat;

    public float Cleanliness
    {
        get => cleanliness;
        set => StartFadeTo(value);
    }

    private void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        mat.SetFloat("_NoiceSeed", Random.Range(3f, 50f));
    }

    public void ResetAll()
    {
        cleanliness = 0;

        if (mat != null)
            mat.SetFloat("_fadeValue", cleanliness);
    }

    private void StartFadeTo(float value)
    {
        float difference = Mathf.Abs(cleanliness - value);
        difference = Mathf.Clamp01(difference);

        float duration = difference * fadeOutTime;
        IEnumerator fadeRoutine = FadeOutDirt(value, duration);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeOutDirt(float targetValue, float duration)
    {
        float startValue = cleanliness;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float point = elapsedTime / duration;
            cleanliness = Mathf.Lerp(startValue, targetValue, point);
            mat.SetFloat("_fadeValue", cleanliness);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
