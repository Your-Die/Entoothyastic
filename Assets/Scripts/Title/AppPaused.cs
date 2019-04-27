using UnityEngine;

public class AppPaused : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        _pauseMenu.SetActive(true);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        Cursor.visible = true;
        Time.timeScale = 0.1f;
    }

    public void UnPause()
    {
        _pauseMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}