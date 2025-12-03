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
            Transform player = PlayerGO.instance.transform;

            playerXandZ.position = new Vector3(player.position.x, playerXandZ.position.y, player.position.z);

            cameraTargetPosition.position = playerXandZ.position + cameraOffset;

            cameraTargetPosition.LookAt(player);
        }
    }
}