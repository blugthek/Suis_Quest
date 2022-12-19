using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GamePlay;
using Manager;
using ScriptableObjects;
using States.NodeStates;
using PlayerStates = GamePlay.PlayerStatesManager;
using PlayerState = Class.PlayerBaseState.StateName;
using U = Utility.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Class
{
    public class Node : MonoBehaviour
    {
    
        [Serializable]
        public enum ThisNodeEvent
        {
            Normal,
            Dialog,
            DrawCard,
        }

        [Header("Init")] 
        public int ThisNodeIndex;
        public Transform ThisNodePos;
        public NodeEvent CurrentNodeEvent;
        public ThisNodeEvent ThisNodeSetting;

        [Header("Variable")] 
        public List<Node> NextNode;
        public bool Selectable;
        public bool IsDisease;

        [Header("Extra")]
        public Renderer ThisRenderer;

        public Outline ThisOutline;

        [Header("Dialog")]
        public List<NodeDialogs> ThisDialog;
        public static event Action<NodeDialogs> OnNodeDialog;
        public int CurrentSeq;
        public bool Exporting = false;
        private const string FilePath = "Assets/Resources/";
        private SimpleRotate RIndicator;
        public Sprite ThisImage;

        [Header("Extra Ending"),TextArea(10,15)] 
        public string BadEndingTxt;
        [TextArea(10,15)]
        public string GoodEndingTxt;

        public void Awake()
        {
            if (ThisNodeSetting == ThisNodeEvent.Dialog)
            {
                CurrentSeq = ThisDialog[0].Seq;
                DialogManager.OnReqDialog += FetchCurrentDialog;
                CheckDirectory();
            }
            ThisRenderer = gameObject.GetComponent<Renderer>();
        }

        private bool isInit = false;

        public void Init(SimpleRotate indicator,Outline outline)
        {
            ThisOutline = outline;
            CurrentNodeEvent = ThisNodeSetting switch
            {
                ThisNodeEvent.Normal => new NodeNormal(ThisNodeIndex, this),
                ThisNodeEvent.Dialog => new NodeDialog(ThisNodeIndex, this),
                ThisNodeEvent.DrawCard => new NodeDrawCard(ThisNodeIndex, this),
                _ => throw new ArgumentOutOfRangeException()
            };
            if (ThisNodeSetting == ThisNodeEvent.Dialog)
            {
                var pos = transform.position;
                pos.y += 2;
                RIndicator = Instantiate(indicator,pos,Quaternion.identity);
            }

            ThisNodePos = transform;
            isInit = true;
        }
        
        private float triggerTimer;

        private void Update()
        {
            if (isInit)
            {
                CurrentNodeEvent.Process();
                NodeProcess();
            }
            if (Exporting)
            {
                Exporting = false;
                ExportNodeDialogs();
            }
        }

        private void NodeProcess()
        {
            if (ThisNodeSetting != ThisNodeEvent.Dialog && RIndicator)
            {
                RIndicator.gameObject.SetActive(false);
            }
            if (NextNode.Count <= 0) return;
            NodeSelectorProcess();
            foreach (var t in NextNode)
            {
                Debug.DrawLine(ThisNodePos.position,t.ThisNodePos.position,Color.red);
            }
        }

        private void NodeSelectorProcess()
        {
            if (PlayerStates.Instance.CheckCurrentStateName(PlayerState.Select))
            {
                if (NodeManager.Instance.CurrentNode == this)
                {
                    foreach (var node in NextNode)
                    {
                        node.NodeSelector(NodeManager.Instance.DiceNumber);
                    }
                }
            }
            if (!PlayerStates.Instance.CheckCurrentStateName(PlayerState.Select))
            {
                Selectable = false;
            }
            if (Selectable)
            {
                ChangeColorOnPointingToward();
            }
            else
            {
                ThisRenderer.material.color = Color.white;
                ThisOutline.OutlineColor = Color.clear;
            }
        }
        
        private void ChangeColorOnPointingToward()
        {
            if (NodeManager.Instance.Selection == this)
            {
                triggerTimer += Time.deltaTime;
                if (triggerTimer >= 0.1f)
                {
                    ThisRenderer.material.color = Color.red;
                    ThisOutline.OutlineColor = Color.red;
                }
            }
            if (NodeManager.Instance.Selection != this)
            {
                ThisRenderer.material.color = Color.green;
                ThisOutline.OutlineColor = Color.green;
                triggerTimer = 0;
            }
        }

        private void NodeSelector(int routine)
        {
            if (routine <= 0) return;
            Selectable = true;
            var nextRoutine = routine - 1;
            foreach (var node in NextNode)
            {
                node.NodeSelector(nextRoutine);
            }
        }

        #region Dialog

        public void StartDialog()
        {
            if (ThisNodeSetting == ThisNodeEvent.Dialog)
            {
                FetchCurrentDialog(CurrentSeq);
            }
            else
            {
                GameManager.InvokeChangedState();
            }
        }

        private void FetchCurrentDialog(int seq)
        {
            if (NodeManager.Instance.CurrentNode != this) return;
            CurrentSeq = seq;
            foreach (var dialog in ThisDialog.Where(dialog => seq == dialog.Seq))
            {
                OnNodeDialog?.Invoke(dialog);
            }
        }

        private string fileName;
        private void CheckDirectory()
        {
            fileName = FilePath + this.gameObject.name;
            U.CheckDirectory(fileName,FilePath);
        }

        private void ExportNodeDialogs()
        {
            U.ExportNodeDialogs(fileName,ThisDialog);
        }

        #endregion

        #region Draw Card

        public void StartDrawCard()
        {
            
        }

        #endregion
        
    }
}