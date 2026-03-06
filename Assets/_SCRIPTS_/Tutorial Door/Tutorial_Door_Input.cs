using DG.Tweening;
using MapRooms;
using UnityEngine;

public class Tutorial_Door_Input : MonoBehaviour
{
    enum ElementInputs
    {
        None,Ground,Water,Fire,Air
    }
    [SerializeField] private bool isPlayerReady = false;
    private bool isOnBeat = false;
    private ElementInputs currentPlayerInput;

    [Header("Door Required Combo")]
    [SerializeField] private ElementInputs[] comboArray;
    private int currentCounter;
    
    [Header("Transform Positions")]
    [SerializeField] private Transform rhythmBarTransform;
    [SerializeField] private Transform rhythmBarMaxTransform;
    [SerializeField] private Transform rhythmBarMinTransform;

    [Header("Door Interaction Tile")]
    [SerializeField] private FloorTileGO playerStartTile;

    [Header("Input Bar Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private int direction =1;


    float actualMoveSpeed = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.onCombat1 += Register_Player_Input_For_Combat_Ground;
        Input.onCombat2 += Register_Player_Input_For_Combat_Water;
        Input.onCombat3 += Register_Player_Input_For_Combat_Air;
        Input.onCombat4 += Register_Player_Input_For_Combat_Fire;

        rhythmBarMaxTransform.GetComponent<MeshRenderer> ().enabled = false;
        rhythmBarMinTransform.GetComponent<MeshRenderer>().enabled = false;

        actualMoveSpeed = 2f / MusicRhythmTimer.SecondsBetweenBars();

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerReady) {
            if (MusicRhythmTimer.BeatIncreased())
            {
                currentCounter += 1;
                if (currentCounter > comboArray.Length - 1) { currentCounter = 0; }
                isOnBeat = true;
            }
            
        }


    }

    private void LateUpdate()
    {

        if (isOnBeat) 
        {
            MoveComboInputBar();
            checkBarValidPos();
        }
        
        currentPlayerInput = ElementInputs.None;
    }

    void Register_Player_Input_For_Combat_Air()
    {
        currentPlayerInput = ElementInputs.Air;
        PlayerComboCheck();
    }
    void Register_Player_Input_For_Combat_Ground()
    {
        currentPlayerInput = ElementInputs.Ground;
        PlayerComboCheck();
    }
    void Register_Player_Input_For_Combat_Fire()
    {
        currentPlayerInput = ElementInputs.Fire;
        PlayerComboCheck();
    }
    void Register_Player_Input_For_Combat_Water()
    {
        currentPlayerInput = ElementInputs.Water;
        PlayerComboCheck();
    }
    bool CheckPlayerIsReady()
    {
        //Add check for combat phase
        return playerStartTile.ContainsPlayer();
    }

   void PlayerComboCheck()
    {
        if (MusicRhythmTimer.instance.Accuracy() > 0)
        {
            if (currentPlayerInput == comboArray[currentCounter])
            {
                print("Player inputted correct combo");
            }
            else
            {
                
            }
        }
        else
        {
        
        }
    }

    void MoveComboInputBar()
    {
        rhythmBarTransform.localPosition = rhythmBarTransform.localPosition 
            + Vector3.forward * actualMoveSpeed * direction * MusicRhythmTimer.MusicDelta();
    }

    void checkBarValidPos()
    {
        //float offset = 0;
        if (rhythmBarTransform.localPosition.z > rhythmBarMaxTransform.localPosition.z)
        {
            float offset = Mathf.Abs(rhythmBarTransform.localPosition.z - rhythmBarMaxTransform.localPosition.z);

            //atEndOfCombo = true;
            rhythmBarTransform.localPosition = rhythmBarMaxTransform.localPosition + Vector3.back * offset;
            invertDirection();

        }
        else if (rhythmBarTransform.localPosition.z < rhythmBarMinTransform.localPosition.z)
        {
           float offset = Mathf.Abs(rhythmBarTransform.localPosition.z - rhythmBarMinTransform.localPosition.z);

            //atEndOfCombo = true;
            rhythmBarTransform.localPosition = rhythmBarMinTransform.localPosition  + Vector3.forward * offset;
            invertDirection();
            
        }
    }

    void invertDirection()
    {
        float distanceToMin = Mathf.Abs(rhythmBarTransform.localPosition.z - rhythmBarMinTransform.localPosition.z);
        float distanceToMax = Mathf.Abs(rhythmBarTransform.localPosition.z - rhythmBarMaxTransform.localPosition.z);

        if (distanceToMin > distanceToMax) 
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }
}
