using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private GameOverScreen gameOver;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject player;

    [Header("Timer Settings")]
    [SerializeField] private float currentTime;
    [HideInInspector] public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isRunning)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            GameOver();
        }
        
        timerText.text = FormatTime(currentTime);
    }

    private string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        int milliseconds = (int) (1000 * (time - minutes * 60 - seconds));

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void GameOver()
    {
        gameOver.Setup(currentTime);
    }

    public float getTime()
    {
        return currentTime;
    }
}
