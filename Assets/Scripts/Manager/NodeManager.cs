using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Class;
using Manager;
using UnityEngine;
using UnityEngine.Events;

namespace GamePlay
{
    public class NodeManager : MonoBehaviour
    {
        [Header("Init")] 
        public static NodeManager Instance;

        public List<Node> NodeList;
        public List<Node> NodeDiaList;
        
        public Camera MainCam;
        private bool isReady = false;
        public SimpleRotate RotateIndicator;

        [Header("Event")]

        public  UnityEvent<int> NodeSelectEvent;
        public  UnityEvent<Node,Node> NodeSelectedEvent;
        public static event Action<List<Node>> SendDiNode; 


        [Header("Variables")] 
        public PlayerManager ThisPlayerManager;
        public int CurrentNodeIndex;
        public int CurrentPlayerIndex;
        public Node CurrentNode;
        public int DiceNumber;
        public bool Click = false;

        [Header("Testing")] 
        public List<Node> WhatConnect;

        private async void Awake()
        {
            Instance = this;
            Click = false;
            MainCam = Camera.main;
            RollDiceManager.RolledDice += RollDiceManagerOnRolledDice;
            foreach (var i in NodeList.Select((node, index) => new { index, node }))
            {
                var node = i.node;
                var index = i.index;

                node.ThisNodeIndex = index;
                var nodeOutline = node.gameObject.AddComponent<Outline>();
                OutlineInit(nodeOutline);
                node.Init(RotateIndicator,nodeOutline);
                if (node.ThisNodeSetting == Node.ThisNodeEvent.Dialog)
                {
                    NodeDiaList.Add(node);
                }
            }
            SendDiNode?.Invoke(NodeDiaList);
            PlayerSetup();
            isReady = true;
            await Task.CompletedTask;
        }

        private void OutlineInit(Outline outline)
        {
            outline.OutlineColor = Color.clear;
            outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            outline.OutlineWidth = 5.0f;
        }

        public async void Start()
        {
            
        }

        private void RollDiceManagerOnRolledDice(int obj)
        {
            Click = false;
            WhatConnect = new List<Node>();
            nodeConnect = new List<List<Node>>();
            DiceNumber = obj;
        }

        private void PlayerSetup()
        {
            ThisPlayerManager = PlayerManager.Instance;
            ThisPlayerManager.PlayerInit();
            CurrentPlayerIndex = ThisPlayerManager.StartIndex;
            CurrentNode = GetNode(CurrentPlayerIndex);
            NodeSelectedEvent?.Invoke(CurrentNode,CurrentNode);
        }

        [Header("Selection")] 
        public Node Selection;
        public Node Selected;
        public float SelectionThreshold = 0.999f;
        public float Nearest;
        private void Update()
        {
            if (!isReady) return;
            
            Check();
            SelectedNode();
        }

        private void Check()
        {
            Selection = null;
            Nearest = 0.0f;
            var currentRay = MainCam.ScreenPointToRay(Input.mousePosition);
            foreach (var node in NodeList)
            {
                var vector1 = currentRay.direction;
                var vector2 = node.ThisNodePos.position - currentRay.origin;

                var lookPercent = Vector3.Dot(vector1.normalized, vector2.normalized);
                if (lookPercent > SelectionThreshold && lookPercent >= Nearest)
                {
                    Selection = node;
                    Nearest = lookPercent;
                }
            }
        }

        private bool isClicking = false;
        private async void SelectedNode()
        {
            if (Selection == null) return;
            if (!Input.GetMouseButtonDown(0)) return;
            CurrentNodeIndex = Selection.ThisNodeIndex;
            if (Selection.Selectable && !isClicking)
            {
                isClicking = true;
                Selected = Selection;
                Debug.Log(CurrentNodeIndex);
                await MakeMovePath(Selected);
                await Task.Delay(1000);
                Debug.Log("Done Make Move Path");
                isClicking = false;
            }
        }

        private Node GetNode(int nodeIndex)
        {
            Node tempNode = null;
            foreach (var i in NodeList.Select((node, index) => new { index, node }))
            {
                var node = i.node;
                var index = i.index;

                if (index == nodeIndex)
                {
                    tempNode = node;
                }
            }

            return tempNode;
        }
        
        private List<List<Node>> nodeConnect;

        private async Task MakeMovePath(Node target)
        {
            if (Click || !PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Select)) return;
            Click = true;
            foreach (var node in from node in NodeList from t in node.NextNode where t == target select node)
            {
                if (node.Selectable)
                {
                    nodeConnect.Insert(0,new List<Node> { node });
                }
            }
            
            count = 0;
            
            var counting = Mathf.Abs(nodeConnect.Count);

            if (counting > 0)
            {
                var tempnode = nodeConnect[0][0];
                nodeConnect.Clear();
                nodeConnect.Insert(0,new List<Node> { tempnode });
                
                for (int i = 0; i < nodeConnect.Count; i++)
                {
                    MakeNodeMovePath(nodeConnect[i][0], i);
                }

                while (count < 5)
                {
                    await Task.Delay(10);
                }

                /*for (var i = 0; i < counting; i++)
                {
                    for (var k = 0; k < counting; k++)
                    {
                        if (nodeConnect[k] != null) continue;
                        if (nodeConnect[i].Count >= nodeConnect[k].Count) continue;
                        var tempList = nodeConnect[i];
                        nodeConnect[i] = null;
                        nodeConnect[i] = nodeConnect[k];
                        nodeConnect[k] = null;
                        nodeConnect[k] = tempList;
                        // (NodeConnect[j], NodeConnect[k]) = (NodeConnect[k], NodeConnect[j]);
                    }
                }*/
                WhatConnect = nodeConnect[0];
            }
            WhatConnect.Add(Selected);
            
            await Task.Delay(100);
            
            NodeSelectedEvent?.Invoke(Selected,CurrentNode);
            NodeSelectEvent?.Invoke(CurrentNodeIndex);
            CurrentPlayerIndex = CurrentNodeIndex;
            CurrentNode = Selected;

            await Task.CompletedTask;
        }
        
        private int count = 0;

        private void MakeNodeMovePath(Node target,int index)
        {
            count++;
            if (target == CurrentNode || count > 5)
            {
                count = 5;
                return;
            }
            List<Node> checkNode = new List<Node>();
            Node tempNode = null;
            foreach (var node in NodeList)
            {
                foreach (var nodelist in node.NextNode)
                {
                    if (nodelist == target)
                    {
                        checkNode.Insert(0,node);
                    }
                }
            }

            tempNode = checkNode[0];
            var tempDist = 10000.0f;
            
            if (checkNode.Count > 1)
            {
                foreach (var node in checkNode)
                {
                    if (node.Selectable)
                    {
                        var dist = Vector3.Distance(node.ThisNodePos.position, CurrentNode.ThisNodePos.position);
                        if (dist < tempDist && node.Selectable)
                        {
                            tempNode = node;
                            tempDist = dist;
                        }
                    }
                }
            }
                
            nodeConnect[index].Insert(0,tempNode);
            MakeNodeMovePath(tempNode,index);
        }

        public void StartNodeDialog()
        {
            CurrentNode.StartDialog();
        }

        public void StartNodeDrawCard()
        {
            CurrentNode.StartDrawCard();
        }

        public bool CheckNodeState(NodeEvent.NodeStateName arg)
        {
            return arg == CurrentNode.CurrentNodeEvent.ThisNodeStateName;
        }
    }
}
