using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopWatch : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = currentTime.ToString("00:00.00");
    }
}
