using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MouthController : MonoBehaviour
{
    [SerializeField] private List<Tooth> _teeth;

    [SerializeField] private List<MouthDefinition> _mouthDefinitions;

    [SerializeField] private bool _disableOnFinish = true;

    private int _definitionIndex = 0;

    private List<Tooth> _unmatchedTeeth;

    public UnityEvent AllDefinitionsMatched;

    private void OnEnable()
    {
        _definitionIndex = 0;

        var definition = _mouthDefinitions.First();
        SetDefinition(definition);
    }

    private void SetDefinition(MouthDefinition definition)
    {
        _unmatchedTeeth = definition.Apply(_teeth).ToList();

        foreach (Tooth tooth in _unmatchedTeeth)
            tooth.Matched.AddListener(OnToothMatched);
    }

    private void OnToothMatched(Tooth tooth)
    {
        tooth.Matched.RemoveListener(OnToothMatched);
        _unmatchedTeeth.Remove(tooth);

        if (!_unmatchedTeeth.Any())
            OnDefinitionFinished();
    }

    private void OnDefinitionFinished()
    {
        _definitionIndex++;
        if (_definitionIndex < _mouthDefinitions.Count)
        {
            MouthDefinition nextDefinition = _mouthDefinitions[_definitionIndex];
            SetDefinition(nextDefinition);
        }
        else
        {
            AllDefinitionsMatched?.Invoke();

            if (_disableOnFinish)
                enabled = false;
        }
    }
}