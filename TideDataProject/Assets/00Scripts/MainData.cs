using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//Google表單功能
using UnityEngine.Networking;

public class MainData : MonoBehaviour
{
    public Menu menu;
    public Button LogOutConButton, ModeStu_BackButton, Modes_BackButton,Stu_BackButton;
    [Header("切換模式")]
    public Scrollbar[] TogglePageBars = new Scrollbar[2];
    public Toggle[] ModeToggles = new Toggle[2];
    public GameObject[] ToggleModePages = new GameObject[2];
    private bool isTurtorial;
    private int StageNum, QuizCount;
    [Header("頁面")]
    public GameObject LoadingPage;
    public GameObject MainInformPage, NoInformPage, LogOutConPage;
    //Mode
    public GameObject ModesPage, Mode_ClassPage, Mode_StudentPage;
    //StudentInfo
    public GameObject StudentInfoPage, Stu_AllScorePage, Stu_ScorePage;
    [Header("教師資料")]
    public Text T_SchoolText, T_NameText;
    [Header("學生資料")]
    //基本資料
    public Text ModeText;
    public Text StageNameText, StuClassText,StuNumText,StuNameText;
    //AllScore
    public Text AllScore_TimesText;
    public Sprite WrongBackPic1, WrongBackPic2;
    //TestScore
    public Text Score_DateText, Score_ScoreText;
    [Header("按鈕生成位置")]
    public Transform ClassBtnPlace;
    public Transform StudentBtnPlace,AllScoreBtnPlace,ScoreTimePlace,ScoreTextPlace;
    [Header("BtnPrefabs")]
    public bool SetClass, SetName, SetTime;
    public GameObject ClassBtnPrefab;
    public GameObject StudentBtnPrefab,AllScore_TimeBtnPrefab,Score_TimePrefab,Score_TextPrefab;
    public string ClickedClassName,ClickStudentName,ClickDate, DetailData;
    public string[] ClassData,StuNameData;
    public List<string> StuData, StuSelectNames,ClassSelectNames;
    [Header("設定班級全體資料")]
    //1005新增
    public AllTestFunction TestFunction;
    public Text ClassDataClassText,ClassTestText;
    public GameObject ClassDataPage,ClassTestPage;
    public Transform ClassDtatContent;
    public GameObject ClassDataItem, ClassDataItem2;
    public Sprite DoneSprite, NotDoneSprite;
    public Scrollbar ClassDataBar, ClassUnitBar;
    public Scrollbar ClassDataBar_Long;
    public List<GameObject> ClassDataObjects;
    public List<string> AllData = new List<string>();
    [Header("設定教學模式資料")]
    public GameObject Stu_TutorialPage;
    public Text Tutorial_TimesText;
    public GameObject[] Stu_StagePages = new GameObject[8];
    public List<GameObject> TutorialTimesObjects;
    [Header("設定詳細資料")]
    public int QuizNumber;
    public GameObject Stu_Tu_DetailPage;
    public GameObject DetailTextPrefab;
    public Transform Tu_DetailPlace;
    //確認各項完成幾次
    public int[] FinishTimes;
    [Header("教學單元")]
    public Color TutorialFinColor, TutorialUnFinColor;
    //抓圖鑑欄位
    public Transform TutrialLeft, TutorialRight;
    [Header("單元一")]
    //圖鑑欄位+換頁
    public GameObject[] Stage1Pages = new GameObject[3];
    public Toggle[] Stage1Toggle = new Toggle[3];
    [Header("單元二")]
    //圖鑑欄位
    public GameObject Stage2Page;
    [Header("單元三")]
    //圖鑑欄位+換頁
    public GameObject[] Stage3Pages = new GameObject[2];
    public Toggle[] Stage3Toggle = new Toggle[2];
    [Header("單元四")]
    //圖鑑欄位+換頁
    public GameObject[] Stage4Pages = new GameObject[3];
    public Toggle[] Stage4Toggle = new Toggle[3];
    [Header("單元五")]
    //圖鑑欄位+換頁
    public GameObject[] Stage5Pages = new GameObject[2];
    public Toggle[] Stage5Toggle = new Toggle[2];
    [Header("單元六")]
    //圖鑑欄位+換頁
    public GameObject[] Stage6Pages = new GameObject[3];
    public Toggle[] Stage6Toggle = new Toggle[3];
    [Header("APP")]
    //圖鑑欄位+換頁
    public GameObject[] APPPages = new GameObject[2];
    public Toggle[] APPToggle = new Toggle[2];
    public GameObject[] AppTimesObjects1, AppTimesObjects2;
    // Start is called before the first frame update
    void Start()
    {
        LogOutConButton.onClick.AddListener(LogOutConBtn);
        ModeStu_BackButton.onClick.AddListener(ModeStu_BackBtn);
        Modes_BackButton.onClick.AddListener(Modes_BackBtn);
        Stu_BackButton.onClick.AddListener(Stu_BackBtn);
    }

