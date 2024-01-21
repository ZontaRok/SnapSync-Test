using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;

public class Firabese : MonoBehaviour
{
    public GameObject loginP, singupP, profP,forgotPassP,napakP,gameP;

    public InputField loginEmail, loginPassword, singupEmail, singupPassword, singupUserName, singupCPassword, forgotPassEmail;

    public Text napaka_Naslov_text, napaka_Sporocilo_text,uporabnik_ime_text,uporabnik_email_text;

    public Toggle zapomni;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    bool isSingIn = false;

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

                InitializeFirebase();

            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void GameP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(false);
        forgotPassP.SetActive(false);
        gameP.SetActive(true);
    }

    public void OpenloginP()
    {
        loginP.SetActive(true);
        singupP.SetActive(false);
        profP.SetActive(false);
        forgotPassP.SetActive(false);
        gameP.SetActive(false);
    }

    public void OpensinginP()
    {
        loginP.SetActive(false);
        singupP.SetActive(true);
        profP.SetActive(false);
        forgotPassP.SetActive(false);
        gameP.SetActive(false);
    }

    public void OpenprofP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(true);
        forgotPassP.SetActive(false);
        gameP.SetActive(false);
    }

    public void OpenForgotPasswordP()
    {
        loginP.SetActive(false);
        singupP.SetActive(false);
        profP.SetActive(false);
        forgotPassP.SetActive(true);
        gameP.SetActive(false);
    }

  
    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            PrikazNotifikacij("Error", "porapvi polje");
            return;
        }
  
        SignInUser(loginEmail.text, loginPassword.text);
    }

    public void SingUpUser()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singupPassword.text) && string.IsNullOrEmpty(singupCPassword.text) && string.IsNullOrEmpty(singupUserName.text))
        {
            PrikazNotifikacij("Error", "porapvi polja");
            return;
        }

        CreateUser(singupEmail.text, singupPassword.text, singupUserName.text);

        
    }
    public void ForgotPass()
    {
        if(string.IsNullOrEmpty(forgotPassEmail.text))
        {
            PrikazNotifikacij("Error", "prazni email polje");


            return;
        }

        ForgetPasswordSubmit(forgotPassEmail.text);
    }

    private void PrikazNotifikacij(string title, string msg)
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

        auth.SignOut();
        profP.SetActive(false);
        uporabnik_email_text.text = "";
        uporabnik_ime_text.text = "";


        OpenloginP();
    }


    void CreateUser(string email, string password, string UserName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        PrikazNotifikacij("Error", GetErrorMessage(errorCode));
                    }
                }
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            UpdateUserProfile(UserName);
        });
    }

    public void SignInUser(string email, string password )
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread  (task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        PrikazNotifikacij("Error", GetErrorMessage(errorCode));
                    }
                }

                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            uporabnik_ime_text.text = "" + result.User.DisplayName;
            uporabnik_email_text.text = "" + result.User.Email;
            OpenprofP();
        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                isSingIn = true;
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    void UpdateUserProfile(string UserName)
    {

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = UserName,
                PhotoUrl = new System.Uri("https://placehold.jp/150x150.png"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");

                PrikazNotifikacij("Alert", "Accunt Successfully Created");
            });
        }
    }

    bool isSinged = false;

    void Update()
    {
        if (isSingIn)
        {
            if (isSinged)
            {
                isSinged = true;
                uporabnik_ime_text.text = "" + user.DisplayName;
                uporabnik_email_text.text = "" + user.Email;
                OpenprofP();
            }
        }
    }

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "The account already exists with different credentials";
                break;
            case AuthError.MissingPassword:
                message = "Password is needed";
                break;
            case AuthError.WeakPassword:
                message = "The password is weak";
                break;
            case AuthError.WrongPassword:
                message = "Geslo ni pravilno";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "The account with that email already exists";
                break;
            case AuthError.InvalidEmail:
                message = "invalid email";
                break;
            case AuthError.MissingEmail:
                message = "Email is needed";
                break;
            default:
                message = "An error occurred";
                break;
        }
        return message;
    }

   void ForgetPasswordSubmit(string forgotPassEmail)
    {
        auth.SendPasswordResetEmailAsync(forgotPassEmail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
            }

            if (task.IsFaulted)
            {
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        PrikazNotifikacij("Error", GetErrorMessage(errorCode));
                    }
                }

                PrikazNotifikacij("Alert", "Successfully Send Email For Reset Password");

            }

        });

    }

}
