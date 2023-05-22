using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//寄Email
using System.Net;
using System.Net.Mail;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//Google表單功能
using UnityEngine.Networking;
//定義文化、語言相關的資訊
using System.Globalization;
//處理執行序活動和資料類別的存取
using System.Threading;


//Login、SignUp、FindPass功能
public class Menu : MonoBehaviour
{

    [Header("各種Button")]
    //Login
    public Button LoginButton;
    public Button SignUpButton, SearchPasswordButton,LoginNoAccountButton,LoginWrongPassButton,LoginEmailWrongButton;
    //SignUp
    public Button SignCancelButton, SignConfirmButton, SignAddConfirmButton, SignAddCancelButton, SignConConfirmButton, SignConCancelButton, SignEmailWrongButton, SignWriteInfoButton, SignAccountExistButton, SignSchoolExistButton;
    //FindPass
    public Button FindPassButton, BackLoginButton, FindNoAccountButton, FindWrongEmailButton;
    [Header("學校DropDown")]
    public Dropdown SignScoolDropDown;
    public Dropdown LoginSchoolDropdown,FindSchoolDropdown;
    public bool isSignUP, isAdd,isAddSchool;
    public string[] SchoolOptions, LoginSchoolOptions;
    [Header("教師資料")]
    public string[] TeacherData,SplitData;
    public string LoginTeacherData;
    [Header("各種頁面")]
    //Login
    public GameObject LoginPage;
    public GameObject LoginNoAccountPage, LoginWrongPassPage,LoginEmailWrongPage;
    //資料設定畫面
    public GameObject DataSettingPage,DataAddListPage,DataScorePage;
    //SignUp
    public GameObject SignupPage, SignAccountExistPage, SchoolExistPage,SignAddNoSchoolPage, SignAddSchoolPage, SignupConfirmPage, SignPasswordErrorPage, SignEmailWrongPage, SignWriteInfoPage;
    //FindPass
    public GameObject FindPassPage, FindNoAccountPage, FindEmailWrongPage, FindPassHint;
    //Other
    public GameObject LoadingPage;
    [Header("各種Input")]
    //Login
    public InputField LoginEmailInput;
    public InputField LoginPasswordInput;
    //SignUp
    public InputField SignNameInput, SignEmailInput, SignPasswordInput, SignPasswordInput2, SignAddSchoolInput;
    //FindPass
    public InputField FindPassEmailInput;
    //驗證是不是輸入email
    private const string mailPattern = @"\w{1,63}@[a-zA-Z0-9]{2,63}\.[a-zA-Z]{2,63}(\.[a-zA-Z]{2,63})?$";
    public static Regex MailRegex = new Regex(mailPattern, RegexOptions.Compiled);
    // Start is called before the first frame update
    void Start()
    {
        LoadingPage.SetActive(true);
        //讀取學校名稱
        StartCoroutine(ReadSchoolData());
        //讀取教師資料
        //按鈕事件
        //Login
        LoginButton.onClick.AddListener(LoginBtn);
        LoginNoAccountButton.onClick.AddListener(LoginNoAccountBtn);
        LoginWrongPassButton.onClick.AddListener(LoginWrongPassBtn);
        LoginEmailWrongButton.onClick.AddListener(LoginEmailWrongBtn);
        SignUpButton.onClick.AddListener(SignUpBtn);
        SearchPasswordButton.onClick.AddListener(SearchPassBtn);
        //SignUp
        SignAddCancelButton.onClick.AddListener(SignAddCancelBtn);
        SignAddConfirmButton.onClick.AddListener(SignAddConfirmBtn);
        SignCancelButton.onClick.AddListener(SignCancelBtn);
        SignConfirmButton.onClick.AddListener(SignConfirmBtn);

        SignConCancelButton.onClick.AddListener(SignConCancelBtn);
        SignConConfirmButton.onClick.AddListener(SignConConfirmBtn_N);

        SignEmailWrongButton.onClick.AddListener(SignEmailWrongBtn);
        SignWriteInfoButton.onClick.AddListener(SignWriteInfoBtn);
        SignAccountExistButton.onClick.AddListener(SignACExistBtn);
        SignSchoolExistButton.onClick.AddListener(SignSExistBtn);
        //FindPass
        FindPassButton.onClick.AddListener(FindPassBtn);
        BackLoginButton.onClick.AddListener(BackLoginBtn);
        FindNoAccountButton.onClick.AddListener(FindNoAccountBtn);
        FindWrongEmailButton.onClick.AddListener(FindWrongEmailBtn);

    }
    private void Update()
    {
    }
    #region 登入功能
    //關閉應用程式
    public void ExitBtn()
    {
        Application.Quit();
    }
    //忘記密碼
    public void SearchPassBtn()
    {
        LoginPage.SetActive(false);
        FindSchoolDropdown.value = 0;
        FindPassEmailInput.text = "";
        LoginPasswordInput.text = "";
        FindPassHint.SetActive(false);
        FindPassPage.SetActive(true);
    }
    //註冊
    public void SignUpBtn()
    {
        LoginPage.SetActive(false);
        SignupPage.SetActive(true);
        SignScoolDropDown.value = 1;
        SignNameInput.text = "";
        SignEmailInput.text = "";
        SignPasswordInput.text = "";
        SignPasswordInput2.text = "";
        SignScoolDropDown.value = 1;
    }
    #region 登入按鈕
    public void LoginBtn()
    {
        //確認信箱格式
        if (!MailRegex.IsMatch(LoginEmailInput.text))
        {
            LoginEmailWrongPage.SetActive(true);
            LoginEmailInput.text = "";
        }
        else
        {
            StartCoroutine(ReadTeacherData_Login());
            LoadingPage.SetActive(true);
        }
    }
    public void CheckLoginInfo()
    {
        //讀取教師資料
        string teacherrecord = "";
        bool findAccount = false;
        foreach (string line in TeacherData)
        {
            if (line.Contains(LoginSchoolDropdown.options[LoginSchoolDropdown.value].text)&&line.Contains(LoginEmailInput.text))
            {
                teacherrecord = line;
                findAccount = true;
                break;
            }
        }
        if (!findAccount)
        {
            LoginNoAccountPage.SetActive(true);
        }
        else
        {
            string[] Infos = teacherrecord.Split('/');
            //確認密碼(第4個)+進入資訊頁面
            if (LoginPasswordInput.text == Infos[3])
            {
                LoginTeacherData = teacherrecord;
                //先進入設定畫面
                LoginPage.SetActive(false);
                DataSettingPage.SetActive(true);
                DataAddListPage.SetActive(false);
                DataScorePage.SetActive(false);
            }
            else
            {
                LoginWrongPassPage.SetActive(true);
            }
        }
    }
    #endregion
    #region 彈出視窗

