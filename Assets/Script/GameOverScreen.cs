using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private GameObject gameOverScreen;

    void Start()
    {
        gameOverScreen.SetActive(false);
    }

    public void Setup(float time)
    {
        gameOverScreen.SetActive(true);
        finalTime.text = time.ToString("00:00.00");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("BackRoom1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
