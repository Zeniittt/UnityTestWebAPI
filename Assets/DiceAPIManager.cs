using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BetRequest
{
    public string betType;
    public int betAmount;
}

public class BetResponse
{
    public string result;
    public int totalAmount;

    public BetResponse(string result, int totalAmount)
    {
        this.result = result;
        this.totalAmount = totalAmount;
    }
}

public class DiceAPIManager : MonoBehaviour
{
    [SerializeField] string baseUrl = "http://localhost:5007/api/Account";

    [SerializeField] TMP_Text betStatusResult;
    [SerializeField] TMP_InputField amountToBet;
    [SerializeField] TMP_Text currentMoney;

    [SerializeField] GameObject panelSpinner;

    private void Start()
    {
        if(SceneData.data is ResponseLogin responseLogin)
        {
            currentMoney.text = responseLogin.totalAmount.ToString();
        }
    }

    private void Bet(string _betType, int _betAmount)
    {
        BetRequest betRequest = new BetRequest { betType = _betType, betAmount = _betAmount }; 
        string jsonData = JsonConvert.SerializeObject(betRequest);

        StartCoroutine(PostRequest($"{baseUrl}/dice", jsonData, (response) =>
        {


            if (response.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = response.downloadHandler.text;

                Debug.Log(jsonResponse);

                BetResponse betResponse = JsonUtility.FromJson<BetResponse>(jsonResponse);

                Debug.Log(betResponse.result);
                Debug.Log(betResponse.totalAmount);


                /*                GameObject newPanelSpin = Instantiate(panelSpinPrefab);
                                newPanelSpin.GetComponentInChildren<SpinnerTrigger>().segment = Convert.ToInt32(betResponse.result);
                */
                panelSpinner.SetActive(true);
                SpinnerTrigger.instance.segment = Convert.ToInt32(betResponse.result);
                StartCoroutine(SetMoneyToUI(betResponse.totalAmount.ToString()));

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
        int amount = Convert.ToInt32(amountToBet.text);

        Bet("odd", amount);
    }

    public void BetEvenAction()
    {
        int amount = Convert.ToInt32(amountToBet.text);

        Bet("even", amount);
    }

    IEnumerator SetMoneyToUI(string money)
    {
        yield return new WaitForSeconds(4f);
        currentMoney.text = money;
    }

}
