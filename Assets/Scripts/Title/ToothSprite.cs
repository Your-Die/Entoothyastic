using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ToothSprite : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private List<GameObject> _dirtObjects;
    private List<GameObject> _unusedDirt;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        ResetTooth();
    }

    public void ToothClick()
    {
        _audioSource.clip = (_audioClips[Random.Range(0, _audioClips.Length)]);
        _audioSource.Play();
        SelectDirt();
        
    }

    private void SelectDirt()
    {
        if (_unusedDirt.Count == 0) return;
        var dirt = _unusedDirt[Random.Range(0, _unusedDirt.Count)];
        _unusedDirt.Remove(dirt);
        dirt.SetActive(true);
    }
    public void ResetTooth()
    {
        _unusedDirt = new List<GameObject>(_dirtObjects);
        foreach (var o in _unusedDirt)
        {
            o.SetActive(false);
        }
    }
}
