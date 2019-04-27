using System.Collections.Generic;
using System.Linq;
using Chinchillada.Timers;
using UnityEngine;

public class InputHandler : MonoBehaviourSingleton<InputHandler>
{
    [SerializeField] private Timer _cooldown = new Timer(0);

    private readonly List<object> _inhibitors = new List<object>();

    private bool IsInhibited => _inhibitors.Any();

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
