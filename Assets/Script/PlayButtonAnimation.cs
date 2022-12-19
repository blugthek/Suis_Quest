using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using SM = UnityEngine.SceneManagement.SceneManager;

public class PlayButtonAnimation : MonoBehaviour
{
    [SerializeField] private float uiScale = 1;
    [SerializeField] private float uiScaleSize = 1.2f;
    [SerializeField] private float animateTime = 0.5f;
    [SerializeField] private float animateClickTime = 0.8f;
    [SerializeField] private float uiClickScale = 0.8f;

    public void OnHover()
    {
        LeanTween.scale(this.gameObject, new Vector3(uiScaleSize, uiScaleSize, uiScaleSize), animateTime).setEase(LeanTweenType.easeOutBounce);
    }

    public void OnLeave()
    {
        LeanTween.scale(this.gameObject, new Vector3(uiScale, uiScale, uiScale), animateTime).setEase(LeanTweenType.easeOutBounce);
    }

    public void OnClick()
    {
        SM.LoadScene(0);
    }
}
