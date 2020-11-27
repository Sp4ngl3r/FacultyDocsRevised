using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class double_click_registerer : MonoBehaviour, IPointerClickHandler
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
        //Debug.Log("Double Clicked");
        Global.is_file_chosen_for_uploading_after_downloading = true;
        categories_management cm = new categories_management();
        cm.download_to_file();
        Global.done_double_clicking = true;
        


    }

    void OnMultiClick()
    {
        //Debug.Log("MultiClick Clicked");
    }
}
