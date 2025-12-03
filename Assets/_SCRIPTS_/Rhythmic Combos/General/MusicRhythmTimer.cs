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

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        float musicDelta = musicTimeLastFrame > music.time ? 0 : music.time - musicTimeLastFrame;

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
    
    float Allowence()
    {
        return basicMetronomeObject.SecondsBetweenBeats() / 3f;
    }
}
