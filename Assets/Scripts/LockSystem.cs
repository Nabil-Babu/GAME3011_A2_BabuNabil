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
    HARD
}
public class LockSystem : MonoBehaviour
{


    public Image TopFrame;
    public Image BottomFrame;
    public GameObject TopPick;
    public GameObject BottomPick; 

    [SerializeField] private float topPickAngle;
    [SerializeField] private float bottomPickAngle;
    [SerializeField] private float bottomPickTurnRate = 0.001f;

    [SerializeField] private float _targetTopAngle;
    [SerializeField] private float _targetBottomAngle;
    [SerializeField] private float _targetAngleThresholds = 0.500f;

    [SerializeField] private bool _topTargetReached = false;
    [SerializeField] private bool _bottomTargetReached = false;

    void Start()
    {
        _targetTopAngle = 360.0f * Random.Range(0.0f, 1.0f);
        _targetBottomAngle = 360.0f * Random.Range(0.0f, 1.0f);
    }
    
    void Update()
    {
        CalculatePickAngles();
        RotatePicks();
        CheckForTarget();
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
    }

    void RotatePicks()
    {
        TopPick.transform.eulerAngles = new Vector3(0, 0, topPickAngle);
        BottomPick.transform.eulerAngles = new Vector3(0,0,bottomPickAngle);
    }
}
