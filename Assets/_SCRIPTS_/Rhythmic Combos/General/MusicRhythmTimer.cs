using UnityEngine;

public class MusicRhythmTimer : MonoBehaviour
{
    public BasicMetronomeObject basicMetronomeObject;
    public AudioSource music;

    public static MusicRhythmTimer instance;

    int bar = 1;
    int beat = 1;

    float beatTimer = 0f;
    float barTimer = 0f;

    float musicTimeLastFrame = 0;

    float musicDelta = 0f;

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        musicDelta = musicTimeLastFrame > music.time ? 0 : music.time - musicTimeLastFrame;

        musicTimeLastFrame = music.time;

        beatTimer += musicDelta;
        barTimer += musicDelta;

        if (beatTimer >= basicMetronomeObject.SecondsBetweenBeats())
        {
            float remainder = beatTimer - basicMetronomeObject.SecondsBetweenBeats();

            beat++;

            if (beat > basicMetronomeObject.beatsInBar)
            {
                beat = 1;
                bar++;
                barTimer = remainder;
            }

            beatTimer = remainder;
        }
    }

    public bool OnBar()
    {
        return barTimer < Allowence() || barTimer > basicMetronomeObject.SecondsInABar() - Allowence();
    }

    public bool OnBeat(int beat = -1)
    {
        if (beat < 1)
        {
            return beatTimer < Allowence() || beatTimer > basicMetronomeObject.SecondsBetweenBeats() - Allowence();
        }

        if (this.beat == beat)
        {
            return beatTimer < Allowence();
        }
        else if (this.beat + 1 == beat)
        {
            return beatTimer > basicMetronomeObject.SecondsBetweenBeats() - Allowence();
        }

        return false;
    }

    /// <summary>
    /// This gives a percenage: 1 is bang on, 0 is just within allowence. Less than 0 means it was too off beat to be allowed!
    /// </summary>
    /// <returns></returns>
    public float Accuracy()
    {
        return  (Allowence() - DistanceFromBeat()) / Allowence();
    }

    public float DistanceFromBeat()
    {
        float previousBeat = PreviousBeat();
        float nextBeat = NextBeat();

        // We are closer to previous beat 
        if (ClosestToBeatA(previousBeat, nextBeat))
        {
            return Mathf.Abs(previousBeat);
        }

        return nextBeat;
    }

    public static float MusicDelta()
    {
        return instance.musicDelta;
    }
    
    public static float SecondsBetweenBeats()
    {
        return instance.basicMetronomeObject.SecondsBetweenBeats();
    }

    float PreviousBeat()
    {
        return -beatTimer;
    }

    float NextBeat()
    {
        return basicMetronomeObject.SecondsBetweenBeats() - beatTimer;
    }

    bool ClosestToBeatA(float a, float b)
    {
        return Mathf.Abs(a) > b;
    }
    
    float Allowence()
    {
        return basicMetronomeObject.SecondsBetweenBeats() / 3f;
    }
}
