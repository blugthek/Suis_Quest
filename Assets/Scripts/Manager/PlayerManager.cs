using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Class;
using UnityEngine;

namespace GamePlay
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Init")]
        public static PlayerManager Instance;

        [Header("Player Setup")] 
        public PlayerBehavior ThisPlayerBehavior;

        public int StartIndex;

        [field: Header("Event")] 
        public static event Action MoveCompleted;

        private void Awake() => Instance = this;

        public void PlayerInit()
        {
            NodeManager.Instance.NodeSelectedEvent.AddListener(TaskMoveTo);
        }

        private async Task PlayerMoveTo(Node targetNode, Node startNode)
        {
            var tempPath = NodeManager.Instance.WhatConnect;
            
            if (targetNode != startNode)
            {
                if (tempPath == null)
                {
                    await ThisPlayerBehavior.Movement(targetNode.ThisNodePos);
                }
                else
                {
                    foreach (var node in tempPath)
                    {
                        await ThisPlayerBehavior.Movement(node.ThisNodePos);
                    }
                }
            }
            else
            {
                await ThisPlayerBehavior.Movement(startNode.ThisNodePos);
            }
        }

        private async void TaskMoveTo(Node targetNode, Node startNode)
        {
            await PlayerMoveTo(targetNode,startNode);

            MoveCompleted?.Invoke();
            await Task.CompletedTask;
        }
    }
}