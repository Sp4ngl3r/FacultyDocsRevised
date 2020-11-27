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

namespace Firebase.Sample.Database {
  using Firebase;
  using Firebase.Database;
  using Firebase.Extensions;
  using Firebase.Unity.Editor;
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  // Handler for UI buttons on the scene.  Also performs some
  // necessary setup (initializing the firebase app, etc) on
  // startup.
  public class UIHandler : MonoBehaviour {

    ArrayList leaderBoard = new ArrayList();
    Vector2 scrollPosition = Vector2.zero;
    private Vector2 controlsScrollViewVector = Vector2.zero;

    public GUISkin fb_GUISkin;

    private const int MaxScores = 5;
    private string logText = "";
    public static string email = "";
    private int score = 100;
    private Vector2 scrollViewVector = Vector2.zero;
    protected bool UIEnabled = true;

    const int kMaxLogSize = 16382;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start() {
      leaderBoard.Clear();
      leaderBoard.Add("Firebase Top " + MaxScores.ToString() + " Scores");

      FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available) {
          InitializeFirebase();
        } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase() {
      FirebaseApp app = FirebaseApp.DefaultInstance;
      // NOTE: You'll need to replace this url with your Firebase App's database
      // path in order for the database connection to work correctly in editor.
      app.SetEditorDatabaseUrl("https://facultydocs-1b5e4.firebaseio.com/");
      if (app.Options.DatabaseUrl != null)
        app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
      StartListener();
      isFirebaseInitialized = true;
    }

