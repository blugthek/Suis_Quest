using System;
using TMPro;
using UnityEngine;

namespace Class
{
    public class PlayerBaseState
    {
        public enum StateName
        {
            RollDice,
            Select,
            Walking,
            Waiting,
        }

        public enum StateEvent
        {
            Enter,
            Update,
            Exit,
        }

        [Header("State Setting")]
        public StateName ThisStateName;
        public StateEvent ThisStateEvent;

        public PlayerBaseState NextState;

        [Header("Player DECO")] 
        public readonly TextMeshPro TxtStatus;

        public PlayerBaseState(TextMeshPro txtStatus)
        {
            TxtStatus = txtStatus;
        }

        public virtual void Enter()
        {
            ThisStateEvent = StateEvent.Update;
        }
        
        public virtual void Update()
        {
            ThisStateEvent = StateEvent.Update;
        }
        
        public virtual void Exit()
        {
            ThisStateEvent = StateEvent.Exit;
        }

        public PlayerBaseState Process()
        {
            var result = this;

            switch (ThisStateEvent)
            {
                case StateEvent.Enter:
                    Enter();
                    break;
                case StateEvent.Update:
                    Update();
                    break;
                case StateEvent.Exit:
                    Exit();
                    return NextState;
                default:
                    throw new ArgumentException();
            }

            return result;
        }
    }
}