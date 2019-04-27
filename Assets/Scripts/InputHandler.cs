using System.Collections.Generic;
using System.Linq;
using Chinchillada.Timers;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Timer _cooldown = new Timer(0);

    private readonly List<object> _inhibitors = new List<object>();

    private static InputHandler _instance;

    private bool IsInhibited => _inhibitors.Any();

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

    public bool TryGetKey(out char? key)
    {
        var inputString = Input.inputString;

        if (IsInhibited || 
            _cooldown.IsRunning || 
            !Input.anyKeyDown || 
            string.IsNullOrEmpty(inputString))
        {
            key = null;
            return false;
        }

        key = inputString.First();
        _cooldown.Start();
        return true;
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
