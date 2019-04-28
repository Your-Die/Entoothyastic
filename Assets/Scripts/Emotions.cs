using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Emotions : MonoBehaviour
{
    [SerializeField] private float _failureAmount = 30;
    [SerializeField] private float _successAmount = 10;

    [SerializeField] private SkinnedMeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void OnEnable()
    {
        ToothController.ToothMatched += OnToothMatched;
        InputHandler.Instance.InvalidInputRegistered.AddListener(OnFailure);
    }


    private void OnDisable()
    {
        ToothController.ToothMatched -= OnToothMatched;
        InputHandler.Instance.InvalidInputRegistered.RemoveListener(OnFailure);
    }

    private void OnToothMatched()
    {
        var blendShapes = GetValidBlendShapes(blendWeight => blendWeight > 0).ToList();
        if (!blendShapes.Any())
            return;

        (int index, float weight) = blendShapes.ChooseRandom();
        float newWeight = weight - _successAmount;

        SetWeight(index, newWeight);
    }

    private void OnFailure()
    {
        var blendShapes = GetValidBlendShapes(blendWeight => blendWeight < 100).ToList();
        if (!blendShapes.Any())
            return;

        (int index, float weight) = blendShapes.ChooseRandom();
        var newWeight = weight + _failureAmount;

        SetWeight(index, newWeight);
    }

    private void SetWeight(int index, float value)
    {
        var clamped = Mathf.Clamp(value, 0, 100);
        _renderer.SetBlendShapeWeight(index, clamped);
    }

    private IEnumerable<(int index, float weight)> GetValidBlendShapes(Func<float, bool> predicate)
    {
        int[] indices = {0, 1};
        return indices.Select(index => (index, _renderer.GetBlendShapeWeight(index)))
            .Where(tuple => predicate(tuple.Item2));
    }
}