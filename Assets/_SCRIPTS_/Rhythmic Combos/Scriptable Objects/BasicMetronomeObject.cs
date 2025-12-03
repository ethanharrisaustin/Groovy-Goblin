using UnityEngine;

[CreateAssetMenu(fileName = "BasicMetronomeObject", menuName = "Rhythm and Beats/Basic Metronome", order = 0)]
public class BasicMetronomeObject : ScriptableObject
{
    public int beatsInBar = 4;
    public int bpm = 120;
    public int MillisecondsBetweenBeats()
    {
        return (int)(60f / (float)bpm * 1000f);
    }

    public float SecondsBetweenBeats()
    {
        return 60f / bpm;
    }

    public float SecondsInABar()
    {
        return SecondsBetweenBeats() * beatsInBar;
    }

    public bool IsNewBar(int beatNumber)
    {
        return beatNumber % beatsInBar == 0;
    }
}
