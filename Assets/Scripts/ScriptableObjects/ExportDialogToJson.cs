using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [Serializable]
    public class PlayerDialogMockup
    {
        public int Seq;
        public string Dialog;
        public int ConnectToSeq;
        public bool IsChoice;
        public string ChoiceA;
        public int AConnectToSeq;
        public bool IsForceEndA;
        public bool ForceQuitA;
        public string ChoiceB;
        public int BConnectToSeq;
        public bool IsForceEndB;
        public bool ForceQuitB;
    }
    
    [Serializable]
    public class PListDB
    {
        public List<PlayerDialogMockup> PlayerDialogs;
    }
    public class ExportDialogToJson : MonoBehaviour
    {
        public string FilePath = "Assets/Resources/";
        public string FileName = "Json.txt";
        public PlayerDialogList PlayerDialogList;
        public PListDB PlayerDialogs;
        public List<PlayerDialog> ExecutedDialogs;

        [Header("Switch")]
        public bool Writing = false;
        public bool Loading = false;
        public bool Executing = false;
        private string pathName;

        private void Awake()
        {
            pathName = FilePath + FileName;
            ExecutedDialogs = PlayerDialogList.ThisDialog;
            CheckDirectory();
        }

        private void CheckDirectory()
        {
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            if (!File.Exists(pathName))
            {
                File.Create(pathName);
            }
        }

        private void Update()
        {
            switch (true)
            {
                case true when Writing:
                    Writing = false;
                    ExportToJson();
                    break;
                case true when Loading:
                    Loading = false;
                    ImportFromJson();
                    break;
                case true when Executing:
                    ExecuteJsonToGame();
                    Executing = false;
                    break;
            }
        }

        private void ExportToJson()
        {
            ExecutedDialogs = PlayerDialogList.ThisDialog;
            string temp = "{ \"PlayerDialogs\" : [";
            foreach (var dialog in ExecutedDialogs)
            {
                temp += JsonUtility.ToJson(dialog,true) + ",";
            }

            temp = temp[..^1];
            temp += "]}";
            
            File.WriteAllText(pathName,temp);
        }

        private void ImportFromJson()
        {
            var jsonfromfile = File.ReadAllText(pathName);

            PlayerDialogs = JsonUtility.FromJson<PListDB>(jsonfromfile);
        }

        private void ExecuteJsonToGame()
        {
            if (PlayerDialogs == null) return;
            foreach (var dialog in PlayerDialogs.PlayerDialogs)
            {
                if (dialog.Seq < 0) continue;
                foreach (var dialogReal in ExecutedDialogs.Where(dialogReal => dialogReal.Seq == dialog.Seq))
                {
                    dialogReal.Dialog = dialog.Dialog;
                    dialogReal.ConnectToSeq = dialog.ConnectToSeq;
                    dialogReal.IsChoice = dialog.IsChoice;
                    dialogReal.ChoiceA = dialog.ChoiceA;
                    dialogReal.AConnectToSeq = dialog.AConnectToSeq;
                    dialogReal.IsForceEndA = dialog.IsForceEndA;
                    dialogReal.ForceQuitA = dialog.ForceQuitA;
                    dialogReal.ChoiceB = dialog.ChoiceB;
                    dialogReal.BConnectToSeq = dialog.BConnectToSeq;
                    dialogReal.IsForceEndB = dialog.IsForceEndB;
                    dialogReal.ForceQuitB = dialog.ForceQuitB;
                }
            }
        }
    }
}