    public void LoginNoAccountBtn()
    {
        LoginSchoolDropdown.value = 0;
        LoginEmailInput.text = "";
        LoginPasswordInput.text = "";
        LoginNoAccountPage.SetActive(false);
    }
    public void LoginWrongPassBtn()
    {
        LoginPasswordInput.text = "";
        LoginWrongPassPage.SetActive(false);
    }
    public void LoginEmailWrongBtn()
    {
        LoginEmailWrongPage.SetActive(false);
    }
    #endregion
    #endregion

    #region 登出設定
    public void LogOutSet()
    {
        LoginSchoolDropdown.value = 0;
        LoginEmailInput.text = "";
        LoginPasswordInput.text = "";
        LoginTeacherData = "";
        LoginPage.SetActive(true);
    }
    #endregion

    #region 註冊功能
    #region 新增學校名稱
    //DropDownEvent
    public void SignSchoolDropDownEvent()
    {
        if (SignScoolDropDown.value == 0)
        {
            SignAddSchoolInput.text = "";
            SignAddSchoolPage.SetActive(true);
        }
    }
    //確認學校名稱有沒有重複
    public void SignSExistBtn()
    {
        SchoolExistPage.SetActive(false);
        SignAddSchoolPage.SetActive(false);
        SignAddSchoolInput.text = "";
        SignScoolDropDown.value = 1;
    }
    //確認新增學校名稱
    public void SignAddConfirmBtn()
    {
        isAddSchool = false;
        isAdd = false;
        if (SignAddSchoolInput.text != "")
        {
            //確認學校名稱是不是已經存在
            for (int i = 0; i < SchoolOptions.Length; i++)
            {
                if (SignAddSchoolInput.text == SchoolOptions[i])
                {
                    isAdd = true;
                    break;
                }
            }
            if (isAdd)
            {
                //學校存在
                SignAddSchoolInput.text = "";
                SchoolExistPage.SetActive(true);
            }
            else
            {
                //寫入新學校名稱
                StartCoroutine(WriteToSheet_School());
            }
        }
        else
        {
            SignAddNoSchoolPage.SetActive(true);
        }
    }
    #endregion
    //確認填入的資料
    public void SignConfirmBtn()
    {
        if (SignEmailInput.text == "" || SignNameInput.text == "" || SignPasswordInput.text == ""||SignPasswordInput2.text =="")
        {
            SignWriteInfoPage.SetActive(true);
        }
        else
        {
            //驗證是不是信箱格式
            if (!MailRegex.IsMatch(SignEmailInput.text))
            {
                SignEmailWrongPage.SetActive(true);
            }
            else
            {
                //確認兩次輸入正確
                if (SignPasswordInput.text != SignPasswordInput2.text)
                {
                    SignPasswordErrorPage.SetActive(true);
                }
                else
                {
                    SignupConfirmPage.SetActive(true);
                }
            }
        }
    }
    #region 確認新增
    public void SignConConfirmBtn_N()
    {
        StartCoroutine(ReadTeacherData_Sign());
    }
    public void SignConform()
    {
        bool isSigned = false;
        //確認帳號有沒有註冊過
        for (int i = 0; i < TeacherData.Length; i++)
        {
            SplitData = TeacherData[i].Split('/');
            if (SplitData[2] == SignEmailInput.text)
            {
                isSigned = true;
                break;
            }
        }
        if (isSigned)
        {
            SignAccountExistPage.SetActive(true);
        }
        else
        {
            LoadingPage.SetActive(true);
            StartCoroutine(WriteTeacherData());
        }
    }
    #endregion
    //取消註冊
    public void SignCancelBtn()
    {
        LoginPage.SetActive(true);
        SignupPage.SetActive(false);
        LoginEmailInput.text = "";
        LoginPasswordInput.text = "";
        LoginSchoolDropdown.value = 0;
    }

