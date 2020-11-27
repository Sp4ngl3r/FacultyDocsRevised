using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.ComponentModel;

public class ADMINaccess : MonoBehaviour
{

    public static string[] folder_names_in_the_cloud_directory = new string[32];

    



    public Text foldername;

    public Text facultyname;
    public Text subjectCode;
    

    public static string[] FacultyFoldernames = new string[100];
    public static string[] filenames_in_the_cloud_directory = new string[16];


    public GameObject folder_names_loading_animation;
    public GameObject filenames_loading_animation;
    public Button[] dynamicFolderButtons;
    public Sprite button_sprite;


    public Button[] dynamicButtons2;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button b in dynamicFolderButtons)
        {
            b.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

        ////setting dynamic ADMIN Folder Buttons ************************************************************* 
        for (int j = 0; j < folder_names_in_the_cloud_directory.Length; j++)
        {
            UnityEngine.Debug.Log("foldernames of the faculty are ****** : " + folder_names_in_the_cloud_directory[j]);
            dynamicFolderButtons[j].gameObject.SetActive(true);


            dynamicFolderButtons[j].GetComponentInChildren<Text>().text = folder_names_in_the_cloud_directory[j];

            dynamicFolderButtons[j].name = folder_names_in_the_cloud_directory[j];
            if (dynamicFolderButtons[j].GetComponentInChildren<Text>().text.Length != 0)
            {
                folder_names_loading_animation.SetActive(false);


                dynamicFolderButtons[j].GetComponentInChildren<Image>().sprite = button_sprite;
                ColorBlock colors = dynamicFolderButtons[j].colors;
                colors.normalColor = Color.white;
                dynamicFolderButtons[j].colors = colors;


            }


        }
        for (int j = 0; j < filenames_in_the_cloud_directory.Length; j++)
        {
            UnityEngine.Debug.Log("filenames  in folder are ****** : " + filenames_in_the_cloud_directory[j]);
            dynamicButtons2[j].gameObject.SetActive(true);



            //dynamicButtons2[j].GetComponentInChildren<Text>().text = "texttttttttt";
            //Global.extension_length = Int32.Parse(filenames_in_the_cloud_directory[j].Substring(0, 1));

            dynamicButtons2[j].GetComponentInChildren<Text>().text = filenames_in_the_cloud_directory[j];
            if (dynamicButtons2[j].GetComponentInChildren<Text>().text.Length != 0)
            {
                filenames_loading_animation.SetActive(false);


            }
        }

        

    }

    public void GetFoldersOfFaculty()
    {
        folder_names_loading_animation.SetActive(true);
        string faculty_name = facultyname.text.ToString();
        string subcode = subjectCode.text.ToString();


        //write the real time database getter function here

        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        string str = "Faculties/" + faculty_name + "/" + Global.year + "/" + subcode + "/" + Global.section + "/";
        UnityEngine.Debug.Log(" database url value = @@@@@@@@@@@@@@@@@@@@@@@@@" + str);
        for (int i = 0; i < folder_names_in_the_cloud_directory.Length; i++)
        {
            folder_names_in_the_cloud_directory[i] = null;
        }
        FirebaseDatabase.DefaultInstance.GetReference(str).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                UnityEngine.Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                UnityEngine.Debug.Log("key ****** " + snapshot.Key);
                UnityEngine.Debug.Log("value ******* " + snapshot.Value);

                newScoreMap = convert_to_dictionary<string, object>(snapshot.Value);





                for (int i = 0; i <= newScoreMap.Count; i++)
                {
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i));

                    if (newScoreMap.Keys.ElementAt(i) != null)
                    {
                        folder_names_in_the_cloud_directory[0] = newScoreMap.Keys.ElementAt(i);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[0]);



                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 1));
                    if (newScoreMap.Keys.ElementAt(i + 1) != null)
                    {
                        //folder_names_in_the_cloud_directory[1] = newScoreMap.Keys.ElementAt(i + 1);
                        UnityEngine.Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[1]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 2));
                    if (newScoreMap.Keys.ElementAt(i + 2) != null)
                    {
                        folder_names_in_the_cloud_directory[2] = newScoreMap.Keys.ElementAt(i + 2);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[2]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 3));
                    if (newScoreMap.Keys.ElementAt(i + 3) != null)
                    {
                        folder_names_in_the_cloud_directory[3] = newScoreMap.Keys.ElementAt(i + 3);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[3]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 4));
                    if (newScoreMap.Keys.ElementAt(i + 4) != null)
                    {
                        folder_names_in_the_cloud_directory[4] = newScoreMap.Keys.ElementAt(i + 4);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[4]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 5));
                    if (newScoreMap.Keys.ElementAt(i + 5) != null)
                    {
                        folder_names_in_the_cloud_directory[5] = newScoreMap.Keys.ElementAt(i + 5);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[5]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 6));
                    if (newScoreMap.Keys.ElementAt(i + 6) != null)
                    {
                        folder_names_in_the_cloud_directory[6] = newScoreMap.Keys.ElementAt(i + 6);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[6]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 7));
                    if (newScoreMap.Keys.ElementAt(i + 7) != null)
                    {
                        folder_names_in_the_cloud_directory[7] = newScoreMap.Keys.ElementAt(i + 7);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[7]);
                    }


                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 8));
                    if (newScoreMap.Keys.ElementAt(i + 8) != null)
                    {
                        folder_names_in_the_cloud_directory[8] = newScoreMap.Keys.ElementAt(i + 8);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[8]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 9));
                    if (newScoreMap.Keys.ElementAt(i + 9) != null)
                    {
                        folder_names_in_the_cloud_directory[9] = newScoreMap.Keys.ElementAt(i + 9);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[9]);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 10));
                    if (newScoreMap.Keys.ElementAt(i + 10) != null)
                    {
                        folder_names_in_the_cloud_directory[10] = newScoreMap.Keys.ElementAt(i + 10);
                        //Debug.Log("filename in cloud = " + folder_names_in_the_cloud_directory[10]);





                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 11));
                    if (newScoreMap.Keys.ElementAt(i + 11) != null)
                    {
                        folder_names_in_the_cloud_directory[11] = newScoreMap.Keys.ElementAt(i + 11);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 12));
                    if (newScoreMap.Keys.ElementAt(i + 12) != null)
                    {
                        folder_names_in_the_cloud_directory[12] = newScoreMap.Keys.ElementAt(i + 12);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 13));
                    if (newScoreMap.Keys.ElementAt(i + 13) != null)
                    {
                        folder_names_in_the_cloud_directory[13] = newScoreMap.Keys.ElementAt(i + 13);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 14));
                    if (newScoreMap.Keys.ElementAt(i + 14) != null)
                    {
                        folder_names_in_the_cloud_directory[14] = newScoreMap.Keys.ElementAt(i + 14);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 15));
                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                    {
                        folder_names_in_the_cloud_directory[15] = newScoreMap.Keys.ElementAt(i + 15);


                    }
                    //Debug.Log("********************** value: " + (string)newScoreMap.Values.ElementAt(i));





                    //keys[i] = newScoreMap.Keys.ElementAt(i);
                    //Global.category_name = (string)newScoreMap.Values.ElementAt(i);
                    //Global.faculty_name = (string)newScoreMap.Values.ElementAt(i);
                    //Global.destination_filename_with_extension_for_downloading = (string)newScoreMap.Values.ElementAt(i);
                    //Global.section = (string)newScoreMap.Values.ElementAt(i);
                    //Global.subject = (string)newScoreMap.Values.ElementAt(i);
                    //Global.year = (string)newScoreMap.Values.ElementAt(i);

                    //values[i] = (string)newScoreMap.Values.ElementAt(i);

                    //string filename_url = (string)((Dictionary<string, object>)newScoreMap)["filename_url"];
                    //Debug.Log("Filenmae url get working **************************************** " + filename_url);
                }
                UnityEngine.Debug.Log("$$$$$$$$$$$$$$$$ reached the null point");




            }
        });






    }

    public static Dictionary<string, object> convert_to_dictionary<TKey, TValue>(object obj)
    {
        if (obj is Dictionary<string, object> stringDictionary)
        {
            return stringDictionary;
        }


        return null;
    }

    public  void addNewFolderName()
    {
        //************************uploading ADMIN folder names to firebase real time database **************************
        Firebase.Sample.Database.UIHandler uihandler = new Firebase.Sample.Database.UIHandler();
        string foldname = foldername.text.ToString();
        uihandler.ADMINaddFolderName(foldname);
    }
    public void generate_summary_PDF()
    {
        //File.Open("C:/Users/manoj.LAPTOP-AKB0DTBI/source/repos/CourseFileADMIN/bin/CourseFileADMIN");


        Process myProcess = new Process();
        try
        {
            myProcess.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.
            myProcess.StartInfo.FileName = "C:/Users/manoj.LAPTOP-AKB0DTBI/source/repos/CourseFileADMIN/bin/Debug/CourseFileADMIN";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            // This code assumes the process you are starting will terminate itself.
            // Given that is is started without a window so you cannot terminate it
            // on the desktop, it must terminate itself or you can do it programmatically
            // from this application using the Kill method.
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public  void removeFoldername()
    {

        //***************************** removing ADMIN folder names from real time database*******************************
        string str = "ADMIN/" + "Foldernames" + "/" + foldername.text.ToString();

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(str); //+ Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading);
        reference.RemoveValueAsync();
        UnityEngine.Debug.Log(" deleted folder " + str);



    }
    public void GetFilesFromFolder(Button button)
    {
        filenames_loading_animation.SetActive(true);
        string faculty_name = facultyname.text.ToString();
        Global.faculty_name = faculty_name;
        string subcode = subjectCode.text.ToString();
        Global.subject = subcode;
        Global.category_name = button.name;


        foreach (Button b in dynamicButtons2)
        {
            b.gameObject.SetActive(false);
        }


        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        string str = "Faculties/" + faculty_name + "/" + Global.year + "/" + subcode + "/" + Global.section + "/" + button.name + "/";
        UnityEngine.Debug.Log(" database url value = @@@@@@@@@@@@@@@@@@@@@@@@@" + str);
        for (int i = 0; i < filenames_in_the_cloud_directory.Length; i++)
        {
            filenames_in_the_cloud_directory[i] = null;
        }
        FirebaseDatabase.DefaultInstance.GetReference(str).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                UnityEngine.Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                UnityEngine.Debug.Log("key ****** " + snapshot.Key);
                UnityEngine.Debug.Log("value ******* " + snapshot.Value);

                newScoreMap = convert_to_dictionary<string, object>(snapshot.Value);





                for (int i = 0; i <= newScoreMap.Count; i++)
                {
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i));

                    if (newScoreMap.Keys.ElementAt(i) != null)
                    {
                        filenames_in_the_cloud_directory[0] = newScoreMap.Keys.ElementAt(i);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[0]);



                    }
                    UnityEngine.Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 1));
                    if (newScoreMap.Keys.ElementAt(i + 1) != null)
                    {
                        filenames_in_the_cloud_directory[1] = newScoreMap.Keys.ElementAt(i + 1);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[1]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 2));
                    if (newScoreMap.Keys.ElementAt(i + 2) != null)
                    {
                        filenames_in_the_cloud_directory[2] = newScoreMap.Keys.ElementAt(i + 2);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[2]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 3));
                    if (newScoreMap.Keys.ElementAt(i + 3) != null)
                    {
                        filenames_in_the_cloud_directory[3] = newScoreMap.Keys.ElementAt(i + 3);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[3]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 4));
                    if (newScoreMap.Keys.ElementAt(i + 4) != null)
                    {
                        filenames_in_the_cloud_directory[4] = newScoreMap.Keys.ElementAt(i + 4);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[4]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 5));
                    if (newScoreMap.Keys.ElementAt(i + 5) != null)
                    {
                        filenames_in_the_cloud_directory[5] = newScoreMap.Keys.ElementAt(i + 5);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[5]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 6));
                    if (newScoreMap.Keys.ElementAt(i + 6) != null)
                    {
                        filenames_in_the_cloud_directory[6] = newScoreMap.Keys.ElementAt(i + 6);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[6]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 7));
                    if (newScoreMap.Keys.ElementAt(i + 7) != null)
                    {
                        filenames_in_the_cloud_directory[7] = newScoreMap.Keys.ElementAt(i + 7);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[7]);
                    }


                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 8));
                    if (newScoreMap.Keys.ElementAt(i + 8) != null)
                    {
                        filenames_in_the_cloud_directory[8] = newScoreMap.Keys.ElementAt(i + 8);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[8]);


                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 9));
                    if (newScoreMap.Keys.ElementAt(i + 9) != null)
                    {
                        filenames_in_the_cloud_directory[9] = newScoreMap.Keys.ElementAt(i + 9);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[9]);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 10));
                    if (newScoreMap.Keys.ElementAt(i + 10) != null)
                    {
                        filenames_in_the_cloud_directory[10] = newScoreMap.Keys.ElementAt(i + 10);
                        //Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[10]);





                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 11));
                    if (newScoreMap.Keys.ElementAt(i + 11) != null)
                    {
                        filenames_in_the_cloud_directory[11] = newScoreMap.Keys.ElementAt(i + 11);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 12));
                    if (newScoreMap.Keys.ElementAt(i + 12) != null)
                    {
                        filenames_in_the_cloud_directory[12] = newScoreMap.Keys.ElementAt(i + 12);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 13));
                    if (newScoreMap.Keys.ElementAt(i + 13) != null)
                    {
                        filenames_in_the_cloud_directory[13] = newScoreMap.Keys.ElementAt(i + 13);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 14));
                    if (newScoreMap.Keys.ElementAt(i + 14) != null)
                    {
                        filenames_in_the_cloud_directory[14] = newScoreMap.Keys.ElementAt(i + 14);

                    }
                    //Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 15));
                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                    {
                        filenames_in_the_cloud_directory[15] = newScoreMap.Keys.ElementAt(i + 15);


                    }
                    //Debug.Log("********************** value: " + (string)newScoreMap.Values.ElementAt(i));
                    if (newScoreMap.Keys.ElementAt(i + 16) != null)
                    {
                        filenames_in_the_cloud_directory[16] = newScoreMap.Keys.ElementAt(i + 16);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 17) != null)
                    {
                        filenames_in_the_cloud_directory[17] = newScoreMap.Keys.ElementAt(i + 17);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 18) != null)
                    {
                        filenames_in_the_cloud_directory[18] = newScoreMap.Keys.ElementAt(i + 18);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 19) != null)
                    {
                        filenames_in_the_cloud_directory[19] = newScoreMap.Keys.ElementAt(i + 19);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 20) != null)
                    {
                        filenames_in_the_cloud_directory[20] = newScoreMap.Keys.ElementAt(i + 20);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 21) != null)
                    {
                        filenames_in_the_cloud_directory[21] = newScoreMap.Keys.ElementAt(i + 21);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 22) != null)
                    {
                        filenames_in_the_cloud_directory[22] = newScoreMap.Keys.ElementAt(i + 22);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                    {
                        filenames_in_the_cloud_directory[23] = newScoreMap.Keys.ElementAt(i + 23);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 24) != null)
                    {
                        filenames_in_the_cloud_directory[24] = newScoreMap.Keys.ElementAt(i + 24);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 25) != null)
                    {
                        filenames_in_the_cloud_directory[25] = newScoreMap.Keys.ElementAt(i + 25);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 26) != null)
                    {
                        filenames_in_the_cloud_directory[26] = newScoreMap.Keys.ElementAt(i + 26);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 27) != null)
                    {
                        filenames_in_the_cloud_directory[27] = newScoreMap.Keys.ElementAt(i + 27);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 28) != null)
                    {
                        filenames_in_the_cloud_directory[28] = newScoreMap.Keys.ElementAt(i + 28);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 29) != null)
                    {
                        filenames_in_the_cloud_directory[29] = newScoreMap.Keys.ElementAt(i + 29);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 30) != null)
                    {
                        filenames_in_the_cloud_directory[30] = newScoreMap.Keys.ElementAt(i + 30);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 31) != null)
                    {
                        filenames_in_the_cloud_directory[31] = newScoreMap.Keys.ElementAt(i + 31);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 32) != null)
                    {
                        filenames_in_the_cloud_directory[32] = newScoreMap.Keys.ElementAt(i + 32);


                    }




                    //keys[i] = newScoreMap.Keys.ElementAt(i);
                    //Global.category_name = (string)newScoreMap.Values.ElementAt(i);
                    //Global.faculty_name = (string)newScoreMap.Values.ElementAt(i);
                    //Global.destination_filename_with_extension_for_downloading = (string)newScoreMap.Values.ElementAt(i);
                    //Global.section = (string)newScoreMap.Values.ElementAt(i);
                    //Global.subject = (string)newScoreMap.Values.ElementAt(i);
                    //Global.year = (string)newScoreMap.Values.ElementAt(i);

                    //values[i] = (string)newScoreMap.Values.ElementAt(i);

                    //string filename_url = (string)((Dictionary<string, object>)newScoreMap)["filename_url"];
                    //Debug.Log("Filenmae url get working **************************************** " + filename_url);
                }
                //Debug.Log("$$$$$$$$$$$$$$$$ reached the null point");




            }
        });
    }

}
