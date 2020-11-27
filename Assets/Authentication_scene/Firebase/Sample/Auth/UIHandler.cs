// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

namespace Firebase.Sample.Auth {
  using Firebase.Extensions;
  using System;
    using System.Collections;
    using System.Collections.Generic;
  using System.Threading.Tasks;
  using UnityEngine;
  using UnityEngine.SceneManagement;
    using LukeWaffel.BUI;


    // Handler for UI buttons on the scene.  Also performs some
    // necessary setup (initializing the firebase app, etc) on
    // startup.
    public class UIHandler : MonoBehaviour {
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseAuth otherAuth;
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    public GUISkin fb_GUISkin;
    private string logText = "";
    public static string email = "";
    protected string password = "";
    protected string displayName = "";
    protected string phoneNumber = "";
    protected string receivedCode = "";






        public static string flag = "false";
    // Whether to sign in / link or reauthentication *and* fetch user profile data.
    protected bool signInAndFetchProfile = false;
    // Flag set when a token is being fetched.  This is used to avoid printing the token
    // in IdTokenChanged() when the user presses the get token button.
    private bool fetchingToken = false;
    // Enable / disable password input box.
    // NOTE: In some versions of Unity the password input box does not work in
    // iOS simulators.
    public bool usePasswordInput = false;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    bool UIEnabled = true;

    // Set the phone authentication timeout to a minute.
    private uint phoneAuthTimeoutMs = 60 * 1000;
    // The verification id needed along with the sent code for phone authentication.
    private string phoneAuthVerificationId;

    // Options used to setup secondary authentication object.
    private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions {
        ApiKey = "",
        AppId = "",
        ProjectId = ""
    };

    const int kMaxLogSize = 16382;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public virtual void Start() {
             //userInfo;/* = new Firebase.Auth.IUserInfo();*/
            //Global.faculty_name = getFacultynameFromEmail();
           

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
          InitializeFirebase();
                    //if (Global.signedin == false)
                    //{
                    //    SignOut();
                    //}

                } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
    }

