using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Difficulty
{
    EASY,
    MED,
    HARD,
    TOTAL
}
public class LockSystem : MonoBehaviour
{
    public Image TopFrame;
    public Image BottomFrame;
    public GameObject TopPick;
    public GameObject BottomPick;
    public Difficulty lockDifficulty;
    public GameObject[] tumblers;
    public AudioSource audioSource;
    public AudioClip sweetSpotSound;
    public AudioClip unlockSound;
    
    [SerializeField] private float topPickAngle;
    [SerializeField] private float bottomPickAngle;
    [SerializeField] private float bottomPickTurnRate = 0.001f;
    [SerializeField] private int requiredTumblers = 1;

    [SerializeField] private float _targetTopAngle;
    [SerializeField] private float _targetBottomAngle;
    [SerializeField] private float _targetAngleThresholds = 0.500f;
    [SerializeField] private bool _topTargetReached = false;
    [SerializeField] private bool _bottomTargetReached = false;


    private bool topAudioPlayed = false;
    private bool bottomAudioPlayed = false;
    void Start()
    {
        InitLock();
    }
    
    void Update()
    {
        CalculatePickAngles();
        RotatePicks();
        CheckForTarget();
        CheckUnlockAttempt();
    }

    void CalculatePickAngles()
    {
        Vector3 relative = transform.InverseTransformPoint(Input.mousePosition);
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        topPickAngle = angle; 

        if (Input.GetKey(KeyCode.A))
        {
            bottomPickAngle -= bottomPickTurnRate; 
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            bottomPickAngle += bottomPickTurnRate; 
        }

        if (bottomPickAngle >= 360)
        {
            bottomPickAngle = 0; 
        }
        
        if (bottomPickAngle < 0)
        {
            bottomPickAngle = 360; 
        }
    }

    void CheckForTarget()
    {
        _topTargetReached = Math.Abs(topPickAngle - _targetTopAngle) < _targetAngleThresholds;
        _bottomTargetReached = Math.Abs(bottomPickAngle - _targetBottomAngle) < _targetAngleThresholds;
        TopFrame.color = _topTargetReached ? Color.green : Color.black;
        BottomFrame.color = _bottomTargetReached ? Color.green : Color.black;
        if (_topTargetReached)
        {
            if(!topAudioPlayed) audioSource.PlayOneShot(sweetSpotSound);
            topAudioPlayed = true;
        }
        else
        {
            topAudioPlayed = false;
        }

        if (_bottomTargetReached)
        {
            if(!bottomAudioPlayed) audioSource.PlayOneShot(sweetSpotSound);
            bottomAudioPlayed = true;
        }
        else
        {
            bottomAudioPlayed = false;
        }
    }

    void RotatePicks()
    {
        TopPick.transform.eulerAngles = new Vector3(0, 0, topPickAngle);
        BottomPick.transform.eulerAngles = new Vector3(0,0,bottomPickAngle);
    }

    public void InitLock()
    {
        _targetTopAngle = 360.0f * Random.Range(0.0f, 1.0f);
        _targetBottomAngle = 360.0f * Random.Range(0.0f, 1.0f);
        lockDifficulty = (Difficulty) Random.Range(0, (int) Difficulty.TOTAL);
        requiredTumblers = (int)lockDifficulty + 1;
        DisplayTumblers();
    }

    void DisplayTumblers()
    {
        for (int i = 0; i < requiredTumblers; i++)
        {
            tumblers[i].SetActive(true);
        }
    }

    private void CheckUnlockAttempt()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_topTargetReached && _bottomTargetReached)
            {
                requiredTumblers--;
                tumblers[requiredTumblers].SetActive(false);
                _targetTopAngle = 360.0f * Random.Range(0.0f, 1.0f);
                _targetBottomAngle = 360.0f * Random.Range(0.0f, 1.0f);
                audioSource.PlayOneShot(unlockSound);
            }
        }
    }

    private void ResetLock()
    {
        InitLock();
    }
    
    
}
