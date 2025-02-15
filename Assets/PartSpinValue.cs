using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartSpinValue : MonoBehaviour
{

    [SerializeField] private TMP_Text value;

    void Start()
    {
        GetValue();
    }
    
    private void GetValue()
    {
        value = GetComponentInChildren<TMP_Text>();
        value.text = transform.name;
    }
}
