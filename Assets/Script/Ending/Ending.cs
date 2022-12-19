using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Ending : MonoBehaviour
{
    public Image endImage;
    public Sprite goodEndImage;
    public Sprite badEndImage;
    public CanvasGroup blackScreen;
    public GameObject badEndPanel;
    public Text endText;
    public Text endTextResult;
    public GameObject endPanel;
    public GameObject goodEndObject;
    public GameObject goodEndRibbon;
    public GameObject goodEndButtonPanel;
    public bool isGoodEnd;
    private Vector3 endPanelLocalScale;
    private Vector3 goodEndObjectLocalScale;
    private Vector3 goodEndRibbonLocalScale;
    private void Awake()
    {
        endPanelLocalScale = endPanel.transform.localScale;
        goodEndObjectLocalScale = goodEndObject.transform.localScale;
        goodEndRibbonLocalScale = goodEndRibbon.transform.localScale;
        endPanel.transform.localScale = Vector3.zero;
        goodEndObject.transform.localScale = Vector3.zero;
        goodEndRibbon.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (isGoodEnd)
        {
            endImage.sprite = goodEndImage;
            endTextResult.text = "ฉากจบ - ดี";
        }
        else
        {
            endImage.sprite = badEndImage;
            endTextResult.text = "ฉากจบ - ไม่ดี";
        }
    }

    public void OnEnding(bool endCheck)
    {
        isGoodEnd = endCheck;
        LeanTween.alphaCanvas(blackScreen, 1, 1.5f).setDelay(0.5f);
        endText.transform.LeanMoveLocal(new Vector2(0, 0),1).setDelay(2).setEaseOutQuart();
        endText.transform.LeanMoveLocal(new Vector2(0, -1250),1).setDelay(5).setEaseOutQuart();
        LeanTween.alphaCanvas(blackScreen, 0, 1.5f).setDelay(6);
        endPanel.transform.LeanScale(endPanelLocalScale, 0.2f).setDelay(7.5f);
    }
    
    public void Close()
    {
        endPanel.transform.LeanScale(Vector3.zero, 0.3f).setEaseInBack();
        if (isGoodEnd)
        {
            OnGoodEnd();
        }

        if (!isGoodEnd)
        {
            badEndPanel.SetActive(true);
        }
    }

    public void OnGoodEnd()
    {
        goodEndObject.transform.LeanScale(goodEndObjectLocalScale, 0.2f).setDelay(0.5f);
        goodEndRibbon.transform.LeanScale(goodEndRibbonLocalScale, 0.2f).setDelay(1f);
        goodEndButtonPanel.transform.LeanMoveLocal(new Vector2(0, 100),1).setDelay(1.3f).setEaseOutQuart();
    }

    public void onTryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onMenu()
    {
        SceneManager.LoadScene(1);
    }

    

}
