using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPIO_TOOL_WIN
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gpio GPIO = null;

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            this.Closing += Gpio_Closing;
            Init();
        }

        private void Gpio_Closing(object sender, CancelEventArgs e)
        {
            GPIO.ExitSuperIo();
            e.Cancel = false;
            Console.WriteLine("gpio tool exit...");
        }

        private void Init()
        {
            GPIO = new Gpio();
            bool initResult = GPIO.Initialize();
            if (!initResult)
            {
                error.Visibility = Visibility.Visible;
                mode_btn.IsEnabled = false;
                out_btn.IsEnabled = false;
            }
            else
            {
                GPIO.InitSuperIO();
                GPIO.InitGpioReg();
                chip_type.Content = GPIO.GetChipName();
                InitGpioModeAndVal();
                GPIO.ExitSuperIo();

                AddTextChangeEvent();
                
                chip_type.Visibility = Visibility.Visible;
                chip_name.Visibility = Visibility.Visible;
            }
        }

        private void InitGpioModeAndVal()
        {
            byte gp_mode = GPIO.ReadGpioMode();
            FormatGPIOModeVal(gp_mode);
            string str_mode = Utils.BinaryStrToHexStr(Utils.ByteToBinaryStr(gp_mode));
            mode_val.Text = str_mode;

            byte gp_value = GPIO.ReadGpioVal();
            FormatGPIOOutVal(gp_value);
            string str_val = Utils.BinaryStrToHexStr(Utils.ByteToBinaryStr(gp_value));
            out_val.Text = str_val;
        }
        

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.ShowDialog();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private void ModeTextChanged(object sender, TextChangedEventArgs e)
        {
            var t = sender as TextBox;
            TextBox tb = this.gpio_mode_grid.FindName(t.Name) as TextBox;
            Console.WriteLine("mode textbox:" + tb.Name + "----" + tb.Text);
            if (Utils.VerificationNumber(Utils.StrToNum(tb.Text)))
            {
                string mode = GetAllModeStr();
                Console.WriteLine("ModeTextChanged............" + mode);
                string str = Utils.BinaryStrToHexStr(mode);
                mode_val.Text = str;
            }
            else {
                MessageBox.Show("0 or 1");
                tb.TextChanged -= new TextChangedEventHandler(ModeTextChanged);
                tb.Text = "";
                tb.TextChanged += new TextChangedEventHandler(ModeTextChanged);
            }
        }

        private void OutTextChanged(object sender, TextChangedEventArgs e)
        {
            var t = sender as TextBox;
            TextBox tb = this.gpio_mode_grid.FindName(t.Name) as TextBox;
            Console.WriteLine("out textbox:" + tb.Name + "----" + tb.Text);
            if (Utils.VerificationNumber(Utils.StrToNum(tb.Text)))
            {
                string value = GetAllValueStr();
                Console.WriteLine("OutTextChanged............" + value);
                string str = Utils.BinaryStrToHexStr(value);
                out_val.Text = str;
            }
            else
            {
                MessageBox.Show("Please enter 0 or 1");
                tb.TextChanged -= new TextChangedEventHandler(OutTextChanged);
                tb.Text = "";
                tb.TextChanged += new TextChangedEventHandler(OutTextChanged);
            }
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            String value = mode_val.Text;
            InputGpioVal(value, "mode");
        }

        private void Out_Click(object sender, RoutedEventArgs e)
        {
            String value = out_val.Text;
            InputGpioVal(value, "");
        }

        private void InputGpioVal(String val, string type)
        {
            String value = val;
            try
            {
                byte gp_value = Convert.ToByte(value, 16);
                Console.WriteLine("gp value:" + gp_value);
                if (type == "mode")
                {
                    FormatGPIOModeVal(gp_value);
                    GPIO.InitSuperIO();
                    GPIO.SetGpioMode(gp_value);
                    GPIO.ExitSuperIo();
                }
                else
                {
                    FormatGPIOOutVal(gp_value);
                    GPIO.InitSuperIO();
                    GPIO.SetGpioOutValue(gp_value);
                    GPIO.ExitSuperIo();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter 0-255 hexadecimal data!");
                return;
            }
        }

        private void FormatGPIOModeVal(byte val)
        {
            String str = Utils.ByteToBinaryStr(val);
            Console.WriteLine("FormatGPIOModeVal-----:" + str);
            char[] gpio_vals = str.ToCharArray();
            gp80.Text = gpio_vals[0] + "";
            gp81.Text = gpio_vals[1] + "";
            gp82.Text = gpio_vals[2] + "";
            gp83.Text = gpio_vals[3] + "";
            gp84.Text = gpio_vals[4] + "";
            gp85.Text = gpio_vals[5] + "";
            gp86.Text = gpio_vals[6] + "";
            gp87.Text = gpio_vals[7] + "";
        }

        private void FormatGPIOOutVal(byte val)
        {
            String str = Utils.ByteToBinaryStr(val);
            Console.WriteLine("FormatGPIOOutVal-----:" + str);
            char[] gpio_vals = str.ToCharArray();
            gp80_out.Text = gpio_vals[0] + "";
            gp81_out.Text = gpio_vals[1] + "";
            gp82_out.Text = gpio_vals[2] + "";
            gp83_out.Text = gpio_vals[3] + "";
            gp84_out.Text = gpio_vals[4] + "";
            gp85_out.Text = gpio_vals[5] + "";
            gp86_out.Text = gpio_vals[6] + "";
            gp87_out.Text = gpio_vals[7] + "";
        }

        private void AddTextChangeEvent()
        {
            foreach (UIElement control in gpio_mode_grid.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.TextChanged += new TextChangedEventHandler(ModeTextChanged);
                }
            }
            foreach (UIElement control in gpio_value_grid.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.TextChanged += new TextChangedEventHandler(OutTextChanged);
                }
            }
        }

        private String GetAllModeStr()
        {
            StringBuilder sb = new StringBuilder(8);
            foreach (UIElement control in gpio_mode_grid.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    sb.Append(textBox.Text);
                }
            }
            return sb.ToString();     
        }

        private String GetAllValueStr()
        {
            StringBuilder sb = new StringBuilder(8);
            foreach (UIElement control in gpio_value_grid.Children)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    sb.Append(textBox.Text);
                }
            }
            return sb.ToString();
        }

        private void ChangeToCn(object sender, RoutedEventArgs e)
        {
            menu_cn.IsChecked = true;
            menu_en.IsChecked = false;

            Utils.ChangeLaungage("cn");
        }

        private void ChangeToEn(object sender, RoutedEventArgs e)
        {
            menu_en.IsChecked = true;
            menu_cn.IsChecked = false;

            Utils.ChangeLaungage("en");
        }

        
    }
}
