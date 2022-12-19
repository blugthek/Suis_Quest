using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private TutorialManager tutorialManager;
    private GameObject tutorialObject;
    private int buttonID;

    public void Awake()
    {
        GameObject tutorialObject = GameObject.Find("TutorialManager");
        tutorialManager = tutorialObject.GetComponent<TutorialManager>();
    }
    
    public void SetID(int ID)
    {
        buttonID = ID;
    }
    public void OnClick()
    {
        tutorialManager.OnClick(buttonID);
    }

    
    
}
