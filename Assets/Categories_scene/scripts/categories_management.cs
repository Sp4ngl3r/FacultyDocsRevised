using System.Collections;
using System.Collections.Generic;

using UnityEngine;

//using UnityEditor.UI;
using UnityEngine.UI;
using UnityEditor;
using Firebase.Sample.Storage;
using UnityEngine.SceneManagement;
using Crosstales.FB;
using System;
using System.IO;
using Firebase.Storage;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Linq;

using LukeWaffel.BUI;

public class categories_management : MonoBehaviour
{

    public Button ethics;
    public Button Vision;
    public Button calender;
    public Button Subject;
    public Button Syllabus;
    public Button coop;
    public Button Teaching;
    public Button Lesson;
    public Button Class;
    public Button Individual;
    public Button Student;
    public Button CIA;
    public Button Assignment;
    public Button aat;
    public Button Tutorial;
    public Button Remedial;
    public Button Challenging;
    public Button Inclusive;
    public Button Beyond;
    public Button Question;
    public Button FAQ;
    public Button SEE;
    public Button Notes;
    public Button Result;
    public Button CO;
    public Button Suggestions;


    public GameObject loading_animation;

    public GameObject folder_names_loading_animation;

    
    
    

    string explorer_path = "C:"+"\\"+"FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.semester + "\\" + Global.section + "\\" + Global.category_name + "\\";
    public RawImage File_viewer_area;
    public Text selected_file_text_box;
    public Text viewpanel;

    //public GameObject dynamicButtonPrefab;
    //public GameObject panelToAttachDynamicButtonsTo;
    public  Button[] dynamicButtons;
    public  Button[] dynamicButtons2;
    public Button[] dynamicFolderButtons;
    //public Text[] dynamicButtonsText;


    //public GameObject uihandler;
    public static string file_name = "";



    public static string[] filenames_in_the_cloud_directory = new string[32];

    public static string[] ADMINFoldernames = new string[100];


    public Sprite button_sprite  ;

    private void Start()
    {
        //if (Global.year.Length != 0 && Global.subject.Length != 0 && Global.section.Length != 0 && Global.semester.Length != 0 )
        //{

        //}
            folder_names_loading_animation.SetActive(true);

        //for (int j = 0; j < filenames_in_the_cloud_directory.Length; j++)
        //{
        //    Debug.Log("filenames are ****** : " + filenames_in_the_cloud_directory[j]);
        //    dynamicButtons2[j].gameObject.SetActive(true);



        //    //dynamicButtons2[j].GetComponentInChildren<Text>().text = "texttttttttt";
        //    //Global.extension_length = Int32.Parse(filenames_in_the_cloud_directory[j].Substring(0, 1));

        //    dynamicButtons2[j].GetComponentInChildren<Text>().text = filenames_in_the_cloud_directory[j];
        //    if (dynamicButtons2[j].GetComponentInChildren<Text>().text.Length != 0)
        //    {
        //        loading_animation.SetActive(false);


        //    }
        //}
        setADMINbuttons();

        if (Global.done_uploading==true)
        {

            UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

            pasted_message_box.body = " Uploaded successfully !";
            pasted_message_box.body = " Do you want to save file to this computer ? ";

            pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));
            pasted_message_box.buttons.Add(new UIButton("NO", close_message_box_and_delete_the_uploaded_file, "MessageButton"));

            BUI.Instance.AddToQueue(pasted_message_box);



