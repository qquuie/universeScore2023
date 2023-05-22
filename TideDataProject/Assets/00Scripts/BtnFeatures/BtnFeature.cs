using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnFeature : MonoBehaviour
{
    public bool isClickClass,isClickName,isClickTime = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    //班級按鈕
    public void ClickClass()
    {
        isClickClass = true;
    }
    //姓名按鈕
    public void ClickName()
    {
        isClickName = true;
    }
    //時間按鈕
    public void ClickTime()
    {
        isClickTime = true;
    }
}
