using Firebase.Sample.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faculty_Details : MonoBehaviour
{
    public static string faculty_name = "";// GetFacultyDetails.faculty_name_value.ToString();
    public static string destination_filename_with_extension_for_uploading = "";// GetFacultyDetails.destination_filename_value.ToString();
    public static string local_file_url = "";// GetFacultyDetails.local_filename_value.ToString();
    public static string storage_bucket = "";
    public static string folder_name = "";


    public static void getDetails()
    {

        faculty_name = GetFacultyDetails.faculty_name_value.ToString();
        //destination_filename_with_extension_for_uploading = GetFacultyDetails.destination_filename_value.ToString();
        //local_file_url = GetFacultyDetails.local_filename_value.ToString();
        storage_bucket = "";
        //folder_name = GetFacultyDetails.folder_name_value.ToString();

    }
    public void Start()
    {
        if (faculty_name.Equals(""))
        {
            //UIHandler.fac_name = "default_dump";
            //faculty_name = "defaullt_dump";
        }
        else
        {
            //UIHandler.fac_name = faculty_name;
        }
        if (destination_filename_with_extension_for_uploading.Equals(""))
        {
            //UIHandler.destination_filename_with_extension_for_uploading = "last_uploaded_file.txt";
            //destination_filename_with_extension_for_uploading = "last_uploaded_file.txt";
        }
        else
        {
            //UIHandler.destination_filename_with_extension_for_uploading = destination_filename_with_extension_for_uploading;

        }
        if (local_file_url.Equals(""))
        {
            //UIHandler.local_file_url = "D:/manoj.jpg";
            //local_file_url = "D:/manoj.jpg";
            //Debug.Log(" enter a valid local file url / file does not exist");
        }
        else
        {
            //UIHandler.local_file_url = local_file_url;
        }
        if (storage_bucket.Equals(""))
        {
            storage_bucket = "gs://facultydocs-1b5e4.appspot.com/";
            UIHandler.storage_bucket = "gs://facultydocs-1b5e4.appspot.com/";
        }

        else
        {
            UIHandler.storage_bucket = storage_bucket;
        }

    }

    

    
    
}
