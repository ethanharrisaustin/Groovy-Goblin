using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public static Action onCombat1, onCombat2, onCombat3, onCombat4;

    static Input instance;

    public static Vector2 movement;

    public enum Direction { none, north, east, south, west };

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void OnCombat1(InputAction.CallbackContext context)
    {
        onCombat1?.Invoke();
    }

    public void OnCombat2(InputAction.CallbackContext context)
    {
        onCombat2?.Invoke();
    }

    public void OnCombat3(InputAction.CallbackContext context)
    {
        onCombat3?.Invoke();
    }

    public void OnCombat4(InputAction.CallbackContext context)
    {
        onCombat4?.Invoke();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public static Direction MovementAsDirection(bool favourX = true)
    {
        bool xIsBiggest = favourX ? Mathf.Abs(movement.x) * 0.5f > Mathf.Abs(movement.y) :  Mathf.Abs(movement.x) >= Mathf.Abs(movement.y) * 0.5f;

        if (xIsBiggest)
        {
            if (Mathf.Abs(movement.x) < 0.2f) return Direction.none;
            if (movement.x > 0) return Direction.east;
            return Direction.west;
        }
        else
        {
            if (Mathf.Abs(movement.y) < 0.2f) return Direction.none;
            if (movement.y > 0) return Direction.north;
            return Direction.south;
        }
    }
}
