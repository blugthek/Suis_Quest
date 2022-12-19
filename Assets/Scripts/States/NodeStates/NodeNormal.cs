using Class;
using GamePlay;
using UnityEngine;

namespace States.NodeStates
{
    public class NodeNormal : NodeEvent
    {
        public NodeNormal(int nodeIndex, Node parent) : base(nodeIndex, parent)
        {
            ThisNodeStateName = NodeStateName.Normal;
        }
        public override void Starting()
        {
            NodeManager.Instance.NodeSelectEvent.AddListener(NodeNormalEffect);
            base.Starting();
        }

        public override void Running()
        {
            base.Running();
        }

        private void NodeNormalEffect(int selectedNode)
        {
            if (selectedNode == NodeIndex)
            {
                Debug.Log("Yes it hit me " + NodeIndex);
            }
        }

    }
}