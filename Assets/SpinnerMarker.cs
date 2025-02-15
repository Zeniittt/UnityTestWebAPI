using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public  class SpinnerMarker : MonoBehaviour
{
    public static SpinnerMarker instance;

    public int value = 0;

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.value = Convert.ToInt32(collision.name);
    }
}
