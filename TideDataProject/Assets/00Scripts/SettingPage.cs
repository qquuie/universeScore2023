using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
//呼叫本機端檔案
using System.Runtime.InteropServices;


public class SettingPage : MonoBehaviour
{
    public Menu menu;
    public MainData MainPage;
    public bool isClickOnce;
    [Header("按鈕")]
    public Button SetClassButton;
    public Button SetScoreButton, SetEnterButton,Add_BackButton,Add_UpdateButton,Score_BackButton,Score_SaveButton, Score_SaveConButton, Score_CancelConButton;
    [Header("頁面")]
    public GameObject DataSettingPage;
    public GameObject AddListPage, ScorePage, ScoreCantBlackPage, ScoreSaveConPage, LoadingPage;
    [Header("儲存班級名冊")]
    private string Local_FileName, LocalFilepath;
    public ScrollSnapRect ScrollPic;
    public string[] NameData;
    [Header("編輯配分")]
    private string[] DownloadScores;
    private string ScoreData;
    public Toggle[] QuizToggles = new Toggle[6];
    public GameObject[] QuizPages = new GameObject[6];
    public Transform[] ScoreInputPlaces = new Transform[6];
    public List<InputField> ScoreInputs;
    // Start is called before the first frame update
    void Start()
    {
        //Setting
        SetClassButton.onClick.AddListener(SetClassBtn);
        SetScoreButton.onClick.AddListener(SetScoreBtn);
        SetEnterButton.onClick.AddListener(SetEnterBtn);
        //AddList
        Add_BackButton.onClick.AddListener(Add_BackBtn);
        Add_UpdateButton.onClick.AddListener(Add_UpdateBtn);
        //Score
        Score_BackButton.onClick.AddListener(Score_BackBtn);
        Score_SaveButton.onClick.AddListener(Score_SaveBtn);
        Score_SaveConButton.onClick.AddListener(Score_SaveConBtn);
        Score_CancelConButton.onClick.AddListener(Score_CancelConBtn);
        ScoreInputs.Clear();
        for (int i = 0; i < ScoreInputPlaces.Length; i++)
        {
            for (int j = 0; j < ScoreInputPlaces[i].childCount; j++)
            {
                ScoreInputs.Add(ScoreInputPlaces[i].GetChild(j).GetChild(1).GetComponent<InputField>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Setting
    //班級名冊
    public void SetClassBtn()
    {
        DataSettingPage.SetActive(false);
        ScrollPic.StartSet();
        AddListPage.SetActive(true);
    }
    #region 設定配分
    public void SetScoreBtn()
    {
        LoadingPage.SetActive(true);
        StartCoroutine(ReadScoreData());
    }
    public void ResetScores()
    {
        for (int i = 0; i < ScoreInputs.Count; i++)
        {
            ScoreInputs[i].text = DownloadScores[i];
        }
        QuizToggles[0].isOn = true;
        for (int i = 0; i < QuizPages.Length; i++)
        {
            if (i == 0)
            {
                QuizPages[i].SetActive(true);
            }
            else
            {
                QuizPages[i].SetActive(false);
            }
        }
        DataSettingPage.SetActive(false);
        ScorePage.SetActive(true);
        LoadingPage.SetActive(false);
    }
    #endregion
    //進入學生資料頁面
    public void SetEnterBtn()
    {
        DataSettingPage.SetActive(false);
        LoadingPage.SetActive(true);
        //CALL設定主頁面顯示的資料
        MainPage.SetClassBtnPlace();
    }
    #endregion

    #region AddList
    public void Add_BackBtn()
    {
        DataSettingPage.SetActive(true);
        AddListPage.SetActive(false);
    }

    public void Add_UpdateBtn()
    {
        //呼叫本機端檔案
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "文字檔案(*.txt)\0*.txt\0";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.dataPath.Replace('/', '\\');//默認路逕
        openFileName.title = "選擇班級名冊";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetSaveFileName(openFileName))
        {
            LoadingPage.SetActive(true);
            Local_FileName = openFileName.fileTitle;
            LocalFilepath = openFileName.file;
            //接下來寫資料到EXCEL
            NameData = File.ReadAllLines(LocalFilepath);
            StartCoroutine(WriteToSheet_Class());
        }
    }
    #endregion

    #region Score
    //切換頁面
    public void QuizToggleEvent()
    {
        for (int i = 0; i < QuizPages.Length; i++)
        {
            QuizPages[i].SetActive(false);
        }
        
        for (int i = 0; i < QuizToggles.Length; i++)
        {
            if (QuizToggles[i].isOn)
            {
                QuizPages[i].SetActive(true);
            }
        }
    }

    public void Score_BackBtn()
    {
        DataSettingPage.SetActive(true);
        ScoreCantBlackPage.SetActive(false);
        ScoreSaveConPage.SetActive(false);
        ScorePage.SetActive(false);
    }
    public void Score_SaveBtn()
    {
        bool isBlank = false;
        //如果有欄位是空白的
        for (int i = 0; i < ScoreInputs.Count; i++)
        {
            if (ScoreInputs[i].text == "")
            {
                isBlank = true;
                break;
            }
        }
        if (isBlank)
        {
            ScoreCantBlackPage.SetActive(true);
        }
        else
        {
            ScoreSaveConPage.SetActive(true);
        }
    }
    public void Score_SaveConBtn()
    {
        LoadingPage.SetActive(true);
        ScoreSaveConPage.SetActive(false);
        ScoreData = "";
        for (int i = 0; i < ScoreInputs.Count-1; i++)
        {
            ScoreData += ScoreInputs[i].text + ",";
        }
        ScoreData += ScoreInputs[ScoreInputs.Count - 1].text;
        StartCoroutine(WriteScoreData());
    }
    public void Score_CancelConBtn()
    {
        ScoreSaveConPage.SetActive(false);
    }
    #endregion

    #region 讀取資料
    //讀取配分
    IEnumerator ReadScoreData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //測驗配分的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbzF7d7qybA3mTw2PkZiAQpBb9EqSaQua88RoRGXmutddpw1PC7LfbMvBV0QIjUtCYh8/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //下載的資料
                DownloadScores = www.downloadHandler.text.Split(',');
                ResetScores();
                Debug.Log("Download Score!");
            }
        }
    }
    #endregion

    #region 寫入資料
    //班級名冊
    IEnumerator WriteToSheet_Class()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        form.AddField("ClassName", menu.LoginTeacherData.Split('/')[0] + "_" + Local_FileName.Split('.')[0]);
        string Data = "";
        for (int i = 0; i < NameData.Length - 1; i++)
        {
            Data += NameData[i] + ",";
        }
        Data += NameData[NameData.Length - 1];
        form.AddField("Studata", Data);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbylwdZsT9oKcOCSBKl0WaOUVqxZG3xAyUQs-2QrkBHvwdxioxbj2Zq-OUOsXKHawuypdQ/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                LoadingPage.SetActive(false);
                Debug.Log("Upload Complete!");
            }
        }
    }
    //寫入配分
    IEnumerator WriteScoreData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //改成寫一欄，資訊用,隔開
        form.AddField("Data", ScoreData);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbzF7d7qybA3mTw2PkZiAQpBb9EqSaQua88RoRGXmutddpw1PC7LfbMvBV0QIjUtCYh8/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload Complete!");
                LoadingPage.SetActive(false);
                ScorePage.SetActive(false);
                DataSettingPage.SetActive(true);
            }
        }
    }
    #endregion
}
