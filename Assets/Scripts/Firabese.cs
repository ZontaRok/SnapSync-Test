using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UnityEngine.UI namespace

public class Firabese : MonoBehaviour
{
    public GameObject loginP, singupP, profP,forgotPassP, napakP;

    public InputField loginEmail, loginPassword, singupEmail, singupPassword, singupUserName, singupCPassword, forgotPassEmail;

    public Text napaka_Naslov_text, napaka_Sporocilo_text,uporabnik_ime_text,uporabnik_email_text;

    public Toggle zapomni;

    public void OpenloginP()
    {
        loginP.SetActive(true);
        singupP.SetActive(false);
        profP.SetActive(false);
        forgotPassP.SetActive(false);
    }

    public void OpensinginP()
    {
        loginP.SetActive(false);
        singupP.SetActive(true);
        profP.SetActive(false);
        forgotPassP.SetActive(false);
    }

    public void OpenprofP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(true);
        forgotPassP.SetActive(false);
    }

    public void OpenForgotPasswordP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(false);
        forgotPassP.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            PrikazNapake("Error", "porapvi polja");
            return;
        }
  
    }

    public void SingUpUser()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singupPassword.text) && string.IsNullOrEmpty(singupCPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            PrikazNapake("Error", "porapvi polja");
            return;
        }


        
    }
    public void ForgotPass()
    {
        if(string.IsNullOrEmpty(forgotPassEmail.text))
        {
            PrikazNapake("Error", "prazni email polje");


            return;
        }
    }

    private void PrikazNapake(string title, string msg)
    {
        napaka_Naslov_text.text = "" + title;
        napaka_Sporocilo_text.text ="" + msg;

       napakP.SetActive(true);
    }

    public void Zapri_Napako()
    {
        napaka_Naslov_text.text = "";
        napaka_Sporocilo_text.text = "";

        napakP.SetActive(false);

    }
    public void LogOut()
    {
        profP.SetActive(false);
        uporabnik_email_text.text = "";
        uporabnik_ime_text.text = "";
        OpenloginP();
    }
}
