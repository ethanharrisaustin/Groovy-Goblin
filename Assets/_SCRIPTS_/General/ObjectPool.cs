using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject original;

    List<GameObject> objectPool = new List<GameObject>();

    public GameObject SpawnObject()
    {
        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].activeSelf) continue;

            objectPool[i].SetActive(true);

            return objectPool[i];
        }

        GameObject newObject = Instantiate(original, transform);

        objectPool.Add(newObject);

        return newObject;
    }

    public void DestroyObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
