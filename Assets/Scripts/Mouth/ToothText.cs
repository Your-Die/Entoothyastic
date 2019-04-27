using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ToothText : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private Color _matchedTextColor = Color.red;

    [SerializeField] private int _requiredRepetitions = 1;

    [SerializeField] private bool _disableOnSuccess = true;

    [SerializeField] private ToothInfo _toothInfo;
    [SerializeField] private TMP_Text _textField;

    private InputHandler _inputHandler;

    private int _characterIndex;

    private int _repetitions = 0;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Reset();
        }
    }

    public bool TextActive
    {
        set => _textField.gameObject.SetActive(value);
    }

    [FormerlySerializedAs("OnInputSuccess")]
    public Event Matched;
    public Event RepetitionsCompleted;

    private void OnEnable()
    {
        if (_textField == null)
            _textField = GetComponentInChildren<TMP_Text>();
        if (_toothInfo == null)
            _toothInfo = GetComponent<ToothInfo>();

        TextActive = true;
        Reset();
    }

    private void OnDisable() => TextActive = false;

    private void Reset()
    {
        ResetRepetitions();
        ResetMatching();
    }

    private void Update()
    {
        if (InputHandler.Instance.TryGetKey(out var input))
            MatchCharacter(input.Value);
    }

    private void UpdateText()
    {
        string matched = this.Text.Substring(0, _characterIndex);
        string notMatched = this.Text.Substring(_characterIndex);

        string colored = TMPHelper.WrapColor(matched, _matchedTextColor);

        _textField.text = colored + notMatched;
    }

    private void MatchCharacter(char input)
    {
        if (input.EqualsIgnoreCase(this.Text[_characterIndex]))
            OnCharacterMatched();
        else
            ResetMatching();
    }

    private void ResetMatching()
    {
        _characterIndex = 0;
        _textField.text = this.Text;

        this.enabled = string.IsNullOrEmpty(this.Text) == false;
    }

    private void ResetRepetitions() => _repetitions = 0;

    private void OnCharacterMatched()
    {
        _toothInfo.OnSuccessfullyBrushed_1x.AddListener(OnBrushComplete);
        InputHandler.Instance.AddInhibitor(this);

        _characterIndex++;

        UpdateText();

        if (_characterIndex >= Text.Length)
            OnMatched();
    }

    private void OnBrushComplete(ToothInfo info)
    {
        InputHandler.Instance.RemoveInhibitor(this);
    }

    private void OnMatched()
    {
        _repetitions++;
        Matched?.Invoke(this);

        if (_repetitions < _requiredRepetitions)
            ResetMatching();
        else
            OnRepetitionsCompleted();
    }

    private void OnRepetitionsCompleted()
    {
        RepetitionsCompleted?.Invoke(this);

        if (_disableOnSuccess)
            enabled = false;
    }

    [Serializable]
    public class Event : UnityEvent<ToothText> { }
}
