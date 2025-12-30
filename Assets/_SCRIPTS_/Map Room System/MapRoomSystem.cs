namespace MapRoomSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MapNavigation;
    using UnityEngine;
    using Saving;

    public class MapRoomSystem : MonoBehaviour
    {
        #region  Variables
        public static MapRoomSystem instance;
        
        [Space]

        public RoomObjectFlyInSettings[] objectFlyInSettings;

        [Space]
        public GroupOfRooms groupOfRooms;
        List<RoomObjectPool> roomObjectPools = new List<RoomObjectPool>();

        Room currentRoom = null;

        #endregion
        
        #region Monobehaviour Functions
        void Awake()
        {
            instance = this;

            roomObjectPools.Clear();

            for (int i = 0; i < transform.childCount; ++i)
            {
                RoomObjectPool roomObjectPool = transform.GetChild(i).GetComponent<RoomObjectPool>();

                if (roomObjectPool != null) roomObjectPools.Add(roomObjectPool);
            }

            SpawnRoom(groupOfRooms.rooms[0]);
        }

        #endregion

        #region Spawning in Rooms

        public void SpawnRoomImmediately(Room room)
        {
            RemoveCurrentRoom();

            // Spawn and set positions
            for (int i = 0; i < room.roomObjects.Length; ++i)
            {
                RoomObjectPool roomObjectPool = GetRoomObjectPool(room.roomObjects[i]);

                if (roomObjectPool == null) continue;

                roomObjectPool.SpawnRoomObject(room.roomObjects[i]);
            }
        }

        public async void SpawnRoom(Room room, string entranceToPlacePlayer = "")
        {
            if (currentRoom == room) return;

            RemoveCurrentRoom();

            // Spawn and set positions
            for (int i = 0; i < room.roomObjects.Length; ++i)
            {
                RoomObjectPool roomObjectPool = GetRoomObjectPool(room.roomObjects[i]);

                if (roomObjectPool == null) continue;

                roomObjectPool.SpawnRoomObject(room.roomObjects[i]);
            }

            currentRoom = room;

            // Place player
            if (entranceToPlacePlayer != "")
            {
                DoorRoomTransitionGO doorRoomTransitionGO = GetDoorRoomTransitionGO(entranceToPlacePlayer);

                if (doorRoomTransitionGO != null)
                {
                    PlayerGO.instance.transform.position = doorRoomTransitionGO.targetPosition;
                }
            }

            // Set inside the object pool that we have started spawning objects in 
            OnStartSpawning();

            // While objects are spawning in, wait 
            while(!FinishedFlyingIn()) await Task.Delay(20);

            // Once they have all been spawned, initialise them!
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].InitAllRoomObjects();

            PlayerGO.instance.ActivatePlayer();
        }

        bool finishedFlyingIn = false;
        void OnStartSpawning()
        {
            finishedFlyingIn = false;
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].OnStartSpawning();
        }
        bool FinishedFlyingIn()
        {
            if (finishedFlyingIn) return true;

            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].FinishedFlyingIn() == false) return false;
            }

            finishedFlyingIn = true;
            return true;
        }

        #endregion

        #region  Swapping to Rooms

        public void SwapToRoom(string roomUniqueID)
        {
            for (int i = 0; i < groupOfRooms.rooms.Length; ++i)
            {
                if (groupOfRooms.rooms[i].roomUniqueID == roomUniqueID)
                {
                    SwapToRoom(groupOfRooms.rooms[i]);
                    return;
                }
            }
        }
        public async void SwapToRoom(Room room)
        {
            if (currentRoom == room) return;

            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i] == null) continue;

                roomObjectPools[i].RemoveAllRoomObjects();
            }

            OnStartRemoving();
            
            // While objects are spawning in, wait 
            while(!FinishedFlyingOut()) await Task.Yield();

            SpawnRoom(room, currentRoom.roomUniqueID);
        }

        bool finishedFlyingOut = false;
        void OnStartRemoving()
        {
            finishedFlyingOut = false;
            for (int i = 0; i < roomObjectPools.Count; ++i) roomObjectPools[i].OnStartRemoving();
        }
        bool FinishedFlyingOut()
        {
            if (finishedFlyingOut) return true;

            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].FinishedFlyingOut() == false) return false;
            }

            finishedFlyingOut = true;
            return true;
        }

        #endregion

        #region Door transtion

        DoorRoomTransitionGO GetDoorRoomTransitionGO(string roomUniqueID)
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObjectPools[i].prefab.GetComponent<DoorRoomTransitionGO>() == null) continue;

                for (int x = 0; x < roomObjectPools[i].pool.Count; ++x)
                {
                    DoorRoomTransitionGO doorRoomTransitionGO = roomObjectPools[i].pool[x].GetComponent<DoorRoomTransitionGO>();

                    if (doorRoomTransitionGO.roomUniqueID == roomUniqueID) return doorRoomTransitionGO;
                }
            }

            return null;
        }

        #endregion

        #region General

        RoomObjectPool GetRoomObjectPool(RoomObject roomObject)
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                if (roomObject == null || roomObject.prefab == null)
                {
                    Debug.Log("This room object is null");
                    return null;
                }

                if (roomObjectPools[i] == null || roomObjectPools[i].prefab == null)
                {
                    roomObjectPools.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!RoomObject.PrefabIsMatching(roomObject, roomObjectPools[i].prefab)) continue;

                return roomObjectPools[i];
            }


            #if UNITY_EDITOR

            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).TryGetComponent(out RoomObjectPool roomObjectPool))
                {
                    if (!RoomObject.PrefabIsMatching(roomObject, roomObjectPool.prefab)) continue;

                    roomObjectPools.Add(roomObjectPool);



                    return roomObjectPool;
                }
            }

            #endif

            GameObject newPoolParent = new GameObject(roomObject.prefab.name + " Pool");
            newPoolParent.transform.parent = transform;

            RoomObjectPool newRoomObjectPool = newPoolParent.AddComponent<RoomObjectPool>();
            roomObjectPools.Add(newRoomObjectPool);

            newRoomObjectPool.prefab = roomObject.prefab;

            return newRoomObjectPool;
        }

        public void RemoveCurrentRoom()
        {
            for (int i = 0; i < roomObjectPools.Count; ++i)
            {
                roomObjectPools[i].DestroyAllRoomObjects();
            }

            #if UNITY_EDITOR

            RoomObjectGO[] activeRoomObjectGOs = FindObjectsByType<RoomObjectGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            for (int i = 0; i < activeRoomObjectGOs.Length; ++i)
            {
                activeRoomObjectGOs[i].gameObject.SetActive(false);
            }

            #endif
        }

        #if UNITY_EDITOR

        public bool MakeRoom(out RoomObject[] roomObjects)
        {
            RoomObjectGO[] allRoomObjectGOs = FindObjectsByType<RoomObjectGO>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            List<RoomObject> roomObjectsList = new List<RoomObject>();

            for (int i = 0; i < allRoomObjectGOs.Length; ++i)
            {
                RoomObject newRoomObject = allRoomObjectGOs[i].GetRoomObject();

                if (ListContainsThisObject(roomObjectsList, newRoomObject)) continue;

                roomObjectsList.Add(newRoomObject);
            }

            roomObjects = roomObjectsList.ToArray();

            return true;
        }

        bool ListContainsThisObject(List<RoomObject> roomObjectsList, RoomObject c_roomObject)
        {
            for (int i = 0; i < roomObjectsList.Count; ++i)
            {
                if (RoomObject.Matching(roomObjectsList[i], c_roomObject)) return true;
            }

            return false;
        }

        #endif

        #endregion

        #region Saving Room States

        public void SaveCurrentRoom()
        {
            if (currentRoom == null) return;

            List<RoomObjectSave> roomObjectSaves = new List<RoomObjectSave>();

            for (int x = 0; x < roomObjectPools.Count; ++x)
            {
                for (int y = 0; y < roomObjectPools[x].pool.Count; ++y)
                {
                    if (roomObjectPools[x].pool[y].GetRoomObjectSave(out RoomObjectSave roomObjectSave))
                    {
                        roomObjectSaves.Add(roomObjectSave);
                    }
                }
            }

            //Saving.Room room = Saving.Room.
        }

        #endregion
    }

}

