using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RB_BeatSpawner : MonoBehaviour
{
    public ObjectPool objectPool;
    public BasicMetronomeObject basicMetronomeObject;
    public float _pixelsPerSecond = 200f;

    public AudioSource music;

    public float counter = 0f;

    int currentBeat = -2;

    float musicTimeLastFrame = 0f;

    public static float musicDelta { get; private set; }

    public static float pixelsPerSecond { get { return instance._pixelsPerSecond; } }

    public static RB_BeatSpawner instance;

    /*

    static bool quitApplication = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quitApplication = false;

        AsyncUpdate();
    }

    async void AsyncUpdate()
    {
        while (true)
        {
            Debug.Log("Looping");
            if (quitApplication) return;

            

            await Task.Delay(10);
        }
    }

    void OnApplicationQuit()
    {
        quitApplication = true;
    }
    
    */

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        musicDelta = music.time - musicTimeLastFrame;
        musicTimeLastFrame = music.time;

        Spawner();

        //MoveBeats(musicDelta);
    }

    void Spawner()
    {
        counter += musicDelta;

        if (counter >= basicMetronomeObject.SecondsBetweenBeats())
        {            
            float remainder = basicMetronomeObject.SecondsBetweenBeats() - counter;

            //RB_Beat beat = SpawnBeat(currentBeat);

            RB_Beat beat = objectPool.SpawnObject().GetComponent<RB_Beat>();

            beat.SpawnBeat(currentBeat, basicMetronomeObject);

            // MoveBeat(beat, -basicMetronomeObject.beatsInBar * (1f + remainder));

            beat.MoveBeat(-basicMetronomeObject.beatsInBar * (1f + remainder));

            counter = -remainder;

            currentBeat++;
        }
    }

/* 
    void MoveBeats(float musicDelta)
    {
        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (!objectPool[i].gameObject.activeSelf) continue;

            MoveBeat(objectPool[i], musicDelta);

            if (objectPool[i].transform.localPosition.x < -basicMetronomeObject.beatsInBar  * pixelsPerSecond)
            {
                objectPool[i].gameObject.SetActive(false);
            }
        }
    }

    void MoveBeat(RB_Beat beat, float seconds)
    {
        beat.transform.localPosition = CreateVector2(beat.transform.localPosition.x - seconds * pixelsPerSecond);
    }
*/
    /*

    #region Object Pooling

    List<RB_Beat> objectPool = new List<RB_Beat>();

    RB_Beat SpawnBeat(int beatNumber)
    {
        RB_Beat result = null;

        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].gameObject.activeSelf) continue;

            result = objectPool[i];
            result.gameObject.SetActive(true);
        }

        if (result == null)
        {
            result = Instantiate(beatPrefab, beatsParent).GetComponent<RB_Beat>();
            objectPool.Add(result);
        }

        result.SpawnBeat(beatNumber, basicMetronomeObject);

        return result;
    }

    #endregion

    */
}
