using UnityEngine;
using System;

public class Input : MonoBehaviour
{
    public static Action onCombat1, onCombat2, onCombat3, onCombat4;

    static Input instance;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void OnCombat1()
    {
        onCombat1?.Invoke();
    }

    void OnCombat2()
    {
        onCombat2?.Invoke();
    }

    void OnCombat3()
    {
        onCombat3?.Invoke();
    }

    void OnCombat4()
    {
        onCombat4?.Invoke();
    }
}