    // Update is called once per frame
    void Update()
    {
        ClassUnitBar.value = ClassDataBar.value;
        //載入班級
        if (SetClass)
        {
            for (int i = 0; i < ClassBtnPlace.childCount; i++)
            {
                if (ClassBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickClass)
                {
                    //載入學生名字
                    ClickedClassName = ClassBtnPlace.GetChild(i).GetChild(1).GetComponent<Text>().text;
                    SetNameBtnPlace();
                }
            }
        }
        //按下名字
        if (SetName)
        {
            for (int i = 0; i < StudentBtnPlace.childCount; i++)
            {
                if (StudentBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickName)
                {
                    //學生名字
                    ClickStudentName = StudentBtnPlace.GetChild(i).GetChild(0).GetComponent<Text>().text;
                    SetStudent();
                }
            }
        }
        //查看測驗細項
        if (SetTime)
        {
            for (int i = 0; i < AllScoreBtnPlace.childCount; i++)
            {
                if (AllScoreBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickTime)
                {
                    //檢查日期
                    ClickDate = AllScoreBtnPlace.GetChild(i).GetChild(0).GetComponent<Text>().text;
                    SetScore();
                }
            }
        }
    }
    #region 切換模式功能
    public void ModeToggleEvent(int Num)
    {
        for (int j = 0; j < ToggleModePages.Length; j++)
        {
            if (j == Num)
            {
                ToggleModePages[j].SetActive(true);
                TogglePageBars[j].value = 1;
            }
            else
            {
                ToggleModePages[j].SetActive(false);
            }
        }
    }
    #endregion

