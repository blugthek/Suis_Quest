using Class;
using GamePlay;
using UnityEngine;

namespace States.NodeStates
{
    public class NodeDrawCard : NodeEvent
    {
        private bool entering = false;
        public NodeDrawCard(int nodeIndex, Node parent) : base(nodeIndex, parent)
        {
            ThisNodeStateName = NodeStateName.DrawCard;
        }
        public override void Starting()
        {
            NodeManager.Instance.NodeSelectEvent.AddListener(NodeNormalEffect);
            base.Starting();
        }

        public override void Running()
        {
            if (!entering || NodeIndex != CurrentNode) return;
            if (PlayerStatesManager.Instance.CheckCurrentStateName(PlayerBaseState.StateName.Waiting))
            {
                
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