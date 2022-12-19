using System;
using System.Collections;
using System.Collections.Generic;
using Class;
using GamePlay;
using TMPro;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{
    [SerializeField] private float MaximumTimeDuration;
    private float timer;

    [Header("Variable")] 
    private string timerText;
    public TextMeshProUGUI Timer;

    private void Awake()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Waiting)) return;
        if (!(timer >= 0)) return;
        UpdateTimer();
        timer -= Time.deltaTime;
        UpdateDisplayTimer();
    }

    private void ResetTimer()
    {
        timer = MaximumTimeDuration * 60;
    }

    private void UpdateTimer()
    {
        var min = Mathf.FloorToInt(timer / 60);
        var sec = Mathf.FloorToInt(timer % 60);

        timerText = min + " : " + sec;
        if (timer <= 0)
        {
            timerText = " 00 : 00";
        }
    }

    private void UpdateDisplayTimer()
    {
        Timer.text = timerText;
    }
}
