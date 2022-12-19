using Class;
using GamePlay;
using Manager;
using TMPro;
using UnityEngine;

namespace States
{
    public class RollDice : PlayerBaseState
    {
        public float Timer = 0;
        public bool trigger = false;
        public RollDice(TextMeshPro txtStatus) : base(txtStatus)
        {
            ThisStateName = StateName.RollDice;
        }
        
        
        public override void Enter()
        {
            TxtStatus.text = "ทอยเต๋า";
            RollDiceManager.RolledDice += DiceRolled;
            
            base.Enter();
        }

        private void DiceRolled(int d)
        {
            trigger = true;
        }

        public override void Update()
        {
            if (!trigger) return;
            NextState = PlayerStatesManager.Instance.Select;
            RollDiceManager.RolledDice -= DiceRolled;
            trigger = false;
            ThisStateEvent = StateEvent.Exit;
        }
    }
}