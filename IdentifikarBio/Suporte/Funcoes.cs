using IdentifikarBio.Database.Objetos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    public class Funcoes
    {

        public static String formataHora(DateTime now)
        {
            now = new DateTime();
            String[] dt = now.ToString().Split(' ');
            string data = dt[0];
            string hora = dt[1];
            return hora;
        }

        public static String formataData(String dataOld)
        {
            String[] dt = dataOld.Split('/');
            String dia = dt[0];
            String mes = dt[1];
            String ano = dt[2];

            String[] ldias = {"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                              "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                              "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                              "31"};

            dia = ldias[Convert.ToInt32(dia)];
            mes = ldias[Convert.ToInt32(mes)];
            //MessageBox.Show("DATAOLD: " + dataOld + " DataNova:" + ano + "-" + mes + "-" + dia);
            return ano + "-" + mes + "-" + dia;
        }

        public static String formataDataMobile(DateTime dt)
        {
            string ano = Convert.ToString(dt.Year);
            string mes = Convert.ToString(dt.Month).PadLeft(2, '0');
            string dia = Convert.ToString(dt.Day).PadLeft(2, '0');
            return mes + "/" + dia + "/" + ano;
        }

        public static String formataDataHora(String dataOld)
        {

            String[] dt = dataOld.Split(' ');
            String data = dt[0];
            String tempo = dt[1];

            dt = data.Split('/');
            String dia = dt[0];
            String mes = dt[1];
            String ano = dt[2];

            String[] ldias = {"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
                              "11", "12", "13", "14", "15", "16", "17", "18", "19", "20",
                              "21", "22", "23", "24", "25", "26", "27", "28", "29", "30",
                              "31"};

            dia = ldias[Convert.ToInt32(dia)];
            mes = ldias[Convert.ToInt32(mes)];
            //MessageBox.Show("DATAOLD: " + dataOld + " Data Nova: " + ano + "-" + mes + "-" + dia + " " + tempo);
            return ano + "-" + mes + "-" + dia + " " + tempo;
        }

        public static String formataDinheiro(String valor)
        {
            String dindin = String.Format("{0:C}", Convert.ToDecimal(valor)) + "000";
            //MessageBox.Show(valor + " - " + dindin);
            return dindin;
        }
        

        public static String formataIdItem(int id)
        {
            String valor = "" + id;

            while (valor.Length < 4) valor = "0" + valor;
            //MessageBox.Show(valor + " - " + dindin);
            return valor;
        }

        public static String FormataCpfCnpj(String dado)
        {
            if (dado.Length == 11)
            {
                return String.Format(@"{0:000\.000\.000\-00}", long.Parse(dado));
            }
            else
            {
                return String.Format(@"{0:00\.000\.000\/0000\-00}", long.Parse(dado));
            }
        }

        public static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string formata2decimal(string text)
        {
            if (text.Contains(","))
            {
                String[] valores = text.Split(',');

                while (valores[1].Length < 2) valores[1] += "0";

                return valores[0] + "," + valores[1];
            }
            else return text;

        }

        public double ConvertDegreeAngleToDouble(string point)
        {
            //Example: 17.21.18S

            var multiplier = (point.Contains("S") || point.Contains("W")) ? -1 : 1; //handle south and west

            point = Regex.Replace(point, "[^0-9.]", ""); //remove the characters

            var pointArray = point.Split('.'); //split the string.

            //Decimal degrees = 
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600

            var degrees = Double.Parse(pointArray[0]);
            var minutes = Double.Parse(pointArray[1]) / 60;
            var seconds = Double.Parse(pointArray[2]) / 3600;

            return (degrees + minutes + seconds) * multiplier;
        }


        public static string GetDDCoord(string coord, string sentido, LogFile log)
        {
            var multiplier = (sentido.Contains("S") || sentido.Contains("W")) ? -1 : 1; //handle south and west

            var sdegrees = "";
            var sminutes = "";
            var sseconds = "0";
            //04644.4161
            if (coord.Length == 10) //longitude
            {

                sdegrees = coord.Substring(0, 3);

                sminutes = coord.Substring(3);

                //sseconds = coord.Substring(6, 4);// + "," + coord.Substring(8);

            }
            else //latitude 2336.1702
            {

                sdegrees = coord.Substring(0, 2);

                sminutes = coord.Substring(2);

                //sseconds = coord.Substring(5, 4);// +"," + coord.Substring(7);

                ///Console.WriteLine(sdegrees + " " + sminutes + " " + sseconds);
            }

            var degrees = Convert.ToDouble(sdegrees.Replace(".", ","));

            var minutes = Double.Parse(sminutes.Replace(".", ",")) / 60;
            //var seconds = Math.Round(Double.Parse("0," + sseconds) * 60, 0) / 3600;

            log.write(sdegrees + " " + degrees + " | " + sminutes + " " + minutes);

            double dd = (degrees + minutes) * multiplier;

            return Convert.ToString(dd).Replace(",", ".");

        }


        public static string GetDataAtual()
        {
            var da = DateTime.Now;

            return da.Year + "-" + Convert.ToString(da.Month).PadLeft(2, '0') + "-" + Convert.ToString(da.Day).PadLeft(2, '0') + " " + Convert.ToString(da.Hour).PadLeft(2, '0') + ":" + Convert.ToString(da.Minute).PadLeft(2, '0') + ":" + Convert.ToString(da.Second).PadLeft(2, '0');
        }


        public static string FormataExcecao(Exception e)
        {
            var msg = e.HelpLink + "-" + e.Message + " S[" + e.StackTrace + "]" + "\r\n";
            var inner = e.InnerException;
            while (inner != null)
            {
                msg += "Inner: " + inner.Message + "\r\n";
                inner = inner.InnerException;
            }

            return msg;
        }

        public static bool ValidateCPF(string cpf)
        {
            if (String.IsNullOrEmpty(cpf))
                return true;

            string value = cpf.Replace(",", "");

            value = value.Replace("-", "");

            if (value.Length != 11)
                return false;

            bool equals = true;

            for (int i = 1; i < 11 && equals; i++)
                if (value[i] != value[0])
                    equals = false;

            if (equals || value == "12345678909")
                return false;

            int[] numbers = new int[11];

            for (int i = 0; i < 11; i++)
                numbers[i] = int.Parse(value[i].ToString());

            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += (10 - i) * numbers[i];

            int result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }
            else if (numbers[9] != 11 - result)
            {
                return false;
            }

            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += (11 - i) * numbers[i];

            result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else if (numbers[10] != 11 - result)
                return false;

            return true;
        }

        public static bool ValidateCNPJ(string cnpj)
        {
            if (String.IsNullOrEmpty(cnpj))
                return true;

            int[] multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum;
            int remainder;
            string digit;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];
            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = remainder.ToString();
            tempCnpj = tempCnpj + digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;
            digit = digit + remainder.ToString();
            return cnpj.EndsWith(digit);
        }

        public static ObProxy getUserProxy()
        {
            ObProxy obProxy = null;
            try
            {
                if (File.Exists(".\\Proxy.conf"))
                {
                    var jsce = File.ReadAllText(".\\Proxy.conf");
                    if (!String.IsNullOrEmpty(jsce))
                    {
                        var json = Funcoes.Decrypt(jsce, true);
                        obProxy = JsonConvert.DeserializeObject<ObProxy>(json);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return obProxy;
        }

        public static string Encrypt(string secureUserData, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(secureUserData);
            string key = string.Empty;
            byte[] resultArray;

            // Get the key from Web.Config file
            //key = ConfigurationManager.AppSettings.Get("EncKey");
            key = "MAKV2SPBNI99212";


            if (useHashing)
            {

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] resultArray;
            string key = string.Empty;

            //key = ConfigurationManager.AppSettings.Get("SecurityKey"); 
            key = "MAKV2SPBNI99212";


            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;


            ICryptoTransform cTransform = tdes.CreateDecryptor();
            resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);


            tdes.Clear();


            return UTF8Encoding.UTF8.GetString(resultArray);

        }

    }
}
