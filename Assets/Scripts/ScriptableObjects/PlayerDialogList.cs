using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects
{
    
    [CreateAssetMenu(fileName = "PlayerDialogList", menuName = "ScriptableObjects/PlayerDialogList", order = 1)]
    public class PlayerDialogList : ScriptableObject
    {
        [Header("Dialog List")] public List<PlayerDialog> ThisDialog;

        public List<PlayerDialog> GetDialogList()
        {
            var tempDialog = new List<PlayerDialog>();
            tempDialog.AddRange(ThisDialog.Select(playerDialog => playerDialog));
            return tempDialog;
        }
        
        [Header("Update Dialog")] public bool IsUpdate;

        public List<PlayerDialog> UpdateDialogList(List<PlayerDialog> oldList)
        {
            List<PlayerDialog> tempDialog;
            var oldDialog = oldList;
            tempDialog = GetDialogList();
            for (int i = 0; i < oldDialog.Count; i++)
            {
                for (int j = 0; j < tempDialog.Count; j++)
                {
                    if (oldDialog[i].Seq == tempDialog[j].Seq)
                    {
                        oldDialog[i].UpdateStruct(tempDialog[j].Dialog);
                    }
                }
            }
            return oldDialog;
        }
    }
}