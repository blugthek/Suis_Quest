using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialSetting[] tutorialSettings;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Text headerText;
    [SerializeField] private Text tutorialContent;
    [SerializeField] private Image tutorialImage;
    private int tutorialID = 0;
    private TutorialButton m_TutorialButton;

    private Sprite[] tutorialImageDisplay;
    private String[] tutorialContentDisplay;
    private String[] headerContentDisplay;
    private int[] buttonIDData; 
    private void Awake()
    {
        var tutorialContentData = new String[tutorialSettings.Length];
        var tutorialHeaderData = new String[tutorialSettings.Length];
        var tutorialIDData = new int[tutorialSettings.Length];
        var tutorialImageData = new Sprite[tutorialSettings.Length];
        
        foreach (TutorialSetting t in tutorialSettings)
        {
            var button = Instantiate(tutorialButton);
            var buttonText = button.GetComponentInChildren( typeof(Text)) as Text;
            m_TutorialButton = button.GetComponent<TutorialButton>();
            buttonText.text = t.tutorialName;
            button.transform.parent = contentPanel.transform;

            t.tutorialButtonID = tutorialID;

            tutorialContentData[tutorialID] = t.tutorialContent;
            tutorialHeaderData[tutorialID] = t.header;
            tutorialIDData[tutorialID] = t.tutorialButtonID;
            tutorialImageData[tutorialID] = t.tutorialImage;

            m_TutorialButton.SetID(tutorialIDData[tutorialID]);
            tutorialID++;
            
        }

        tutorialImageDisplay = tutorialImageData;
        tutorialContentDisplay = tutorialContentData;
        headerContentDisplay = tutorialHeaderData;
        buttonIDData = tutorialIDData;

        headerText.text = headerContentDisplay[0];
        tutorialContent.text = tutorialContentDisplay[0];
        tutorialImage.sprite = tutorialImageDisplay[0];
    }
    
    public void OnClick(int ID)
    {
        Debug.Log("ID Data is " + buttonIDData[ID]);
        Debug.Log("Header Data is " + headerContentDisplay[ID]);
        Debug.Log("Content Data is " + tutorialContentDisplay[ID]);
        
        headerText.text = headerContentDisplay[ID];
        tutorialContent.text = tutorialContentDisplay[ID];
        tutorialImage.sprite = tutorialImageDisplay[ID];

    }

}
