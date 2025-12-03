using MapRoomSystem;
using UnityEngine;
using static Input;

namespace MapNavigation
{
    public class PlayerGO : MoveableObjectGO
    {
        public static PlayerGO instance;

        [Space, Header("Player Movement Settings")]
        public float moveToTileTime;
        public float timeBetweenMovements = -1;

        [HideInInspector] bool playerIsActive = true;

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }

        protected override void Update()
        {
            base.Update();

            if (!playerIsActive) return;

            MovementHandler();
        }

        Direction previousDirection;
        float moveInputCooldown = 0f;
        bool favourX = false;
        void MovementHandler()
        {
            Direction direction = MovementAsDirection(favourX);

            //if (direction != previousDirection) moveInputCooldown = 0f;
            previousDirection = direction;

            if (moveInputCooldown > 0f)
            {
                moveInputCooldown -= Time.deltaTime;
                return;
            }

            switch (direction)
            {
                case Direction.north:
                MoveNorthInput();
                favourX = false;
                break;

                case Direction.east:
                MoveEastInput();
                favourX = true;
                break;

                case Direction.south:
                MoveSouthInput();
                favourX = false;
                break;

                case Direction.west:
                MoveWestInput();
                favourX = true;
                break;

                default: return;
            }

            moveInputCooldown = timeBetweenMovements <= moveToTileTime ? moveToTileTime * 0.995f : timeBetweenMovements;
        }

        public virtual void MoveNorthInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveNorth(out newFloorTile)) return;

            SetPositionTo(newFloorTile, moveToTileTime);
        }
        public virtual void MoveEastInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveEast(out newFloorTile)) return;

           SetPositionTo(newFloorTile, moveToTileTime);
        }
        public virtual void MoveSouthInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveSouth(out newFloorTile)) return;

            SetPositionTo(newFloorTile, moveToTileTime);
        }
        public virtual void MoveWestInput()
        {
            FloorTileGO newFloorTile;
            if (!CanMoveWest(out newFloorTile)) return;

            SetPositionTo(newFloorTile, moveToTileTime);
        }

        public void DeactivatedPlayer()
        {
            playerIsActive = false;
        }
        public void ActivatePlayer()
        {
            playerIsActive = true;

            gameObject.SetActive(true);
        }

        public bool PlayerIsAbleToMove()
        {
            if (playerIsActive == false) return false;

            return moveInputCooldown <= 0f;
        }
    }
}