    protected void StartListener() {
      FirebaseDatabase.DefaultInstance
        .GetReference("Faculties").OrderByChild("score")
        .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
          if (e2.DatabaseError != null) {
            Debug.LogError(e2.DatabaseError.Message);
            return;
          }
          Debug.Log("Received values for Faculties.");
          string title = leaderBoard[0].ToString();
          leaderBoard.Clear();
          leaderBoard.Add(title);
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children) {
              if (childSnapshot.Child("score") == null
              || childSnapshot.Child("score").Value == null) {
                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                break;
              } else {
                Debug.Log("Faculties entry : " +
                childSnapshot.Child("email").Value.ToString() + " - " +
                childSnapshot.Child("score").Value.ToString());
                leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
                + "  " + childSnapshot.Child("email").Value.ToString());
              }
            }
          }
        };
    }

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        Application.Quit();
      }
             
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

    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.
    TransactionResult AddScoreTransaction(MutableData mutableData) {
      List<object> Faculties = mutableData.Value as List<object>;

      if (Faculties == null) {
                Faculties = new List<object>();
      } else if (mutableData.ChildrenCount >= MaxScores) {
        // If the current list of scores is greater or equal to our maximum allowed number,
        // we see if the new score should be added and remove the lowest existing score.
        long minScore = long.MaxValue;
        object minVal = null;
        foreach (var child in Faculties) {
          if (!(child is Dictionary<string, object>))
            continue;
          long childScore = (long)((Dictionary<string, object>)child)["score"];
                    string extension = (string)((Dictionary<string, object>)child)["extension"];
                    
                    Debug.Log("extension url get working **************************************** "+extension);
                    if (childScore < minScore) {
            minScore = childScore;
            minVal = child;
          }
        }
        // If the new score is lower than the current minimum, we abort.
        if (minScore > score) {
          return TransactionResult.Abort();
        }
                // Otherwise, we remove the current lowest to be replaced with the new score.
                Faculties.Remove(minVal);
      }

      // Now we add the new score as a new entry that contains the email address and score.
      Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
      //newScoreMap["score"] = score;
      //newScoreMap["email"] = email;
            //***********************************************************************************************************************************************
            newScoreMap["year"] = Global.year;
            newScoreMap["facultyname"] = Global.faculty_name;
            newScoreMap["subject"] = Global.subject;

            newScoreMap["section"] = Global.section;

            newScoreMap["category_name"] = Global.category_name;

            newScoreMap["filename"] = Global.destination_filename_with_extension_for_uploading;
            newScoreMap["extension"] = Global.extension;

            //newScoreMap["filename_url"] = Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading;

            //***********************************************************************************************************************************************
            Faculties.Add(newScoreMap);
            //Faculties.Add("Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading );

      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = Faculties;
      return TransactionResult.Success(mutableData);
    }
        //***********************************************************************************************************************************************
       //public static void remove_from_real_time_database()
       // {
            

       // }
        public static string GetFilename(string s)
        {
            string str = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '.')
                {
                    str = (s.Substring(0, i));
                    break;
                }
            }
            return str;
        }
        
        public void AddScore() {
      //if (score == 0 || string.IsNullOrEmpty(email)) {
      //  DebugLog("invalid score or email.");
      //  return;
      //}
      DebugLog(String.Format("Attempting to add score {0} {1}",
        email, score.ToString()));
            //string str = "";
            //string ref = "";
            string filename = GetFilename(Global.destination_filename_with_extension_for_uploading);
            //string filename = GetFilename(Global.filename_to_copy);

            //string extension = Global.extension;
            string filenamewithextension =   filename + Global.extension + Global.extension.Length;//without dot
            
            string str = "Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name +"/"+filenamewithextension; 
            //Debug.Log("logging reference : "+ str);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(str); //+ Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading);



         DebugLog("Running Transaction...");
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.


            //reference.Child("Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading ).Push();
            //string push_id = reference.getKey();  //creating unique id here

            //inserting data into Hashmap to store that data
            //Map data = new HashMap();
            //data.put("name", "my_name");
            //data.put("type", "type here");


            //this Hashmap points to location where you want to store above data.
            //Map map1 = new HashMap();
            //map1.put("A/1/" + push_id, data);
            reference.RunTransaction(AddScoreTransaction)
              .ContinueWithOnMainThread(task =>
              {
                  if (task.Exception != null)
                  {
                      DebugLog(task.Exception.ToString());
                  }
                  else if (task.IsCompleted)
                  {
                      DebugLog("Transaction complete.");
                  }
              });


        }
        public void ADMINaddFolderName(string ADMINfoldernameToAdd)
        {
            //if (score == 0 || string.IsNullOrEmpty(email)) {
            //  DebugLog("invalid score or email.");
            //  return;
            //}
            DebugLog(String.Format("Attempting to add score {0} {1}",
              email, score.ToString()));
            //string str = "";
            //string ref = "";
            string filename = GetFilename(Global.destination_filename_with_extension_for_uploading);
            //string filename = GetFilename(Global.filename_to_copy);

            //string extension = Global.extension;
            string filenamewithextension = filename + Global.extension + Global.extension.Length;//without dot

            string str = "ADMIN/" + "Foldernames" + "/" + ADMINfoldernameToAdd;
            //Debug.Log("logging reference : "+ str);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(str); //+ Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading);



            DebugLog("Running Transaction...");
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.


            //reference.Child("Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading ).Push();
            //string push_id = reference.getKey();  //creating unique id here

            //inserting data into Hashmap to store that data
            //Map data = new HashMap();
            //data.put("name", "my_name");
            //data.put("type", "type here");


            //this Hashmap points to location where you want to store above data.
            //Map map1 = new HashMap();
            //map1.put("A/1/" + push_id, data);
            reference.RunTransaction(AddScoreTransaction)
              .ContinueWithOnMainThread(task =>
              {
                  if (task.Exception != null)
                  {
                      DebugLog(task.Exception.ToString());
                  }
                  else if (task.IsCompleted)
                  {
                      DebugLog("Transaction complete.");
                  }
              });


        }
        //***********************************************************************************************************************************************

        // Render the log output in a scroll view.
        void GUIDisplayLog() {
      scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
      GUILayout.Label(logText);
      GUILayout.EndScrollView();
    }

    // Render the buttons and other controls.
    void GUIDisplayControls() {
      if (UIEnabled) {
        controlsScrollViewVector =
            GUILayout.BeginScrollView(controlsScrollViewVector);
        GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Email:", GUILayout.Width(Screen.width * 0.20f));
                email = GUILayout.TextField(email);
                GUILayout.EndHorizontal();

                GUILayout.Space(20);

                //GUILayout.BeginHorizontal();
                //GUILayout.Label("Score:", GUILayout.Width(Screen.width * 0.20f));
                //int.TryParse(GUILayout.TextField(score.ToString()), out score);
                //GUILayout.EndHorizontal();

                GUILayout.Space(20);

                if (!String.IsNullOrEmpty(email) && GUILayout.Button("Enter Score")) {
          AddScore();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Go Offline")) {
          FirebaseDatabase.DefaultInstance.GoOffline();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Go Online")) {
          FirebaseDatabase.DefaultInstance.GoOnline();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
      }
    }

    void GUIDisplayLeaders() {
      GUI.skin.box.fontSize = 36;
      scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
      GUILayout.BeginVertical(GUI.skin.box);

      foreach (string item in leaderBoard) {
        GUILayout.Label(item, GUI.skin.box, GUILayout.ExpandWidth(true));
      }

      GUILayout.EndVertical();
      GUILayout.EndScrollView();
    }

    // Render the GUI:
    void OnGUI() {
      GUI.skin = fb_GUISkin;
      if (dependencyStatus != DependencyStatus.Available) {
        GUILayout.Label("One or more Firebase dependencies are not present.");
        GUILayout.Label("Current dependency status: " + dependencyStatus.ToString());
        return;
      }
      Rect logArea, controlArea, leaderBoardArea;

      if (Screen.width < Screen.height) {
        // Portrait mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
        leaderBoardArea = new Rect(0, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
        logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
      } else {
        // Landscape mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
        leaderBoardArea = new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f, Screen.height * 0.5f);
        logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
      }

      GUILayout.BeginArea(leaderBoardArea);
      GUIDisplayLeaders();
      GUILayout.EndArea();

      GUILayout.BeginArea(logArea);
      GUIDisplayLog();
      GUILayout.EndArea();

      GUILayout.BeginArea(controlArea);
      GUIDisplayControls();
      GUILayout.EndArea();
    }
  }
}
