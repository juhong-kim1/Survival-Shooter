using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UiManager uiManager;
    public AudioSource bgmAudioSource;
    private int score;

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }

    public void Start()
    {
        var findGo = GameObject.FindWithTag("Player");
        var playerHealth = findGo.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += () => EndGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGameOver) return;

            IsPaused = !IsPaused;

            if (IsPaused)
            {
                Time.timeScale = 0f;
                uiManager.ShowPauseMenu(true);
            }
            else
            {
                Time.timeScale = 1f;
                uiManager.ShowPauseMenu(false);
            }
        }


    }

    public void AddScore(int add)
    {
        score += add;
        uiManager.SetUpdateScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        Invoke("ShowGameOverUI", 2f);
    }

    private void ShowGameOverUI()
    {
        uiManager.GameOverUpdate(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        uiManager.ShowPauseMenu(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnMusicVolumeChange(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    public void OnEffectsVolumeChange(float volume)
    {
        SoundManager.Instance.sfxVolume = volume;
    }

    public void OnSoundToggle(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
    }


}
