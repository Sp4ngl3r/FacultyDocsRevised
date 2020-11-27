using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Firebase.Sample.Auth;
using System.Threading.Tasks;

public class Teacher_parameters_selector : MonoBehaviour
{

    public Dropdown choose_year;
    //public Dropdown choose_subject;
    public Dropdown choose_section;
    public Dropdown choose_semester;


    public Text chosen_year;
    //public Text chosen_subject;
    public Text chosen_section;
    public Text chosen_semester;

    List<string> year_values = new List<string>() {"select", "2017-18 (EVEN)", "2017-18 (ODD)", "2018-19 (EVEN)", "2018-19 (ODD)", "2019-20 (EVEN)", "2019-20 (ODD)", "2020-21 (EVEN)", "2020-21 (ODD)", "2021-22 (EVEN)", "2021-22 (ODD)" };
    //List<string> subject_values = new List<string>() { "select", "4-Database Management Systems", "4-Linux System Programming", "4-Automata Theory and Formal Languages", "4-Operating System", "4-Microprocessors and Microcontrollers", "4-Design and Analysis of Algorithms", "4-Engineering Mathematics", "5-Artificial Intelligence", "5-Software Enginnerin", "5-Advanced Java","5-Management And Entrepreneurship", "5-Computer Networks", "5-Database Manangement System", "5-Emerging Technologies" };
    List<string> section_values = new List<string>() { "select", "A", "B", "C","D","E" };

    List<string> semester_values = new List<string>() { "select", "1", "2", "3", "4", "5", "6", "7", "8" };




    public static string year ="";
    //public static string subject="";
    public static string section="";
    public static string semester = "";
    public Text subjectcode;
    

    

    public Text faculty_name_label;

    public Text TeacherDetailsDisplayHeader;

    public Text list_of_sections_to_upload_to;








    // Start is called before the first frame update
    void Start()
    {
        //choose_year.GetComponent<Dropdown>().itemText.text = Global.year;
        
        PopulateList();
        chosen_year.text = "";
        Debug.Log(" faculty name = " + Global.faculty_name);
        
        if (Global.signedin==true)
            faculty_name_label.text = "Welcome Prof. "+ Global.faculty_name;
        else
            faculty_name_label.text = "Welcome , Please Sign IN!";

        categories_management cat = new categories_management();
        cat.begin_script();



    }

    // Update is called once per frame
   
    public void PopulateList()
    {
        if (Global.year != "" && Global.subject != "" && Global.section != "" && Global.semester!="")
        {
            //TeacherDetailsDisplayHeader.GetComponent<Text>().color = Color.red;
            TeacherDetailsDisplayHeader.text = Global.year + " --> " + Global.subject + " --> " + Global.semester + " --> "+Global.section;
            
        }
       

        choose_year.AddOptions(year_values);
        //choose_subject.AddOptions(subject_values);
        choose_section.AddOptions(section_values);
        choose_semester.AddOptions(semester_values);
    }

    public void submit_teacher_parameters()
    {
        Debug.Log(year);
        //Debug.Log(subject);
        Debug.Log(section);
        Debug.Log(semester);
      
        //if(year.Equals(null))
        //{
        //    Debug.Log("please select all fields");
        //}
        //if(subject.Equals(null))
        //{
        //    Debug.Log("please select all fields");

        //}
        //if (section.Equals(null))
        //{
        //    Debug.Log("please select all fields");

        //}
        //Global.year = year;
        Global.subject = subjectcode.text.ToString() ;


        //LOCKDOWN Progress
        
        string[] sections = list_of_sections_to_upload_to.text.ToString().Split(',');
        for(int i =0;i<sections.Length;i++)
        {
            string tempstr = sections[i].ToUpper();
            sections[i] = tempstr;
        }
        Global.list_of_sections_to_upload_to = sections;
        Global.upload_files_counter = Global.list_of_sections_to_upload_to.Length;

        //for(int i =0;i<Global.list_of_sections_to_upload_to.Length;i++)
        //    {
        //    Debug.Log("555555555555" + Global.list_of_sections_to_upload_to[i]);

        //}
        


        //Global.section = section;
        //Global.semester = semester;
        Debug.Log(" semester chosen is = " + Global.semester);
        //TeacherDetailsDisplayHeader.text = Global.year + " --> " + Global.subject + " --> " + Global.semester + " --> " + Global.section;

        if (year.Length!=0 && subjectcode.text.ToString().Length!=0 && list_of_sections_to_upload_to.text.ToString().Length !=0&&semester.Length!=0 &&year!="select" && semester != "select")
        {

            //TeacherDetailsDisplayHeader.GetComponent<Text>().color = Color.white;
            Global.submit_button_clicked = true;
            Debug.Log("trigerred ))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))");
            //Debug.Log(" refersh button clicked bool = " + Global.refresh_button_clicked);
            TeacherDetailsDisplayHeader.text = Global.year + " --> " + Global.subject + " --> " + Global.semester + " --> " + Global.section;
            Global.teacher_parameters_submitted = true;

            //categories_management cat = new categories_management();
            //cat.begin_script();
            //SceneManager.LoadScene(2);
        }
        else
        {
            TeacherDetailsDisplayHeader.text = " Please select year, subjectcode , semester, section";
            //TeacherDetailsDisplayHeader.GetComponent<Text>().color = Color.red;
            Global.submit_button_clicked = false;
        }
        
        

    }
    public void year_index_changed(int index)
    {
        chosen_year.text = year_values[index];
        //chosen_subject.text = subject_values[index];
        //chosen_section.text = section_values[index];
        Global.year_index = index;
        year = chosen_year.text;
        Global.year = year;
        //subject = chosen_subject.text;
        //section = chosen_section.text;
    }
    public void subject_index_changed(int index)
    {
        //chosen_year.text = year_values[index];
        //chosen_subject.text = subject_values[index];
        //chosen_section.text = section_values[index];

        //year = chosen_year.text;
        //subject = chosen_subject.text;
        //section = chosen_section.text;
    }
    public void section_index_changed(int index)
    {
        //chosen_year.text = year_values[index];
        //chosen_subject.text = subject_values[index];
        chosen_section.text = section_values[index];
        Global.section_index = index;
        //year = chosen_year.text;
        //subject = chosen_subject.text;
        section = chosen_section.text;
        Global.section = section;
    }
    public void semester_index_changed(int index)
    {
        //chosen_year.text = year_values[index];
        //chosen_subject.text = subject_values[index];
        chosen_semester.text = semester_values[index];
        Global.semester_index = index;

        //year = chosen_year.text;
        //subject = chosen_subject.text;
        semester = chosen_semester.text;
        Global.semester = semester;
    }
    //public void pop_test()
    //{
    //    Debug.Log((string)Global.copy_paste_stack.Pop());

    //}
    public void go_back_scene()
    {
        //Global.signedin = false;
        //UIHandler ing = new UIHandler();
        //ing.Start();
        //ing.SignOut();

        //UIHandler.auth.SignOut();

        Global.clearGlobal();
        SceneManager.LoadScene(1);

    }

    



}
