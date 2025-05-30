using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private Image buttonIcon;
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;

    public Canvas settings;
    private bool isMusicPaused;
    
    private void Start()
    {
        settings.gameObject.SetActive(false);
        UpdateButtonIcon();
    }

    public void OpenSettings()
    {
        settings.gameObject.SetActive(true);
    }
    
    public void ToggleMusic()
    {
        if (isMusicPaused)
        {
            menuMusic.UnPause();
            isMusicPaused = false;
        }
        else
        {
            menuMusic.Pause();
            isMusicPaused = true;
        }
        
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        buttonIcon.sprite = isMusicPaused ? pauseIcon : playIcon;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
