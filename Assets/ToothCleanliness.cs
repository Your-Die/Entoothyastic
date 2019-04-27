using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothCleanliness : MonoBehaviour
{
    public GameObject tooth;

    [Range(0,1)]
    public float cleanliness;

    public Material mat;

    private void Start() {
        mat = tooth.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("Vector1_C6895969", cleanliness);
        Debug.Log(mat.GetFloat("Vector1_C6895969"));
    }
}
