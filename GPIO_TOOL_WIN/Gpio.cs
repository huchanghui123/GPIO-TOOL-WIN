using System;

namespace GPIO_TOOL_WIN
{
    class Gpio
    {
        private static OpenLibSys.Ols MyOls;
        public static string gpbase = "a07";

        public bool Initialize()
        {
            MyOls = new OpenLibSys.Ols();
            return MyOls.GetStatus() == (uint)OpenLibSys.Ols.Status.NO_ERROR;
        }

        public void ExitSuperIo()
        {
            if (MyOls != null)
            {
                MyOls.WriteIoPortByte(0x2e, 0x02);
                MyOls.WriteIoPortByte(0x2f, 0x02);
            }
            
        }

        public void InitSuperIO()
        {
            MyOls.WriteIoPortByte(0x2e, 0x87);
            MyOls.WriteIoPortByte(0x2e, 0x01);
            MyOls.WriteIoPortByte(0x2e, 0x55);
            MyOls.WriteIoPortByte(0x2e, 0x55);
        }

        public void InitGpioReg()
        {
            //select logic device
            MyOls.WriteIoPortByte(0x2e, 0x07);
            MyOls.WriteIoPortByte(0x2f, 0x07);
            //Speecial Function Selection Register3(Index=2Ch, Default=89h)
            MyOls.WriteIoPortByte(0x2e, 0x2c);
            MyOls.WriteIoPortByte(0x2f, 0x89);
            Console.WriteLine("init gpio end ...");
        }

        public int SuperIoInw(byte data)
        {
            int val;
            MyOls.WriteIoPortByte(0x2e, data++);
            val = MyOls.ReadIoPortByte(0x2f) << 8;
            Console.WriteLine("SuperIo_Inw  val1:" + Convert.ToString(val, 16));
            MyOls.WriteIoPortByte(0x2e, data);
            val |= MyOls.ReadIoPortByte(0x2f);
            Console.WriteLine("SuperIo_Inw  val2:" + Convert.ToString(val, 16));
            return val;
        }

        public string GetChipName()
        {
            ushort chip_type;
            chip_type = (ushort)SuperIoInw(0x20);
            Console.WriteLine("chip type :" + Convert.ToString(chip_type, 16));
            return "IT" + Convert.ToString(chip_type, 16);
        }

        public byte ReadGpioMode()
        {
            byte b = 0;
            try
            {
                MyOls.WriteIoPortByte(0x2e, 0xcf);
                b = MyOls.ReadIoPortByte(0x2f);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occured:\n" + ex.Message);
            }
            return b;
        }

        public byte ReadGpioVal()
        {
            byte b = 0;
            try
            {
                //MyOls.WriteIoPortByte(0x2e, 0xcf);
                b = MyOls.ReadIoPortByte(Convert.ToUInt16(Gpio.gpbase, 16));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occured:\n" + ex.Message);
            }
            return b;
        }

        public void SetGpioMode(byte b)
        {
            try
            {
                MyOls.WriteIoPortByte(0x2e, 0xcf);
                MyOls.WriteIoPortByte(0x2f, b);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occured:\n" + ex.Message);
            }
            
        }

        public void SetGpioOutValue(byte b)
        {
            try
            {
                MyOls.WriteIoPortByte(Convert.ToUInt16(Gpio.gpbase, 16), b);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("An error occured:\n" + ex.Message);
            }
        }
    }
}
