using UnityEngine;

public class RB_BeatSpawner : MonoBehaviour
{
    public ObjectPool objectPool;
    public BasicMetronomeObject basicMetronomeObject;
    public float _pixelsPerSecond = 200f;

    public AudioSource music;

    public float counter = 0f;

    int currentBeat = 0;

    public static float pixelsPerSecond { get { return instance._pixelsPerSecond; } }

    public static RB_BeatSpawner instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (MusicRhythmTimer.MusicDelta() < 0) return;

        _Spawner();
    }

    void _Spawner()
    {
        counter += MusicRhythmTimer.MusicDelta();

        if (counter >= basicMetronomeObject.SecondsBetweenBeats())
        {            
            float remainder = basicMetronomeObject.SecondsBetweenBeats() - counter;

            RB_Beat beat = objectPool.SpawnObject().GetComponent<RB_Beat>();

            beat.SpawnBeat(currentBeat, basicMetronomeObject);

            beat.MoveBeat(-basicMetronomeObject.beatsInBar * (1f + remainder));

            counter = -remainder;

            currentBeat++;
        }
    }

    float beatCounter = 0f;
    void Spawner()
    {
        beatCounter += Time.deltaTime;

        if (beatCounter >= basicMetronomeObject.SecondsBetweenBeats())
        {            
            SpawnBeat();

            beatCounter = 0;
        }
    }

    void SpawnBeat()
    {
        
    }
}
