using UnityEngine;

public class RB_Beat : MonoBehaviour
{
    public Vector2 normalScale = new Vector2(1f, 0.5f);
    public Vector2 newBarScale = new Vector2(1f, 1f);

    BasicMetronomeObject basicMetronomeObject;
    bool isNewBar = false;

    // Update is called once per frame
    void Update()
    {
        SetScale();
        MoveBeat(RB_BeatSpawner.musicDelta);
    }

    public virtual void SpawnBeat(int beatNumber, BasicMetronomeObject basicMetronomeObject)
    {
        this.basicMetronomeObject = basicMetronomeObject;

        isNewBar = basicMetronomeObject.IsNewBar(beatNumber);

        transform.localPosition = Vector2.zero;

        SetScale();
    }

    void SetScale()
    {
        transform.localScale = isNewBar ? newBarScale : normalScale;
    }

    public void MoveBeat(float seconds)
    {
        transform.localPosition = CreateVector2(transform.localPosition.x - seconds * RB_BeatSpawner.pixelsPerSecond);

        if (transform.localPosition.x < -basicMetronomeObject.beatsInBar * RB_BeatSpawner.pixelsPerSecond) gameObject.SetActive(false);
    }

    Vector2 vector2 = new Vector2();
    Vector2 CreateVector2(float x)
    {
        vector2.x = x;
        vector2.y = 0f;
        return vector2;
    }
}
