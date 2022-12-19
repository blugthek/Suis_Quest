using Class;
using GamePlay;
using TMPro;
using UnityEngine;

namespace States
{
    public class Select : PlayerBaseState
    {
        private bool trigger = false;
        public Select(TextMeshPro txtStatus) : base(txtStatus)
        {
            ThisStateName = StateName.Select;
        }
        
        public override void Enter()
        {
            TxtStatus.text = "เลือกเป้าหมาย...";
            NodeManager.Instance.NodeSelectEvent.AddListener(Selecting);
            
            base.Enter();
        }

        private void Selecting(int target)
        {
            trigger = true;
        }

        public override void Update()
        {
            if (!trigger) return;
            NextState = PlayerStatesManager.Instance.Walking;
            trigger = false;
            ThisStateEvent = StateEvent.Exit;
        }
    }
}