    // Handle initialization of the necessary firebase modules:
    protected void InitializeFirebase() {
      //DebugLog("Setting up Firebase Auth");
      auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
      auth.StateChanged += AuthStateChanged;
      auth.IdTokenChanged += IdTokenChanged;
      // Specify valid options to construct a secondary authentication object.
      if (otherAuthOptions != null &&
          !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
            String.IsNullOrEmpty(otherAuthOptions.AppId) ||
            String.IsNullOrEmpty(otherAuthOptions.ProjectId))) {
        try {
          otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
            otherAuthOptions, "Secondary"));
          otherAuth.StateChanged += AuthStateChanged;
          otherAuth.IdTokenChanged += IdTokenChanged;
                    
                } catch (Exception) {
          DebugLog("ERROR: Failed to initialize secondary authentication object.");
        }
      }
      AuthStateChanged(this, null);
    }

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        Application.Quit();
      }
    }

    void OnDestroy() {
      if (auth != null) {
        auth.StateChanged -= AuthStateChanged;
        auth.IdTokenChanged -= IdTokenChanged;
        auth = null;
      }
      if (otherAuth != null) {
        otherAuth.StateChanged -= AuthStateChanged;
        otherAuth.IdTokenChanged -= IdTokenChanged;
        otherAuth = null;
      }
    }

    void DisableUI() {
      UIEnabled = false;
    }

    void EnableUI() {
      UIEnabled = true;
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s) {
      Debug.Log(s);
      logText += s + "\n";

      while (logText.Length > kMaxLogSize) {
        int index = logText.IndexOf("\n");
        logText = logText.Substring(index + 1);
      }
      scrollViewVector.y = int.MaxValue;
    }

    // Display additional user profile information.
    protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel) {
      string indent = new String(' ', indentLevel * 2);
      foreach (var kv in profile) {
        var valueDictionary = kv.Value as IDictionary<object, object>;
        if (valueDictionary != null) {
          DebugLog(String.Format("{0}{1}:", indent, kv.Key));
          DisplayProfile<object>(valueDictionary, indentLevel + 1);
        } else {
          DebugLog(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
        }
      }
    }

    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel) {
      string indent = new String(' ', indentLevel * 2);
      DisplayDetailedUserInfo(result.User, indentLevel);
      var metadata = result.Meta;
      if (metadata != null) {
        DebugLog(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
        DebugLog(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
      }
      var info = result.Info;
      if (info != null) {
        DebugLog(String.Format("{0}Additional User Info:", indent));
        DebugLog(String.Format("{0}  User Name: {1}", indent, info.UserName));
        DebugLog(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
        DisplayProfile<string>(info.Profile, indentLevel + 1);
      }
    }

    // Display user information.
    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
      string indent = new String(' ', indentLevel * 2);
      var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
      foreach (var property in userProperties) {
        if (!String.IsNullOrEmpty(property.Value)) {
                    //*********************************************************************************************************************
                    Global.faculty_name = getFacultynameFromEmail(userInfo.Email);
                    Firebase.Sample.Database.UIHandler.email = userInfo.Email;
                    //*********************************************************************************************************************

                    DebugLog(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
        }
      }
    }
        //*********************************************************************************************************************

        public string getFacultynameFromEmail(string email)
        {
            string facultyname = "";
            for (int i =0 ; i <= (email.Length - 1); i++)
            {

                if (email[i] == '@')
                {

                    facultyname = email.Substring(0,i);
                    break;

                }
            }
            return facultyname;

        }
        //*********************************************************************************************************************

        // Display a more detailed view of a FirebaseUser.
        protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel) {

      string indent = new String(' ', indentLevel * 2);
            //if(indent.Length==0)
            //{
            //    DebugLog("not signed IN !!!!");
            //}
      DisplayUserInfo(user, indentLevel);
      DebugLog(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
      DebugLog(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
            string is_signInDone = String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified);
            if(is_signInDone.Length!=0)
            {
                Global.signedin = true;
            }
      DebugLog(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
      var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
      var numberOfProviders = providerDataList.Count;
      if (numberOfProviders > 0) {
        for (int i = 0; i < numberOfProviders; ++i) {
          DebugLog(String.Format("{0}Provider Data: {1}", indent, i));
          DisplayUserInfo(providerDataList[i], indentLevel + 2);
                    //Global.faculty_name = getFacultynameFromEmail(userInfo.Email);

                    //SceneManager.LoadScene(2);

        }
      }
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
      Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
      Firebase.Auth.FirebaseUser user = null;
      if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
      if (senderAuth == auth && senderAuth.CurrentUser != user) {
        bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
        if (!signedIn && user != null) {
          DebugLog("Signed out " + user.UserId);
        }
        user = senderAuth.CurrentUser;
        userByAuth[senderAuth.App.Name] = user;
        if (signedIn) {
          DebugLog("Signed in " + user.UserId);
          displayName = user.DisplayName ?? "";
          DisplayDetailedUserInfo(user, 1);
        }
      }
    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs) {
      Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
      if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken) {
        senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
          task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
      }
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation) {
      bool complete = false;
      if (task.IsCanceled) {
        DebugLog(operation + " canceled.");
      } else if (task.IsFaulted) {
        DebugLog(operation + " encounted an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
          string authErrorCode = "";
          Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
          if (firebaseEx != null) {
            authErrorCode = String.Format("AuthError.{0}: ",
              ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
          }
          DebugLog(authErrorCode + exception.ToString());
        }
      } else if (task.IsCompleted) {
        DebugLog(operation + " completed");
        complete = true;
      }
      return complete;
    }

    // Create a user with the email and password.
    public Task CreateUserWithEmailAsync() {
      DebugLog(String.Format("Attempting to create user {0}...", email));
      DisableUI();

      // This passes the current displayName through to HandleCreateUserAsync
      // so that it can be passed to UpdateUserProfile().  displayName will be
      // reset by AuthStateChanged() when the new user is created and signed in.
      string newDisplayName = displayName;
      return auth.CreateUserWithEmailAndPasswordAsync(email, password)
        .ContinueWithOnMainThread((task) => {
          EnableUI();
          if (LogTaskCompletion(task, "User Creation")) {
            var user = task.Result;
            DisplayDetailedUserInfo(user, 1);
            return UpdateUserProfileAsync(newDisplayName: newDisplayName);
          }
          return task;
        }).Unwrap();
    }

    // Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync(string newDisplayName = null) {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to update user profile");
        return Task.FromResult(0);
      }
      displayName = newDisplayName ?? displayName;
      DebugLog("Updating user profile");
      DisableUI();
      return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
        DisplayName = displayName,
        PhotoUrl = auth.CurrentUser.PhotoUrl,
      }).ContinueWithOnMainThread(task => {
        EnableUI();
        if (LogTaskCompletion(task, "User profile")) {
          DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
      });
    }

    // Sign-in with an email and password.
    public Task SigninWithEmailAsync() {
            
      //DebugLog(String.Format("Attempting to sign in as {0}...", email));
      DisableUI();
            //Global.faculty_name = signInAndFetchProfile;
      if (signInAndFetchProfile) 
        {
                //flag = "true";
                //DebugLog(flag);
                //Global.signedin = true ;
                DebugLog(" if got executed ");
                //Debug.Log(Global.signedin);

                


                //SceneManager.LoadScene(1);
                //LukeWaffel.BUI.UIBox pasted_message_box = new LukeWaffel.BUI.UIBox("pastedID", "MessageBox");

                //pasted_message_box.body = " Logged in successfully !";
                ////pasted_message_box.body = " Do you want to save file to this computer ? ";

                //pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));
                ////pasted_message_box.buttons.Add(new UIButton("NO", close_message_box_and_delete_the_uploaded_file, "MessageButton"));



                //BUI.Instance.AddToQueue(pasted_message_box);

                return auth.SignInAndRetrieveDataWithCredentialAsync(Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(HandleSignInWithSignInResult);


            }
      //if(flag==true)
      //      {
      //          SceneManager.LoadScene(1);
      //      }
      else
            {
                //Global.signedin = true;
                //for(int i =0; i<email.Length;i++)
                //{
                //    if(email[i]=='@')
                //    {
                //        Global.faculty_name = email.Substring(0, i);
                //        break;
                //    }

                //}
                //DebugLog(" else got executed ");
                //Debug.Log(Global.signedin);
                


                 return auth.SignInWithEmailAndPasswordAsync(email, password)
                    .ContinueWithOnMainThread(HandleSignInWithUser);
            }


    }

    // This is functionally equivalent to the Signin() function.  However, it
    // illustrates the use of Credentials, which can be aquired from many
    // different sources of authentication.
    public Task SigninWithEmailCredentialAsync() {
      DebugLog(String.Format("Attemptttttting to sign in as {0}...", email));
      DisableUI();
      if (signInAndFetchProfile) {
        return auth.SignInAndRetrieveDataWithCredentialAsync(
          Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
            HandleSignInWithSignInResult);
      } else {
        return auth.SignInWithCredentialAsync(
          Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
            HandleSignInWithUser);
      }
    }
        void CloseBox(string id)
        {
            BUI.Instance.CloseBox(id);
        }
        void close_message_box(UIBox boxInfo, UIButton buttonInfo)
        {
            

            CloseBox(boxInfo.id);
            EnableUI();
            //update_cloud();



            //Global.upload_bool = true;
            //SceneManager.LoadScene(3);




        }

        // Attempt to sign in anonymously.
        public Task SigninAnonymouslyAsync() {
      DebugLog("Attempting to sign anonymously...");
      DisableUI();
      return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(HandleSignInWithUser);
    }

    public void AuthenticateToGameCenter() {
      #if UNITY_IOS
        Social.localUser.Authenticate(success => {
          Debug.Log("Game Center Initialization Complete - Result: " + success);
        });
      #else
        Debug.Log("Game Center is not supported on this platform.");
      #endif
    }

    public Task SignInWithGameCenterAsync() {
      var credentialTask = Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync();
      var continueTask = credentialTask.ContinueWithOnMainThread(task => {
        if(!task.IsCompleted)
          return null;

        if(task.Exception != null)
          Debug.Log("GC Credential Task - Exception: " + task.Exception.Message);

        var credential = task.Result;

        var loginTask = auth.SignInWithCredentialAsync(credential);
        return loginTask.ContinueWithOnMainThread(HandleSignInWithUser);
      });

      return continueTask;
    }

    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task) {
      EnableUI();
      if (LogTaskCompletion(task, "Sign-in")) {
        //DebugLog(String.Format("{0} signed in", task.Result.DisplayName));
                Global.signedin = true;
                Global.email = email;
                for (int i=0;i<Global.email.Length;i++)
                {
                    if(Global.email[i]=='@')
                    {
                        Global.faculty_name = Global.email.Substring(0, i);
                        break;
                    }
                }
                
                
                
                DebugLog(" user has logged in successfully with courseFile ");
                DebugLog(Global.email);
                DebugLog(Global.signedin + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                if (Global.email.Equals("csedsceprojects@gmail.com") && Global.signedin == true)
                {
                    DebugLog(" ADMINscene");
                    SceneManager.LoadSceneAsync(4);
                }
                if (Global.signedin == true && !Global.email.Equals("csedsceprojects@gmail.com"))
                {
                    DebugLog("NonADMIN scene");
                    SceneManager.LoadSceneAsync(2);

                }
                //else if(Global.signedin==false)
                //{
                //    DisableUI();
                //    DebugLog(" wrong email or password ");
                //    UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

                //    pasted_message_box.body = " wrong email or password";
                //    pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

                //    BUI.Instance.AddToQueue(pasted_message_box);

                //}
                else

                {

                    DisableUI();
                    DebugLog(" wrong email or password ");
                    UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

                    pasted_message_box.body = " wrong email or password";
                    pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

                    BUI.Instance.AddToQueue(pasted_message_box);
                
                }
            }
            //else
            //{
            //    DisableUI();
            //    DebugLog(" wrong email or password ");
            //    UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

            //    pasted_message_box.body = " wrong email or password";
            //    pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

            //    BUI.Instance.AddToQueue(pasted_message_box);


            //}
        }

    // Called when a sign-in with profile data completes.
    void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task) {
      EnableUI();
      if (LogTaskCompletion(task, "Sign-in")) {
        DisplaySignInResult(task.Result, 1);
      }
    }

    // Link the current user with an email / password credential.
    protected Task LinkWithEmailCredentialAsync() {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to link credential to user.");
        var tcs = new TaskCompletionSource<bool>();
        tcs.SetException(new Exception("Not signed in"));
        return tcs.Task;
      }
      DebugLog("Attempting to link credential to user...");
      Firebase.Auth.Credential cred =
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
      if (signInAndFetchProfile) {
        return
          auth.CurrentUser.LinkAndRetrieveDataWithCredentialAsync(cred).ContinueWithOnMainThread(
            task => {
              if (LogTaskCompletion(task, "Link Credential")) {
                DisplaySignInResult(task.Result, 1);
              }
            }
          );
      } else {
        return auth.CurrentUser.LinkWithCredentialAsync(cred).ContinueWithOnMainThread(task => {
          if (LogTaskCompletion(task, "Link Credential")) {
              DisplayDetailedUserInfo(task.Result, 1);
          }
        });
      }
    }

    // Reauthenticate the user with the current email / password.
    protected Task ReauthenticateAsync() {
      var user = auth.CurrentUser;
      if (user == null) {
        DebugLog("Not signed in, unable to reauthenticate user.");
        var tcs = new TaskCompletionSource<bool>();
        tcs.SetException(new Exception("Not signed in"));
        return tcs.Task;
      }
      DebugLog("Reauthenticating...");
      DisableUI();
      Firebase.Auth.Credential cred = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
      if (signInAndFetchProfile) {
        return user.ReauthenticateAndRetrieveDataAsync(cred).ContinueWithOnMainThread(task => {
          EnableUI();
          if (LogTaskCompletion(task, "Reauthentication")) {
            DisplaySignInResult(task.Result, 1);
          }
        });
      } else {
        return user.ReauthenticateAsync(cred).ContinueWithOnMainThread(task => {
          EnableUI();
          if (LogTaskCompletion(task, "Reauthentication")) {
            DisplayDetailedUserInfo(auth.CurrentUser, 1);
          }
        });
      }
    }

    // Reload the currently logged in user.
    public void ReloadUser() {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to reload user.");
        return;
      }
      DebugLog("Reload User Data");
      auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task => {
        if (LogTaskCompletion(task, "Reload")) {
          DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
      });
    }

    // Fetch and display current user's auth token.
    public void GetUserToken() {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to get token.");
        return;
      }
      DebugLog("Fetching user token");
      fetchingToken = true;
      auth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(task => {
        fetchingToken = false;
        if (LogTaskCompletion(task, "User token fetch")) {
          DebugLog("Token = " + task.Result);
        }
      });
    }

    // Display information about the currently logged in user.
    void GetUserInfo() {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to get info.");
      } else {
        DebugLog("Current user info:");
        DisplayDetailedUserInfo(auth.CurrentUser, 1);
      }
    }

    // Unlink the email credential from the currently logged in user.
    protected Task UnlinkEmailAsync() {
      if (auth.CurrentUser == null) {
        DebugLog("Not signed in, unable to unlink");
        var tcs = new TaskCompletionSource<bool>();
        tcs.SetException(new Exception("Not signed in"));
        return tcs.Task;
      }
      DebugLog("Unlinking email credential");
      DisableUI();
      return auth.CurrentUser.UnlinkAsync(
        Firebase.Auth.EmailAuthProvider.GetCredential(email, password).Provider)
          .ContinueWithOnMainThread(task => {
            EnableUI();
            LogTaskCompletion(task, "Unlinking");
          });
    }

    // Sign out the current user.
    public void SignOut() {
      DebugLog("Signing out.");
            //Global.signedin = false;
            //Global.faculty_name = "";
            //Debug.Log(Global.signedin);

            //flag = "false";
            //DebugLog(flag);
           

            auth.SignOut();
            Global.signedin = false;
        }

    // Delete the currently logged in user.
    protected Task DeleteUserAsync() {
      if (auth.CurrentUser != null) {
        DebugLog(String.Format("Attempting to delete user {0}...", auth.CurrentUser.UserId));
        DisableUI();
        return auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task => {
          EnableUI();
          LogTaskCompletion(task, "Delete user");
        });
      } else {
        DebugLog("Sign-in before deleting user.");
        // Return a finished task.
        return Task.FromResult(0);
      }
    }

    // Show the providers for the current email address.
    protected void DisplayProvidersForEmail() {
      auth.FetchProvidersForEmailAsync(email).ContinueWithOnMainThread((authTask) => {
        if (LogTaskCompletion(authTask, "Fetch Providers")) {
          DebugLog(String.Format("Email Providers for '{0}':", email));
          foreach (string provider in authTask.Result) {
            DebugLog(provider);
          }
        }
      });
    }

    // Send a password reset email to the current email address.
    protected void SendPasswordResetEmail() {
      auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread((authTask) => {
        if (LogTaskCompletion(authTask, "Send Password Reset Email")) {
          DebugLog("Password reset email sent to " + email);
        }
      });
    }

    // Begin authentication with the phone number.
    protected void VerifyPhoneNumber() {
      var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(auth);
      phoneAuthProvider.VerifyPhoneNumber(phoneNumber, phoneAuthTimeoutMs, null,
        verificationCompleted: (cred) => {
          DebugLog("Phone Auth, auto-verification completed");
          if (signInAndFetchProfile) {
            auth.SignInAndRetrieveDataWithCredentialAsync(cred).ContinueWithOnMainThread(
              HandleSignInWithSignInResult);
          } else {
            auth.SignInWithCredentialAsync(cred).ContinueWithOnMainThread(HandleSignInWithUser);
          }
        },
        verificationFailed: (error) => {
          DebugLog("Phone Auth, verification failed: " + error);
        },
        codeSent: (id, token) => {
          phoneAuthVerificationId = id;
          DebugLog("Phone Auth, code sent");
        },
        codeAutoRetrievalTimeOut: (id) => {
          DebugLog("Phone Auth, auto-verification timed out");
        });
    }

    // Sign in using phone number authentication using code input by the user.
    protected void VerifyReceivedPhoneCode() {
      var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(auth);
      // receivedCode should have been input by the user.
      var cred = phoneAuthProvider.GetCredential(phoneAuthVerificationId, receivedCode);
      if (signInAndFetchProfile) {
        auth.SignInAndRetrieveDataWithCredentialAsync(cred).ContinueWithOnMainThread(
          HandleSignInWithSignInResult);
      } else {
        auth.SignInWithCredentialAsync(cred).ContinueWithOnMainThread(HandleSignInWithUser);
      }
    }

    // Determines whether another authentication object is available to focus.
    protected bool HasOtherAuth { get { return auth != otherAuth && otherAuth != null; } }

    // Swap the authentication object currently being controlled by the application.
    protected void SwapAuthFocus() {
      if (!HasOtherAuth) return;
      var swapAuth = otherAuth;
      otherAuth = auth;
      auth = swapAuth;
      DebugLog(String.Format("Changed auth from {0} to {1}",
                              otherAuth.App.Name, auth.App.Name));
    }

    void GUIDisplayGameCenterControls() {
      bool isOnIosDevice = Application.platform == RuntimePlatform.IPhonePlayer;
      bool isOnOSXDesktop = (Application.platform == RuntimePlatform.OSXEditor ||
                              Application.platform == RuntimePlatform.OSXPlayer);

      if(isOnIosDevice || isOnOSXDesktop)
      {
        if (GUILayout.Button(new GUIContent("Authenticate To Game Center"))) {
          AuthenticateToGameCenter();
        }

        bool gameCenterEnabled = (isOnIosDevice ?
                                    Firebase.Auth.GameCenterAuthProvider.IsPlayerAuthenticated() :
                                    false);
        using (new ScopedGuiEnabledModifier(gameCenterEnabled))
        {
          string tooltip = "";
          if(!gameCenterEnabled) {
            tooltip = "No Game Center player authenticated.";
          }
          if (GUILayout.Button(new GUIContent("Sign In With Game Center", tooltip))) {
            SignInWithGameCenterAsync();
          }
        }
      }
    }

    // Render the log output in a scroll view.
    void GUIDisplayLog() {
      scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
      GUILayout.Label(logText);
      GUILayout.EndScrollView();
    }

