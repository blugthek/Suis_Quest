using Class;
using GamePlay;
using TMPro;
using UnityEngine;

namespace States
{
    public class Walking : PlayerBaseState
    {
        public Walking(TextMeshPro txtStatus) : base(txtStatus)
        {
            ThisStateName = StateName.Walking;
        }
        public override void Enter()
        {
            TxtStatus.text = "กำลังเดิน...";
            PlayerManager.MoveCompleted += MoveDone;
            
            base.Enter();
        }

        private bool moveDone = false;
        private void MoveDone()
        {
            moveDone = true;
        }

        public override void Update()
        {
            if (!moveDone) return;
            NextState = PlayerStatesManager.Instance.Waiting;
            PlayerManager.MoveCompleted -= MoveDone;
            moveDone = false;
            ThisStateEvent = StateEvent.Exit;
        }
    }
}