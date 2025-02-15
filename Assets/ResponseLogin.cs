using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseLogin
{
    public string status;
    public int totalAmount;

    public ResponseLogin(string status, int totalAmount)
    {
        this.status = status;
        this.totalAmount = totalAmount;
    }
}
