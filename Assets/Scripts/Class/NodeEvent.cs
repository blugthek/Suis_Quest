using System;
using GamePlay;
using UnityEngine;

namespace Class
{
    public class NodeEvent
    {
        public enum NodeStateName
        {
            Normal,
            Dialog,
            DrawCard,
        }

        public enum NodeState
        {
            Start,
            Running,
            Stop,
        }

        [Header("State Setting")] 
        public NodeStateName ThisNodeStateName;
        public NodeState ThisNodeState;

        public NodeEvent NextEvent;

        [Header("This Node Parent")] 
        public Node NodeParent;
        protected int CurrentNode;

        protected readonly int NodeIndex;

        public NodeEvent(int nodeIndex,Node parent)
        {
            this.NodeIndex = nodeIndex;
            NodeParent = parent;
        }

        public virtual void Starting()
        {
            ThisNodeState = NodeState.Running;
        }

        public virtual void Running()
        {
            ThisNodeState = NodeState.Running;
        }

        public virtual void Stopping()
        {
            ThisNodeState = NodeState.Stop;
        }

        public NodeEvent Process()
        {
            var result = this;

            switch (ThisNodeState)
            {
                case NodeState.Start:
                    Starting();
                    break;
                case NodeState.Running:
                    Running();
                    break;
                case NodeState.Stop:
                    Stopping();
                    return NextEvent;
                default:
                    ThisNodeState = NodeState.Start;
                    break;
            }

            return result;
        }
    }
}