using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UnityEngine.UI namespace

public class Firabese : MonoBehaviour
{
    public GameObject loginP, singupP, profP;

    public InputField loginEmail, loginPassword, singupEmail, singupPassword, singupUserName, singupCPassword;

    public void OpenloginP()
    {
        loginP.SetActive(true);
        singupP.SetActive(false);
        profP.SetActive(false);
    }

    public void OpensinginP()
    {
        loginP.SetActive(false);
        singupP.SetActive(true);
        profP.SetActive(false);
    }

    public void OpenprofP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            return;
        }
  
    }

    public void SingUpUser()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singupPassword.text) && string.IsNullOrEmpty(singupCPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            return;
        }


        
    }
    public void LoginPassword()
    {
    }
}