    #region 切換教學單元物種分類
    public void AnimalToggleEvent()
    {

        switch (StageNum)
        {
            case 0:
                break;
            case 1:
                for (int i = 0; i < Stage1Toggle.Length; i++)
                {
                    if (Stage1Toggle[i].isOn)
                    {
                        Stage1Pages[i].SetActive(true);
                    }
                    else
                    {
                        Stage1Pages[i].SetActive(false);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < Stage3Toggle.Length; i++)
                {
                    if (Stage3Toggle[i].isOn)
                    {
                        Stage3Pages[i].SetActive(true);
                    }
                    else
                    {
                        Stage3Pages[i].SetActive(false);
                    }
                }
                break;
            case 4:
                for (int i = 0; i < Stage4Toggle.Length; i++)
                {
                    if (Stage4Toggle[i].isOn)
                    {
                        Stage4Pages[i].SetActive(true);
                    }
                    else
                    {
                        Stage4Pages[i].SetActive(false);
                    }
                }
                break;
            case 5:
                for (int i = 0; i < Stage5Toggle.Length; i++)
                {
                    if (Stage5Toggle[i].isOn)
                    {
                        Stage5Pages[i].SetActive(true);
                    }
                    else
                    {
                        Stage5Pages[i].SetActive(false);
                    }
                }
                break;
            case 6:
                for (int i = 0; i < Stage6Toggle.Length; i++)
                {
                    if (Stage6Toggle[i].isOn)
                    {
                        Stage6Pages[i].SetActive(true);
                    }
                    else
                    {
                        Stage6Pages[i].SetActive(false);
                    }
                }
                break;
            case 8:
                for (int i = 0; i < APPToggle.Length; i++)
                {
                    if (APPToggle[i].isOn)
                    {
                        APPPages[i].SetActive(true);
                    }
                    else
                    {
                       APPPages[i].SetActive(false);
                    }
                }
                break;
        }
    }
    #endregion

    #region 載入該名學生紀錄
    public void SetStudent()
    {
        SetName = false;
        ModesPage.SetActive(true);
        ModeToggles[0].isOn = true;
        ToggleModePages[0].SetActive(true);
        ToggleModePages[1].SetActive(false);
        TogglePageBars[0].value = 1;
        Mode_StudentPage.SetActive(false);
        ClassTestPage.SetActive(false);
        ClassDataPage.SetActive(false);

    }
    //教學按鈕
    public void TutorialBtn(int Num)
    {
        isTurtorial = true;
        StageNum = Num;
        LoadingPage.SetActive(true);
        //讀取學生資料
        StartCoroutine(ReadStudentData());
    }
    //測驗按鈕
    public void TestBtn(int Num)
    {
        isTurtorial = false;
        StageNum = Num;
        LoadingPage.SetActive(true);
        //讀取學生資料
        StartCoroutine(ReadStudentData());
    }

    #region 設定成績
    public void SetScore()
    {
        SetTime = false;
        for (int i = 0; i < AllScoreBtnPlace.childCount; i++)
        {
            AllScoreBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickTime = false;
        }
        string[] Date = ClickDate.Split('-');
        DetailData = "";
        for (int i = 0; i < StuData.Count; i++)
        {
            if (StuData[i].Contains(Date[0]))
            {
                DetailData = StuData[i];
                break; 
            }
        }
        Score_DateText.text = Date[0] + "\n-" + Date[1];
        Score_DateText.text.Replace("\n", "\\n");
        Score_ScoreText.text = DetailData.Split('@')[8].Split('_')[1];
        //有幾題
        QuizCount = int.Parse(DetailData.Split('@')[9]);
        if (ScoreTimePlace.childCount != 0)
        {
            for (int i = 0; i < ScoreTimePlace.childCount; i++)
            {
                Destroy(ScoreTimePlace.GetChild(i).gameObject);
                Destroy(ScoreTextPlace.GetChild(i).gameObject);
            }
        }
        for (int i = 10; i < 10+QuizCount; i++)
        {
            //設定其他時間細項
            Instantiate(Score_TimePrefab, ScoreTimePlace);
            Instantiate(Score_TextPrefab, ScoreTextPlace);
        }
        Invoke("SetDetailScore", 0.1f);
    }

    void SetDetailScore()
    {
        for (int i = 10; i < 10 + QuizCount; i++)
        {
            int Time = int.Parse(DetailData.Split('@')[i]);
            ScoreTimePlace.GetChild(i - 10).GetComponent<Text>().text = "第" + (i - 9).ToString() + "題：";
            ScoreTimePlace.GetChild(i - 10).GetChild(0).GetComponent<Text>().text = (Time / 60).ToString("00") + ":" + (Time % 60).ToString("00");
            //設定答案細項
            string[] Ans = DetailData.Split('@')[i + QuizCount].Split('_');
            if (Ans[0].Contains("器官配對"))
            {
                Ans[0]= Ans[0].Replace("器官配對", "構造配對");
            }
            ScoreTextPlace.GetChild(i - 10).GetChild(0).GetComponent<Text>().text = Ans[0];
            ScoreTextPlace.GetChild(i - 10).GetChild(1).GetChild(0).GetComponent<Text>().text = Ans[1];
        }
        Stu_AllScorePage.SetActive(false);
        Stu_ScorePage.SetActive(true);
    }
    #endregion
    #endregion

    #region BTN_Event
    public void LogOutConBtn()
    {
        MainInformPage.SetActive(false);
        StudentInfoPage.SetActive(false);
        ModesPage.SetActive(false);
        ModesPage.SetActive(false);
        ClassDataPage.SetActive(false);
        ClassTestPage.SetActive(false);
        Mode_StudentPage.SetActive(false);
        Mode_ClassPage.SetActive(true);
        menu.LogOutSet();
        SetClass = false;
        SetName = false;
        SetTime = false;
    }

    #region 查看教學細項Btn
    public void TutorialDetailBtn(int QuizNum)
    {
        if (Tu_DetailPlace.childCount != 0)
        {
            for (int i = 0; i < Tu_DetailPlace.childCount; i++)
            {
                Destroy(Tu_DetailPlace.GetChild(i).gameObject);
            }
        }
        QuizNumber = QuizNum;
        for (int i = 0; i < FinishTimes[QuizNum]; i++)
        {
            Instantiate(DetailTextPrefab, Tu_DetailPlace);
        }
        Invoke("SetTutorialDetails", 0.1f);
    }
    void SetTutorialDetails()
    {
        List<string> StartTime = new List<string>();
        List<string> EndTime = new List<string>();
        if (StageNum == 8)
        {
            for (int i = 0; i < StuData.Count; i++)
            {
                string[] Data = StuData[i].Split('@');
                //魚類
                if (Data[5]=="0")
                {
                    if (Data[6] == QuizNumber.ToString())
                    {
                        if (Data[7] == "True")
                        {
                            //開始時間
                            StartTime.Add(Data[3]);
                            //結束時間
                            EndTime.Add(Data[4]);
                        }
                    }
                }
                //植物類
                else
                {
                    if (Data[6] == (QuizNumber-3).ToString())
                    {
                        if (Data[7] == "True")
                        {
                            //開始時間
                            StartTime.Add(Data[3]);
                            //結束時間
                            EndTime.Add(Data[4]);
                        }
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < StuData.Count; i++)
            {
                string[] Data = StuData[i].Split('@');
                if (Data[7 + QuizNumber] == "true")
                {
                    //開始時間
                    StartTime.Add(Data[5]);
                    //結束時間
                    EndTime.Add(Data[6]);
                }
            }
        }    

        for (int i = 0; i < Tu_DetailPlace.childCount; i++)
        {
            //開始時間
            Tu_DetailPlace.GetChild(i).GetComponent<Text>().text = StartTime[i];
            //結束時間
            Tu_DetailPlace.GetChild(i).GetChild(0).GetComponent<Text>().text = EndTime[i];
        }
        Stu_Tu_DetailPage.SetActive(true);
    }
    #endregion

    #region 返回選班級
    public void ModeStu_BackBtn()
    {
        //切換狀態
        SetName = false;
        //重設ClassBtn
        if (ClassBtnPlace.childCount != 0)
        {
            for (int i = 0; i < ClassBtnPlace.childCount; i++)
            {
                Destroy(ClassBtnPlace.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < ClassSelectNames.Count; i++)
        {
            Instantiate(ClassBtnPrefab, ClassBtnPlace);
        }
        Invoke("SetClassName", 0.1f);
    }
    void SetClassName()
    {
        for (int i = 0; i < ClassBtnPlace.childCount; i++)
        {
            ClassBtnPlace.GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text = ClassSelectNames[i];
        }
        SetClass = true;
        Mode_StudentPage.SetActive(false);
        ClassDataPage.SetActive(false);
        ClassTestPage.SetActive(false);
        Mode_ClassPage.SetActive(true);
    }
    #endregion

    #region 返回選學生
    public void Modes_BackBtn()
    {
        SetTime = false;
        if (StudentBtnPlace.childCount != 0)
        {
            for (int i = 0; i < StudentBtnPlace.childCount; i++)
            {
                Destroy(StudentBtnPlace.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < StuSelectNames.Count; i++)
        {
            Instantiate(StudentBtnPrefab, StudentBtnPlace);
        }
        Invoke("SetStuNames", 0.1f);
    }
    void SetStuNames()
    {
        for (int i = 0; i < StuSelectNames.Count; i++)
        {
            Instantiate(StudentBtnPrefab, StudentBtnPlace);
            StudentBtnPlace.GetChild(i).GetChild(0).GetComponent<Text>().text = StuSelectNames[i];
        }
        SetName = true;
        StudentInfoPage.SetActive(false);
        ModesPage.SetActive(false);
        ClassTestPage.SetActive(false);
        ClassDataPage.SetActive(true);
        ClassDataBar.value = 0;
        ClassDataBar_Long.value = 1;
        ClassUnitBar.value = 0;
        Mode_StudentPage.SetActive(true);
    }
    #endregion 

    #region 返回選時間
    public void Stu_BackBtn()
    {
        for (int i = 0; i < AllScoreBtnPlace.childCount; i++)
        {
            AllScoreBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickTime = false;
        }
        SetTime = true;
        Stu_AllScorePage.SetActive(true);
        Stu_ScorePage.SetActive(false);
    }
    #endregion

    #endregion

    #region 讀取資料
    #region 載入班級資料
    public void SetClassBtnPlace()
    {
        SetClass = false;
        //設定教師資料
        T_SchoolText.text = menu.LoginTeacherData.Split('/')[0];
        T_NameText.text = menu.LoginTeacherData.Split('/')[1];
        if (ClassBtnPlace.childCount != 0)
        {
            for (int i = 0; i < ClassBtnPlace.childCount; i++)
            {
                Destroy(ClassBtnPlace.GetChild(i).gameObject);
            }
        }
        MainInformPage.SetActive(true);
        StudentInfoPage.SetActive(false);
        ModesPage.SetActive(false);
        Mode_StudentPage.SetActive(false);
        ClassTestPage.SetActive(false);
        ClassDataPage.SetActive(false);
        Mode_ClassPage.SetActive(true);
        StartCoroutine(ReadClassData());
    }
    IEnumerator ReadClassData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //辨識要讀取的類型 Class為讀取班級名稱
        form_Read.AddField("Type", "Class");
        //班級跟學生名冊的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbylwdZsT9oKcOCSBKl0WaOUVqxZG3xAyUQs-2QrkBHvwdxioxbj2Zq-OUOsXKHawuypdQ/exec", form_Read))
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
                ClassData = www.downloadHandler.text.Split(',');
                ClassSelectNames.Clear();
                for (int i = 0; i < ClassData.Length; i++)
                {
                    if (ClassData[i].Contains(menu.LoginTeacherData.Split('/')[0]))
                    {
                        ClassSelectNames.Add(ClassData[i].Split('_')[1]);
                    }
                }
                if (ClassSelectNames.Count == 0)
                {
                    LoadingPage.SetActive(false);
                    NoInformPage.SetActive(true);
                }
                //設定班級按鈕
                else
                {
                    if (ClassBtnPlace.childCount != 0)
                    {
                        for (int i = 0; i < ClassBtnPlace.childCount; i++)
                        {
                            Destroy(ClassBtnPlace.GetChild(i).gameObject);
                        }
                    }
                    for (int i = 0; i < ClassSelectNames.Count; i++)
                    {
                        Instantiate(ClassBtnPrefab, ClassBtnPlace);
                    }
                }
                Invoke("SetClass_", 0.1f);
                Debug.Log("Download Class!");
            }
        }
    }
    void SetClass_()
    {
        for (int i = 0; i < ClassSelectNames.Count; i++)
        {
            ClassBtnPlace.GetChild(i).GetChild(1).GetComponent<Text>().text = ClassSelectNames[i];
        }
        SetClass = true;
        LoadingPage.SetActive(false);
    }
    #endregion

    #region 讀取學生姓名
    public void SetNameBtnPlace()
    {
        SetClass = false;
        LoadingPage.SetActive(true);
        for (int i = 0; i < ClassBtnPlace.childCount; i++)
        {
            ClassBtnPlace.GetChild(i).GetComponent<BtnFeature>().isClickClass = false;
        }
        StartCoroutine(ReadNameData());
    }
    IEnumerator ReadNameData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //辨識要讀取的類型
        form_Read.AddField("Type", "Student");
        //讀特定工作表名稱，名稱格式：學校名稱_班級
        form_Read.AddField("ClassName", menu.LoginTeacherData.Split('/')[0]+"_"+ ClickedClassName);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbylwdZsT9oKcOCSBKl0WaOUVqxZG3xAyUQs-2QrkBHvwdxioxbj2Zq-OUOsXKHawuypdQ/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //下載的資料：首2筆資料是名稱欄，剩下欄位的偶數為姓名
                StuNameData = www.downloadHandler.text.Split(',');
                StuSelectNames.Clear();
                for (int i = 0; i < (StuNameData.Length/2)-1; i++)
                {
                    StuSelectNames.Add(StuNameData[(i+3)+i]);
                }
                for (int i = 0; i < StudentBtnPlace.childCount; i++)
                {
                    Destroy(StudentBtnPlace.GetChild(i).gameObject);
                }
                for (int i = 0; i < StuSelectNames.Count; i++)
                {
                    Instantiate(StudentBtnPrefab, StudentBtnPlace);
                }
                #region 設定全體資料生成
                ClassDataClassText.text = ClickedClassName;
                ClassTestText.text = ClickedClassName;
                ClassDataObjects.Clear();
                for (int i = 0; i < ClassDtatContent.childCount; i++)
                {
                    Destroy(ClassDtatContent.GetChild(i).gameObject);
                }
                #endregion
                StartCoroutine(ReadAllStuData());
                Debug.Log("Download Name!");
            }
        }
    }
    void SetNames()
    {
        for (int i = 0; i < StuSelectNames.Count; i++)
        {
            StudentBtnPlace.GetChild(i).GetChild(0).GetComponent<Text>().text = StuSelectNames[i];
        }
        Mode_ClassPage.SetActive(false);
        Mode_StudentPage.SetActive(true);
        ClassTestPage.SetActive(false);
        ClassDataPage.SetActive(true);
        ClassDataBar.value = 0;
        ClassDataBar_Long.value = 1;
        ClassUnitBar.value = 0;
        SetName = true;
        LoadingPage.SetActive(false);
    }
    #endregion
    #region  讀取學生資料(全體)
    IEnumerator ReadAllStuData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式_讀取全體資料
        form_Read.AddField("method", "ReadClassData");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxaqA5uZDxsysUoUEQfJpirTIovZU9hwlZk8qt9g-ZFBk1myAQkmaaE6w7amnONkA-_dQ/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                #region 新增班級全體資料物件
                for (int i = 0; i < StuSelectNames.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        Instantiate(ClassDataItem, ClassDtatContent);
                    }
                    else
                    {
                        Instantiate(ClassDataItem2, ClassDtatContent);
                    }
                }
                for (int i = 0; i < ClassDtatContent.childCount; i++)
                {
                    ClassDataObjects.Add(ClassDtatContent.GetChild(i).gameObject);
                }
                #endregion
                //下載的資料
                string[] Data = www.downloadHandler.text.Split(',');
                AllData.Clear();
                //僅使用該班級的資料
                for (int i = 0; i < Data.Length; i++)
                {                 
                    if (Data[i].Contains("@"))
                    {
                        string[] Split = Data[i].Split('@');
                        if (Split[1] == T_SchoolText.text && Split[2] == ClickedClassName)
                        {
                            AllData.Add(Data[i]);
                        }
                    }
                }
                #region 區分資料
                for (int i = 0; i < StuSelectNames.Count; i++)
                {
                    #region 教學
                    bool[] isTutor = new bool[7];
                    for (int j = 0; j < AllData.Count; j++)
                    {
                        string[] Split = AllData[j].Split('@');
                        //有通關一次就有紀錄
                        if (Split[4] == StuSelectNames[i]&&Split[5]=="0")
                        {
                            isTutor[int.Parse(Split[0])] = true;
                        }
                    }
                    #endregion
                    #region 測驗
                    bool[] isTest = new bool[6];
                    //測驗的紀錄_只有分數
                    List<int> Test1 = new List<int>();
                    List<int> Test2 = new List<int>();
                    List<int> Test3 = new List<int>();
                    List<int> Test4 = new List<int>();
                    List<int> Test5 = new List<int>();
                    List<int> Test6 = new List<int>();
                    for (int j = 0; j < AllData.Count; j++)
                    {
                        string[] Split = AllData[j].Split('@');
                        //收集成績
                        int QuizNum = int.Parse(Split[0]) - 1;
                        //有通關一次就有紀錄
                        if (Split[4] == StuSelectNames[i] && Split[5] == "1")
                        {
                            isTest[QuizNum] = true;
                            switch (QuizNum)
                            {
                                case 0:
                                    Test1.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                                case 1:
                                    Test2.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                                case 2:
                                    Test3.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                                case 3:
                                    Test4.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                                case 4:
                                    Test5.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                                case 5:
                                    Test6.Add(int.Parse(Split[9].Split('_')[1]));
                                    break;
                            }
                        }                       
                    }
                    //排列成績
                    string[] HighScore = new string[6];
                    //排列分數(由高到低)
                    for (int K = 0; K < isTest.Length; K++)
                    {
                        if (isTest[K])
                        {
                            switch (K)
                            {
                                case 0:
                                    Test1 = Test1.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test1[0].ToString();
                                    break;
                                case 1:
                                    Test2 = Test2.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test2[0].ToString();
                                    break;
                                case 2:
                                    Test3 = Test3.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test3[0].ToString();
                                    break;
                                case 3:
                                    Test4 = Test4.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test4[0].ToString();
                                    break;
                                case 4:
                                    Test5 = Test5.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test5[0].ToString();
                                    break;
                                case 5:
                                    Test6 = Test6.OrderByDescending(number => number).ToList();
                                    HighScore[K] = Test6[0].ToString();
                                    break;
                            }
                        }
                    }
                    #endregion
                    #region 顯示資料
                    ClassDataObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = StuSelectNames[i];
                    //設定其他資料
                    //操作教學
                    if (isTutor[0])
                    {
                        ClassDataObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = DoneSprite;
                    }
                    else
                    {
                        ClassDataObjects[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = NotDoneSprite;
                    }
                    //其他單元
                    for (int j = 0; j < ClassDataObjects[i].transform.GetChild(2).childCount; j++)
                    {
                        //教學
                        if (isTutor[j + 1])
                        {
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(0).GetComponent<Image>().sprite = DoneSprite;
                        }
                        else
                        {
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(0).GetComponent<Image>().sprite = NotDoneSprite;
                        }
                        //測驗
                        if (isTest[j])
                        {
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(1).GetComponent<Text>().text = HighScore[j];
                            if (int.Parse(HighScore[j]) < 60)
                            {
                                ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(1).GetComponent<Text>().color = Color.red;
                            }
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(2).gameObject.SetActive(false);
                        }
                        else
                        {
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(1).gameObject.SetActive(false);
                            ClassDataObjects[i].transform.GetChild(2).GetChild(j).GetChild(2).gameObject.SetActive(true);
                        }
                    }

                    #endregion
                }
                //1005新增_測驗的全體資料
                TestFunction.SetTest();
                Invoke("SetNames", 0.5f);
                #endregion 
            }
        }
    }

    #endregion

    #region  讀取學生資料
    IEnumerator ReadStudentData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //輸入要讀取的工作表號碼，從0開始，測驗及教學的資料在同一個單元的工作表內
        form_Read.AddField("Num",StageNum);
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxaqA5uZDxsysUoUEQfJpirTIovZU9hwlZk8qt9g-ZFBk1myAQkmaaE6w7amnONkA-_dQ/exec", form_Read))
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
                string[] Data = www.downloadHandler.text.Split(',');
                StuData.Clear();
                //APP資料
                if (StageNum == 8)
                {
                    for (int i = 1; i < Data.Length; i++)
                    {
                        if (Data[i].Contains(menu.LoginTeacherData.Split('/')[0]) && Data[i].Contains(ClickedClassName) && Data[i].Contains(ClickStudentName))
                        {
                            StuData.Add(Data[i]);
                        }
                    }
                }
                //VR單元資料
                else
                {
                    for (int i = 0; i < Data.Length; i++)
                    {
                        if (Data[i].Contains(menu.LoginTeacherData.Split('/')[0]) && Data[i].Contains(ClickedClassName) && Data[i].Contains(ClickStudentName))
                        {
                            string[] SplitData = Data[i].Split('@');
                            if (isTurtorial)
                            {
                                if (SplitData[4] == "0")
                                {
                                    StuData.Add(Data[i]);
                                }
                            }
                            else
                            {
                                if (SplitData[4] == "1")
                                {
                                    StuData.Add(Data[i]);
                                }
                            }
                        }
                    }

                }
                Invoke("SetData", 0.1f);
                Debug.Log("Download School!");
            }
        }
    }
    public void SetData()
    {
        if (StuData.Count > 0)
        {
            #region 基本資料
            StuClassText.text = ClickedClassName;
            StuNameText.text = ClickStudentName;
            StuNumText.text = StuData[0].Split('@')[2];
            //設定單元名稱
            switch (StageNum)
            {
                case 0:
                    StageNameText.text = "VR 操作教學 - 潮來潮往";
                    break;
                case 1:
                    StageNameText.text = "單元一 動物的顏色與花紋";
                    break;
                case 2:
                    StageNameText.text = "單元二 一般動物的構造與運動方式";
                    break;
                case 3:
                    StageNameText.text = "單元三 水中動物的構造與運動方式";
                    break;
                case 4:
                    StageNameText.text = "單元四 動物的形體、肌肉、骨骼";
                    break;
                case 5:
                    StageNameText.text = "單元五 動物的生活方式";
                    break;
                case 6:
                    StageNameText.text = "單元六 動物的覓食行為";
                    break;
                case 8:
                    StageNameText.text = "APP資料紀錄";
                    break;
            }
            #endregion
            //確認模式
            //教學
            #region 設定教學圖鑑資料
            if (isTurtorial)
            {
                //設定基本資料
                ModeText.text = "教學模式";
                Tutorial_TimesText.text = StuData.Count.ToString();
                #region 分類資料
                TutorialTimesObjects.Clear();
                switch (StageNum)
                {
                    //潮汐教學
                    case 0:
                        #region 設定潮汐
                        //重設
                        FinishTimes = new int[7];
                        for (int i = 0; i < TutrialLeft.childCount; i++)
                        {
                            TutorialTimesObjects.Add(TutrialLeft.GetChild(i).gameObject);
                        }
                        for (int i = 0; i < TutorialRight.childCount; i++)
                        {
                            TutorialTimesObjects.Add(TutorialRight.GetChild(i).gameObject);
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                TutorialTimesObjects[i].transform.GetChild(1).GetComponent<Text>().color = TutorialFinColor;
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                TutorialTimesObjects[i].transform.GetChild(1).GetComponent<Text>().color = TutorialUnFinColor;
                                //設定細項按鈕
                                TutorialTimesObjects[i].GetComponent<Button>().interactable = false;
                            }
                        }
                        #endregion
                        break;
                    case 1:
                        #region 設定單元1
                        //重設
                        FinishTimes = new int[13];
                        for (int i = 0; i < Stage1Pages.Length; i++)
                        {
                            for (int j = 0; j < Stage1Pages[i].transform.childCount; j++)
                            {
                                TutorialTimesObjects.Add(Stage1Pages[i].transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = false;
                            }
                        }
                        //設定開啟頁面
                        for (int i = 0; i < Stage1Pages.Length; i++)
                        {
                            Stage1Pages[i].SetActive(false);
                        }
                        Stage1Pages[0].SetActive(true);
                        Stage1Toggle[0].isOn = true;
                        #endregion
                        break;
                    case 2:
                        #region 設定單元2
                        //重設
                        FinishTimes = new int[5];
                        for (int i = 0; i < Stage2Page.transform.childCount; i++)
                        {
                            TutorialTimesObjects.Add(Stage2Page.transform.GetChild(i).gameObject);
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].GetComponent<Button>().interactable = false;
                            }
                        }
                        #endregion
                        break;
                    case 3:
                        #region 設定單元3
                        //重設
                        FinishTimes = new int[8];
                        for (int i = 0; i < Stage3Pages.Length; i++)
                        {
                            for (int j = 0; j < Stage3Pages[i].transform.childCount; j++)
                            {
                                TutorialTimesObjects.Add(Stage3Pages[i].transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = false;
                            }
                        }
                        //設定開啟頁面
                        for (int i = 0; i < Stage3Pages.Length; i++)
                        {
                            Stage3Pages[i].SetActive(false);
                        }
                        Stage3Pages[0].SetActive(true);
                        Stage3Toggle[0].isOn = true;
                        #endregion
                        break;
                    case 4:
                        #region 設定單元4
                        //重設
                        FinishTimes = new int[13];
                        for (int i = 0; i < Stage4Pages.Length; i++)
                        {
                            for (int j = 0; j < Stage4Pages[i].transform.childCount; j++)
                            {
                                TutorialTimesObjects.Add(Stage4Pages[i].transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = false;
                            }
                        }
                        //設定開啟頁面

                        for (int i = 0; i < Stage4Pages.Length; i++)
                        {
                            Stage4Pages[i].SetActive(false);
                        }
                        Stage4Pages[0].SetActive(true);
                        Stage4Toggle[0].isOn = true;
                        #endregion
                        break;
                    case 5:
                        #region 設定單元5
                        //重設
                        FinishTimes = new int[8];
                        for (int i = 0; i < Stage5Pages.Length; i++)
                        {
                            for (int j = 0; j < Stage5Pages[i].transform.childCount; j++)
                            {
                                TutorialTimesObjects.Add(Stage5Pages[i].transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = false;
                            }
                        }
                        //設定開啟頁面
                        for (int i = 0; i < Stage5Pages.Length; i++)
                        {
                            Stage5Pages[i].SetActive(false);
                        }
                        Stage5Pages[0].SetActive(true);
                        Stage5Toggle[0].isOn = true;
                        #endregion
                        break;
                    case 6:
                        #region 設定單元6
                        //重設
                        FinishTimes = new int[13];
                        for (int i = 0; i < Stage6Pages.Length; i++)
                        {
                            for (int j = 0; j < Stage6Pages[i].transform.childCount; j++)
                            {
                                TutorialTimesObjects.Add(Stage6Pages[i].transform.GetChild(j).gameObject);
                            }
                        }
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            for (int j = 0; j < FinishTimes.Length; j++)
                            {
                                if (Data[7 + j] == "true")
                                {
                                    FinishTimes[j]++;
                                }
                            }
                        }
                        for (int i = 0; i < FinishTimes.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                TutorialTimesObjects[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                TutorialTimesObjects[i].transform.GetComponent<Button>().interactable = false;
                            }
                        }
                        //設定開啟頁面
                        for (int i = 0; i < Stage6Pages.Length; i++)
                        {
                            Stage6Pages[i].SetActive(false);
                        }
                        Stage6Pages[0].SetActive(true);
                        Stage6Toggle[0].isOn = true;
                        #endregion
                        break;
                    case 8:
                        #region APP
                        //重設
                        //0-2、3-7
                        //0:魚類，有3種、1:葉子，有5種
                        FinishTimes = new int[8];
                        #region 處理回答次數物件
                        AppTimesObjects1 = new GameObject[3];
                        AppTimesObjects2 = new GameObject[5];
                        for (int i = 0; i < APPPages[0].transform.childCount; i++)
                        {
                            AppTimesObjects1[i] = APPPages[0].transform.GetChild(i).gameObject;
                        }
                        for (int i = 0; i < APPPages[1].transform.childCount; i++)
                        {
                            AppTimesObjects2[i] = APPPages[1].transform.GetChild(i).gameObject;
                        }
                        //有沒有收集到
                        for (int i = 0; i < StuData.Count; i++)
                        {
                            string[] Data = StuData[i].Split('@');
                            int type = int.Parse(Data[5]);
                            switch (type)
                            {
                                case 0:
                                    if (Data[7] == "True")
                                    {
                                        FinishTimes[int.Parse(Data[6])]++;
                                    }
                                    break;
                                case 1:
                                    if (Data[7] == "True")
                                    {
                                        FinishTimes[3 + int.Parse(Data[6])]++;
                                    }
                                    break;
                            }
                        }
                        #endregion
                        #region 顯示物件次數
                        for (int i = 0; i < AppTimesObjects1.Length; i++)
                        {
                            if (FinishTimes[i] != 0)
                            {
                                AppTimesObjects1[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i].ToString();
                                //設定細項按鈕
                                AppTimesObjects1[i].GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                AppTimesObjects1[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                AppTimesObjects1[i].GetComponent<Button>().interactable = false;
                            }
                        }
                        for (int i = 0; i < AppTimesObjects2.Length; i++)
                        {
                            if (FinishTimes[i+3] != 0)
                            {
                                AppTimesObjects2[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = FinishTimes[i+3].ToString();
                                //設定細項按鈕
                                AppTimesObjects2[i].GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                AppTimesObjects2[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
                                //設定細項按鈕
                                AppTimesObjects2[i].GetComponent<Button>().interactable = false;
                            }
                        }
                        #endregion
                        for (int i = 0; i < APPPages.Length; i++)
                        {
                            APPPages[i].SetActive(false);
                        }
                        APPPages[0].SetActive(true);
                        APPToggle[0].isOn = true;
                        #endregion
                        break;
                }
                #endregion
                //設定頁面
                for (int i = 0; i < Stu_StagePages.Length; i++)
                {
                    Stu_StagePages[i].SetActive(false);
                }
                if (StageNum == 8)
                {
                    Stu_StagePages[7].SetActive(true);
                }
                else 
                {
                    Stu_StagePages[StageNum].SetActive(true);
                }

                Stu_ScorePage.SetActive(false);
                Stu_AllScorePage.SetActive(false);
                StudentInfoPage.SetActive(true);
                Stu_TutorialPage.SetActive(true);
                LoadingPage.SetActive(false);
            }
            #endregion
            //測驗
            else
            {
                //設定基本資料
                ModeText.text = "測驗模式";
                //設定總覽畫面
                AllScore_TimesText.text = StuData.Count.ToString();
                for (int i = 0; i < AllScoreBtnPlace.childCount; i++)
                {
                    Destroy(AllScoreBtnPlace.GetChild(i).gameObject);
                }
                for (int i = 0; i < StuData.Count; i++)
                {
                    Instantiate(AllScore_TimeBtnPrefab, AllScoreBtnPlace);
                }
                Invoke("Test_AllScore", 0.1f);
            }

        }
        else
        {
            LoadingPage.SetActive(false);
            NoInformPage.SetActive(true);
        }
    }
    //設定測驗紀錄
    void Test_AllScore()
    {
        for (int i = 0; i < StuData.Count; i++)
        {
            string[] Data = StuData[i].Split('@');
            AllScoreBtnPlace.GetChild(i).GetChild(0).GetComponent<Text>().text = Data[5] + "-" + Data[6];
            int Time = int.Parse(Data[7]);
            //如果分數沒有及格就變紅色
            int Score = int.Parse(Data[8].Split('_')[1]);
            if (Score < 60)
            {
                AllScoreBtnPlace.GetChild(i).GetComponent<Image>().sprite = WrongBackPic1;
                AllScoreBtnPlace.GetChild(i).GetChild(1).GetComponent<Image>().sprite = WrongBackPic2;
            }
            AllScoreBtnPlace.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = (Time / 60).ToString("00") + "分";
        }
        Stu_ScorePage.SetActive(false);
        Stu_TutorialPage.SetActive(false);
        StudentInfoPage.SetActive(true);
        Stu_AllScorePage.SetActive(true);
        LoadingPage.SetActive(false);
        SetTime = true;
    }
    #endregion
    #endregion
}
