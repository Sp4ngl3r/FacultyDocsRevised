using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Sample.Storage;


public class GetFacultyDetails : MonoBehaviour
{

    public static  string faculty_name_value;
    public static string local_filename_value;
    public static string folder_name_value;
    public static string destination_filename_value;
    public Text faculty_name;
    public Text local_filename;
    public Text folder_name;//NOTE folder name is replaced by category name
    public Text destination_filename;

    // Start is called before the first frame update
    void Start()




    {
        


        //SceneManager.LoadScene(0);







    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void load_scene()
    {
        faculty_name_value = faculty_name.text.ToString();
        //local_filename_value = local_filename.text.ToString();
        //destination_filename_value = destination_filename.text.ToString();
        //folder_name_value = folder_name.text.ToString();
        Faculty_Details.getDetails();
        //UIHandler.fac_name = faculty_name.text.ToString();
        //UIHandler.local_file_url = local_filename.text.ToString();
        //folder_name_value = folder_name.text.ToString();
        //UIHandler.destination_filename_with_extension = destination_filename.text.ToString();
        
        Global.faculty_name = Firebase.Sample.Auth.UIHandler.email;
        //Global.local_file_url = local_filename_value;
        //Global.destination_filename_with_extension_for_uploading = destination_filename_value;
        
        SceneManager.LoadScene(1);
    }
}
