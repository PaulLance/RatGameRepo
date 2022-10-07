using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogin : MonoBehaviour
{
    [Serializable]
    public class DebugLoginData
    {
        [SerializeField] public string LoginName;
        [SerializeField] public string LoginPassword;
    }

    [SerializeField]
    public List<DebugLoginData> data = new List<DebugLoginData>()
    {
        new DebugLoginData(){ LoginName = "Cat", LoginPassword = "123456"},
        new DebugLoginData(){ LoginName = "Rat3", LoginPassword = "123456"},
        new DebugLoginData(){ LoginName = "SmallRat", LoginPassword = "123456"},
        new DebugLoginData(){ LoginName = "BigRat", LoginPassword = "123456"},
    };

    public GameObject loginButtonPrefab;
    public Transform buttonsParent;

    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var login in data)
        {
            var loginButtonGo = Instantiate(loginButtonPrefab, buttonsParent);
            var loginButton = loginButtonGo.GetComponent<Button>();
            loginButton.onClick.AddListener(() => LoginAs(login));
            var loginButtonText = loginButtonGo.GetComponentInChildren<TextMeshProUGUI>();
            loginButtonText.text = login.LoginName;
        }
    }

    public void LoginAs(DebugLoginData loginData)
    {
        nameInput.text = loginData.LoginName;
        passwordInput.text = loginData.LoginPassword;
        loginButton.onClick.Invoke();
        nameInput.enabled = false;
        passwordInput.enabled = false;
        loginButton.enabled = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