    #region 彈出視窗按鈕功能
    public void SignAddCancelBtn()
    {
        SignAddSchoolPage.SetActive(false);
        SignScoolDropDown.value = 1;
    }
    public void SignConCancelBtn()
    {
        SignupConfirmPage.SetActive(false);
    }
    public void SignPassErrorCloseBtn()
    {
        SignPasswordInput.text = "";
        SignPasswordInput2.text = "";
    }
    public void SignEmailWrongBtn()
    {
        SignEmailWrongPage.SetActive(false);
        SignEmailInput.text = "";
    }
    public void SignWriteInfoBtn()
    {
        SignWriteInfoPage.SetActive(false);
    }
    public void SignACExistBtn()
    {
        SignEmailInput.text = "";
        SignNameInput.text = "";
        SignPasswordInput.text = "";
        SignPasswordInput2.text = "";
        SignScoolDropDown.value = 1;
        SignAccountExistPage.SetActive(false);
        SignupConfirmPage.SetActive(false);
    }
    #endregion
    #endregion

    #region 查詢密碼功能
    #region 查詢功能按鈕
    public void FindPassBtn()
    {
        if (!MailRegex.IsMatch(FindPassEmailInput.text))
        {
            FindEmailWrongPage.SetActive(true);
            FindPassEmailInput.text = "";
        }
        else
        {
            LoadingPage.SetActive(true);
            StartCoroutine(ReadTeacherData_FindPass());
        }
    }
    public void CheckEmail()
    {
        //讀取教師資料
        string teacherrecord = "";
        bool findAccount = false;
        foreach (string line in TeacherData)
        {
            if (line.Contains(FindSchoolDropdown.options[FindSchoolDropdown.value].text) && line.Contains(FindPassEmailInput.text))
            {
                teacherrecord = line;
                findAccount = true;
                break;
            }
        }
        //確認帳號
        if (!findAccount)
        {
            FindNoAccountPage.SetActive(true);
        }
        else
        {
            string[] Infos = teacherrecord.Split('/');
            FindPassHint.GetComponent<Text>().text = "密碼為："+Infos[3];
            FindPassHint.SetActive(true);
            #region SendEmail_目前無法寄信
            //MailMessage mailMessage = new MailMessage();
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            ////  Attachment attachment = new Attachment(@"Assets/A.png");    //指定要夾帶的物件路徑
            //mailMessage.From = new MailAddress("cthps2022vr@gmail.com", "潮來潮往、永續共榮：海洋生物教育虛擬實境系統學習歷程系統", System.Text.Encoding.UTF8);
            //mailMessage.To.Add(FindPassEmailInput.text);
            ////信箱標題
            //mailMessage.Subject = "學習歷程系統密碼";
            ////信箱內文
            //mailMessage.Body = Infos[1] + "您好，此信件為潮來潮往、永續共榮：海洋生物教育虛擬實境系統學習歷程系統寄出，為保護您的隱私權及您的帳號，請確認開啟此信件時無他人一同觀看\n以下為您在此教師系統註冊時所使用之密碼：\n" + Infos[3];
            //mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            //mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            //mailMessage.Priority = MailPriority.High;
            //smtpClient.Port = 587;
            ////登入寄件人帳號(有輸入信箱及密碼)
            //smtpClient.Credentials = new NetworkCredential("cthps2022vr@gmail.com", "Cthps2022vr") as ICredentialsByHost;
            //smtpClient.EnableSsl = true;
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object sender,
            //                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //                                System.Security.Cryptography.X509Certificates.X509Chain chain,
            //                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            //smtpClient.Send(mailMessage);
            #endregion
            // Invoke("Send", 0.2f);
        }
    }
    public void Send()
    {
        FindPassHint.SetActive(true);
    }
    #endregion
    public void BackLoginBtn()
    {
        FindPassPage.SetActive(false);
        LoginPage.SetActive(true);
    }
    #region 彈出視窗
    public void FindNoAccountBtn()
    {
        FindNoAccountPage.SetActive(false);
    }
    public void FindWrongEmailBtn()
    {
        FindEmailWrongPage.SetActive(false);
    }
    #endregion
    #endregion

