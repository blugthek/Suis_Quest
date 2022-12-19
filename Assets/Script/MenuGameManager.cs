using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameManager : MonoBehaviour
{
    public CanvasGroup blackScreen;
    
    // Update is called once per frame
    private void Start()
    {
        LeanTween.alphaCanvas(blackScreen, 0, 1.5f);
    }
    
}
