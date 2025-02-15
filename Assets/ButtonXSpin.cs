using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonXSpin : MonoBehaviour
{
    public void ClosePanelSpin()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
