using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance { get; private set; }

    public enum KeyState
    {
        KeyDown, Key, KeyUp, KeyNone
    }

    private Dictionary<KeyCode, KeyState> keyState = new();
    private KeyCode[] target =
    {
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
    };

    public KeyState GetKeyState(KeyCode key) => keyState[key];

    public InputSystem()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (KeyCode key in target)
            keyState.Add(key, KeyState.KeyNone);
    }

    public void PreTick()
    {
        foreach (KeyCode key in target)
        {
            if (Input.GetKey(key))
            {
                if (keyState[key] == KeyState.KeyUp || keyState[key] == KeyState.KeyNone)
                    keyState[key] = KeyState.KeyDown;
                else
                    keyState[key] = KeyState.Key;
            }
            else
            {
                if (keyState[key] == KeyState.KeyUp || keyState[key] == KeyState.KeyNone)
                    keyState[key] = KeyState.KeyNone;
                else
                    keyState[key] = KeyState.KeyUp;
            }
        }
    }
}
