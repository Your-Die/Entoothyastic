using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothCleanliness : MonoBehaviour {
    

    [Range(0, 1)]
    public float cleanliness;
    [SerializeField] float fadeOutTime = 1;
    public Material mat;

    private void Start() {
        mat = gameObject.GetComponent<Renderer>().material;

        mat.SetFloat("_NoiceSeed", Random.Range(3f, 50f));

    }

    public void ToothCleaned() {
        StartCoroutine(FadeOutDirt());
    }

    

    IEnumerator FadeOutDirt() {
        // fade from opaque to transparent

        // loop over 1 second backwards
        for ( ; fadeOutTime <= 1; fadeOutTime += Time.deltaTime) {
            cleanliness = fadeOutTime;
            // set color with i as alpha
            mat.SetFloat("_fadeValue", cleanliness);
            yield return null;
        }
    }
}
