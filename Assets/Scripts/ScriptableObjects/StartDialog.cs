using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "StartDialog", menuName = "ScriptableObjects/StartDialog", order = 1)]

    public class StartDialog : ScriptableObject
    {
        [Header("Dialog")]
        public string Dialog;

        public bool Done;
    }
}
