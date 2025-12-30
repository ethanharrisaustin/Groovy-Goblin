using DG.Tweening;
using MapRoomSystem;
using UnityEngine;

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


        protected void SetPositionTo(FloorTileGO floorTileGO)
        {
            transform.position = new Vector3(floorTileGO.transform.position.x,  transform.position.y, floorTileGO.transform.position.z);
        }
        public void SetPositionTo(FloorTileGO floorTileGO, float moveTime)
        {
            SetPositionTo(floorTileGO.GetPosition(), moveTime);
        }

        public void SetPositionTo(Vector3 position, float moveTime)
        {
            objectIsMoving = true;

            transform.DOKill();
            transform.DOMove(position, moveTime).SetEase(Ease.Linear).OnComplete(() => objectIsMoving = false);
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
        
        public virtual FloorTileGO GetFloorTile(Transform gridTrigger)
        {
            Collider[] colliders = Physics.OverlapBox(gridTrigger.position, Vector3.one * 0.08f, Quaternion.identity, layerMask, QueryTriggerInteraction.Collide);

            return FloorTileGO.GetFloorTileGO(colliders);
        }

        public bool ObjectIsMoving()
        {
            return objectIsMoving;
        }
    }
}