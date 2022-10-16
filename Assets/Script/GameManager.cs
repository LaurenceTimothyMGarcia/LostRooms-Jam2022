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
        
        timerText.text = currentTime.ToString("00:00.00");
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
