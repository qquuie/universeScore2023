using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class DialogTest : MonoBehaviour
{
    public Text FilaName;
    [HideInInspector]
    public string win_file = "", win_fileTitle = "";

    public void Refresh()
    {
        FilaName.text = win_fileTitle;
    }

    public void WinCall() //呼叫開啟一個檔案選擇的頁面
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "MP4(*.mp4;*.m4v)\0*.mp4;*.m4v\0MOV(*.mov;*.qt)\0*.mov;*.qt\0\0";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默認路逕
        openFileName.title = "選擇播放影片";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetSaveFileName(openFileName))
        {
            Debug.Log(openFileName.file);
            Debug.Log(openFileName.fileTitle);
            FilaName.text = openFileName.fileTitle;
            win_file = openFileName.file;
            win_fileTitle = openFileName.fileTitle;
        }
    }
}