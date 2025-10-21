using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RhythmicCombo", menuName = "Scriptable Objects/RhythmicCombo")]
[System.Serializable]
public class RhythmicCombos : ScriptableObject
{
    public Combo[] combos;

    public List<Combo> GetPotentialCombos(List<int> combatInputs)
    {
        List<Combo> returnValue = new List<Combo>();

        for (int i = 0; i < combos.Length; ++i)
        {
            if (!PotentialCombo(combatInputs, combos[i])) continue;

            returnValue.Add(combos[i]);
        }

        return returnValue;
    }
    

    public Combo GetCompletedCombo(List<int> combatInputs)
    {
        if (CompletedCombo(combatInputs, out Combo completedCombo)) return completedCombo;

        return null;
    }
    public bool CompletedCombo(List<int> combatInputs, out Combo completedCombo)
    {
        for (int i = 0; i < combos.Length; ++i)
        {
            if (!PotentialCombo(combatInputs, combos[i])) continue;

            completedCombo = combos[i];
            return true;
        }

        completedCombo = null;
        return false;
    }


    bool PotentialCombo(List<int> combatInputs, Combo cCombo)
    {
        if (cCombo == null || cCombo.comboPieces == null) return false;
        if (cCombo.comboPieces.Length < combatInputs.Count) return false;

        for (int i = 0; i < combatInputs.Count; ++i) if (!cCombo.comboPieces[i].Matches(combatInputs[i])) return false;

        return true;
    }

    public static RhythmicCombos Get()
    {
        return GetRhythmicCombos.Get();
    }
}

[System.Serializable]
public class Combo
{
    public string name;
    public ComboPiece[] comboPieces;
}

[System.Serializable]
public class ComboPiece
{
    public float beatBetweenPrevious = 1;

    public ComboButton comboNumber;

    public bool Matches(int combatIndex)
    {
        return ((int)comboNumber - 1) == combatIndex;
    }

}

public enum ComboButton
{
    notSet, buttonA, buttonB, buttonX, buttonY
}