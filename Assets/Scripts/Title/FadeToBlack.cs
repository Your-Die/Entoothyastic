using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] private Color _fadeColor;

    [SerializeField] private float _fadeTime;
    [SerializeField] private Image _imageFade;
    public UnityEvent OnFadeFinished;
    private float time;
    public bool IsInAction;

    public void Fade()
    {
        _imageFade.color = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, 0);
        _imageFade.raycastTarget = true;
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        while (time < _fadeTime)
        {
            time += Time.deltaTime;
            var fraction = time / _fadeTime;
            _imageFade.color = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, fraction);
            yield return null;
        }
        OnFadeFinished.Invoke();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene!");
        Cursor.visible = true;
        if (IsInAction) SceneManager.LoadScene(sceneName);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
