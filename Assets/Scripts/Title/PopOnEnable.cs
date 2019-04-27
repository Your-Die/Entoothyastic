using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnEnable : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _animationSpeed;
    private float _timeOnEnable;
    private void OnEnable()
    {
        _timeOnEnable = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * _animationCurve.Evaluate((Time.time - _timeOnEnable) * _animationSpeed);
    }
}
