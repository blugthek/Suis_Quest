using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Class;
using GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Manager
{
    public class RollDiceManager : MonoBehaviour
    {
        [Header("Init")]
        public GameObject Panel;
        public Button RollDiceButton;
        public TextMeshProUGUI DiceNumber;

        [Header("Setup")] 
        private static int diceNumber;
        public static event Action<int> RolledDice;
        public List<GameObject> Dice;
        private void Awake()
        {
            Panel.SetActive(true);
            RollDiceButton.onClick.AddListener(DiceRolling);
        }

        private void Update()
        {
            Panel.SetActive(PlayerStatesManager.Instance.currentState.ThisStateName == PlayerBaseState.StateName.RollDice);
        }

        private async void DiceRolling()
        {
            diceNumber = Random.Range(1, 6);
            var biasDice = Random.Range(1,6);
            if (biasDice > diceNumber)
            {
                diceNumber = biasDice;
            }
            await OnRolledDice();
            DiceNumber.text = diceNumber.ToString();
            DiceShow(diceNumber);
            await Task.Delay(600);
            RolledDice?.Invoke(diceNumber);
        }

        private async Task OnRolledDice()
        {
            for (var i = 0; i < 15; i++)
            {
                var diceNum = Random.Range(1, 6);
                DiceShow(diceNum);
                DiceNumber.text = diceNum.ToString();
                await Task.Delay(10 * (i + 1));
            }

            await Task.Delay(300);
            await Task.CompletedTask;
        }

        private void DiceShow(int diceNum)
        {
            for (int i = 0; i < Dice.Count; i++)
            {
                Dice[i].SetActive(i + 1 == diceNum);
            }
        }
    }
}
