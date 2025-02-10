using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BetRequest
{
    public string choice;
    public int amountToBet;
}

public class BetResponse
{
    public string result;
    public int currentMoney;
}

public class DiceAPIManager : MonoBehaviour
{
    [SerializeField] string baseUrl = "http://localhost:5007/api/"; 

    [SerializeField] TMP_Text betStatusResult;
    [SerializeField] TMP_InputField amountToBet;
    [SerializeField] TMP_Text currentMoney;


    private void Bet(string _choice, int _amountToBet)
    {
        BetRequest betRequest = new BetRequest { choice = _choice, amountToBet = _amountToBet }; 
        string jsonData = JsonConvert.SerializeObject(betRequest);

        StartCoroutine(PostRequest($"{baseUrl}/bet", jsonData, (response) =>
        {

            if (response.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = response.downloadHandler.text;
                BetResponse betResponse = JsonUtility.FromJson<BetResponse>(jsonResponse);

                SetToUI(betResponse);
            }
            else
            {
                betStatusResult.text = response.downloadHandler.text;
                betStatusResult.color = Color.red;
                Debug.LogError("Error: " + response.error);
            }
        }));
    }

    private IEnumerator PostRequest(string url, string jsonData, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(request);
            }
            else
            {
                callback?.Invoke(request);
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    public void BetOddAction()
    {
        Bet("L?", Convert.ToInt32(amountToBet.text));
    }

    public void BetEvenAction()
    {
        Bet("Ch?n", Convert.ToInt32(amountToBet.text));
    }

    private void SetToUI(BetResponse betResponse)
    {
        betStatusResult.text = betResponse.result;
        currentMoney.text = betResponse.currentMoney.ToString();
    }
}
