using System;
using System.Threading.Tasks;
using Class;
using GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static event Action GameManagerCallChangeState;
        public static event Action FirstStart;
        private static bool firstTime = true;
        public static bool gameIsNotEnd;
        private void Awake()
        {
            PlayerStatesManager.EnterWaiting += WaitingProcess;
            gameIsNotEnd = true;
        }
        

        public static void InvokeChangedState()
        {
            GameManagerCallChangeState?.Invoke();
        }

        private static async void WaitingProcess()
        {
            if (gameIsNotEnd)
            {
                Debug.Log("Enter Waiting Process");
                await Task.Delay(1000);
                switch (true)
                {
                    case true when firstTime:
                        FirstStart?.Invoke();
                        firstTime = false;
                        Debug.Log("Call for first Time");
                        await Task.Delay(2000);
                        GameManagerCallChangeState?.Invoke();
                        break;
                    case true when NodeManager.Instance.CheckNodeState(NodeEvent.NodeStateName.Normal):
                        Debug.Log("Call for timer");
                        await Task.Delay(2000);
                        GameManagerCallChangeState?.Invoke();
                        break;
                    case true when NodeManager.Instance.CheckNodeState(NodeEvent.NodeStateName.Dialog):
                        Debug.Log("Call For dialog");
                        NodeManager.Instance.StartNodeDialog();
                        break;
                    case true when NodeManager.Instance.CheckNodeState(NodeEvent.NodeStateName.DrawCard):
                        Debug.Log("Call For Draw Card");
                        NodeManager.Instance.StartNodeDrawCard();
                        break;
                }
            }
            
        }

        /*[SerializeField] private Transform targetPos;
        [SerializeField] private Transform thisPos;
        [SerializeField] private bool trigger = false;
        [SerializeField] private int point;

        private void Update()
        {
            CheckingProcess();
        }

        public void CheckingProcess()
        {
            if (targetPos.position == thisPos.position && !trigger)
            {
                trigger = true;
                point++;
            }
        }

        public void SceneLoader()
        {
            SceneManager.LoadScene("EndScene");
        }*/
    }
}