    #region 讀取資料功能
    #region 讀取學校名稱
    IEnumerator ReadSchoolData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyYvCzfpC9rkr4jxHa_70GTOMVKNuVd2ECVnxZFGTMLYQXPeAIJMnR1Yvv1Y1dyz7k2/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                SchoolOptions = new string[0];
                //下載的資料
                SchoolOptions = www.downloadHandler.text.Split(',');
                Invoke("LoadSchoolName", 0.1f);
                Debug.Log("Download School!");
            }
        }
    }
    #region 按筆畫(？)順序排列
    void LoadSchoolName()
    {
        string[] s = SchoolOptions;
        List<string> SchoolSorts = new List<string>();
        List<string> SchoolSorts_Sign = new List<string>();
        SchoolSorts_Sign.Add("新增學校...");
        //繁體中文排列
        CultureInfo StroCi = new CultureInfo("zh-tw", false);
        //設定目前執行緒的文化特性
        Thread.CurrentThread.CurrentCulture = StroCi;
        //排字串陣列中的項目
        Array.Sort(s);
        //i等於字串陣列的陣列值，i小於字串陣列的陣列值,i+1(迴圈)
        for (int i = s.GetLowerBound(0); i <= s.GetUpperBound(0); i++)
        {
            //添加字串陣列到List陣列內容
            SchoolSorts.Add(s.GetValue(i) + "");
            SchoolSorts_Sign.Add(s.GetValue(i) + "");
        }
        //更新學校下拉選單
        SignScoolDropDown.ClearOptions();
        LoginSchoolDropdown.ClearOptions();
        FindSchoolDropdown.ClearOptions();
        SignScoolDropDown.AddOptions(SchoolSorts_Sign);
        LoginSchoolDropdown.AddOptions(SchoolSorts);
        FindSchoolDropdown.AddOptions(SchoolSorts);
        if (isAddSchool)
        {
            for (int i = 0; i < SchoolSorts_Sign.Count; i++)
            {
                if (SchoolSorts_Sign[i] == SignAddSchoolInput.text)
                {
                    SignScoolDropDown.value = i;
                    break;
                }
            }
        }
        else
        {
            SignScoolDropDown.value = 1;
        }
        LoginSchoolDropdown.value = 0;
        FindSchoolDropdown.value = 0;
        LoadingPage.SetActive(false);
    }
    #endregion
    #endregion

    #region 讀取教師資料
    //註冊時讀取
    IEnumerator ReadTeacherData_Sign()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbx6Wa1Zq-tFCxUpvrwwxpptYnjeedx0_151b4KJUUJIHGXcQgj9rwC00p8tqCyttAIPag/exec", form_Read))
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
                TeacherData = www.downloadHandler.text.Split(',');
                SignConform();
                Debug.Log("Download School!");
            }
        }
    }
    //登入時讀取
    IEnumerator ReadTeacherData_Login()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbx6Wa1Zq-tFCxUpvrwwxpptYnjeedx0_151b4KJUUJIHGXcQgj9rwC00p8tqCyttAIPag/exec", form_Read))
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
                TeacherData = www.downloadHandler.text.Split(',');
                LoadingPage.SetActive(false);
                CheckLoginInfo();
                Debug.Log("Download School!");
            }
        }
    }
    //找密碼時讀取
    IEnumerator ReadTeacherData_FindPass()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbx6Wa1Zq-tFCxUpvrwwxpptYnjeedx0_151b4KJUUJIHGXcQgj9rwC00p8tqCyttAIPag/exec", form_Read))
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
                TeacherData = www.downloadHandler.text.Split(',');
                LoadingPage.SetActive(false);
                CheckEmail();
                Debug.Log("Download School!");
            }
        }
    }
    #endregion
    #endregion
    #region 寫入資料功能
    #region 寫入學校資料

    IEnumerator WriteToSheet_School()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        form.AddField("SchoolName", SignAddSchoolInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyYvCzfpC9rkr4jxHa_70GTOMVKNuVd2ECVnxZFGTMLYQXPeAIJMnR1Yvv1Y1dyz7k2/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("UploadSchool Complete!");
                //讀取學校名稱(更新學校名稱)
                isAddSchool = true;
                SignAddSchoolPage.SetActive(false);
                LoadingPage.SetActive(true);
                StartCoroutine(ReadSchoolData());
            }
        }
    }
    #endregion

    #region 寫入教師資料
    IEnumerator WriteTeacherData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //個人資訊用\t隔開
        form.AddField("TeacherData", SignScoolDropDown.options[SignScoolDropDown.value].text + "/" + SignNameInput.text + "/" + SignEmailInput.text + "/" + SignPasswordInput.text);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbx6Wa1Zq-tFCxUpvrwwxpptYnjeedx0_151b4KJUUJIHGXcQgj9rwC00p8tqCyttAIPag/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload TeacherData Complete!");
                //上傳成功
                LoadingPage.SetActive(false);
                SignupPage.SetActive(false);
                LoginPage.SetActive(true);
            }
        }
    }

    #endregion
    #endregion

}
