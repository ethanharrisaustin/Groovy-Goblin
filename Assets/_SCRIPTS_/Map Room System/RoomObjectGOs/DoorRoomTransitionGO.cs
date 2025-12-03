using System.Threading.Tasks;
using MapNavigation;
using UnityEngine;

namespace MapRoomSystem
{
    public class DoorRoomTransitionGO : TriggerTileGO
    {
        [Space]
        public string roomUniqueID;
        bool hasPlayer = false;
        bool movingIntoDoor = false;
        bool playerNeedsToWalkAwayFirst = false;

        public override void Init()
        {
            base.Init();

            hasPlayer = ContainsPlayer();

            if (hasPlayer)
            {
                playerNeedsToWalkAwayFirst = true;
            }
        }

        public override void GetValues(out string[] values)
        {
            values = new string[] { roomUniqueID };
        }

        public override void SetValues(string[] values)
        {
            roomUniqueID = values[0];
        }

        protected override void OnObjectEnter(Collider other)
        {
            base.OnObjectEnter(other);

            hasPlayer = ContainsPlayer();
        }

        protected override void OnObjectExit(Collider other)
        {
            base.OnObjectExit(other);

            hasPlayer = ContainsPlayer();
        }

        protected override void Update()
        {
            base.Update();

            if (playerNeedsToWalkAwayFirst)
            {
                if (hasPlayer == false) playerNeedsToWalkAwayFirst = false;
                return;
            }

            if (!hasPlayer) return;

            if (PlayerGO.instance.PlayerIsAbleToMove() == false) return;

            Input.Direction moveDirection = Input.MovementAsDirection();

            if (moveDirection == DirectionIntoDoor())
            {
                movingIntoDoor = true;

                PlayerGO.instance.DeactivatedPlayer();

                MoveToRoom();
            }
        }

        Input.Direction DirectionIntoDoor()
        {
            PlayerGO playerGO = PlayerGO.instance;

            Vector3 playerPos = playerGO.transform.position;
            Vector3 doorPos = transform.position;

            bool xDiffBiggest = Mathf.Abs(playerPos.x - doorPos.x) >= Mathf.Abs(playerPos.z - doorPos.z);

            if (xDiffBiggest)
            {
                if (playerPos.x - doorPos.x <= 0) return Input.Direction.east;

                return Input.Direction.west;
            }
            else
            {
                if (playerPos.z - doorPos.z <= 0) return Input.Direction.north;

                return Input.Direction.south;
            }
        }

        async void MoveToRoom()
        {
            PlayerGO.instance.SetPositionTo(transform.position, PlayerGO.instance.moveToTileTime);

            await Task.Delay((int)(1000 * PlayerGO.instance.moveToTileTime));

            MapRoomSystem.instance.SwapToRoom(roomUniqueID);

            movingIntoDoor = false;
        }
    }
}