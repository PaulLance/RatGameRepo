using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayfabAuth : MonoBehaviour
{
    LoginWithPlayFabRequest logInRequest;
    LogInUI logInUI;
    [SerializeField] bool IsAuthenticated = false;
    string name;

    RegisterPlayFabUserRequest registerRequest;


    private void Start()
    {
        logInUI = FindObjectOfType<LogInUI>();
    }

    public void LogIn()
    {
        logInRequest = new LoginWithPlayFabRequest();
        name = logInUI.LogInUserName.text;
        logInRequest.Username = logInUI.LogInUserName.text;
        logInRequest.Password = logInUI.LogInPassword.text;
        PlayFabClientAPI.LoginWithPlayFab(logInRequest, OnSuccess, OnFailure);
    }

    public void Register()
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest();
        registerRequest.Email = logInUI.RegisterEmail.text;
        registerRequest.Username = logInUI.RegisterName.text;
        registerRequest.Password = logInUI.RegisterPassword.text;
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnSuccessRegister, OnFailureRegister);

    }

    private void OnFailureRegister(PlayFabError error)
    {
        Debug.Log("Failed to create account");
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnSuccessRegister(RegisterPlayFabUserResult result)
    {
        Debug.Log("Your account has been created");
    }

    private void OnFailure(PlayFabError error)
    {
        IsAuthenticated = false;
        Debug.Log(error.GenerateErrorReport());

    }

    private void OnSuccess(LoginResult result)
    {
        IsAuthenticated = true;
        logInUI.DisactiveAllForms();
        MainLoobyManager.lobbyManager.ConnectToMaster();
    }

    private void OnFailName(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

    private void OnSuccessName(UpdateUserTitleDisplayNameResult obj)
    {
        

    }
}
