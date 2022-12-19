using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Class;
using GamePlay;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manager
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance;
        public static event Action<int> OnReqDialog;
        public static event Action OnGoodChoice;
        public static event Action OnBadChoice;

        [Header("Dialog")] 
        public List<StartDialog> StartDialogs;
        public PlayerDialogList PlayerDialog;
        private List<PlayerDialog> thisDialog;
        public bool UpdateDialogOnClick=false;

        [Header("Variables")] 
        public GameObject Panel;

        public Button ButtA;
        public TextMeshProUGUI ButtAText;
        public Button ButtB;
        public TextMeshProUGUI ButtBText;
        public Button ButtC;
        public TextMeshProUGUI ButtCText;

        public TextMeshProUGUI Dialog;
        public Sprite DefImage;
        public Image DialogImage;

        public int ConnectDialog;

        private void Awake()
        {
            Instance = this;
            thisDialog = PlayerDialog.GetDialogList();
            Node.OnNodeDialog += OnNodeDialogCalled;
            GameManager.FirstStart += OnFirstStart;
            GameManager.GameManagerCallChangeState += DisablePopupDialogs;
        } 

        public void Start()
        {
            foreach (var dialog in StartDialogs)
            {
                dialog.Done = false;
            }
            DialogImage.sprite = DefImage;
            Panel.SetActive(false);
            ButtA.gameObject.SetActive(false);
            ButtB.gameObject.SetActive(false);
            ButtC.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            Node.OnNodeDialog -= OnNodeDialogCalled;
            GameManager.FirstStart -= OnFirstStart;
            GameManager.GameManagerCallChangeState -= DisablePopupDialogs;
        }

        public void Update()
        {
            var tempNode = NodeManager.Instance.CurrentNode;
            if (tempNode.ThisImage != null)
            {
                DialogImage.sprite = tempNode.ThisImage;
            }
            if (!PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Waiting))
            {
                Panel.SetActive(false);
            }
            if (PlayerDialog.IsUpdate || UpdateDialogOnClick)
            {
                UpdateDialogOnClick = false;
                PlayerDialog.IsUpdate = false;
                thisDialog = PlayerDialog.GetDialogList();
                foreach (var dialog in thisDialog.Where(dialog => ConnectDialog == dialog.Seq))
                {
                    CurrentDialog = dialog;
                    ButtBText.text = "ต่อไป";
                    Dialog.text = dialog.Dialog;
                    ButtA.gameObject.SetActive(false);
                    ButtB.gameObject.SetActive(true);
                    ButtC.gameObject.SetActive(false);
                    if (!dialog.IsChoice) continue;
                    ButtBText.text = dialog.ChoiceA;
                    ButtCText.text = dialog.ChoiceB;
                    ButtC.gameObject.SetActive(true);
                }
            }
        }

        private async void OnFirstStart()
        {
            var tempNode = NodeManager.Instance.CurrentNode;
            Panel.SetActive(true);
            if (tempNode.ThisImage != null)
            {
                DialogImage.sprite = tempNode.ThisImage;
            }
            for (var i = 0; i < StartDialogs.Count ; i++)
            {
                Dialog.text = StartDialogs[i].Dialog;
                ButtAText.text = "ต่อไป";
                ButtA.gameObject.SetActive(true);
                var i1 = i;
                await Task.Delay(500);
                ButtA.onClick.AddListener(delegate { NextDialogS(StartDialogs[i1]); });
                while (!StartDialogs[i].Done)
                {
                    await Task.Delay(100);
                }
                ClearListenner();
                Dialog.text = "";
            }

            DisablePopupDialogs();
            Panel.SetActive(false);
            ClearListenner();
            await Task.Delay(1000);
            GameManager.InvokeChangedState();
            GameManager.FirstStart -= OnFirstStart;
        }

        private void NextDialogS(StartDialog dialog)
        {
            dialog.Done = true;
        }

        private async void OnNodeDialogCalled(NodeDialogs obj)
        {
            DisablePopupDialogs();
            Panel.SetActive(true);
            await Task.Delay(200);
            ButtA.gameObject.SetActive(true);
            ConnectDialog = obj.ConnectToSeq;
            Dialog.text = obj.Dialog;
            ButtA.onClick.AddListener(NextDialog);
        }

        public PlayerDialog CurrentDialog;
        private async void NextDialog()
        {
            var tempNode = NodeManager.Instance.CurrentNode;
            if (tempNode.ThisImage != null)
            {
                DialogImage.sprite = tempNode.ThisImage;
            }
            ButtA.onClick.RemoveListener(NextDialog);
            ClearListenner();
            await Task.Delay(200);
            DisablePopupDialogs();
            await Task.Delay(200);
            foreach (var dialog in thisDialog.Where(dialog => ConnectDialog == dialog.Seq))
            {
                CurrentDialog = dialog;
                Dialog.text = dialog.Dialog;
                ButtA.gameObject.SetActive(false);
                ButtB.gameObject.SetActive(true);
                ButtC.gameObject.SetActive(false);
                ButtB.onClick.AddListener(ReqDialog);
                if (dialog.IsChoice)
                {
                    ButtBText.text = dialog.ChoiceA;
                    ButtCText.text = dialog.ChoiceB;
                    ButtB.onClick.RemoveListener(ReqDialog);
                    ButtB.onClick.AddListener(ReqDialogA);
                    ButtC.gameObject.SetActive(true);
                    ButtC.onClick.AddListener(ReqDialogB);
                }
            }
        }

        private void ReqDialog()
        {
            ButtA.onClick.RemoveListener(ReqDialog);
            ClearListenner();
            OnReqDialog?.Invoke(CurrentDialog.ConnectToSeq);
        }

        private async void ReqDialogA()
        {
            var tempNode = NodeManager.Instance.CurrentNode;
            DisablePopupDialogs();
            ButtB.onClick.RemoveListener(ReqDialogA);
            ClearListenner();
            if (CurrentDialog.ForceQuitA)
            {
                Debug.Log("Help yourself A");
                
                Dialog.text = "คุณให้ชาวบ้านดูแลตัวเอง";
                DisableButton();
                await Task.Delay(2000);
                switch (tempNode.IsDisease)
                {
                    case true:
                        tempNode.IsDisease = false;
                        await OnBadTerm(tempNode);
                        break;
                    case false:
                        tempNode.IsDisease = false;
                        await OnGoodTerm(tempNode);
                        break;
                }
            }
            if (CurrentDialog.IsForceEndA)
            {
                Panel.SetActive(false);
                await Task.Delay(500);
                GameManager.InvokeChangedState();
                Panel.SetActive(false);
            }
            if (CurrentDialog.ForceAmbulanceA)
            {
                Debug.Log("See Doc A");
                
                Dialog.text = "คุณให้คำแนะนำกับชาวบ้าน";
                DisableButton();
                await Task.Delay(2000);
                switch (tempNode.IsDisease)
                {
                    case true:
                        tempNode.IsDisease = false;
                        await OnGoodTerm(tempNode);
                        break;
                    case false:
                        tempNode.IsDisease = false;
                        await OnBadTerm(tempNode);
                        break;
                }
            }
            else
            {
                OnReqDialog?.Invoke(CurrentDialog.AConnectToSeq);
            }
        }
        private async void ReqDialogB()
        {
            var tempNode = NodeManager.Instance.CurrentNode;
            DisablePopupDialogs();
            ButtC.onClick.RemoveListener(ReqDialogB);
            ClearListenner();
            if (CurrentDialog.ForceQuitB)
            {
                Debug.Log("Help yourself B");
                
                Dialog.text = "คุณให้ชาวบ้านดูแลตัวเอง";
                DisableButton();
                await Task.Delay(2000);
                switch (tempNode.IsDisease)
                {
                    case true:
                        tempNode.IsDisease = false;
                        await OnBadTerm(tempNode);
                        break;
                    case false:
                        tempNode.IsDisease = false;
                        await OnGoodTerm(tempNode);
                        break;
                }
            }
            if (CurrentDialog.IsForceEndB)
            {
                Panel.SetActive(false);
                await Task.Delay(500);
                GameManager.InvokeChangedState();
                Panel.SetActive(false);
            }
            if (CurrentDialog.ForceAmbulanceB)
            {
                Debug.Log("See Doc B");
                
                Dialog.text = "คุณให้คำแนะนำกับชาวบ้าน";
                DisableButton();
                await Task.Delay(2000);
                switch (tempNode.IsDisease)
                {
                    case true:
                        tempNode.IsDisease = false;
                        await OnGoodTerm(tempNode);
                        break;
                    case false:
                        tempNode.IsDisease = false;
                        await OnBadTerm(tempNode);
                        break;
                }
            }
            else
            {
                OnReqDialog?.Invoke(CurrentDialog.BConnectToSeq);
            }
        }

        private void DisablePopupDialogs()
        {
            Dialog.text = "";
            ButtAText.text = "ต่อไป";
            ButtBText.text = "ต่อไป";
            ButtCText.text = "ต่อไป";
            DisableButton();
        }

        private void DisableButton()
        {
            ButtA.gameObject.SetActive(false);
            ButtB.gameObject.SetActive(false);
            ButtC.gameObject.SetActive(false);
        }

        private void ClearListenner()
        {
            ButtA.onClick.RemoveAllListeners();
            ButtB.onClick.RemoveAllListeners();
            ButtC.onClick.RemoveAllListeners();
        }

        private async Task OnGoodTerm(Node tempNode)
        {
            OnGoodChoice?.Invoke();
            Dialog.text = tempNode.GoodEndingTxt;
            await Task.Delay(2000);
            tempNode.ThisNodeSetting = Node.ThisNodeEvent.Normal;
            GameManager.InvokeChangedState();
        }
        
        private async Task OnBadTerm(Node tempNode)
        {
            OnBadChoice?.Invoke();
            Dialog.text = tempNode.BadEndingTxt;
            await Task.Delay(2000);
            tempNode.ThisNodeSetting = Node.ThisNodeEvent.Normal;
            GameManager.InvokeChangedState();
        }
        
    }
}