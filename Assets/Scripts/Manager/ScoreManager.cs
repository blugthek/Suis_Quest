using System;
using System.Collections.Generic;
using System.Linq;
using Class;
using GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int MaxScore;
        [SerializeField] private int CurrentScore;
        [SerializeField] private List<Node> NodeDiList;
        [SerializeField] private int Intc;
        public GameObject endGame;
        public GameObject dicePanel;
        public GameObject interFacePanel;

        [Header("UI")]
        public TextMeshProUGUI Score;
        private void Awake()
        {
            DialogManager.OnBadChoice += DialogManagerOnBadChoice;
            DialogManager.OnGoodChoice += DialogManagerOnGoodChoice;
            GameManager.GameManagerCallChangeState += CheckScore;
            NodeManager.SendDiNode += OnSendDiNode;
            CurrentScore = 0;
        }
        private void OnSendDiNode(List<Node> obj)
        {
            NodeDiList = obj;
            MaxScore = NodeDiList.Count;
        }

        private void OnDestroy()
        {
            DialogManager.OnBadChoice -= DialogManagerOnBadChoice;
            DialogManager.OnGoodChoice -= DialogManagerOnGoodChoice;
            GameManager.GameManagerCallChangeState -= CheckScore;
            NodeManager.SendDiNode -= OnSendDiNode;
        }

        private void DialogManagerOnGoodChoice()
        {
            Debug.Log("Call Add Score");
            CurrentScore++;
        }

        private void DialogManagerOnBadChoice()
        {
            Debug.Log("Call De Score");
            CurrentScore--;
        }

        private void CheckScore()
        {
            Debug.Log("On Load Scene");
            if (CurrentScore == -1)
            {
                dicePanel.GetComponent<Canvas>().enabled = false;
                interFacePanel.SetActive(false);
                GameManager.gameIsNotEnd = false;
                LoadBadScene();
            }
            if (CurrentScore >= MaxScore)
            {
                dicePanel.GetComponent<Canvas>().enabled = false;
                GameManager.gameIsNotEnd = false;
                interFacePanel.SetActive(false);
                LoadGoodScene();
            }
        }

        private void FixedUpdate()
        {
            Intc = NodeDiList.Count(node => node.ThisNodeSetting != Node.ThisNodeEvent.Dialog);
            Score.text = Intc + " / " + MaxScore;
        }

        private void LoadGoodScene()
        {
            Ending end = endGame.GetComponent<Ending>();
            end.OnEnding(true);
        }

        private void LoadBadScene()
        {
            Ending end = endGame.GetComponent<Ending>();
            end.OnEnding(false);
        }
    }
}