            Global.upload_files_counter--;

            
            if(Global.upload_files_counter==0)
            {
                Global.done_uploading = false;

            }
            else
            {
                



                Global.current_uploading_file_counter++;
                upload_from_file();
                
            }
        }
        //if done_uploading is true and it was uploaded after editing then do not display dialog box
        if (Global.done_downloading == true)
        {
            UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

            pasted_message_box.body = " Downloaded successfully !";
            pasted_message_box.body = " Do you want to save the file on this computer ?";

            pasted_message_box.buttons.Add(new UIButton("YES", close_message_box, "MessageButton"));
            pasted_message_box.buttons.Add(new UIButton("NO", close_message_box_and_delete_the_downloaded_file, "MessageButton"));



            BUI.Instance.AddToQueue(pasted_message_box);
            Global.done_downloading = false;
        }
        if (Global.done_deleting==true)
        {
            UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

            pasted_message_box.body = " Deleted successfully !";
            pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

            BUI.Instance.AddToQueue(pasted_message_box);
            delete_from_real_time_database();

            Global.done_deleting = false;
        }
        if(Global.done_double_clicking == true)
        {
            Application.OpenURL("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_downloading);
            //Global.done_double_clicking = false;

        }
        //if (Global.teacher_parameters_submitted == true)
        //{
        //    Teacher_parameters_selector tp = new Teacher_parameters_selector();
        //    tp.TeacherDetailsDisplayHeader.text = Global.year + " --> " + Global.subject + " --> " + Global.section;

        //}
        //if(Global.done_pasting==true)
        //{
        //    UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

        //    pasted_message_box.body = " Pasted and Uploaded successfully !";
        //    pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

        //    BUI.Instance.AddToQueue(pasted_message_box);
        //    Global.done_pasting = false;
        //}
        begin_script();
    }
    public void begin_script()
    {
        
        foreach (Button b in dynamicButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in dynamicButtons2)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in dynamicFolderButtons)
        {
            b.gameObject.SetActive(false);
        }

    }
    // Start is called before the first frame update


    void Update()
    {



            



        //if (Global.refresh_button_clicked == true)
            for (int j = 0; j < filenames_in_the_cloud_directory.Length; j++)
            {
                Debug.Log("filenames are ****** : " + filenames_in_the_cloud_directory[j]);
                dynamicButtons2[j].gameObject.SetActive(true);
               


            //dynamicButtons2[j].GetComponentInChildren<Text>().text = "texttttttttt";
            //Global.extension_length = Int32.Parse(filenames_in_the_cloud_directory[j].Substring(0, 1));

            dynamicButtons2[j].GetComponentInChildren<Text>().text = filenames_in_the_cloud_directory[j];
            if (dynamicButtons2[j].GetComponentInChildren<Text>().text.Length!=0)
            {
                loading_animation.SetActive(false);


            }
        }

            ////setting dynamic ADMIN Folder Buttons ************************************************************* 
        for (int j = 0; j < ADMINFoldernames.Length; j++)
        {
            Debug.Log("filenames are ****** : " + ADMINFoldernames[j]);
            dynamicFolderButtons[j].gameObject.SetActive(true);


            dynamicFolderButtons[j].GetComponentInChildren<Text>().text = ADMINFoldernames[j].Substring(2, ADMINFoldernames[j].Length - 2);
            
            dynamicFolderButtons[j].name = ADMINFoldernames[j].Substring(2,ADMINFoldernames[j].Length-2);
            if (dynamicFolderButtons[j].GetComponentInChildren<Text>().text.Length != 0)
            {
                folder_names_loading_animation.SetActive(false);


                dynamicFolderButtons[j].GetComponentInChildren<Image>().sprite = button_sprite;
                ColorBlock colors = dynamicFolderButtons[j].colors;
                colors.normalColor = Color.white;
                dynamicFolderButtons[j].colors = colors;


            }
        }





        //Debug.Log("the code has reached its end");
    }

    // Update is called once per frame

    public void folder_name_setter(Button button)
    {
        //if (Global.faculty_name.Length != 0 && Global.year!="select" && Global.subject!="select" && Global.section!="select") 
        loading_animation.SetActive(true);

        foreach (Button b in dynamicButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in dynamicButtons2)
        {
            b.gameObject.SetActive(false);
        }




        //foreach (Button b in dynamicButtons2)
        //{
        //    b.gameObject.SetActive(false);
        //}


        Global.category_name = button.name;
        viewpanel.text = button.name + "";




        if(Global.faculty_name.Length!=0&& Global.year.Length!=0&& Global.subject.Length!=0 &&Global.semester.Length!=0 &&Global.section.Length!=0)

        { 

        if ((Directory.Exists("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name) == false))
        {
            Directory.CreateDirectory("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name);
        }

        DirectoryInfo dir = new DirectoryInfo("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name);
        FileInfo[] info = dir.GetFiles("*.*");
        int no_of_files = info.Length;
        string[] filenames_in_the_directory = new string[no_of_files];






        Debug.Log(Global.year);
        Debug.Log(Global.subject);
        Debug.Log(Global.section);
        Debug.Log(Global.category_name);
        Debug.Log("directory info " + dir);
        int count = 0;
        foreach (FileInfo f in info)
        {

            filenames_in_the_directory[count] = getFilenameFromUrl(f.ToString());
            if (f.Length != 0)
                count++;

        }



            //int flag_for_setting_visiblity_of_buttons=0;
           
                ///////********* setting dynamic buttons 1
                //for (int i = 0; i < filenames_in_the_directory.Length; i++)
                //{
                //    Debug.Log("filenames  in local are : " + filenames_in_the_directory[i]);
                //    dynamicButtons[i].gameObject.SetActive(true);
                //    //dynamicButtons[0].GetComponent<Text>().text = "texttttttttt";
                //    dynamicButtons[i].GetComponentInChildren<Text>().text = filenames_in_the_directory[i];
                //    //flag_for_setting_visiblity_of_buttons++;

                //}
            
        }



        //*******setting dynamic buttons2 

        
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
            string str = "Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester +  "/" + Global.section + "/" + button.name + "/";
            Debug.Log(" database url value = @@@@@@@@@@@@@@@@@@@@@@@@@" + str);
        for (int i = 0; i < filenames_in_the_cloud_directory.Length; i++)
        {
            filenames_in_the_cloud_directory[i] = null;
        }
        FirebaseDatabase.DefaultInstance.GetReference(str).GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                            // Handle the error...
                            Debug.Log(task.Exception.ToString());
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot snapshot = task.Result;
                                Debug.Log("key ****** " + snapshot.Key);
                                Debug.Log("value ******* " + snapshot.Value);

                                newScoreMap = convert_to_dictionary<string, object>(snapshot.Value);





                                for (int i = 0; i <= newScoreMap.Count; i++)
                                {
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i));

                                    if (newScoreMap.Keys.ElementAt(i) != null)
                                    {
                                        filenames_in_the_cloud_directory[0] = newScoreMap.Keys.ElementAt(i);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[0]);



                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 1));
                                    if (newScoreMap.Keys.ElementAt(i + 1) != null)
                                    {
                                        filenames_in_the_cloud_directory[1] = newScoreMap.Keys.ElementAt(i + 1);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[1]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 2));
                                    if (newScoreMap.Keys.ElementAt(i + 2) != null)
                                    {
                                        filenames_in_the_cloud_directory[2] = newScoreMap.Keys.ElementAt(i + 2);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[2]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 3));
                                    if (newScoreMap.Keys.ElementAt(i + 3) != null)
                                    {
                                        filenames_in_the_cloud_directory[3] = newScoreMap.Keys.ElementAt(i + 3);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[3]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 4));
                                    if (newScoreMap.Keys.ElementAt(i + 4) != null)
                                    {
                                        filenames_in_the_cloud_directory[4] = newScoreMap.Keys.ElementAt(i + 4);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[4]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 5));
                                    if (newScoreMap.Keys.ElementAt(i + 5) != null)
                                    {
                                        filenames_in_the_cloud_directory[5] = newScoreMap.Keys.ElementAt(i + 5);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[5]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 6));
                                    if (newScoreMap.Keys.ElementAt(i + 6) != null)
                                    {
                                        filenames_in_the_cloud_directory[6] = newScoreMap.Keys.ElementAt(i + 6);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[6]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 7));
                                    if (newScoreMap.Keys.ElementAt(i + 7) != null)
                                    {
                                        filenames_in_the_cloud_directory[7] = newScoreMap.Keys.ElementAt(i + 7);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[7]);
                                    }


                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 8));
                                    if (newScoreMap.Keys.ElementAt(i + 8) != null)
                                    {
                                        filenames_in_the_cloud_directory[8] = newScoreMap.Keys.ElementAt(i + 8);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[8]);


                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 9));
                                    if (newScoreMap.Keys.ElementAt(i + 9) != null)
                                    {
                                        filenames_in_the_cloud_directory[9] = newScoreMap.Keys.ElementAt(i + 9);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[9]);

                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 10));
                                    if (newScoreMap.Keys.ElementAt(i + 10) != null)
                                    {
                                        filenames_in_the_cloud_directory[10] = newScoreMap.Keys.ElementAt(i + 10);
                                        Debug.Log("filename in cloud = " + filenames_in_the_cloud_directory[10]);





                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 11));
                                    if (newScoreMap.Keys.ElementAt(i + 11) != null)
                                    {
                                        filenames_in_the_cloud_directory[11] = newScoreMap.Keys.ElementAt(i + 11);

                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 12));
                                    if (newScoreMap.Keys.ElementAt(i + 12) != null)
                                    {
                                        filenames_in_the_cloud_directory[12] = newScoreMap.Keys.ElementAt(i + 12);

                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 13));
                                    if (newScoreMap.Keys.ElementAt(i + 13) != null)
                                    {
                                        filenames_in_the_cloud_directory[13] = newScoreMap.Keys.ElementAt(i + 13);

                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 14));
                                    if (newScoreMap.Keys.ElementAt(i + 14) != null)
                                    {
                                        filenames_in_the_cloud_directory[14] = newScoreMap.Keys.ElementAt(i + 14);

                                    }
                                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 15));
                                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                                    {
                                        filenames_in_the_cloud_directory[15] = newScoreMap.Keys.ElementAt(i + 15);


                                    }
                                    Debug.Log("********************** value: " + (string)newScoreMap.Values.ElementAt(i));
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
                                Debug.Log("$$$$$$$$$$$$$$$$ reached the null point");
                                



                            }
                        });
        
        //else
        //{
        //    for(int i =0; i<filenames_in_the_cloud_directory.Length;i++)
        //    Debug.Log(" filenmae in cloud directory *******************************************= " + filenames_in_the_cloud_directory[i]);
        //}

        //for (int j = 0; j < filenames_in_the_cloud_directory.Length - 1; j++)
        //{
        //    Debug.Log("filenames are ****** : " + filenames_in_the_cloud_directory[j]);
        //    dynamicButtons2[j].gameObject.SetActive(true);
        //    //dynamicButtons2[j].GetComponentInChildren<Text>().text = "texttttttttt";
        //    //Global.extension_length = Int32.Parse(filenames_in_the_cloud_directory[j].Substring(0, 1));

        //    dynamicButtons2[j].GetComponentInChildren<Text>().text = filenames_in_the_cloud_directory[j];
        //}





    }
    
    public void open_file_explorer()
    {

        Global.is_file_chosen_for_uploading_from_file_explorer = true;
      
        explorer_path = FileBrowser.OpenSingleFile();   //gives the path till the file in the local system
        
        selected_file_text_box.text = explorer_path;
        Global.local_file_url = explorer_path;

        if (selected_file_text_box.text.EndsWith(".png")|| selected_file_text_box.text.EndsWith(".jpg")||selected_file_text_box.text.EndsWith(".jpeg"))
        {
            getImage();
        }
        else
        {
            
        }


    }
    public void open_folder_explorer()
    {
        explorer_path = FileBrowser.OpenSingleFolder();//gives me the url till the folder 
        selected_file_text_box.text = explorer_path;
        Global.selected_explorer_path_for_downloading_the_file = explorer_path;
        Global.is_folder_explorer_used_to_assign_path_to_download = true;



    }

    void getImage()
    {
        if (explorer_path != null)
        {
            UpdateImage();
        }

    }
    void UpdateImage()
    {
        WWW www = new WWW("file:///"+ explorer_path);
        File_viewer_area.texture = www.texture;

    }
    public void setGlobalFilenameUrlToCopy(Button b)
    {

        







        Global.filename_to_copy = b.GetComponentInChildren<Text>().text;

        Global.filename_url_to_copy=("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name+"/"+ Global.filename_to_copy);
        //Debug.Log("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + b.gameObject.GetComponentInChildren<Text>().text + " pushed to stack");
        //Debug.Log((string)copy_paste_stack.Pop() + " poped from stack");


        Global.local_file_button_selected_to_delete = true;

    }

    public void copy_or_push_to_stack()
    {
       

        //Global.copy_paste_stack.Push(Global.filename_url_to_copy);
        Debug.Log(" Pushed to stack " + Global.filename_url_to_copy);
        Debug.Log(" filname_to_copy " + Global.filename_to_copy);
        Debug.Log(" filename to download = " + Global.destination_filename_with_extension_for_downloading);
        Global.filename_to_copy = Global.destination_filename_with_extension_for_downloading;
        Global.filename_url_to_copy ="C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name + "/" + Global.filename_to_copy;
        Debug.Log(" the **********final filename url to copy is = " + Global.filename_url_to_copy);
        //Global.filename_url_to_copy= 



    }
    public void paste_or_pop_from_stack()
    {   if(Global.filename_url_to_copy.Length!=0&&Global.filename_url_to_copy.Length!=0)
        {
            UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

            pasted_message_box.body = " Pasted successfully into local storage !";
            pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

            BUI.Instance.AddToQueue(pasted_message_box);






            //string src = (string)Global.copy_paste_stack.Pop();   //unity reads only front slashes in urls
            string dest = "C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name+"/"+ Global.filename_to_copy;
        Debug.Log("copied url = " + Global.filename_url_to_copy);
        if (Directory.Exists(dest))
        {
            File.Copy(Global.filename_url_to_copy, dest, true);
        }
        else
        {
            Directory.CreateDirectory("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name);
            File.Copy(Global.filename_url_to_copy, dest, true);
        }

        //************************uploading to firebase real time database while pasting **************************
        Firebase.Sample.Database.UIHandler uihandler = new Firebase.Sample.Database.UIHandler();
        Debug.Log(" uploading to firebase database ");
        Global.extension = UIHandler.GetExtension(Global.destination_filename_with_extension_for_uploading)+Global.filename_to_copy;
        uihandler.AddScore();

            //**********************uploading to cloud storage while pasting********************************************
            //upload_from_file_while_pasting();
            //Global.paste_bool = true;
            //SceneManager.LoadScene(3);
        }
        else
        {
            UIBox pasted_message_box_warning = new UIBox("pastedID", "MessageBox");

            pasted_message_box_warning.body = " Please select file to copy ";
            pasted_message_box_warning.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

            BUI.Instance.AddToQueue(pasted_message_box_warning);

        }

    }

    void CloseBox(string id)
    {
        BUI.Instance.CloseBox(id);
    }
    void close_message_box(UIBox boxInfo,UIButton buttonInfo)
    {
        //upload the recent file to cloud again
        //upload_from_file();
        if(Global.is_folder_explorer_used_to_assign_path_to_download==true)
        {
            string src = Global.selected_explorer_path_for_downloading_the_file +"\\"+ Global.destination_filename_with_extension_for_downloading;
            string dest = "C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.semester + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading;

            File.Copy(src,dest,true);
            Global.selected_explorer_path_for_downloading_the_file = "";
            Global.is_folder_explorer_used_to_assign_path_to_download = false;       
        }
        
        CloseBox(boxInfo.id);
        //update_cloud();



        //Global.upload_bool = true;
        //SceneManager.LoadScene(3);




    }
    public static void update_cloud()
    {


    }
    void close_message_box_and_delete_the_downloaded_file(UIBox boxInfo, UIButton buttonInfo)
    {
        //upload the recent file to cloud again 
        Global.shud_I_delete_after_viewing_file = true;
        //upload_from_file();
        if (Global.done_double_clicking == true)
        {
            File.Delete("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.semester +"\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading);
            Debug.Log("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject +  "\\" + Global.semester + "\\" + Global.semester + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading + " has been deleted ");
            Global.done_double_clicking = false;
        }
        //if the file is downloaded via the open folder explorer then delete the file from the folder explorer variable
        if(Global.is_folder_explorer_used_to_assign_path_to_download == true)
        {
            //if (File.Exists("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading))
            //{
            //    File.Delete("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading);
            //    Debug.Log("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_downloading + " has been deleted ");
            //}
            //File.Delete(Global.selected_explorer_path_for_downloading_the_file+"\\"+Global.destination_filename_with_extension_for_downloading);
            //Debug.Log(Global.selected_explorer_path_for_downloading_the_file + "\\" + Global.destination_filename_with_extension_for_downloading+" has been deleted");
            Global.destination_filename_with_extension_for_downloading = "";
            Global.selected_explorer_path_for_downloading_the_file = "";
            Global.is_file_chosen_for_uploading_from_file_explorer = false;
        }
        CloseBox(boxInfo.id);
        //update_cloud();
        //upload_from_file();


        //Global.upload_bool = true;
        //SceneManager.LoadScene(3);


    }
    void close_message_box_and_delete_the_uploaded_file(UIBox boxInfo, UIButton buttonInfo)
    {
        File.Delete("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.semester + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_uploading);
        Debug.Log("C:" + "\\" + "FacultyDocs" + "\\" + Global.faculty_name + "\\" + Global.year + "\\" + Global.subject + "\\" + Global.semester + "\\" + Global.section + "\\" + Global.category_name + "\\" + Global.destination_filename_with_extension_for_uploading + " has been deleted ");
        CloseBox(boxInfo.id);

    }

    public void upload_from_file()
    {

        if (Global.list_of_sections_to_upload_to.Length == 0)
        {
            Debug.Log(" list of sections is empty");
        }
        //LOCKDOWN progress get the list of sections to upload to from global 
       
            //string tempsections = Global.list_of_sections_to_upload_to[j];
            //Debug.Log(" ############################################################# uploading for section " + tempsections);
            
           
            Teacher_parameters_selector obj = new Teacher_parameters_selector();
            //if (obj.TeacherDetailsDisplayHeader.GetComponent<Text>().color != Color.red)
            ////if (Teacher_parameters_selector.TeacherDetailsDisplayHeader.color != Color.red)
            ////{
            //{
            if (Global.submit_button_clicked == true && Global.category_name.Length != 0)
            {
                Global.destination_filename_with_extension_for_downloading = "";

                //upload to local system also *************************************

                string explorer_path_after_downloading = "C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.list_of_sections_to_upload_to[Global.current_uploading_file_counter] + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_downloading;
                //if(file is selected from file explorer
                if (Global.is_file_chosen_for_uploading_from_file_explorer == true)
                {
                    UIHandler.localFilename = explorer_path;
                    Global.local_file_url = explorer_path;
                    Global.is_file_chosen_for_uploading_from_file_explorer = false;
                }
                //if(file is downloaded and then shud be uploaded
                if (Global.is_file_chosen_for_uploading_after_downloading == true)
                {
                    UIHandler.localFilename = explorer_path_after_downloading;
                    Global.local_file_url = explorer_path_after_downloading;

                    Global.is_file_chosen_for_uploading_after_downloading = false;
                }


                if (Directory.Exists("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" +Global.list_of_sections_to_upload_to[Global.current_uploading_file_counter] + "/" + Global.category_name))
                {
                    for (int i = (Global.local_file_url.Length - 1); i >= 0; i--)
                    {

                        if (Global.local_file_url[i] == '\\')
                        {

                            file_name = Global.local_file_url.Substring(i + 1);
                            Debug.Log(" filename extracted when double clicked is " + file_name);
                            break;

                        }
                    }

                }
                if (Directory.Exists("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.list_of_sections_to_upload_to[Global.current_uploading_file_counter] + "/" + Global.category_name))
                {
                    for (int i = (Global.local_file_url.Length - 1); i >= 0; i--)
                    {

                        if (Global.local_file_url[i] == '\\')
                        {

                            file_name = Global.local_file_url.Substring(i + 1);
                            break;

                        }

                    }

                }
                //string explorer_path_after_downloading = "C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading+ Global.destination_filename_with_extension_for_downloading; ;
                ////if(file is selected from file explorer
                //if (Global.is_file_chosen_for_uploading_from_file_explorer == true)
                //{
                //    UIHandler.localFilename = explorer_path;
                //    Global.local_file_url = explorer_path;
                //    Global.is_file_chosen_for_uploading_from_file_explorer = false;
                //}
                ////if(file is downloaded and then shud be uploaded
                //if (Global.is_file_chosen_for_uploading_after_downloading == true)
                //{
                //    UIHandler.localFilename = explorer_path_after_downloading;
                //    Global.local_file_url = explorer_path_after_downloading;

                //    Global.is_file_chosen_for_uploading_after_downloading = false;
                //}

                string dest_file_name = "";

                for (int i = (UIHandler.localFilename.Length - 1); i >= 0; i--)
                {

                    if (UIHandler.localFilename[i] == '\\')
                    {

                        dest_file_name = UIHandler.localFilename.Substring(i + 1);
                        break;

                    }
                }

                Global.destination_filename_with_extension_for_uploading = dest_file_name;
                Debug.Log(dest_file_name);
                UIHandler.upload_bool = true;




                //UIHandler uIHandler = new UIHandler();
                //uIHandler.Start();

                //StartCoroutine(uIHandler.UploadFromFile());
                if (Global.destination_filename_with_extension_for_uploading.Length != 0)
                {
                    Global.upload_bool = true;

                    SceneManager.LoadSceneAsync(3);
                }

                //}
                //}
            
        }

    }

    public void upload_from_file_while_pasting()
    {
        Global.destination_filename_with_extension_for_downloading = "";
        Global.local_file_url = Global.filename_url_to_copy;

        //upload to local system also *************************************


        if (Directory.Exists("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name))
        {
            for (int i = (Global.local_file_url.Length - 1); i >= 0; i--)
            {

                if (Global.local_file_url[i] == '\\')
                {

                    file_name = Global.local_file_url.Substring(i + 1);
                    break;

                }
            }

        }

        UIHandler.localFilename = Global.filename_url_to_copy;

        string dest_file_name = "";

        for (int i = (Global.filename_url_to_copy.Length - 1); i >= 0; i--)
        {

            if (Global.filename_url_to_copy[i] == '\\')
            {

                dest_file_name = Global.filename_url_to_copy.Substring(i + 1);
                break;

            }
        }

        Global.destination_filename_with_extension_for_uploading = dest_file_name;
        Debug.Log("uploading while pasting filename = "+dest_file_name);
        UIHandler.upload_bool = true;




        //UIHandler uIHandler = new UIHandler();
        //uIHandler.Start();

        //StartCoroutine(uIHandler.UploadFromFile());

        Global.upload_bool = true;

        SceneManager.LoadScene(3);

    }


    public void download_to_file()
    {
        if (Global.submit_button_clicked == true && Global.category_name.Length != 0)
        {
            if (Global.destination_filename_with_extension_for_downloading.Length != 0)
            {
                Debug.Log("explorer path = " + explorer_path);
                explorer_path = explorer_path + "\\" + Global.destination_filename_with_extension_for_downloading;
                UIHandler.localFilename = explorer_path;
                Global.destination_filename_with_extension_for_uploading = "";
                Debug.Log(UIHandler.localFilename);

                Global.download_bool = true;
                SceneManager.LoadScene(3);

               

            }
        }
            







        

    }

    public void Delete()
    {
        if (Global.submit_button_clicked == true && Global.category_name.Length != 0)
        {
            if (Global.local_file_button_selected_to_delete == true)
            {
                File.Delete("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name + "/" + Global.filename_to_copy);
                Global.local_file_button_selected_to_delete = false;

                UIBox pasted_message_box = new UIBox("pastedID", "MessageBox");

                pasted_message_box.body = " Deleted Successfully!";
                pasted_message_box.buttons.Add(new UIButton("OK", close_message_box, "MessageButton"));

                BUI.Instance.AddToQueue(pasted_message_box);
                Global.local_file_button_selected_to_delete = false;

            }
            else
            {


                Global.destination_filename_with_extension_for_uploading = "";
                Global.delete_bool = true;
                SceneManager.LoadScene(3);
            }
        }

    }



   
    public void go_back_scene()
    {
        SceneManager.LoadScene(1);
    }

   

    public string getFilenameFromUrl(string url)
    {
        string filename="";
        for (int i = (url.Length - 1); i >= 0; i--)
        {

            if (url[i] == '\\')
            {

                filename = url.Substring(i + 1);
                break;

            }
        }
        return filename;

    }
    
    public void getFilenameToDownload(Button b)
    {

        string download_filename_without_dot = b.GetComponentInChildren<Text>().text;
        Global.filename_to_delete = b.GetComponentInChildren<Text>().text; 
        //for(int i= download_filename_without_dot.Length-1; i>download_filename_without_dot.Length-3;i--)
        //{
        //    string extension = download_filename_without_dot.Substring(i);
        //    string filename = download_filename_without_dot.Substring(0,)


        //}
        int extension_length =Int32.Parse(download_filename_without_dot.Substring(download_filename_without_dot.Length-1));
        //if(Global.extension_length!=3)
        //{
        //    extension_length = Global.extension_length;
        //}
        //else
        //{
        //    extension_length = 3;//default extention length for files
        //}
        //use comments everywhere it is essential to coding in the professinal way
        int indexPos = download_filename_without_dot.Length - (extension_length+1);
        //Debug.Log("0000000000000000000000000" + Global.extension_length);
        string filename_with_dot = download_filename_without_dot.Insert(indexPos, ".");
        string filename_with_dot_and_without_extension_length = filename_with_dot.Substring(0, filename_with_dot.Length - 1);
        //string filename_with_dot_and_without_extension_length = filename_with_dot.Substring(0);
        Global.destination_filename_with_extension_for_downloading = filename_with_dot_and_without_extension_length;
        Debug.Log("filename to download = "+ Global.destination_filename_with_extension_for_downloading);//final filename with all formatting done 
        

    }
    public static Dictionary<string, object> convert_to_dictionary<TKey, TValue>(object obj)
    {
        if (obj is Dictionary<string, object> stringDictionary)
        {
            return stringDictionary;
        }

       
        return null;
    }

    public static void delete_from_real_time_database()
    {
        //DatabaseReference dbNode = FirebaseDatabase.GetInstance().getReference().getRoot().child("Node");
        string str = "Faculties/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.semester + "/" + Global.section + "/" + Global.category_name + "/" + Global.filename_to_delete;

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(str); //+ Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.destination_filename_with_extension_for_uploading);
        reference.RemoveValueAsync();

    }

    public static void setADMINbuttons()
    {

        for (int i = 0; i < ADMINFoldernames.Length; i++)
        {
            ADMINFoldernames[i] = null;
        }
        //*******setting ADMINFolderbuttons  


        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        string str = "ADMIN/" + "Foldernames" + "/";
        Debug.Log(" database url value = @@@@@@@@@@@@@@@@@@@@@@@@@" + str);
        for (int i = 0; i < ADMINFoldernames.Length; i++)
        {
            ADMINFoldernames[i] = null;
        }
        FirebaseDatabase.DefaultInstance.GetReference(str).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("key ****** " + snapshot.Key);
                Debug.Log("value ******* " + snapshot.Value);

                newScoreMap = convert_to_dictionary<string, object>(snapshot.Value);





                for (int i = 0; i <= newScoreMap.Count; i++)
                {
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i));

                    if (newScoreMap.Keys.ElementAt(i) != null)
                    {
                        ADMINFoldernames[0] = newScoreMap.Keys.ElementAt(i);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[0]);



                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 1));
                    if (newScoreMap.Keys.ElementAt(i + 1) != null)
                    {
                        ADMINFoldernames[1] = newScoreMap.Keys.ElementAt(i + 1);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[1]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 2));
                    if (newScoreMap.Keys.ElementAt(i + 2) != null)
                    {
                        ADMINFoldernames[2] = newScoreMap.Keys.ElementAt(i + 2);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[2]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 3));
                    if (newScoreMap.Keys.ElementAt(i + 3) != null)
                    {
                        ADMINFoldernames[3] = newScoreMap.Keys.ElementAt(i + 3);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[3]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 4));
                    if (newScoreMap.Keys.ElementAt(i + 4) != null)
                    {
                        ADMINFoldernames[4] = newScoreMap.Keys.ElementAt(i + 4);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[4]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 5));
                    if (newScoreMap.Keys.ElementAt(i + 5) != null)
                    {
                        ADMINFoldernames[5] = newScoreMap.Keys.ElementAt(i + 5);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[5]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 6));
                    if (newScoreMap.Keys.ElementAt(i + 6) != null)
                    {
                        ADMINFoldernames[6] = newScoreMap.Keys.ElementAt(i + 6);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[6]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 7));
                    if (newScoreMap.Keys.ElementAt(i + 7) != null)
                    {
                        ADMINFoldernames[7] = newScoreMap.Keys.ElementAt(i + 7);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[7]);
                    }


                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 8));
                    if (newScoreMap.Keys.ElementAt(i + 8) != null)
                    {
                        ADMINFoldernames[8] = newScoreMap.Keys.ElementAt(i + 8);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[8]);


                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 9));
                    if (newScoreMap.Keys.ElementAt(i + 9) != null)
                    {
                        ADMINFoldernames[9] = newScoreMap.Keys.ElementAt(i + 9);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[9]);

                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 10));
                    if (newScoreMap.Keys.ElementAt(i + 10) != null)
                    {
                        ADMINFoldernames[10] = newScoreMap.Keys.ElementAt(i + 10);
                        Debug.Log("filename in cloud = " + ADMINFoldernames[10]);





                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 11));
                    if (newScoreMap.Keys.ElementAt(i + 11) != null)
                    {
                        ADMINFoldernames[11] = newScoreMap.Keys.ElementAt(i + 11);

                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 12));
                    if (newScoreMap.Keys.ElementAt(i + 12) != null)
                    {
                        ADMINFoldernames[12] = newScoreMap.Keys.ElementAt(i + 12);

                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 13));
                    if (newScoreMap.Keys.ElementAt(i + 13) != null)
                    {
                        ADMINFoldernames[13] = newScoreMap.Keys.ElementAt(i + 13);

                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 14));
                    if (newScoreMap.Keys.ElementAt(i + 14) != null)
                    {
                        ADMINFoldernames[14] = newScoreMap.Keys.ElementAt(i + 14);

                    }
                    Debug.Log("********************** key : " + newScoreMap.Keys.ElementAt(i + 15));
                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                    {
                        ADMINFoldernames[15] = newScoreMap.Keys.ElementAt(i + 15);


                    }
                    
                    if (newScoreMap.Keys.ElementAt(i + 16) != null)
                    {
                        ADMINFoldernames[16] = newScoreMap.Keys.ElementAt(i + 16);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 17) != null)
                    {
                        ADMINFoldernames[17] = newScoreMap.Keys.ElementAt(i + 17);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 18) != null)
                    {
                        ADMINFoldernames[18] = newScoreMap.Keys.ElementAt(i + 18);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 19) != null)
                    {
                        ADMINFoldernames[19] = newScoreMap.Keys.ElementAt(i + 19);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 20) != null)
                    {
                        ADMINFoldernames[20] = newScoreMap.Keys.ElementAt(i + 20);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 21) != null)
                    {
                        ADMINFoldernames[21] = newScoreMap.Keys.ElementAt(i + 21);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 22) != null)
                    {
                        ADMINFoldernames[22] = newScoreMap.Keys.ElementAt(i + 22);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 15) != null)
                    {
                        ADMINFoldernames[23] = newScoreMap.Keys.ElementAt(i + 23);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 24) != null)
                    {
                        ADMINFoldernames[24] = newScoreMap.Keys.ElementAt(i + 24);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 25) != null)
                    {
                        ADMINFoldernames[25] = newScoreMap.Keys.ElementAt(i + 25);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 26) != null)
                    {
                        ADMINFoldernames[26] = newScoreMap.Keys.ElementAt(i + 26);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 27) != null)
                    {
                        ADMINFoldernames[27] = newScoreMap.Keys.ElementAt(i + 27);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 28) != null)
                    {
                        ADMINFoldernames[28] = newScoreMap.Keys.ElementAt(i + 28);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 29) != null)
                    {
                        ADMINFoldernames[29] = newScoreMap.Keys.ElementAt(i + 29);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 30) != null)
                    {
                        ADMINFoldernames[30] = newScoreMap.Keys.ElementAt(i + 30);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 31) != null)
                    {
                        ADMINFoldernames[31] = newScoreMap.Keys.ElementAt(i + 31);


                    }
                    if (newScoreMap.Keys.ElementAt(i + 32) != null)
                    {
                        ADMINFoldernames[32] = newScoreMap.Keys.ElementAt(i + 32);


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
                Debug.Log("$$$$$$$$$$$$$$$$ reached the null point");




            }
        });

    }

    public void openSubjectCodesfile()
    {
        Debug.Log(" opening subject codes file ....");
        Application.OpenURL(Application.streamingAssetsPath+"/SubjectCodes.pdf");
    }



}
