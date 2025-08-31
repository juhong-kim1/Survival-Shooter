using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UiManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverUi;
    public GameObject pauseMenuUi;
    public Image damageEffect;

    public void OnEnable()
    {
        SetUpdateScore(0);
        GameOverUpdate(false);
        ShowPauseMenu(false);

    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(1f, 0f, 0f, 0.1f);
        Invoke("HideDamage", 0.1f);
    }

    private void HideDamage()
    {
        damageEffect.color = new Color(1f, 0f, 0f, 0f);
    }

    public void SetUpdateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }

    public void ShowPauseMenu(bool active)
    {
        pauseMenuUi.SetActive(active);
    }

    public void GameOverUpdate(bool active)
    {
        gameOverUi.SetActive(active);

        if (active)
        {
            StartCoroutine(RestartAfterDelay());
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
