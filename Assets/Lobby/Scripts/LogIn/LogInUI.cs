using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogInUI : MonoBehaviour
{

    [Header("LogInFields")]

    public TMP_InputField LogInPassword;
    public TMP_InputField LogInUserName;

    [Header("RegisterFields")]

    public TMP_InputField RegisterPassword;
    public TMP_InputField RegisterName;
    public TMP_InputField RegisterEmail;

    [SerializeField] GameObject LogInForm;
    [SerializeField] GameObject RegisterForm;
    [SerializeField] GameObject LogInRegisterCanvas;



    private void Start()
    {
        LogInPassword.inputType = TMP_InputField.InputType.Password;
        RegisterPassword.inputType = TMP_InputField.InputType.Password;
    }
    public void ActiveRegisterForm()
    {
        RegisterForm.SetActive(true);
        LogInForm.SetActive(false);
    }

    public void ActiveLogInForm()
    {
        RegisterForm.SetActive(false);
        LogInForm.SetActive(true);
    }

    public void DisactiveAllForms()
    {
        RegisterForm.SetActive(false);
        LogInRegisterCanvas.SetActive(false);
    }


}
