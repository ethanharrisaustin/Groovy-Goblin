using System.Collections.Generic;
using UnityEngine;

public class RB_ComboGhosting : MonoBehaviour
{
    public ObjectPool objectPool;
    RhythmicCombos rhythmicCombos;

    void Awake()
    {
        rhythmicCombos = RhythmicCombos.Get();
    }

    public void ShowComboGhosting(List<int> currentCombatInputs, out bool isValidCombo, out bool finishedCombo)
    {
        List<Combo> potentialCombos = rhythmicCombos.GetPotentialCombos(currentCombatInputs);

        if (potentialCombos.Count == 0)
        {
            isValidCombo = false;
            finishedCombo = false;
            return;
        }

        if (rhythmicCombos.CompletedCombo(currentCombatInputs, out Combo completedCombo))
        {
            isValidCombo = true;
            finishedCombo = true;
            return;
        }

        isValidCombo = true;
        finishedCombo = false;
    }
}
