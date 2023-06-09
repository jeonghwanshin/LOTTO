﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf.Charts.Base;

namespace LOTTO
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[] baLotto_number = new bool[46];
        TextBox[] caBox = new TextBox[7];
        Button[] caButton = new Button[46];
        int iCount = 0;
        BackgroundWorker worker = null;
        //public ChartValues<int> Values { get; set; }

        public MainWindow()
        {            
            InitializeComponent();
            create_array();
            create_button();
            //DataContext = this;
            //Values = new ChartValues<int>();
        }
        private void rand_num_create(bool bAll_rand)
        {
            Random cRandomobj = new Random();
            int[] iaRand_num = new int[6];                        
            int iRandbuff = 0;            
            int iBoxCount = 1;
            if(bAll_rand==true)
            {
                button_clear();
                iCount = 0;
            }
            else if(iCount<6)
            {
                int iRandA_count_buff = 0;
                for (int k = 1; k < 46; k++)
                {
                    if (baLotto_number[k] == true)
                    {
                        iaRand_num[iRandA_count_buff++] = k;
                    }
                }
            }
            while (iCount < 6)
            {
                iRandbuff = cRandomobj.Next(1,46);
                if (!Array.Exists(iaRand_num, x => x == iRandbuff))
                {
                    iaRand_num[iCount++] = iRandbuff;
                    baLotto_number[iRandbuff] = true;
                    caButton[iRandbuff].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE67474"));
                }                
            }
            for (int k = 1; k < 7; k++)
            {
                caBox[k].Text = "";
            }
            for (int k = 1; k < 46; k++)
            {
                if (baLotto_number[k] == true)
                {
                    caBox[iBoxCount++].Text = string.Format("{0}", k);
                }
            }
        }
        private void button_clear()
        {
            for(int i=1; i<46; i++)
            {
                caButton[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdddddd"));
                baLotto_number[i] = false;
            }
        }
          

        private void buttonA1_Click(object sender, RoutedEventArgs e)
        {
            rand_num_create(true);
        }
        private void buttonA2_Click(object sender, RoutedEventArgs e)
        {
            rand_num_create(false);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Button cButton = (Button)sender;            
            int j = 1;
               
            if ((baLotto_number[int.Parse(cButton.Content.ToString())] == false) && (iCount<6 ))
            {
                baLotto_number[int.Parse(cButton.Content.ToString())] = true;
                cButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE67474"));
                iCount++;
            }
            else if ((baLotto_number[int.Parse(cButton.Content.ToString())] == true) && (iCount <= 6))
            {
                baLotto_number[int.Parse(cButton.Content.ToString())] = false;
                cButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdddddd"));
                iCount--;
            }            
            else
            {
                MessageBox.Show("6개 이상 고르지 마세요.");
            }
            for (int i = 1; i < 7; i++)
            {
                caBox[i].Text = "";
            }

            for (int i = 1; i < 46; i++)
            {
                if (baLotto_number[i] == true)
                {
                    caBox[j++].Text = string.Format("{0}", i);
                }
            }

        }

        private void create_button()
        {
            for (int i = 1; i < 46; i++)
            {
                caButton[i] = this.FindName("button" + i.ToString()) as Button;
            }
        }
        private void create_array()
        {
            for (int i = 1; i<7; i++)
            {
                caBox[i] = this.FindName("textBox" + i.ToString()) as TextBox;
            }          
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(textBox_num.Text)) && (int.TryParse(textBox_num.Text, out _)) && (!string.IsNullOrEmpty(textBox_num2.Text)) && (int.TryParse(textBox_num2.Text, out _)))
            {
                button.IsEnabled = false;
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_dowork;
                worker.ProgressChanged += worker_progress;
                worker.RunWorkerAsync();
            }
            else
            {
                textBox_json.Text = "제대로된 값을 입력하슈";
            }            
        }

        private void worker_dowork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int iStartnum = 0;
                int iEndnum = 0;
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    iStartnum = int.Parse(textBox_num.Text);
                    iEndnum = int.Parse(textBox_num2.Text);
                }));
                int iTotnum = iEndnum - iStartnum +1;
                int[] data = new int[46];

                for (int i = iStartnum; i < iEndnum + 1; i++)
                {
                    string strBuff = GetHttpString("https://www.dhlottery.co.kr/common.do?method=getLottoNumber&drwNo=" + i.ToString());
                    if (strBuff == "")
                    {
                        Debug.WriteLine("no data " + i.ToString() + "회차");
                    }
                    else
                    {
                        JObject obj = JObject.Parse(strBuff);
                        if (obj["returnValue"].ToString() == "success")
                        {
                            data[(int)obj["drwtNo1"]] += 1;
                            data[(int)obj["drwtNo2"]] += 1;
                            data[(int)obj["drwtNo3"]] += 1;
                            data[(int)obj["drwtNo4"]] += 1;
                            data[(int)obj["drwtNo5"]] += 1;
                            data[(int)obj["drwtNo6"]] += 1;
                        }
                        else
                        {
                            Debug.WriteLine("failed " + i.ToString() + "회차");
                        }
                    }
                    Debug.WriteLine(((float)i / iTotnum) * 100);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        prog_bar.Value = (int)(((float)i / iTotnum) * 100);
                    }));
                    //worker.ReportProgress((int)((float)i/iTotnum)*100);
                    Thread.Sleep(5);
                }
                data = data.Skip(1).ToArray();
                string s = String.Join(", ", data);                
                List<int> ints = data.ToList();
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    textBox_json.Text = s;
                    try
                    {
                        Chart.Series.Clear();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    
                    Chart.Series.Add(new LiveCharts.Wpf.ColumnSeries()
                    {
                        Title = "횟수",                        
                        Fill = Brushes.Orange,
                        Values = new LiveCharts.ChartValues<int>(ints)

                    }); ;

                    button.IsEnabled = true;
                    
                }));
                
                
            }  
            catch (Exception ex)
            {
                Debug.WriteLine (ex.ToString());   
            }
        }
        private void worker_progress(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                Debug.WriteLine(e.ProgressPercentage);
                prog_bar.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            { Debug.WriteLine (ex.ToString()); }
        }

        

        private string GetHttpString(string strUri)
        {
            string strRspText = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUri); // request 선언
            request.Method = "GET"; // 메소드 방식 설정 /get or post

            request.Timeout = 5 * 1000; // 타임아웃 설정 기본 ms 단위

            using (HttpWebResponse hwr = (HttpWebResponse)request.GetResponse()) // response 메소드로 전송및 응답 요청.
            {
                if(hwr.StatusCode == HttpStatusCode.OK) // 정상 동작
                {
                    Stream respStream = hwr.GetResponseStream(); // 스트림을 생성
                    using(StreamReader sr = new StreamReader(respStream)) // 스트림 리더로 데이터 읽을 준비 using 을 사용하는 이유는 자동으로 스트림을 닫기 위해
                    {
                        strRspText = sr.ReadToEnd(); // 텍스트 데이터를 얻음
                    }
                }
                else
                {
                    strRspText = "";
                }
            }
                return strRspText;
        }

    }
}


//string strBuff = GetHttpString("https://www.dhlottery.co.kr/common.do?method=getLottoNumber&drwNo=" + textBox_num.Text);

//if (strBuff == "")
//{
//    textBox_json.Text = "불러오기 실패";
//    return;
//}

//textBox_json.Text = "";
//JObject obj = JObject.Parse(strBuff);

//if (obj["returnValue"].ToString() == "success")
//{
//    textBox_json.Text += "날짜: " + obj["drwNoDate"].ToString() + "\n";
//    textBox_json.Text += "당첨번호: " + obj["drwtNo1"].ToString() + ", " + obj["drwtNo2"].ToString() + ", "
//        + obj["drwtNo3"].ToString() + ", " + obj["drwtNo4"].ToString() + ", " + obj["drwtNo5"].ToString() + ", "
//        + obj["drwtNo6"].ToString() + "\n";
//    Debug.WriteLine(obj["drwtNo3"].GetType().Name);
//}
//else
//{
//    textBox_json.Text = "불러오기 실패";
//}