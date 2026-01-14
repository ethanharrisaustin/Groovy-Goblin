using UnityEngine;
using MapRoomSystem;

public class UI_HealthBarSpawner : MonoBehaviour
{
    ObjectPool objectPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnHealthBars();
    }

    void SpawnHealthBars()
    {
        bool changedObjectsWithHealth;
        ObjectWithHealthGO[] objectWithHealthGOs = GetObjectWithHealthGOs(out changedObjectsWithHealth);

        if (!changedObjectsWithHealth) return;

        objectPool.DestroyAll();

        for (int i = 0; i < objectWithHealthGOs.Length; ++i)
        {
            UI_HealthBar healthBar = objectPool.SpawnObject().GetComponent<UI_HealthBar>();

            healthBar.objectWithHealthGO = objectWithHealthGOs[i];
        }
    }

    ObjectWithHealthGO[] GetObjectWithHealthGOs(out bool recalculatedActives)
    {
        return MapRoomSystem.MapRoomSystem.GetRoomObjectGOs<ObjectWithHealthGO>(out recalculatedActives);
    }
}
