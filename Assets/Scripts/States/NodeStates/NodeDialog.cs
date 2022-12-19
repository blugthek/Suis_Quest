using Class;
using GamePlay;
using UnityEngine;

namespace States.NodeStates
{
    public class NodeDialog : NodeEvent
    {
        private bool entering = false;
        public NodeDialog(int nodeIndex, Node parent) : base(nodeIndex, parent)
        {
            ThisNodeStateName = NodeStateName.Dialog;
        }
        public override void Starting()
        {
            NodeManager.Instance.NodeSelectEvent.AddListener(NodeNormalEffect);
            base.Starting();
        }
        public override void Running()
        {
            if (!entering)return;
            entering = false;
            if (PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Waiting))
            {
                if (NodeIndex == CurrentNode)
                {
                    NodeParent.StartDialog();
                }
            }
            base.Running();
        }

        private void NodeNormalEffect(int selectedNode)
        {
            CurrentNode = selectedNode;
            entering = true;
            if (selectedNode == NodeIndex)
            {
                Debug.Log("Yes it hit me " + NodeIndex);
            }
        }
        
        
    }
}