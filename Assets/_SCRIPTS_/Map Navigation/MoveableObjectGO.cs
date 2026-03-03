using DG.Tweening;
using MapRooms;
using UnityEngine;
using static Input;

namespace MapNavigation
{
    public class MoveableObjectGO : RoomObjectGO
    {
        [Header("Grid Triggers")]
        public Transform gridTriggersHolder;
        public Transform gridTriggerN, gridTriggerE, gridTriggerS, gridTriggerW;
        public Collider gridTriggerCenter;
        [SerializeField] LayerMask layerMask;

        bool objectIsMoving = false;

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PositionTriggersHolder();
        }

        void PositionTriggersHolder()
        {
            gridTriggersHolder.rotation = Quaternion.identity;
        }

        /* 
        protected void SetPositionTo(FloorTileGO floorTileGO)
        {
            transform.position = new Vector3(floorTileGO.transform.position.x,  transform.position.y, floorTileGO.transform.position.z);
        }
        */
        public void SetPositionTo(FloorTileGO floorTileGO)
        { 
            SetPositionTo(floorTileGO.GetPosition());
        }

        public void SetPositionTo(Vector3 position)
        {
            CharacterJumpSettings.GetSettings(out var jumpYCurve, out var jumpXZCurve, out var jumpTime, out var jumpHeight);

            objectIsMoving = true;

            transform.DOKill();
            transform.DOMoveX(position.x, jumpTime).SetEase(jumpXZCurve);
            transform.DOMoveZ(position.z, jumpTime).SetEase(jumpXZCurve);
            transform.DOMoveY(JumpPos(position, jumpHeight), jumpTime).SetEase(jumpYCurve).OnComplete(() => 
            {
                objectIsMoving = false;
                transform.position = position;
            });
        }

        public virtual bool CanMoveNorth(out FloorTileGO floorTileNorth) { return CanMove(gridTriggerN, out floorTileNorth); }
        public virtual bool CanMoveEast(out FloorTileGO floorTileEast) { return CanMove(gridTriggerE, out floorTileEast); }
        public virtual bool CanMoveSouth(out FloorTileGO floorTileSouth) { return CanMove(gridTriggerS, out floorTileSouth); }
        public virtual bool CanMoveWest(out FloorTileGO floorTileWest) { return CanMove(gridTriggerW, out floorTileWest); }

        public virtual bool CanMove(Transform gridTrigger, out FloorTileGO floorTileGO)
        {
            floorTileGO = GetFloorTile(gridTrigger);

            if (floorTileGO == null) return false;

            return floorTileGO.IsEmpty();
        }

        public virtual FloorTileGO GetFloorTileNorth() { return GetFloorTile(gridTriggerN); }
        public virtual FloorTileGO GetFloorTileEast() { return GetFloorTile(gridTriggerE); }
        public virtual FloorTileGO GetFloorTileSouth() { return GetFloorTile(gridTriggerS); }
        public virtual FloorTileGO GetFloorTileWest() { return GetFloorTile(gridTriggerW); }
        public virtual FloorTileGO GetFloorTileCentre() { return GetFloorTile(gridTriggerCenter.transform); }
        
        public virtual FloorTileGO GetFloorTile(Transform gridTrigger)
        {
            Collider[] colliders = Physics.OverlapBox(gridTrigger.position, Vector3.one * 0.08f, Quaternion.identity, layerMask, QueryTriggerInteraction.Collide);

            return FloorTileGO.GetFloorTileGO(colliders);
        }

        public bool ObjectIsMoving()
        {
            return objectIsMoving;
        }

        public Direction DirectionFromAToB(Vector3 posA, Vector3 posB)
        {
            float absX = Mathf.Abs(posA.x - posB.x);
            float absZ = Mathf.Abs(posA.z - posB.z);

            if (absX > absZ)
            {
                if (posA.x > posB.x) return Direction.west;

                return Direction.east;
            }

            if (posA.z > posB.z) return Direction.south;

            return Direction.north;
        }

        float JumpPos(Vector3 position, float jumpHeight)
        {
            float maxY = MaxY(position, transform.position);

            return maxY + jumpHeight;
        }

        float MaxY(Vector3 a, Vector3 b)
        {
            return Mathf.Max(a.y, b.y);
        }
    }
}