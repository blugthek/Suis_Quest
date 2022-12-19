using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerDialog", menuName = "ScriptableObjects/PlayerDialog", order = 1)]
    public class PlayerDialog : ScriptableObject
    {
        public int Seq;
        [TextArea(15,20)]
        [Header("Dialog")]
        public string Dialog;
        public int ConnectToSeq;
        [Header("Choice Setting")]
        [Header("Is it Choice")]
        public bool IsChoice;
        [Header("Choice A")]
        public string ChoiceA;
        public int AConnectToSeq;
        public bool IsForceEndA;
        public bool ForceQuitA;
        public bool ForceAmbulanceA;
        [Header("Choice B")]
        public string ChoiceB;
        public int BConnectToSeq;
        public bool IsForceEndB;
        public bool ForceQuitB;
        public bool ForceAmbulanceB;

        public void UpdateStruct(string newDialog)
        {
            Dialog = newDialog;
        }
    }
}