using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class double_click_registerer_for_local_files : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;

        if (clickCount == 1)
            OnSingleClick();
        else if (clickCount == 2)
            OnDoubleClick();
        else if (clickCount > 2)
            OnMultiClick();
    }

    void OnSingleClick()
    {

        //Debug.Log("Single Clicked");
    }

    void OnDoubleClick()
    {
        Application.OpenURL("C:/FacultyDocs" + "/" + Global.faculty_name + "/" + Global.year + "/" + Global.subject + "/" + Global.section + "/" + Global.category_name + "/" + Global.filename_to_copy);

        ////Debug.Log("Double Clicked");
        //Global.is_file_chosen_for_uploading_after_downloading = true;
        //categories_management cm = new categories_management();
        //cm.download_to_file();
        //Global.done_double_clicking = true;



    }

    void OnMultiClick()
    {
        //Debug.Log("MultiClick Clicked");
    }
}
