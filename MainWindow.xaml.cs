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
        byte[] bLotto_number = new byte[46];
        TextBox[] aBox = new TextBox[7];
        Button[] aButton = new Button[46];
        
        public MainWindow()
        {
            
            InitializeComponent();
            create_array();
            create_button();
        }
        private void rand_num_create()
        {
            int[] rand_num = new int[6];
            int i = 0;
            int j = 1;
            Random randomobj = new Random();
            int randbuff = 0;
            button_clear();
            while (i<6)
            {
                randbuff = randomobj.Next(1,45);
                if (!Array.Exists(rand_num, x => x == randbuff))
                {
                    rand_num[i++] = randbuff;
                    bLotto_number[randbuff] = 1;
                    aButton[randbuff].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE67474"));
                }                
            }
            for (int k = 1; k < 7; k++)
            {
                aBox[k].Text = "";
            }
            for (int k = 1; k < 46; k++)
            {
                if (bLotto_number[k] > 0)
                {
                    aBox[j++].Text = string.Format("{0}", k);
                }
            }

        }
        private void button_clear()
        {
            for(int i=1; i<46; i++)
            {
                aButton[i].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdddddd"));
                bLotto_number[i] = 0;
            }
        }
          

        private void button_Click(object sender, RoutedEventArgs e)
        {
            rand_num_create();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int iCount = 0;
            int j = 1;
            for (int i = 0; i < 46; i++)
            {
                if (bLotto_number[i] > 0)
                {
                    iCount++;
                }
            }            
            if ((bLotto_number[int.Parse(button.Content.ToString())] == 0) && (iCount<6 ))
            {
                bLotto_number[int.Parse(button.Content.ToString())] = 1;
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE67474"));
            }
            else if ((bLotto_number[int.Parse(button.Content.ToString())] == 1) && (iCount <= 6))
            {
                bLotto_number[int.Parse(button.Content.ToString())] = 0;
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdddddd"));
            }            
            else
            {
                MessageBox.Show("6개 이상 고르지 마세요.");
            }
            for(int i=1; i<7; i++)
            {
                aBox[i].Text = "";
            }

            for(int i=1; i<46; i++)
            {
                if (bLotto_number[i]>0)
                {
                    aBox[j++].Text = string.Format("{0}", i);
                }
            }

        }

        private void create_button()
        {
            for (int i = 1; i < 46; i++)
            {
                aButton[i] = this.FindName("button" + i.ToString()) as Button;
            }
        }
        private void create_array()
        {
            for (int i = 1; i<7; i++)
            {
                aBox[i] = this.FindName("textBox" + i.ToString()) as TextBox;
            }          
        }


    }
}
