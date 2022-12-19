using System;
using Class;
using States;
using TMPro;
using UnityEngine;

namespace GamePlay
{
    public class PlayerStatesManager : MonoBehaviour
    {
        [Header("Init")]
        public static PlayerStatesManager Instance;
        public TextMeshPro TxtStatus;

        [Header("Player Setup")]
        public PlayerBaseState currentState;

        [field: Header("Event")] 
        public static event Action EnterWaiting;

        public static void InvokeEnter()
        {
            Debug.Log("Enter State");
            EnterWaiting?.Invoke();
        }

        public RollDice RollDice => new RollDice(TxtStatus);
        public Select Select => new Select(TxtStatus);
        public Walking Walking => new Walking(TxtStatus);
        public Waiting Waiting => new Waiting(TxtStatus);
        public Freezing Freezing => new Freezing(TxtStatus);

        private void Awake() => Instance = this;

        void Start() => currentState = Waiting;

        private void Update() => currentState = currentState.Process();

        public bool CheckCurrentStateName(PlayerBaseState.StateName arg)
        {
            return arg == currentState.ThisStateName;
        }
    }
}