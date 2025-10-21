using UnityEngine;
using UnityEngine.UI;

public class UI_CombatInput : RB_Beat
{
    [Space]
    public Sprite[] combatSprites;
    public Color[] combatColours;
    public Image combatAccuracyLineImg, controllerIconImg;

    public override void SpawnBeat(int combatInput, BasicMetronomeObject basicMetronomeObject)
    {
        base.SpawnBeat(0, basicMetronomeObject); // Make it new bar scale

        combatAccuracyLineImg.color = combatColours[combatInput];
        controllerIconImg.sprite = combatSprites[combatInput];
    }
}
