using UnityEngine;
using DG.Tweening;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MapRoomSystem
{
    public class RoomObjectGO : MonoBehaviour
    {
        [HideInInspector] public RoomObject roomObject;
        [HideInInspector] public string[] objectValues;

        #if UNITY_EDITOR
        public RoomObject GetRoomObject()
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);

            if (prefab == null)
            {
                Debug.Log("Error! One of the RoomObjects' is not a prefab!");
                return null;
            }

            string[] values;
            GetValues(out values);

            return new RoomObject(prefab, transform.position, transform.localScale, transform.eulerAngles, values);
        }
        #endif

        public bool FinishedFlyingIn()
        {
            if (gameObject.activeSelf == false) return true;

            return flyingIn == false;
        }

        public bool FinishedFlyingOut()
        {
            return flyingOut == false || gameObject.activeSelf == false;
        }

        public Vector3 targetPosition;
        bool flyingIn = false;
        public virtual void Spawn(RoomObject roomObject)
        {
            transform.localScale = roomObject.scale;
            transform.eulerAngles = roomObject.rotation;

            targetPosition = roomObject.position;
            FlyObjectIn(targetPosition, ObjectFlyInCategory());

            this.roomObject = roomObject;

            SetValues(roomObject.values);
        }

        public virtual void Remove()
        {
            FlyObjectOut(ObjectFlyInCategory());
        }

        void FlyObjectIn(Vector3 targetPosition, string objectCategory)
        {
            AnimationCurve curve;
            float fallTime, startYPos, initialDelay, delayMultiplier;
            if (!RoomObjectFlyInSettings.GetRoomObjectFlyInSettings(objectCategory, out curve, out fallTime, out startYPos, out initialDelay, out delayMultiplier)) { transform.position = targetPosition; return;  } 

            flyingIn = true;

            transform.position = targetPosition + Vector3.up * startYPos;

            transform.DOMoveY(targetPosition.y, fallTime).SetEase(curve).SetDelay((transform.position.x + transform.position.z + 5) * delayMultiplier + initialDelay).OnComplete(() => flyingIn = false);
        }

        bool flyingOut= false;
        void FlyObjectOut(string objectCategory)
        {
            AnimationCurve curve;
            float fallTime, startYPos, initialDelay, delayMultiplier;
            if (!RoomObjectFlyInSettings.GetRoomObjectFlyOutSettings(objectCategory, out curve, out fallTime, out startYPos, out initialDelay, out delayMultiplier)) { transform.position = targetPosition; return;  } 

            flyingOut = true;

            transform.DOMoveY(transform.position.y + startYPos, fallTime).SetEase(curve).SetDelay((transform.position.x + transform.position.z + 5) * delayMultiplier + initialDelay).OnComplete(() => { flyingOut = false; gameObject.SetActive(false);  });
        }

        protected virtual string ObjectFlyInCategory()
        {
            return "RoomObjectGO";
        }

        public static RoomObjectGO GetRoomObjectGO(Collider collider)
        {
            RoomObjectGO roomObjectGO = collider.GetComponentInChildren<RoomObjectGO>();
            if (roomObjectGO == null) roomObjectGO = collider.GetComponentInParent<RoomObjectGO>();

            return roomObjectGO;
        }

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Init()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        public virtual void GetValues(out string[] values)
        {
            values = null;
        }

        public virtual void SetValues(string[] values)
        {

        }
    }
}