using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllTestFunction : MonoBehaviour
{
    public MainData DataScript;
    public Scrollbar BigBar, SmallBar;
    public AllTestItem[] TestItem;
    //總次數
    public int[] TotalCount = new int[6];
    //總分
    public int[] TotalScore = new int[6];
    //總時間
    public int[] TotalTime = new int[6];
    public TestCount[] Count;
    [System.Serializable]
    public class TestCount
    {
        public int[] AllQuizCount;
        public int[] QuizCount; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SmallBar.value = BigBar.value;
    }
    public void SetTest()
    {
        for (int i = 0; i < 6; i++)
        {
            TotalCount[i] = 0;
            TotalScore[i] = 0;
            TotalTime[i] = 0;
            for (int j = 0; j < Count[i].AllQuizCount.Length; j++)
            {
                Count[i].AllQuizCount[j] = 0;
                Count[i].QuizCount[j] = 0;
            }
        }
        //全部加起來
        for (int i = 0; i < DataScript.AllData.Count; i++)
        {
            string[] Split = DataScript.AllData[i].Split('@');
            //增加總體資料
            //0:辨識單元(從第一單元開始)、6：辨識模式、8：總時間、9：分數
            if (Split[5] == "1")
            {
                TotalCount[int.Parse(Split[0]) - 1]++;
                TotalTime[int.Parse(Split[0]) - 1] += int.Parse(Split[8]);
                TotalScore[int.Parse(Split[0]) - 1] += int.Parse(Split[9].Split('_')[1]);
                //分類個題目
                //16：題目_答案，找正確的這個
                switch (int.Parse(Split[0]) - 1)
                {
                    #region 單元1
                    case 0:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("花色"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                            if (Split_[0].Contains("描述"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                            if (Split_[0].Contains("叫聲"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[2]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[2]++;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region 單元2
                    case 1:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("器官配對"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                            else
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region 單元3
                    case 2:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("器官配對"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                            else
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region 單元4
                    case 3:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("骨骼"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                            else
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region 單元5
                    case 4:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("生物連連看"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                            if (Split_[0].Contains("生活描述"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region 單元6
                    case 5:
                        for (int j = 0; j < 5; j++)
                        {
                            string[] Split_ = Split[16 + j].Split('_');
                            if (Split_[0].Contains("食物"))
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[1]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[1]++;
                                }
                            }
                            else
                            {
                                Count[int.Parse(Split[0]) - 1].AllQuizCount[0]++;
                                if (Split_[1] == "正確")
                                {
                                    Count[int.Parse(Split[0]) - 1].QuizCount[0]++;
                                }
                            }
                        }
                        break;
                        #endregion
                }
            }
        }
        //顯示文字
        for (int i = 0; i < TestItem.Length; i++)
        {
            if (TotalCount[i] != 0)
            {
                TestItem[i].ScoreText.text = (TotalScore[i] / TotalCount[i]).ToString();
                int Time = TotalTime[i] / TotalCount[i];
                TestItem[i].TimeText.text = (Time / 60).ToString("00") + ":" + (Time % 60).ToString("00");
                for (int j = 0; j < Count[i].AllQuizCount.Length; j++)
                {
                    if (Count[i].AllQuizCount[j] == 0)
                    {
                        TestItem[i].QuizObj[j].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0%";
                    }
                    else
                    {
                        float Percent = ((float)Count[i].QuizCount[j] / (float)Count[i].AllQuizCount[j]);
                        TestItem[i].QuizObj[j].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((int)(Percent * 100f)).ToString() + "%";
                    }

                }
            }
            else
            {
                TestItem[i].ScoreText.text = "0";
                TestItem[i].TimeText.text = "00:00";
            }
        }
        BigBar.value = 0;
        SmallBar.value = 0;
    }
}
