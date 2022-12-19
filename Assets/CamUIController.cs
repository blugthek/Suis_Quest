using System.Collections;
using System.Collections.Generic;
using Class;
using GamePlay;
using UnityEngine;

public class CamUIController : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    void Update()
    {
        if (PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Walking))
        {
            Panel.SetActive(true);
        }
        else if (PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Waiting))
        {
            if (NodeManager.Instance.CurrentNode.ThisNodeSetting == Node.ThisNodeEvent.Dialog)
            {
                Panel.SetActive(false);
            }
            else
            {
                Panel.SetActive(true);
            }
        }
        else
        {
            Panel.SetActive(false);
        }
    }
}
