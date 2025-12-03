using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MapRoomSystem
{
    public class RoomObjectPool : MonoBehaviour
    {
        public GameObject prefab;

        public List<RoomObjectGO> pool = new List<RoomObjectGO>();

        public void SpawnRoomObject(RoomObject roomObject)
        {
            if (!RoomObject.PrefabIsMatching(roomObject, prefab)) return;

            RoomObjectGO newRoomObjectGO = GetRoomObjectGO();

            newRoomObjectGO.transform.parent = transform;

            newRoomObjectGO.Spawn(roomObject);
        }

        public void DestroyRoomObject(RoomObject roomObject)
        {
            if (!RoomObject.PrefabIsMatching(roomObject, prefab)) return;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (!RoomObject.Matching(roomObject, pool[i].roomObject)) continue;
                
                pool[i].gameObject.SetActive(false);
            }
        }

        public void DestroyAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].gameObject.SetActive(false);

                ++i;
            }
        }

        public void RemoveAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].Remove();

                ++i;
            }
        }

        public void InitAllRoomObjects()
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Count;)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    continue;
                }

                pool[i].Init();

                ++i;
            }
        }

        RoomObjectGO GetRoomObjectGO()
        {
            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == null)
                {
                    pool.RemoveAt(i);
                    i--;
                    continue;
                }
                
                if (pool[i].gameObject.activeSelf == false)
                {
                    pool[i].gameObject.SetActive(true);
                    return pool[i];
                }
            }

            #if UNITY_EDITOR

            if (GetRoomObjectGOFromChild(out var roomObjectGO))  return roomObjectGO;

            RoomObjectGO newRoomObjectGO = PrefabUtility.InstantiatePrefab(prefab).GetComponentInChildren<RoomObjectGO>();

            #else

            RoomObjectGO newRoomObjectGO = Instantiate(prefab).GetComponentInChildren<RoomObjectGO>();

            #endif

            pool.Add(newRoomObjectGO);

            return newRoomObjectGO;
        }

        bool finishedFlyingIn = false;
        public void OnStartSpawning()
        {
            finishedFlyingIn = false;
        }
        public bool FinishedFlyingIn()
        {
            if (finishedFlyingIn) return true;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i].FinishedFlyingIn() == false) return false;
            }

            finishedFlyingIn = true;
            return true;
        }

        bool finishedFlyingOut = false;
        public void OnStartRemoving()
        {
            finishedFlyingOut = false;
        }
        public bool FinishedFlyingOut()
        {
            if (finishedFlyingOut) return true;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i].FinishedFlyingOut() == false) return false;
            }

            finishedFlyingOut = true;
            return true;
        }

        #region  Unity Editor

        #if UNITY_EDITOR

        bool GetRoomObjectGOFromChild(out RoomObjectGO roomObjectGO)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                RoomObjectGO roGO;
                
                if (!transform.GetChild(i).TryGetComponent(out roGO))
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    continue;
                }

                if (RoomObjectGOInPool(roGO)) continue;

                pool.Add(roGO);

                roGO.gameObject.SetActive(true);

                roomObjectGO = roGO;

                return true;
            }

            roomObjectGO = null;
            return false;
        }

        bool RoomObjectGOInPool(RoomObjectGO roomObjectGO)
        {
            if (roomObjectGO == null) return false;

            for (int i = 0; i < pool.Count; ++i)
            {
                if (pool[i] == roomObjectGO) return true;
            }
            return false;
        }

        #endif

        #endregion
    }
}