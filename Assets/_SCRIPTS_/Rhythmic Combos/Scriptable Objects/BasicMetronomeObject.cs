using UnityEngine;

[CreateAssetMenu(fileName = "BasicMetronomeObject", menuName = "Scriptable Objects/Basic Metronome")]
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

    public bool IsNewBar(int beatNumber)
    {
        return beatNumber % beatsInBar == 0;
    }
}
