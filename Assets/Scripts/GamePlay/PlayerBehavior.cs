using System;
using System.Threading.Tasks;
using UnityEngine;
using static Utility.Utility;

namespace GamePlay
{
    public class PlayerBehavior : MonoBehaviour
    {
        public Vector3 TargetVector3;
        public bool ToTarget = false;
        [Range(1,10)]public float MoveSpeed;
        public void Movement(Vector3 targetPos)
        {
            ToTarget = false;
            TargetVector3 = targetPos;
        }
        
        public async Task Movement(Transform tTransform)
        {
            ToTarget = false;
            var targetPos = tTransform.position;
            TargetVector3 = targetPos;
            while (!ToTarget)
            {
                await Task.Delay(10);
            }

            await Task.CompletedTask;
        }

        private void Update()
        {
            var currentPos = gameObject.transform.position;
            var targetPos = TargetVector3;
            var currentSpeed = MoveSpeed * 0.01f;
            if (Vector3.Distance(currentPos,targetPos) >= 0.01f)
            {
                gameObject.transform.position = LerpVector3(currentPos, targetPos, currentSpeed);
            }
            else
            {
                ToTarget = true;
            }
        }
    }
}