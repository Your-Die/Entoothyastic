using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    [SerializeField] private string _text;

    public void Log()
    {
        Debug.Log(_text);
    }
}
