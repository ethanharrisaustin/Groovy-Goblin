using System.Collections.Generic;
using UnityEngine;

public class ComboInput : MonoBehaviour
{
    public ObjectPool objectPool;
    public BasicMetronomeObject basicMetronomeObject;
    RhythmicCombos rhythmicCombos;
    RB_ComboGhosting comboGhosting;

    public bool makingCombo = false;
    List<CombatInput> currentCombatInputs = new List<CombatInput>();

    void Awake()
    {
        rhythmicCombos = GetRhythmicCombos.Get();
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

        currentCombatInputs.Add(new CombatInput(new int[] { combatIndex }, 0f));


        bool isValidCombo, finishedCombo;
        comboGhosting.ShowComboGhosting(currentCombatInputs, out isValidCombo, out finishedCombo);

        if (finishedCombo)
        {
            makingCombo = false;
            currentCombatInputs.Clear();

            FindFirstObjectByType<ObjectWithHealthGO>().ApplyDamange(40);
        }

        if (!isValidCombo)
        {
            makingCombo = false;
            currentCombatInputs.Clear();
        }

        //Debug.Log("Valid Combo: " + isValidCombo + ", Finished Combo: " + finishedCombo);
    }
}
