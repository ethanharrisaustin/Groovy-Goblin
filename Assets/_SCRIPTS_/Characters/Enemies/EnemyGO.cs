using System.Collections.Generic;
using DG.Tweening;
using MapNavigation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MapRooms
{
    public class EnemyGO : ObjectWithHealthGO
    {
        public bool canMoveDiagonally = false;

        protected override void Update()
        {
            base.Update();

            if (!MusicRhythmTimer.BeatIncreased()) return;

            MoveTowardsPlayer();
        }

        void MoveTowardsPlayer()
        {
            GridPathfinding.allowDiagonalMovement = canMoveDiagonally;
            
            Vector3 playerWorldPos = PlayerGO.instance.GetPosition();

            List<Vector3> pathToPlayer = GridPathfinding.FindPathWorld(GetCenterPosition(), playerWorldPos + Vector3.one * 0.5f);

            if (pathToPlayer.Count <= 1) return;

            MoveToPos(pathToPlayer[1]);
        }

        void MoveToPos(Vector3 position)
        {
            transform.DOMove(new Vector3(position.x, transform.position.y, position.z), 0.2f).SetEase(Ease.InOutQuad);
        }
    }
}