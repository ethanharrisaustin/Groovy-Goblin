using System.Collections.Generic;
using DG.Tweening;
using MapNavigation;
using UnityEngine;

namespace MapRooms
{
    public class EnemyGO : ObjectWithHealthGO
    {
        public bool canMoveDiagonally = false;
        public float moveTime = 0.2f;

        protected override void Update()
        {
            base.Update();

            if (!MusicRhythmTimer.OffBeatIncreased()) return;

            MoveTowardsPlayer();
        }

        void MoveTowardsPlayer()
        {
            GridPathfinding.allowDiagonalMovement = canMoveDiagonally;
            
            Vector3 playerWorldPos = ClosestPosFromPlayer();

            List<Vector3> pathToPlayer = GridPathfinding.FindPathWorld(GetCenterPosition(), playerWorldPos );

            if (pathToPlayer.Count <= 1) return;

            MoveToPos(pathToPlayer[1]);
        }

        void MoveToPos(Vector3 position)
        {
            Input.Direction directionToMove = DirectionFromAToB(transform.position, position);

            FloorTileGO floorTileGO;

            switch(directionToMove)
            {
                case Input.Direction.north:
                    if (!CanMoveNorth(out floorTileGO)) return;
                    break;

                case Input.Direction.east:
                    if (!CanMoveEast(out floorTileGO)) return;
                    break;

                case Input.Direction.south:
                    if (!CanMoveSouth(out floorTileGO)) return;
                    break;

                case Input.Direction.west:
                    if (!CanMoveWest(out floorTileGO)) return;
                    break;

                default: return;
            }

            floorTileGO.AddToTile(this);
            GetFloorTileCentre().RemoveToTile(this);
            SetPositionTo(floorTileGO);
        }

        Vector3 ClosestPosFromPlayer()
        {
            Vector3[] adjacentTiles = AdjacentTiles();

            float shortestDistance = Mathf.Infinity;
            float currentDistance;
            Vector3 chosenTile = PlayerGO.instance.GetPosition();
            for (int i = 0; i < adjacentTiles.Length; ++i)
            {
                Vector2Int tilePos = MapToAStarGrid.WorldLocationToAStarGridPosition(adjacentTiles[i]);

                if (MapToAStarGrid.instance.TileIsObstacle(tilePos)) continue;

                currentDistance = Vector3.Distance(adjacentTiles[i], GetPosition());

                if (currentDistance >= shortestDistance) continue;

                shortestDistance = currentDistance;

                chosenTile = adjacentTiles[i];
            }

            return chosenTile;
        }

        Vector3[] AdjacentTiles()
        {
            if (canMoveDiagonally)
            {
                return PlayerGO.instance.GetAdjacentTiles();
            }
            else
            {
                return PlayerGO.instance.GetAdjacentCardinalTiles();
            }
        }
    }
}