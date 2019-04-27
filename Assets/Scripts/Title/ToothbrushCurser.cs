using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothbrushCurser : MonoBehaviour
{
    [SerializeField] private float _clickDistance;

    [SerializeField] private AudioClip _audioClip;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) AudioManager.instance.Play("Toothbrush");
        if (Input.GetMouseButton(0)) transform.position = Input.mousePosition + new Vector3(0, -_clickDistance);
        else transform.position = Input.mousePosition;
    }
}
