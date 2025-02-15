using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerTrigger : MonoBehaviour
{
    public static SpinnerTrigger instance;


    public Transform spiner;
    public float spinSpeed = 0f;
    public float spinSpeedMax;
    public float slowDown;
    public bool stop = false;
    public bool wasCall = false;

    public int segment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }

    void Start()
    {
        StartSpin();
    }

    private void OnEnable()
    {
        ResetSpin();
        StartSpin();
    }

    #region codecu
    public void StartSpin()
    {
        spinSpeed = spinSpeedMax;
        stop = false;

        Invoke("ToStop", 3f);
    }

    private void FixedUpdate()
    {
        Spinning();
    }

    private void Spinning()
    {
        spiner.Rotate(0, 0, Time.deltaTime * spinSpeed);

        Stopping();
    }

    private void Stopping()
    {
        if (!stop) return;

        if(stop)
        {
            SpinStopping(segment);
        }
    }

    #endregion

    void SpinStopping(float segment)
    {
        /*        float anglePerSegment = 360f / 8;
                float desiredAngle = anglePerSegment * segment;

                transform.DORotate(new Vector3(0, 0, desiredAngle - 22), duration, RotateMode.Fast)
                    .SetEase(Ease.OutQuad);*/

        float anglePerSegment = 360f / 8;
        float desiredAngle = anglePerSegment * segment;

        float currentAngle = SpinnerMarker.instance.value * anglePerSegment;

        if (currentAngle != desiredAngle)
        {
            spinSpeed -= slowDown;
            if (spinSpeed < 90) spinSpeed = 90;
        } else
        {
            spinSpeed -= slowDown;
            if (spinSpeed < 0) spinSpeed = 0;
        }
    }

    private void ToStop()
    {
        stop = true;
    }

    void ResetSpin()
    {
        spinSpeed = 0;
        spinSpeedMax = 300;
        gameObject.transform.rotation =  Quaternion.Euler(0, 0, 67.01f);
    }

}
