using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {
            
            InitializeComponent();
            create_array();
            create_button();
        }
        private void rand_num_create()
        {
            int[] iaRand_num = new int[6];
            int i = 0;
            int j = 1;
            Random cRandomobj = new Random();
            int iRandbuff = 0;
            button_clear();
            while (i<6)
            {
                iRandbuff = cRandomobj.Next(1,45);
                if (!Array.Exists(iaRand_num, x => x == iRandbuff))
                {
                    iaRand_num[i++] = iRandbuff;
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
                    caBox[j++].Text = string.Format("{0}", k);
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
          

        private void button_Click(object sender, RoutedEventArgs e)
        {
            rand_num_create();
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
            //for(int i=1; i<7; i++)
            //{
            //    caBox[i].Text = "";
            //}

            //for(int i=1; i<46; i++)
            //{
            //    if (baLotto_number[i]==true)
            //    {
            //        caBox[j++].Text = string.Format("{0}", i);
            //    }
            //}

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


    }
}
