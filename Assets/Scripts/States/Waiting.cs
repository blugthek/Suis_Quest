using Class;
using GamePlay;
using Manager;
using TMPro;
using UnityEngine;

namespace States
{
    public class Waiting : PlayerBaseState
    {
        public Waiting(TextMeshPro txtStatus) : base(txtStatus)
        {
            ThisStateName = StateName.Waiting;
        }

        private bool moveDone = false;
        public override void Enter()
        {
            TxtStatus.text = "กำลังคิด...";
            PlayerStatesManager.InvokeEnter();
            GameManager.GameManagerCallChangeState += ChangeState;

            base.Enter();
        }

        private void ChangeState()
        {
            moveDone = true;
        }

        public override void Update()
        {
            if (!moveDone) return;
            NextState = PlayerStatesManager.Instance.RollDice;
            GameManager.GameManagerCallChangeState -= ChangeState;
            moveDone = false;
            ThisStateEvent = StateEvent.Exit;
        }
    }
}