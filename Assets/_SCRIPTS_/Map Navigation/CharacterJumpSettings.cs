using UnityEngine;

public class CharacterJumpSettings : MonoBehaviour
{
    public static CharacterJumpSettings instance;

    public AnimationCurve jumpYCurve;
    public AnimationCurve jumpXZCurve;
    public float jumpTime;
    public float jumpHeight;

    void Awake()
    {
        instance = this;
    }

    public static void GetSettings(out AnimationCurve jumpYCurve, out AnimationCurve jumpXZCurve, out float jumpTime, out float jumpHeight)
    {
        jumpYCurve = instance.jumpYCurve;
        jumpXZCurve = instance.jumpXZCurve;
        jumpTime = instance.jumpTime;
        jumpHeight = instance.jumpHeight;
    }
}
