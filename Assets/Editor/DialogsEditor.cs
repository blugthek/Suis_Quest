using System;
using Class;
using ScriptableObjects;
using UnityEditor;

namespace Editor
{
    // [CustomEditor(typeof(PlayerDialog))]
    // public class PlayerDialogEditor : UnityEditor.Editor
    // {
    //     public enum MyButtonType
    //     {
    //         Normal,
    //         EndScenario,
    //         EndingGame,
    //         ToAmbulance,
    //     }
    //
    //     public MyButtonType ButtonTypeA;
    //     public MyButtonType ButtonTypeB;
    //     
    //     
    //     public override void OnInspectorGUI()
    //     {
    //         var playerDialog = target as PlayerDialog;
    //         BaseInspector();
    //         ChoiceInspector();
    //         serializedObject.ApplyModifiedProperties();
    //     }
    //     private void BaseInspector()
    //     {
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("Seq"));
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("Dialog"));
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("ConnectToSeq"));
    //         
    //     }
    //
    //     private void ChoiceInspector()
    //     {
    //         SerializedProperty isChoice = serializedObject.FindProperty("IsChoice");
    //         EditorGUILayout.Separator();
    //         EditorGUILayout.PropertyField(isChoice);
    //         if (isChoice.boolValue)
    //         {
    //             ChoiceA();
    //             ChoiceB();
    //         }
    //     }
    //
    //     private void ChoiceA()
    //     {
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("ChoiceA"));
    //         var forceEnd = serializedObject.FindProperty("IsForceEndA");
    //         var forceQuit = serializedObject.FindProperty("ForceQuitA");
    //         var forceAmbulance = serializedObject.FindProperty("ForceAmbulanceA");
    //         ButtonTypeA = true switch
    //         {
    //             true when forceEnd.boolValue => MyButtonType.EndScenario,
    //             true when forceQuit.boolValue => MyButtonType.EndingGame,
    //             true when forceAmbulance.boolValue => MyButtonType.ToAmbulance,
    //             _ => MyButtonType.Normal
    //         };
    //         ButtonTypeA = (MyButtonType) EditorGUILayout.EnumPopup("Type",ButtonTypeA);
    //         switch (ButtonTypeA)
    //         {
    //             case MyButtonType.Normal:
    //                 EditorGUILayout.PropertyField(serializedObject.FindProperty("AConnectToSeq"));
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = false;
    //                 forceAmbulance.boolValue = false;
    //                 break;
    //             case MyButtonType.EndScenario:
    //                 forceEnd.boolValue = true;
    //                 forceQuit.boolValue = false;
    //                 forceAmbulance.boolValue = false;
    //                 break;
    //             case MyButtonType.EndingGame:
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = true;
    //                 forceAmbulance.boolValue = false;
    //                 break;
    //             case MyButtonType.ToAmbulance:
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = false;
    //                 forceAmbulance.boolValue = true;
    //                 break;
    //         }
    //     }
    //     private void ChoiceB()
    //     {
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("ChoiceB"));
    //         var forceEnd = serializedObject.FindProperty("IsForceEndB");
    //         var forceQuit = serializedObject.FindProperty("ForceQuitB");
    //         var forceAmbulance = serializedObject.FindProperty("ForceAmbulanceB");
    //         ButtonTypeB = true switch
    //         {
    //             true when forceEnd.boolValue => MyButtonType.EndScenario,
    //             true when forceQuit.boolValue => MyButtonType.EndingGame,
    //             true when forceAmbulance.boolValue => MyButtonType.ToAmbulance,
    //             _ => MyButtonType.Normal
    //         };
    //         ButtonTypeB = (MyButtonType) EditorGUILayout.EnumPopup("Type",ButtonTypeB);
    //         switch (ButtonTypeB)
    //         {
    //             case MyButtonType.Normal:
    //                 EditorGUILayout.PropertyField(serializedObject.FindProperty("BConnectToSeq"));
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = false;
    //                 break;
    //             case MyButtonType.EndScenario:
    //                 forceEnd.boolValue = true;
    //                 forceQuit.boolValue = false;
    //                 break;
    //             case MyButtonType.EndingGame:
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = true;
    //                 break;
    //             case MyButtonType.ToAmbulance:
    //                 forceEnd.boolValue = false;
    //                 forceQuit.boolValue = false;
    //                 forceAmbulance.boolValue = true;
    //                 break;
    //         }
    //     }
    // }

    [CustomEditor(typeof(Node))]
    public class NodeDialogsEditor : UnityEditor.Editor
    {
        private Node currentTarget;
        public bool Expose = true;
        public override void OnInspectorGUI()
        {
            Expose = EditorGUILayout.Toggle(Expose);
            if (!Expose)
            {
                currentTarget = (Node) target;
                BaseInspector();
                SwitchNodeInspector();
            }
            else
            {
                base.OnInspectorGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void BaseInspector()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ThisNodeIndex"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ThisNodePos"));
            currentTarget.ThisNodePos = currentTarget.transform;

            currentTarget.ThisNodeSetting = (Node.ThisNodeEvent) EditorGUILayout.EnumPopup("Type",currentTarget.ThisNodeSetting);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("NextNode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Selectable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ThisRenderer"));
        }

        private void SwitchNodeInspector()
        {
            switch (currentTarget.ThisNodeSetting)
            {
                case Node.ThisNodeEvent.Normal:
                    break;
                case Node.ThisNodeEvent.Dialog:
                    NodeDialogInspector();
                    break;
                case Node.ThisNodeEvent.DrawCard:
                    break;
                default:
                    break;
            }
        }

        private void NodeDialogInspector()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ThisDialog"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentSeq"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Exporting"));

        }
    }
}