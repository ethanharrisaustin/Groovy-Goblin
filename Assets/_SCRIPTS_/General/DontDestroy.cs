using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    static List<string> objectNames = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < objectNames.Count; ++i)
        {
            if (gameObject.name == objectNames[i]) Destroy(gameObject);
        }

        objectNames.Add(gameObject.name);

        DontDestroyOnLoad(gameObject);
    }
}
