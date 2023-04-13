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
        TextBox[] aBox = new TextBox[0];
        public MainWindow()
        {
            box_create();
            InitializeComponent();
        }
        
        private void box_create()
        {
            aBox[0] = textBox1;
            aBox[1] = textBox2;
            aBox[2] = textBox3;
            aBox[3] = textBox4;
            aBox[4] = textBox5;
            aBox[5] = textBox6;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int iCount = 0;
            int j = 0;
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
            
            for(int i=0; i<46; i++)
            {
                if (bLotto_number[i]>0)
                {
                    aBox[j++].Text(i.ToString());
                }
            }

        }
    }
}
