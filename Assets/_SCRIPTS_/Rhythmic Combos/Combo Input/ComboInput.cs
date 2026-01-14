using System.Collections.Generic;
using UnityEngine;


namespace Combat
{
    public class ComboInput : MonoBehaviour
    {
        public ObjectPool objectPool;
        public BasicMetronomeObject basicMetronomeObject;
        RB_ComboGhosting comboGhosting;

        public bool makingCombo = false;
        public List<CombatInput> currentCombatInputs = new List<CombatInput>();

        void Awake()
        {
            comboGhosting = GetComponent<RB_ComboGhosting>();
        }

        void OnEnable()
        {
            Input.onCombat1 += Combat1;
            Input.onCombat2 += Combat2;
            Input.onCombat3 += Combat3;
            Input.onCombat4 += Combat4;
        }

        void OnDisable()
        {
            Input.onCombat1 -= Combat1;
            Input.onCombat2 -= Combat2;
            Input.onCombat3 -= Combat3;
            Input.onCombat4 -= Combat4;
        }

        void RhythmTimer()
        {
            
        }

        void Combat1()
        {
            SpawnCombatInput(0);
        }

        void Combat2()
        {
            SpawnCombatInput(1);
        }

        void Combat3()
        {
            SpawnCombatInput(2);
        }

        void Combat4()
        {
            SpawnCombatInput(3);
        }

        void SpawnCombatInput(int combatIndex)
        {
            UI_CombatInput combatInput = objectPool.SpawnObject().GetComponent<UI_CombatInput>();

            combatInput.SpawnBeat(combatIndex, basicMetronomeObject);

            makingCombo = true;

            currentCombatInputs.Add(new CombatInput(new int[] { combatIndex }, MusicRhythmTimer.instance.Accuracy()));

            bool isValidCombo, finishedCombo;
            Combo potentialCombo;
            comboGhosting.ShowComboGhosting(currentCombatInputs, out isValidCombo, out finishedCombo, out potentialCombo);

            if (finishedCombo)
            {
                // We can use this to attack an enemy
                CombatAttack combatAttack = new CombatAttack(potentialCombo, currentCombatInputs);

                makingCombo = false;
                currentCombatInputs.Clear();                
            }

            if (!isValidCombo)
            {
                makingCombo = false;
                currentCombatInputs.Clear();
            }
        }
    }
}