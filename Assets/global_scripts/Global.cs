using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public static Stack copy_paste_stack = new Stack();
    public static string firebase_storage_bucket_helper;// use these variables to concatenate the folder names as and when the user clicks on the categories EG:if user clicks ethics button then the firebase storage bucketname should be concatenated with the folder name as ethics

    //public static string default_faculty_name = " Please Sign IN ! ";
    public static string faculty_name="";
    public static string destination_filename_with_extension_for_uploading="";
    public static string destination_filename_with_extension_for_downloading="";
    public static string local_file_url="";
    public static string year="";
    public static string subject = "";
    public static string semester = "";
    //public static string subjectcode = "";
    public static string section = "";
    public static string category_name = "";
    public static string extension = "";
    public static string filename_url_to_copy = "";
    public static string filename_to_copy = "";
    public static bool upload_bool = false;
    public static bool download_bool = false;
    public static bool delete_bool = false;
    //public static bool paste_bool = false;
    public static bool submit_button_clicked = false;
    public static bool done_uploading = false;
    public static bool done_downloading = false;
    public static bool done_deleting = false;
    //public static bool done_pasting = false;
    public static string filename_to_delete = "";
    public static bool done_double_clicking = false;
    public static bool teacher_parameters_submitted = false;
    public static bool is_file_chosen_for_uploading_from_file_explorer = false;
    public static bool is_file_chosen_for_uploading_after_downloading = false;
    public static bool shud_I_delete_after_viewing_file = false;
    public static bool is_folder_explorer_used_to_assign_path_to_download = false;
    public static string selected_explorer_path_for_downloading_the_file = "";
    public static bool local_file_button_selected_to_delete = false;
    public static bool toggle = false;

    //public static int extension_length = 3;


    public static bool signedin=false;
    public static string email = "";


    public static int year_index = 0;
    public static int semester_index = 0;
    public static int section_index = 0;


    //LOCKDOWN progress
    public static string[] list_of_file_names_to_upload;
    public static string[] list_of_sections_to_upload_to;
    public static string[] list_of_semesters_to_upload_to;
    public static string[] list_of_years_to_upload_to;
    public static int upload_files_counter=0;  //total number of sections and to stop uploading once all sections have been uploaded
    public static int current_uploading_file_counter = 0; // current number of sections uploaded nad iterate through the sections one by one while uploading 



    public static void clearGlobal()
    {
        copy_paste_stack = new Stack();
// use these variables to concatenate the folder names as and when the user clicks on the categories EG:if user clicks ethics button then the firebase storage bucketname should be concatenated with the folder name as ethics

    //public static string default_faculty_name = " Please Sign IN ! ";
     faculty_name = "";
     destination_filename_with_extension_for_uploading = "";
     destination_filename_with_extension_for_downloading = "";
     local_file_url = "";
     year = "";
     subject = "";
     semester = "";
    //public static string subjectcode = "";
    section = "";
    category_name = "";
    extension = "";
    filename_url_to_copy = "";
    filename_to_copy = "";
    upload_bool = false;
    download_bool = false;
    delete_bool = false;
    //public static bool paste_bool = false;
    submit_button_clicked = false;
    done_uploading = false;
    done_downloading = false;
    done_deleting = false;
    //public static bool done_pasting = false;
    filename_to_delete = "";
    done_double_clicking = false;
    teacher_parameters_submitted = false;
    is_file_chosen_for_uploading_from_file_explorer = false;
    is_file_chosen_for_uploading_after_downloading = false;
    shud_I_delete_after_viewing_file = false;
    is_folder_explorer_used_to_assign_path_to_download = false;
    selected_explorer_path_for_downloading_the_file = "";
    local_file_button_selected_to_delete = false;
    toggle = false;

    //public static int extension_length = 3;


    signedin = false;
    email = "";


    year_index = 0;
    semester_index = 0;
    section_index = 0;


}



}
