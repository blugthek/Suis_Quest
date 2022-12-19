using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class OpenUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup background;

    private Vector3 localScale; 

    private void Awake()
    {
        localScale = transform.localScale;
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void Open()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.2f);
        
        transform.LeanScale(localScale, 0.2f);
    }

    public void Close()
    {
        background.LeanAlpha(0, 0.2f);
        transform.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
    }
}
