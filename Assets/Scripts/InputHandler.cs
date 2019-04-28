using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Timers;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Timer _cooldown = new Timer(0);

    private readonly List<object> _inhibitors = new List<object>();

    private static InputHandler _instance;

    private bool IsInhibited => _inhibitors.Any();

    private bool _inputUsed;

    public UnityEvent InvalidInputRegistered;

    public static InputHandler Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<InputHandler>();

            if (_instance != null)
                return _instance;

            _instance = new GameObject(nameof(InputHandler)).AddComponent<InputHandler>();
            return _instance;
        }
    }

    private void Awake()
    {
        ToothController.CharacterMatched += OnInputUsed;

    }

    private void OnInputUsed()
    {
        _inputUsed = true;
    }

    private void LateUpdate()
    {
        if(InputRegistered(out string _) && !_inputUsed){
            InvalidInputRegistered?.Invoke();
            Debug.Log("Invalid input registered.");
        }

        _inputUsed = false;
    }

    public bool TryGetKey(out char? key)
    {
        if (!InputRegistered(out var inputString))
        {
            key = null;
            return false;
        }

        key = inputString.First();
        _cooldown.Start();
        return true;
    }

    private bool InputRegistered(out string inputString)
    {
        inputString = Input.inputString;
        return !IsInhibited && !_cooldown.IsRunning && Input.anyKeyDown && !string.IsNullOrEmpty(inputString);
    }

    public void AddInhibitor(object inhibitor)
    {
        _inhibitors.Add(inhibitor);
    }

    public void RemoveInhibitor(object inhibitor)
    {
        _inhibitors.Remove(inhibitor);
    }
}
