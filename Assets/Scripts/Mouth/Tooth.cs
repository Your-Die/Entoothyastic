using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Tooth : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private Color _matchedTextColor = Color.red;

    [SerializeField] private bool _disableOnSuccess;

    [SerializeField] private TMP_Text _textField;

    private int _characterIndex;

    private KeyCode _currentKey;

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }

    [FormerlySerializedAs("OnInputSuccess")] public Event Matched;

    private void OnEnable()
    {
        ResetMatching();
    }

    private void Update()
    {
        if (!Input.anyKeyDown || string.IsNullOrEmpty(Input.inputString))
            return;

        char input = Input.inputString.First();
        MatchCharacter(input);
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
        if (input == this.Text[_characterIndex])
            OnCharacterMatched();
        else
        {
            ResetMatching();
        }
    }

    private void ResetMatching()
    {
        _characterIndex = 0;
        _textField.text = this.Text;
    }

    private void OnCharacterMatched()
    {
        _characterIndex++;

        UpdateText();

        if (_characterIndex >= Text.Length)
        {
            OnSuccess();
        }
    }

    private void OnSuccess()
    {
        Matched?.Invoke(this);

        if (_disableOnSuccess)
            gameObject.SetActive(false);
    }

    [Serializable]
    public class Event : UnityEvent<Tooth> { }
}
