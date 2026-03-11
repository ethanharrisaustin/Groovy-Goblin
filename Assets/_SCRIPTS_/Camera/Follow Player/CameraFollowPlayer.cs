using MapNavigation;
using UnityEngine;

namespace CameraMovement
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public Transform playerXandZ;
        public Transform cameraTargetPosition;
        public Vector3 cameraOffset;
        // Update is called once per frame
        void LateUpdate()
        {
            PlayerGO player = PlayerGO.instance;

            if (player == null) return;

            playerXandZ.position = new Vector3(player.transform.position.x, playerXandZ.position.y, player.transform.position.z);

            cameraTargetPosition.position = playerXandZ.position + cameraOffset;

            cameraTargetPosition.LookAt(player.transform.position);
        }
    }
}