//*************************************************************************//
    void nextscene()
        {
            if (Global.email.Equals("csedsceprojects@gmail.com") && Global.signedin == true)
            {
                DebugLog(" ADMINscene");
                SceneManager.LoadSceneAsync(4);
            }
            if (Global.signedin == true && !Global.email.Equals("csedsceprojects@gmail.com"))
            {
                DebugLog("NonADMIN scene");
                SceneManager.LoadScene(2);

            }
            else

            {
                DisableUI();
                DebugLog(" wrong email or password ");
                UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

                pasted_message_box.body = " wrong email or password";
                pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

                BUI.Instance.AddToQueue(pasted_message_box);
                
            }
        }

    // Render the buttons and other controls.
    void GUIDisplayControls() {
      if (UIEnabled) {
        controlsScrollViewVector =
          GUILayout.BeginScrollView(controlsScrollViewVector);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Email:", GUILayout.Width(Screen.width * 0.10f));
        email = GUILayout.TextField(email); // assign this from the text file given by FacHelper
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Password:", GUILayout.Width(Screen.width * 0.10f));
        password = usePasswordInput ? GUILayout.PasswordField(password, '*') : GUILayout.TextField(password);
                
                


              
                //GUILayout.EndHorizontal();
                //GUILayout.BeginHorizontal();
                if (GUILayout.Button("SHOW"))
                {
                    if (Global.toggle == false)
                    {
                        usePasswordInput = false;         //Edited for updation of password input on 19/06/2020 - manoj v khatokar
                        Global.toggle = true;
                    }
                    else
                    {
                        usePasswordInput = true;
                        Global.toggle = false;

                    }

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Display Name:", GUILayout.Width(Screen.width * 0.20f));
        //displayName = GUILayout.TextField(displayName);
        //GUILayout.EndHorizontal();

        //GUILayout.Space(20);

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Phone Number:", GUILayout.Width(Screen.width * 0.20f));
        //phoneNumber = GUILayout.TextField(phoneNumber);
        //GUILayout.EndHorizontal();

        //GUILayout.Space(20);

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Phone Auth Received Code:", GUILayout.Width(Screen.width * 0.20f));
        //receivedCode = GUILayout.TextField(receivedCode);
        //GUILayout.EndHorizontal();

        GUILayout.Space(20);

                //if (GUILayout.Button("Create User"))
                //{
                //    CreateUserWithEmailAsync();
                //}
                //if (GUILayout.Button("Sign In Anonymously")) {
                //  SigninAnonymouslyAsync();
                //}
                if (GUILayout.Button("LOGIN")) {
                    
                    SigninWithEmailAsync();
                    //nextscene();
                    
                   


                }
           
                GUIDisplayGameCenterControls();
                //if (GUILayout.Button("Reload User")) {
                //  ReloadUser();
                //}
                //if (GUILayout.Button("Get User Token")) {
                //  GetUserToken();
                //}
                //if (GUILayout.Button("Get User Info")) {
                //  GetUserInfo();
                //}
                //if (GUILayout.Button("Unlink Email Credential")) {
                //  UnlinkEmailAsync();
                //}
                //if (GUILayout.Button("LOG OUT"))
                //{
                //    Global.signedin = false;
                //    Global.clearGlobal();
                //    SignOut();
                //}
                //if (GUILayout.Button("NEXT"))
                //{

                //    //if (Global.faculty_name.Equals("csedsceprojects") && Global.signedin == true)
                //    //{
                //    //    SceneManager.LoadScene(4);
                //    //}
                //    //if (Global.signedin == true)
                //    //{
                //    //    SceneManager.LoadScene(2);

                //    //}
                //    //nextscene();




                //}

                if (HasOtherAuth && GUILayout.Button(String.Format("Switch to auth object {0}",
                                                            otherAuth.App.Name))) {
          SwapAuthFocus();
        }
        GUIDisplayCustomControls();
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
      }
    }

    // Overridable function to allow additional controls to be added.
    protected virtual void GUIDisplayCustomControls() { }

    // Render the GUI:
    void OnGUI() {
      GUI.skin = fb_GUISkin;
      if (dependencyStatus != Firebase.DependencyStatus.Available) {
        GUILayout.Label("One or more Firebase dependencies are not present.");
        GUILayout.Label("Current dependency status: " + dependencyStatus.ToString());
        return;
      }
      Rect logArea, controlArea;

      if (Screen.width < Screen.height) {
        // Portrait mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
        logArea = new Rect(0.0f, Screen.height * 0.5f, Screen.width, Screen.height * 0.5f);
      } else {
        // Landscape mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
        logArea = new Rect(Screen.width * 0.5f, 0.0f, Screen.width * 0.5f, Screen.height);
      }

      GUILayout.BeginArea(logArea);
      //GUIDisplayLog();     //*******************************************************************************************************************//
      GUILayout.EndArea();

      GUILayout.BeginArea(controlArea);
      GUIDisplayControls();
      GUILayout.EndArea();
    }

    private class ScopedGuiEnabledModifier : IDisposable {
      private bool wasEnabled;
      public ScopedGuiEnabledModifier(bool newValue) {
        wasEnabled = GUI.enabled;
        GUI.enabled = newValue;
      }

      public void Dispose() {
        GUI.enabled = wasEnabled;
      }
    }

  }
}
