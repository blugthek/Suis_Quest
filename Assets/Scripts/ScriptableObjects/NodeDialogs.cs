using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NodeDialog", menuName = "ScriptableObjects/NodeDialog", order = 1)]
    public class NodeDialogs : ScriptableObject
    {
        public int Seq;
        [TextArea(15,20)]
        public string Dialog;
        public int ConnectToSeq;
    }
}