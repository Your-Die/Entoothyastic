using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ToothSprite : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private List<GameObject> _dirtObjects;
    private List<GameObject> _unusedDirt;
    public SpriteToothController SpriteToothController;
    private bool isClean;

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
        if (_unusedDirt.Count == 0)
        {
            
            return;
        }

        if (_unusedDirt.Count == 1)
        {
            if (!isClean)
            {
                SpriteToothController.IncreaseCount();
                isClean = true;
            }
        }
        var dirt = _unusedDirt[Random.Range(0, _unusedDirt.Count)];
        _unusedDirt.Remove(dirt);
        dirt.SetActive(false);
    }
    public void ResetTooth()
    {
        foreach (var dirtObject in _dirtObjects)
        {
            dirtObject.SetActive(false);
        }
        isClean = false;
        _unusedDirt = new List<GameObject>(_dirtObjects);
        _unusedDirt.Shuffle();
        _unusedDirt = _unusedDirt.GetRange(0, Random.Range(0, _unusedDirt.Count));
        foreach (var o in _unusedDirt)
        {
            o.SetActive(true);
        }
//        foreach (var o in _unusedDirt)
//        {
//            o.SetActive(true);
//        }
    }
}
public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}