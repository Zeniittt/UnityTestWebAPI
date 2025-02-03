using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;
using TMPro;
using UnityEditor.PackageManager.Requests;



[Serializable]
public class AccountRequest
{
    public string username;
    public string password;
}

[Serializable]
public class AccountDto
{
    public string username;
}

public class ApiManager : MonoBehaviour
{
    [SerializeField] string baseUrl = "http://localhost:5007/api/Account"; // Đổi thành URL của bạn

    [SerializeField] TMP_Text loginRes;
    [SerializeField] TMP_Text signupRes;
    [SerializeField] TMP_Text quickLogRes;
    [SerializeField] TMP_Text allUserText;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField registerUsername;

    private void SignUp(string username)
    {
        AccountRequest account = new AccountRequest { username = username, password = "1" }; // Mật khẩu mặc định là 1
        string jsonData = JsonConvert.SerializeObject(account); // Newtonsoft.Json

        StartCoroutine(PostRequest($"{baseUrl}/signup", jsonData, (response) =>
        {
            Debug.Log("SignUp Response: " + response);
            signupRes.text = response.downloadHandler.text;

            if (response.result == UnityWebRequest.Result.Success)
            {
                signupRes.text = response.downloadHandler.text;
                signupRes.color = Color.green;
            }
            else
            {
                signupRes.text = response.downloadHandler.text;
                signupRes.color = Color.red;
                Debug.LogError("Error: " + response.error);
            }
        }));
    }

    private void Login(string username, string password)
    {
        AccountRequest account = new AccountRequest { username = username, password = password };
        string jsonData = JsonConvert.SerializeObject(account); // Newtonsoft.Json

        StartCoroutine(PostRequest($"{baseUrl}/login", jsonData, (response) =>
        {
            Debug.Log("Login Response: " + response);
            if (response.result == UnityWebRequest.Result.Success)
            {
                loginRes.text = response.downloadHandler.text;
                loginRes.color = Color.green;
            }
            else
            {
                loginRes.text = response.downloadHandler.text;
                loginRes.color = Color.red;
                Debug.LogError("Error: " + response.error);
            }
        }));
    }

    public void GetAllUsers()
    {
        StartCoroutine(GetRequest($"{baseUrl}/all", (response) =>
        {
            allUserText.text = response;
            List<AccountDto> users = JsonConvert.DeserializeObject<List<AccountDto>>(response); // Newtonsoft.Json
            foreach (var user in users)
            {
                Debug.Log("User: " + user.username);
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

    private IEnumerator GetRequest(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            else
            {
                //callback?.Invoke(request.downloadHandler.text);
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    public void SignUpAction() 
    {
        SignUp(registerUsername.text); 
    }

    public void SignInAction()
    {
        Login(username.text, password.text);
    }

    public void QuickLoginAction()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            GetAllUsers();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            allUserText.text = "";
        }
    }
}
