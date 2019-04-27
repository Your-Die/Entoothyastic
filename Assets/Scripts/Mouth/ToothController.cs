using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ToothController : MonoBehaviour
{
    /// <summary>
    /// Text field to display the text.
    /// </summary>
    [SerializeField] private string _text;
    /// <summary>
    /// The color we display text in that has already been typed.
    /// </summary>
    [SerializeField] private Color _matchedTextColor = Color.red;
    
    /// <summary>
    /// The amount of times the player needs to repeat the <see cref="_text"/>.
    /// </summary>
    [SerializeField] private int _requiredRepetitions = 1;

    [SerializeField] private bool _disableOnSuccess = true;

    [SerializeField] private ToothInfo _toothInfo;
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private ToothCleanliness _cleanliness;

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

    public static event Action CharacterMatched;

    private void OnEnable()
    {
        // Get components.
        if (_textField == null)
            _textField = GetComponentInChildren<TMP_Text>();
        if (_toothInfo == null)
            _toothInfo = GetComponent<ToothInfo>();
        if (_cleanliness == null)
            _cleanliness = GetComponentInChildren<ToothCleanliness>();

        // Setup text.
        TextActive = true;
        Reset();
    }

    private void OnDisable() => TextActive = false;

    private void Reset()
    {
        ResetRepetitions();
        ResetMatching();
        ResetDirtiness();
    }


    private void Update()
    {
        // Read input.
        if (InputHandler.Instance.TryGetKey(out var input))
            MatchCharacter(input.Value);
    }

    /// <summary>
    /// Update how we present the text.
    /// </summary>
    private void UpdateText()
    {
        // Get the part of the text we've already matched and the aprt we haven't yet.
        string matched = this.Text.Substring(0, _characterIndex);
        string notMatched = this.Text.Substring(_characterIndex);

        // Make the matched text colored.
        string colored = TMPHelper.WrapColor(matched, _matchedTextColor);

        // Put text in text field.
        _textField.text = colored + notMatched;
    }

    /// <summary>
    /// Try to match the <paramref name="input"/> to the current character.
    /// </summary>
    /// <param name="input"></param>
    private void MatchCharacter(char input)
    {
        if (input.EqualsIgnoreCase(this.Text[_characterIndex]))
            OnCharacterMatched();
        else
            ResetMatching();
    }

    /// <summary>
    /// Reset the current matching.
    /// </summary>
    private void ResetMatching()
    {
        // Reset to first character of text.
        _characterIndex = 0;
        _textField.text = this.Text;

        this.enabled = string.IsNullOrEmpty(this.Text) == false;
    }

    private void ResetRepetitions() => _repetitions = 0;
    private void ResetDirtiness()
    {
        _cleanliness.Reset();
    }

    /// <summary>
    /// Called when the current character is matched.
    /// </summary>
    private void OnCharacterMatched()
    {
        CharacterMatched?.Invoke();

        // Go to next character.
        _characterIndex++;

        // Update the text to show the character has been matched.
        UpdateText();

        // Check if the entire text has been matched.
        if (_characterIndex >= Text.Length)
            OnRepetitionCompleted();
    }

    /// <summary>
    /// Called when the entire text has been matched.
    /// </summary>
    private void OnRepetitionCompleted()
    {
        // Update counter.
        _repetitions++;

        // Update cleanliness.
        _cleanliness.Cleanliness = (float)_repetitions / _requiredRepetitions;

        // Make input wait for toothbrush.
        _toothInfo.OnSuccessfullyBrushed_1x.AddListener(OnBrushComplete);
        InputHandler.Instance.AddInhibitor(this);

        Matched?.Invoke(this);
    }

    /// <summary>
    /// Called when the toothbrush is finished brushing.
    /// </summary>
    /// <param name="info"></param>
    private void OnBrushComplete(ToothInfo info)
    {
        // Allow input again.
        _toothInfo.OnSuccessfullyBrushed_1x.RemoveListener(OnBrushComplete);
        InputHandler.Instance.RemoveInhibitor(this);

        // Check if we should go for another repetition or if we are done.
        if (_repetitions < _requiredRepetitions)
            ResetMatching();
        else
            OnAllRepetitionsCompleted();
    }

    /// <summary>
    /// Called when the text has been matched enough times.
    /// </summary>
    private void OnAllRepetitionsCompleted()
    {
        RepetitionsCompleted?.Invoke(this);

        if (_disableOnSuccess)
            enabled = false;
    }


    [Serializable]
    public class Event : UnityEvent<ToothController> { }
}
