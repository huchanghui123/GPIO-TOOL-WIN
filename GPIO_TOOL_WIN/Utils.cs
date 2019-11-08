using System;
using System.Collections.Generic;
using System.Windows;

namespace GPIO_TOOL_WIN
{
    class Utils
    {
        //把byte转化成2进制字符串 
        public static String ByteToBinaryStr(byte b)
        {
            String result = "";
            byte a = b;
            for (int i = 0; i < 8; i++)
            {
                result = (a % 2) + result;
                a = (byte)(a / 2);
            }
            return result;
        }

        //把二进制字符串转为16进制字符串
        public static String BinaryStrToHexStr(string bytestr)
        {
            string c = bytestr;
            try
            {
                c = Convert.ToUInt16(c, 2).ToString("x2");
            }
            catch (Exception)
            {
                c = "";
            }
            return c;
        }

        public static ushort StrToNum(string str)
        {
            ushort num = 0;
            try
            {
                num = Convert.ToUInt16(str, 16);
            }
            catch (Exception)
            {
                num = 999;
            }
            return num;
        }

        public static bool VerificationNumber(ushort number)
        {
            bool result = false;
            if (number ==0 || number == 1) {
                result = true;
            }
            return result;
        }

        public static void ChangeLaungage(string str)
        {
            string requestedCulture = "";
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
            if (str.Equals("en"))
            {
                requestedCulture = @"Resources\en-us.xaml";
            }
            else {
                requestedCulture = @"Resources\zh-cn.xaml";
            }
            ResourceDictionary resourceDictionary = dictionaryList.Find(d => d.Source.OriginalString.Equals(requestedCulture));
            Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}
