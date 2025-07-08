using TadesApi.Core;
using TadesApi.Core.Models;
using TadesApi.Core.Models.Global;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TadesApi.Core.Models.Security;
using System.Net.Mail;

namespace TadesApi.CoreHelper
{
    public static class CommonFunctions
    {
        public static DateTime Now
        {
            get { return DateTime.Now; }
        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
            var d = from source in target.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object Value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                Value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(x, Value, null);
            }

            return (T)x;
        }

        public static string StartsWith(string srcName)
        {
            return string.Format("{0}%", srcName);
        }

        public static string EndWith(string srcName)
        {
            return string.Format("%{0}", srcName);
        }

        public static string Like(string srcName)
        {
            return string.Format("%{0}%", srcName);
        }

        public static List<string> GetConstraintFields<T>()
        {
            var props = typeof(T).GetProperties();
            List<string> fields = new List<string>();
            foreach (var item in props)
            {
                var attr = GetFrontFieldAttrib(item);
                if (attr != null)
                {
                    if (attr.IsContraint)
                    {
                        fields.Add(item.Name);
                    }
                }
            }

            return fields;
        }

        public static List<string> GetReadonlyFields<T>()
        {
            var props = typeof(T).GetProperties();
            List<string> fields = new List<string>();
            foreach (var item in props)
            {
                var attr = GetFrontFieldAttrib(item);
                if (attr != null)
                {
                    if (attr.IsReadOnly)
                    {
                        fields.Add(item.Name);
                    }
                }
            }

            return fields;
        }

        public static MaintenanceTableAttr GetTableAttr<T>()
        {
            string tableName = typeof(T).Name.Replace("Transformation", "");
            var customAttributes = typeof(T).GetCustomAttributes(typeof(TrfAttrAttribute), false);
            MaintenanceTableAttr attr = new MaintenanceTableAttr();

            attr.TableName = tableName;
            attr.KeyField = "Id";

            if (customAttributes.Count() > 0)
            {
                var atrrProp = (customAttributes.First() as TrfAttrAttribute);
                attr.TableName = atrrProp.TableName ?? tableName;
                attr.DefaultEditMode = atrrProp.DefaultEditMode;
                attr.IsGotoList = atrrProp.IsGotoList;
                attr.KeyField = atrrProp.KeyField;
            }


            return attr;
        }

        public static FrontAttrAttribute GetFrontFieldAttrib(PropertyInfo prop)
        {
            var attributes = Attribute.GetCustomAttributes(prop);

            if (attributes.Any())
                return attributes[0] as FrontAttrAttribute;
            return null;
        }

        public static FrontAttrAttribute GetFrontFieldAttrib<T>(string propName)
        {
            var attributes = Attribute.GetCustomAttributes(typeof(T).GetProperty(propName));

            if (attributes.Any())
                return attributes[0] as FrontAttrAttribute;
            return null;
        }

        public static FieldAttrAttribute GetFieldAttrib(MemberInfo memberInfo)
        {
            var attributes = Attribute.GetCustomAttributes(memberInfo);

            if (attributes.Any())
                return attributes[0] as FieldAttrAttribute;
            return null;
        }

        //***************************** List<T>  ***********************************
        public static T XAddItem<T>(this List<T> list) where T : new()
        {
            var obj = new T();
            list.Add(obj);
            return obj;
        }

        public static List<T> XAdd<T>(this List<T> list, T obj)
        {
            list.Add(obj);
            return list;
        }

        //***********************  Constants **********************
        public static string Endl = Environment.NewLine;
        public static string Br = "<br />" + Endl;
        public static string DateFmt = "MM.DD.YYYY";
        public static string DateSep = ".";
        public static string NumDecsep = ",";
        public static string NumThousep = ".";
        public static string DefText = "";
        public static DateTime DefDate = DateTime.MinValue;
        public static decimal DefNum = 0M;

        //***************************** String ***********************************
        public static string ToUpper(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.ToUpper(new CultureInfo("en-US"));
        }

        public static string ToLower(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.ToLower(new CultureInfo("en-US"));
        }

        public static string Replace(string str, string oFind, string oReplace, StringComparison xctype = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            var iPos = str.IndexOf(oFind, xctype);
            var strReturn = "";
            while (iPos != -1)
            {
                strReturn += str.Substring(0, iPos) + oReplace;
                str = str.Substring(iPos + oFind.Length);
                iPos = str.IndexOf(oFind, xctype);
            }

            if (str.Length > 0)
                strReturn += str;
            return strReturn;
        }

        public static Dictionary<int, string> MonthList()
        {
            var months = new Dictionary<int, string>()
            {
                { 1, "OCAK" },
                { 2, "ŞUBAT" },
                { 3, "MART" },
                { 4, "NİSAN" },
                { 5, "MAYIS" },
                { 6, "HAZİRAN" },
                { 7, "TEMMUZ" },
                { 8, "AĞUSTOS" },
                { 9, "EYLÜL" },
                { 10, "EKİM" },
                { 11, "KASIM" },
                { 12, "ARALIK" },
            };
            return months;
        }

        public static string GetFirstNonEmptyString(params string[] items)
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                    return item;
            }

            return "";
        }

        public static void RemoveAction(this List<CustomAction> customActions, string actionName)
        {
            var record = customActions.Where(x => x.actionName == actionName).FirstOrDefault();
            if (record == null)
                return;
            customActions.Remove(record);
        }

        public static void SignAction(this List<CustomAction> customActions, string actionName)
        {
            var record = customActions.Where(x => x.actionName == actionName).FirstOrDefault();
            if (record == null)
                return;
            customActions.Where(x => x.actionName == actionName).FirstOrDefault().isAvaliable = true;
        }

        public static void SignAction(this List<CustomAction> customActions, bool state, string actionName)
        {
            if (!state)
            {
                return;
            }

            var record = customActions.Where(x => x.actionName == actionName).FirstOrDefault();
            if (record == null)
                return;
            customActions.Where(x => x.actionName == actionName).FirstOrDefault().isAvaliable = true;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            var i = 0;
            foreach (var ch in str)
            {
                i++;

                if (ch == '-' && i != 1)
                    return false;

                if (!_numchars.Contains(ch))
                    return false;
            }

            return true;
        }

        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static IEnumerable<int> GetAllEmptyIndexes(this string source, string matchString = " ")
        {
            matchString = Regex.Escape(matchString);
            foreach (Match match in Regex.Matches(source, matchString))
                yield return match.Index;
        }

        private static IEnumerable<int> FindIndexes(string text, string query)
        {
            return Enumerable.Range(0, text.Length - query.Length)
                .Where(i => query.Equals(text.Substring(i, query.Length)));
        }

        public static string StrRightBack(string str1, string str2)
        {
            var pos1 = InStrRev(str1, str2);
            if (pos1 == 0) return "";
            return Right(str1, str1.Length - pos1 - 1);
        }

        public static string StrLeft(string str, string str2)
        {
            var pos1 = InStr(str, str2);
            if (pos1 == 0) return "";
            return Left(str, pos1 - 1);
        }

        public static string StrRight(string str, string str2)
        {
            var pos1 = InStr(str, str2);
            if (pos1 == 0) return "";
            return Right(str, Len(str) - pos1 - Len(str2) + 1);
        }

        public static string[] Split(string str, string sep)
        {
            if (str == null) str = "";
            return str.Split(new[] { sep }, StringSplitOptions.None);
        }

        public static string Left(string str, int len)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (len > str.Length) return str;
            return str.Substring(0, len);
        }

        public static string Right(string str, int len)
        {
            return str.Substring(str.Length - len);
        }

        public static string Mid(string str, int start, int len)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (len > str.Length) return str;
            return str.Substring(start - 1, len);
        }

        public static int InStr(string str, string str2)
        {
            return str.IndexOf(str2) + 1;
        }

        public static int Len(string str)
        {
            return str.Length;
        }

        public static int InStrRev(string str1, string str2)
        {
            return str1.LastIndexOf(str2);
        }

        public static string XGetCode(this string tval)
        {
            return GetCode(tval);
        }

        public static string XGetCodes(this string tval)
        {
            return GetCodes(tval);
        }

        public static string XStrip(this string tval, Regex regex)
        {
            tval = regex.Replace(tval, "");
            return tval;
        }

        public static List<string> XSplit(this string tval, int len)
        {
            var list = new List<string>();

            if (string.IsNullOrEmpty(tval))
                return list;

            if (len < 1)
                return list;

            for (var i = 0; i < tval.Length; i += len)
            {
                if (len + i > tval.Length)
                    len = tval.Length - i;

                list.Add(tval.Substring(i, len));
            }

            return list;
        }

        public static ExcellFldAttrAttribute GetExcellFieldAttrib(MemberInfo memberInfo)
        {
            var attributes = Attribute.GetCustomAttributes(memberInfo);

            if (attributes.Any())
                return attributes[0] as ExcellFldAttrAttribute;
            return null;
        }

        public static decimal ToNum(object Value)
        {
            if (Value == null)
                return 0;

            if (Value is decimal)
                return (decimal)Value;

            if (Value is short || Value is int || Value is long)
                return Convert.ToDecimal(Value);

            var strVal = _clear(Value.ToString(), _numchars);

            try
            {
                return decimal.Parse(strVal);
            }
            catch (Exception)
            {
                return 0M;
            }
        }

        public static int ToIntWithoutChar(object Value)
        {
            if (Value == null)
                return 0;

            if (Value is int)
                return (int)Value;

            if (Value is short || Value is int || Value is long)
                return Convert.ToInt32(Value);

            var strVal = _clear(Value.ToString(), _numchars);

            try
            {
                return int.Parse(strVal);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long ToLong(object Value)
        {
            if (Value is Int64)
                return (Int64)Value;
            if (Value == null)
                return 0;

            return (Int64)ToNum(Value);
        }

        public static int ToInt(object Value)
        {
            if (Value is Int32)
                return (Int32)Value;
            if (Value == null)
                return 0;

            return (Int32)ToNum(Value);
        }

        public static long ToInt64(this string Value)
        {
            return (Int64)ToNum(Value);
        }

        private static char[] _numchars = new[] { '.', '-', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ',' };

        private static string _clear(string str, Char[] chars)
        {
            var newstr = "";
            foreach (var ch in str)
            {
                if (chars.Any(x => x == ch))
                    newstr += ch;
            }

            return newstr;
        }

        public static string XBase64Encode(this string tval)
        {
            return Base64Encode(tval);
        }

        public static string XTrim(this string tval)
        {
            if (string.IsNullOrEmpty(tval))
                return "";
            return tval.Trim();
        }

        public static string XBase64Decode(this string tval)
        {
            return Base64Decode(tval);
        }

        public static string GetRandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }

        //***********************  Eforms Common Functions **********************
        public static string GetCode(string mstr)
        {
            return (mstr.IndexOf("-") > -1) ? StrLeft(mstr, "-") : mstr;
        }

        public static string GetCodes(string mstr)
        {
            var retval = "";
            var arr = mstr.XSplit(";");
            foreach (var str in arr)
            {
                XstrAppend(ref retval, GetCode(str));
            }

            return retval;
        }

        public static void XstrAppend(ref string mstr, string str1)
        {
            if (string.IsNullOrEmpty(mstr))
                mstr = str1;
            else
                mstr = mstr + ";" + str1;
        }

        public static string[] XstrToArr(string xstr)
        {
            if (string.IsNullOrEmpty(xstr))
                return (new[] { "" });

            return xstr.Split(new[] { ";" }, StringSplitOptions.None);
        }

        public static string ArrToXstr(string[] arr)
        {
            var retval = "";

            foreach (var xstr in arr)
            {
                if (retval == "")
                    retval = xstr;
                else
                    retval += ";" + xstr;
            }

            return retval;
        }

        public static string XstrSort(string str1)
        {
            var xarr = Split(str1, ";");
            Array.Sort(xarr);
            var retval = string.Join(";", xarr);
            return retval;
        }

        public static bool IsError(string xstr)
        {
            return (Left(xstr, 8).ToUpper() == "$ERROR$-" ? true : false);
        }

        public static void AppendText2(ref string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1))
                str1 = str2;
            else
                str1 = str1 + Endl + str2;
        }

        public static bool CheckSysId(string xsysid)
        {
            return ((Len(xsysid) == 24 | Len(xsysid) == 32)) ? true : false;
        }

        //***************************** Double ***********************************
        public static double XFirst(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.First();
        }

        public static double XMax(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Max();
        }

        public static double XMin(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Min();
        }

        public static double XSum(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Sum();
        }

        //------- Double-II
        public static string ToLocalStr(this decimal thisObj, string format = "")
        {
            return CnvtNumberToText(thisObj, format);
        }

        public static decimal ToDecimal(this decimal thisObj, string format)
        {
            return (decimal)thisObj.ToString(format).ToNum();
        }

        //***************************** Other ***********************************
        public static string ToStr(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }


        //***********************  Date/Number/Text Convertions **********************
        public static string CnvtDateToText(DateTime? dt, string stfmt)
        {
            if (dt == null)
                return "";

            if (dt.Value == DefDate)
                return "";

            var yyyy = dt.Value.Year.ToString("0000");
            var MM = dt.Value.Month.ToString("00");
            var dd = dt.Value.Day.ToString("00");
            var hh = dt.Value.Hour.ToString("00");
            var mm = dt.Value.Minute.ToString("00");
            var ss = dt.Value.Second.ToString("00");
            var fff = dt.Value.Millisecond.ToString("000");

            var retval = "";
            retval = Replace(DateFmt, ".", DateSep);

            if (!string.IsNullOrEmpty(stfmt))
                retval = retval + " " + stfmt;

            retval = Replace(retval, "YYYY", yyyy);
            retval = Replace(retval, "MM", MM);
            retval = Replace(retval, "DD", dd);
            retval = Replace(retval, "HH", hh);
            retval = Replace(retval, "NN", mm);
            retval = Replace(retval, "SS", ss);
            retval = Replace(retval, "FFF", fff);
            return retval;
        }

        public static string CnvtNumberToText(decimal? sval, string sformat)
        {
            if (sval == null) return "";

            var retval = string.IsNullOrEmpty(sformat) ? Convert.ToString(sval) : string.Format("{0:" + sformat + "}", sval);
            var xds = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            var xts = NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

            if (!string.IsNullOrEmpty(xds)) retval = Replace(retval, xds, "%D");
            if (!string.IsNullOrEmpty(xts)) retval = Replace(retval, xts, "%T");
            retval = Replace(retval, "%D", NumDecsep);
            retval = Replace(retval, "%T", NumThousep);

            return retval;
        }

        public static DateTime? CnvtTextToDate(string sval)
        {
            if (string.IsNullOrEmpty(sval))
                return null;

            switch (sval.Length)
            {
                case 10:
                    sval = sval + " 00:00:00";
                    break;
                case 16:
                    sval = sval + ":00";
                    break;
                case 19:
                    sval = sval + "";
                    break;

                default:
                    return null;
            }

            string tFmt = DateFmt + " HH:NN:SS";
            int xy = int.Parse(Mid(sval, InStr(tFmt, "YYYY"), 4));
            int xm = int.Parse(Mid(sval, InStr(tFmt, "MM"), 2));
            int xd = int.Parse(Mid(sval, InStr(tFmt, "DD"), 2));
            int xh = int.Parse(Mid(sval, InStr(tFmt, "HH"), 2));
            int xn = int.Parse(Mid(sval, InStr(tFmt, "NN"), 2));
            int xs = int.Parse(Mid(sval, InStr(tFmt, "SS"), 2));

            if (xh >= 24 || xh < 0) xh = 0;
            if (xn >= 60 || xn < 0) xn = 0;
            if (xs >= 60 || xs < 0) xs = 0;

            return (new DateTime(xy, xm, xd, xh, xn, xs));
        }

        public static decimal CnvtTextToNumber(string mstr)
        {
            if (string.IsNullOrEmpty(mstr))
                return 0M;

            var retstr = "";
            for (var i = 1; i <= Len(mstr); i++)
            {
                var xchar = Mid(mstr, i, 1);
                if (InStr("-0123456789" + NumDecsep, xchar) > 0)
                    retstr = retstr + xchar;
            }

            if (retstr == "")
                return 0;

            var retval = Convert.ToDecimal(retstr, new NumberFormatInfo { NumberDecimalSeparator = NumDecsep });
            return retval;
        }

        //public static DateTime? BeginOfDay(this DateTime? date)
        //{
        //    return date.ToString("yyyy-MM-dd 00:00:00.000").ToDate();
        //}
        //public static DateTime? EndOfDay(this DateTime? date)
        //{
        //    return date.ToString("yyyy-MM-dd 23:59:59.998").ToDate();
        //}

        public static DateTime BeginOfDay(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd 00:00:00.000").ToDate();
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd 23:59:59.998").ToDate();
        }

        public static DateTime? YearFirstDay(int year)
        {
            var date = new DateTime(year, 1, 1);

            return date.ToString("yyyy-MM-dd 00:00:00.000").ToDate();
        }

        public static DateTime? YearEndDay(int year)
        {
            var date = new DateTime(year, 12, 31);

            return date.ToString("yyyy-MM-dd 23:59:59.998").ToDate();
        }

        public static DateTime? BeginOfDay(this DateTime? date)
        {
            if (date == null)
                return null;
            return date?.ToString("yyyy-MM-dd 00:00:00.000").ToDate();
        }

        public static DateTime? EndOfDay(this DateTime? date)
        {
            if (date == null)
                return null;
            return date?.ToString("yyyy-MM-dd 23:59:59.998").ToDate();
        }

        public static DateTime? BeginOfMonth(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateTime(date?.Year ?? 0, date?.Month ?? 0, 1);
        }

        public static DateTime? EndOfMonth(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateTime(date?.Year ?? 0, date?.Month ?? 0, 1).AddMonths(1).AddDays(-1);
        }

        public static DateTime BeginOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static DateTime? BeginOfYear(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateTime(date?.Year ?? 0, 1, 1);
        }

        public static DateTime? EndOfYear(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateTime(date?.Year ?? 0, 1, 1).AddYears(1).AddDays(-1);
        }

        public static DateTime BeginOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1).AddYears(1).AddDays(-1);
        }

        public static string ParayiYaziyaCevir(string gelentutar, string dovizTuru = "LİRA")
        {
            if (gelentutar.IsInitial())
                return "";

            decimal dectutar = Convert.ToDecimal(gelentutar);
            string strTutar = dectutar.ToString("F2").Replace('.', ','); // Replace('.',',') ondalık ayraç ayracı           
            string lira = strTutar.Substring(0, strTutar.IndexOf(',')); //tutarın lira kısmı
            string kurus = strTutar.Substring(strTutar.IndexOf(',') + 1, 2);
            string yazi = "";
            string[] birler = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
            string[] onlar = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
            string[] binler = { "KATİRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" }; //KATRİLYON'un önüne ekleme yapılarak artırabilir.
            int grupSayisi = 6;
            lira = lira.PadLeft(grupSayisi * 3, '0'); //sayının soluna '0' eklenerek sayı 'grup sayısı x 3' basakmaklı yapılıyor.
            string grupDegeri;
            for (int i = 0; i < grupSayisi * 3; i += 3) //sayı 3'erli gruplar halinde ele alınıyor.
            {
                grupDegeri = "";
                if (lira.Substring(i, 1) != "0")
                    grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ"; //yüzler
                if (grupDegeri == "BİRYÜZ") //biryüz düzeltiliyor.
                    grupDegeri = "YÜZ";
                grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))]; //onlar
                grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))]; //birler
                if (grupDegeri != "") //binler
                    grupDegeri += "" + binler[i / 3];
                if (grupDegeri == "BİRBİN") //birbin düzeltiliyor.
                    grupDegeri = "BİN";
                yazi += grupDegeri;
            }

            if (yazi != "")
                yazi += $" {dovizTuru} ";
            int yaziUzunlugu = yazi.Length;
            if (kurus.Substring(0, 1) != "0") //kuruş onlar
                yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];
            if (kurus.Substring(1, 1) != "0") //kuruş birler
                yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];
            if (yazi.Length > yaziUzunlugu)
                yazi += " KURUŞ";
            else
                yazi += "";
            return yazi;
        }
        //******************** File **************************

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();

            var ext = Path.GetExtension(path).Replace(".", "");

            //var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { "ai", "application/postscript" },
                { "aif", "audio/x-aiff" },
                { "aifc", "audio/x-aiff" },
                { "aiff", "audio/x-aiff" },
                { "asc", "text/plain" },
                { "atom", "application/atom+xml" },
                { "au", "audio/basic" },
                { "avi", "video/x-msvideo" },
                { "bcpio", "application/x-bcpio" },
                { "bin", "application/octet-stream" },
                { "bmp", "image/bmp" },
                { "cdf", "application/x-netcdf" },
                { "cgm", "image/cgm" },
                { "class", "application/octet-stream" },
                { "cpio", "application/x-cpio" },
                { "cpt", "application/mac-compactpro" },
                { "csh", "application/x-csh" },
                { "css", "text/css" },
                { "dcr", "application/x-director" },
                { "dif", "video/x-dv" },
                { "dir", "application/x-director" },
                { "djv", "image/vnd.djvu" },
                { "djvu", "image/vnd.djvu" },
                { "dll", "application/octet-stream" },
                { "dmg", "application/octet-stream" },
                { "dms", "application/octet-stream" },
                { "doc", "application/msword" },
                { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
                { "docm", "application/vnd.ms-word.document.macroEnabled.12" },
                { "dotm", "application/vnd.ms-word.template.macroEnabled.12" },
                { "dtd", "application/xml-dtd" },
                { "dv", "video/x-dv" },
                { "dvi", "application/x-dvi" },
                { "dxr", "application/x-director" },
                { "eps", "application/postscript" },
                { "etx", "text/x-setext" },
                { "exe", "application/octet-stream" },
                { "ez", "application/andrew-inset" },
                { "gif", "image/gif" },
                { "gram", "application/srgs" },
                { "grxml", "application/srgs+xml" },
                { "gtar", "application/x-gtar" },
                { "hdf", "application/x-hdf" },
                { "hqx", "application/mac-binhex40" },
                { "htm", "text/html" },
                { "html", "text/html" },
                { "ice", "x-conference/x-cooltalk" },
                { "ico", "image/x-icon" },
                { "ics", "text/calendar" },
                { "ief", "image/ief" },
                { "ifb", "text/calendar" },
                { "iges", "model/iges" },
                { "igs", "model/iges" },
                { "jnlp", "application/x-java-jnlp-file" },
                { "jp2", "image/jp2" },
                { "jpe", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "jpg", "image/jpeg" },
                { "js", "application/x-javascript" },
                { "kar", "audio/midi" },
                { "latex", "application/x-latex" },
                { "lha", "application/octet-stream" },
                { "lzh", "application/octet-stream" },
                { "m3u", "audio/x-mpegurl" },
                { "m4a", "audio/mp4a-latm" },
                { "m4b", "audio/mp4a-latm" },
                { "m4p", "audio/mp4a-latm" },
                { "m4u", "video/vnd.mpegurl" },
                { "m4v", "video/x-m4v" },
                { "mac", "image/x-macpaint" },
                { "man", "application/x-troff-man" },
                { "mathml", "application/mathml+xml" },
                { "me", "application/x-troff-me" },
                { "mesh", "model/mesh" },
                { "mid", "audio/midi" },
                { "midi", "audio/midi" },
                { "mif", "application/vnd.mif" },
                { "mov", "video/quicktime" },
                { "movie", "video/x-sgi-movie" },
                { "mp2", "audio/mpeg" },
                { "mp3", "audio/mpeg" },
                { "mp4", "video/mp4" },
                { "mpe", "video/mpeg" },
                { "mpeg", "video/mpeg" },
                { "mpg", "video/mpeg" },
                { "mpga", "audio/mpeg" },
                { "ms", "application/x-troff-ms" },
                { "msh", "model/mesh" },
                { "mxu", "video/vnd.mpegurl" },
                { "nc", "application/x-netcdf" },
                { "oda", "application/oda" },
                { "ogg", "application/ogg" },
                { "pbm", "image/x-portable-bitmap" },
                { "pct", "image/pict" },
                { "pdb", "chemical/x-pdb" },
                { "pdf", "application/pdf" },
                { "pgm", "image/x-portable-graymap" },
                { "pgn", "application/x-chess-pgn" },
                { "pic", "image/pict" },
                { "pict", "image/pict" },
                { "png", "image/png" },
                { "pnm", "image/x-portable-anymap" },
                { "pnt", "image/x-macpaint" },
                { "pntg", "image/x-macpaint" },
                { "ppm", "image/x-portable-pixmap" },
                { "ppt", "application/vnd.ms-powerpoint" },
                { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { "potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
                { "ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
                { "ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12" },
                { "pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12" },
                { "potm", "application/vnd.ms-powerpoint.template.macroEnabled.12" },
                { "ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" },
                { "ps", "application/postscript" },
                { "qt", "video/quicktime" },
                { "qti", "image/x-quicktime" },
                { "qtif", "image/x-quicktime" },
                { "ra", "audio/x-pn-realaudio" },
                { "ram", "audio/x-pn-realaudio" },
                { "ras", "image/x-cmu-raster" },
                { "rdf", "application/rdf+xml" },
                { "rgb", "image/x-rgb" },
                { "rm", "application/vnd.rn-realmedia" },
                { "roff", "application/x-troff" },
                { "rtf", "text/rtf" },
                { "rtx", "text/richtext" },
                { "sgm", "text/sgml" },
                { "sgml", "text/sgml" },
                { "sh", "application/x-sh" },
                { "shar", "application/x-shar" },
                { "silo", "model/mesh" },
                { "sit", "application/x-stuffit" },
                { "skd", "application/x-koan" },
                { "skm", "application/x-koan" },
                { "skp", "application/x-koan" },
                { "skt", "application/x-koan" },
                { "smi", "application/smil" },
                { "smil", "application/smil" },
                { "snd", "audio/basic" },
                { "so", "application/octet-stream" },
                { "spl", "application/x-futuresplash" },
                { "src", "application/x-wais-source" },
                { "sv4cpio", "application/x-sv4cpio" },
                { "sv4crc", "application/x-sv4crc" },
                { "svg", "image/svg+xml" },
                { "swf", "application/x-shockwave-flash" },
                { "t", "application/x-troff" },
                { "tar", "application/x-tar" },
                { "tcl", "application/x-tcl" },
                { "tex", "application/x-tex" },
                { "texi", "application/x-texinfo" },
                { "texinfo", "application/x-texinfo" },
                { "tif", "image/tiff" },
                { "tiff", "image/tiff" },
                { "tr", "application/x-troff" },
                { "tsv", "text/tab-separated-Values" },
                { "txt", "text/plain" },
                { "ustar", "application/x-ustar" },
                { "vcd", "application/x-cdlink" },
                { "vrml", "model/vrml" },
                { "vxml", "application/voicexml+xml" },
                { "wav", "audio/x-wav" },
                { "wbmp", "image/vnd.wap.wbmp" },
                { "wbmxl", "application/vnd.wap.wbxml" },
                { "wml", "text/vnd.wap.wml" },
                { "wmlc", "application/vnd.wap.wmlc" },
                { "wmls", "text/vnd.wap.wmlscript" },
                { "wmlsc", "application/vnd.wap.wmlscriptc" },
                { "wrl", "model/vrml" },
                { "xbm", "image/x-xbitmap" },
                { "xht", "application/xhtml+xml" },
                { "xhtml", "application/xhtml+xml" },
                { "xls", "application/vnd.ms-excel" },
                { "xml", "application/xml" },
                { "xpm", "image/x-xpixmap" },
                { "xsl", "application/xml" },
                { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { "xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
                { "xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12" },
                { "xltm", "application/vnd.ms-excel.template.macroEnabled.12" },
                { "xlam", "application/vnd.ms-excel.addin.macroEnabled.12" },
                { "xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12" },
                { "xslt", "application/xslt+xml" },
                { "xul", "application/vnd.mozilla.xul+xml" },
                { "xwd", "image/x-xwindowdump" },
                { "xyz", "chemical/x-xyz" },
                { "zip", "application/zip" }
            };
        }

        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        //******************JoinId ****************************
        public static string JoinId(params string[] sysids)
        {
            return string.Join("~", sysids);
        }

        public static string SplitCharacter(this string data)
        {
            return "~";
        }

        public static string[] SplitBtcKey(this string data)
        {
            return data.XSplit("~");
        }

        public static string JoinId(List<string> sysids)
        {
            return string.Join("~", sysids);
        }


        public static ActionResponse<T> ReturnResponseError<T>(this ActionResponse<T> response, IEnumerable<string> validationErrorList)
        {
            response.ReturnMessage = validationErrorList.ToList();
            response.IsSuccess = false;
            return response;
        }

        public static ActionResponse<T> ReturnResponseError<T>(this ActionResponse<T> response, string error)
        {
            response.ReturnMessage.Add(error);
            response.IsSuccess = false;
            return response;
        }

        public static PagedAndSortedResponse<T> ReturnResponseError<T>(this PagedAndSortedResponse<T> response, string error)
        {
            response.ReturnMessage.Add(error);
            response.IsSuccess = false;
            return response;
        }

        public static ActionResponse<T> ThrowResponseError<T>(this ActionResponse<T> response, string error)
        {
            response.IsSuccess = false;
            throw new Exception(error);
        }

        public static int CalculateTotalPages(long numberOfRecords, int pageSize)
        {
            int totalPages;

            Math.DivRem(numberOfRecords, pageSize, out var result);

            if (result > 0)
                totalPages = (int)((numberOfRecords / pageSize)) + 1;
            else
                totalPages = (int)(numberOfRecords / pageSize);

            return totalPages;
        }

        public static List<T> GetPagedAndSortedData<T>(IQueryable<T> query, int limit, int page, string sortDirection,
            string sortBy)
        {
            sortDirection ??= "desc";
            sortBy ??= "Id";
            query = sortDirection.ToLower().Equals("asc")
                ? query.OrderBy(sortBy)
                : query.OrderByDescending(sortBy);

            var skipCount = (page - 1) * limit;
            query = query.Skip(skipCount).Take(limit);
            return query.ToListReadUncommitted();
        }


        public static IQueryable<T> GetExcelList<T>(this IQueryable<T> source, string sortDirection, string sortExpression,
            string defaultExpression = "")
        {
            sortExpression = sortExpression.IsInitial() ? defaultExpression : sortExpression;

            var query = source;

            if (sortDirection.ToLower().Equals("asc"))
                query = query.OrderBy(sortExpression);
            else
                query = query.OrderByDescending(sortExpression);

            query = query.Take(1000);
            return query;
        }


        public static bool IsDate(string date)
        {
            DateTime dateTime;
            return DateTime.TryParse(date, out dateTime);
        }

        public static bool IsNumeric(object entity)
        {
            if (entity == null) return false;

            int result;
            return int.TryParse(entity.ToString(), out result);
        }

        public static bool IsDouble(object entity)
        {
            if (entity == null) return false;

            string e = entity.ToString();

            int count = 0;
            int i = 0;
            while ((i = e.IndexOf(".", i)) != -1)
            {
                i += ".".Length;
                count++;
            }

            if (count > 1) return false;

            e = e.Replace(".", "");

            int result;
            return int.TryParse(e, out result);
        }

        public static string[] SplitMultiKey(string srcAccountCode)
        {
            if (srcAccountCode.Trim() == "")
                return null;


            return srcAccountCode.Split("\n");
        }

        public static List<String> Message(string message)
        {
            List<String> returnMessage = new List<String>();
            returnMessage.Add(message);
            return returnMessage;
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public static string? FormatRowToString(object rowValue)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            return (rowValue == DBNull.Value) ? "" : rowValue.ToString();
        }

        public static decimal FormatIntValForLogoInt(object rowValue)
        {
            return (rowValue == DBNull.Value) ? default(decimal) : rowValue.ToString().ToNum();
        }

        public static DateTime FormatDateValForLogoInt(object rowValue)
        {
            return (rowValue == DBNull.Value) ? DefDate : rowValue.ToString().ToDate();
        }

        public static string ConverIdsTostring(int[] Ids)
        {
            var result = string.Join(",", Ids.Select(x => x.ToString()));
            return result;
        }

        public static int[] ConvertStrIdToInt(string[] Ids)
        {
            List<int> retVal = new List<int>();

            foreach (var id in Ids)
            {
                retVal.Add(id.ToInt());
            }

            return retVal.ToArray();
        }


        public static string GetPhoneAreaCode(string phone1)
        {
            if (phone1.IsInitial())
                return "";

            //"0(212)"

            return phone1.Substring(2, 4);
        }

        public static string GetPhone(string phone1)
        {
            if (phone1.IsInitial())
                return "";

            //0(222) 222-2222

            return phone1.Substring(4, 14).Replace(" ", "").Replace("-", "");
        }

        public static string GetPhoneFormatted(string phone1)
        {
            if (phone1.IsInitial())
                return "";

            //input -> 5051231244
            //output -> 0(505) 123-1244

            return $"0({phone1.Substring(0, 3)}) {phone1.Substring(3, 3)}-{phone1.Substring(6, 4)}";
        }

        public static string GetPhoneWithoutFormatted(string phone1)
        {
            if (phone1.IsInitial())
                return "";

            //input -> 0(505) 123-1244
            //output-> 5051231244

            phone1 = phone1.Remove(0, 1);
            return phone1.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
        }

        public static string UppercaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            StringBuilder output = new StringBuilder();
            string[] words = s.Split(' ');
            foreach (string word in words)
            {
                char[] a = word.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                string b = new string(a);
                output.Append(b + " ");
            }

            return output.ToString().Trim();
        }

        public static string GetString(string inValue)
        {
            return (inValue != null) ? (inValue) : String.Empty;
        }


        public static string EntityKeyFieldValue<T>(T entity)
        {
            var props = entity.GetType().GetProperties();


            foreach (var prop in props)
            {
                var attr = Attribute.GetCustomAttributes(prop);

                if (!attr.Any())
                    continue;

                var attrib = attr[0] as FieldAttrAttribute;

                if (attrib == null || !attrib.IsKey)
                    continue;

                var val = prop.GetValue(entity, null);
                return val.ToString();
            }

            return "";
        }

        public static string EntityKeyFieldName<T>()
        {
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var attr = Attribute.GetCustomAttributes(prop);

                if (!attr.Any())
                    continue;

                var attrib = attr[0] as FieldAttrAttribute;

                if (attrib == null || !attrib.IsKey)
                    continue;

                var val = prop.Name;
                return val.ToString();
            }

            return "";
        }

        public static List<string> GetFieldNames<T>(params Expression<Func<T, object>>[] fldNamesExp)
        {
            var names = new List<string>();
            foreach (var fldNameExp in fldNamesExp)
            {
                if (fldNameExp.Body is MemberExpression)
                    names.Add(((MemberExpression)fldNameExp.Body).Member.Name);

                else
                {
                    var op = ((UnaryExpression)fldNameExp.Body).Operand;
                    names.Add(((MemberExpression)op).Member.Name);
                }
            }

            return names;
        }

        public static List<TextValueDto> DoGetSelectModel<T>(this IEnumerable<T> items, params Expression<Func<T, object>>[] fldNamesExp)
        {
            List<TextValueDto> returnList = new List<TextValueDto>();

            var names = GetFieldNames<T>(fldNamesExp);
            foreach (T item in items)
            {
                var props = item.GetType().GetProperties();

                var val = props.Single(pi => pi.Name == names[0]).GetValue(item);

                var TextTextList = new List<string>();

                foreach (var fldName in names.Skip(1).ToArray())
                {
                    var lbl = props.Single(pi => pi.Name == fldName).GetValue(item);

                    TextTextList.Add(lbl.ToString() == null ? "" : lbl.ToString());
                }

                returnList.Add(new TextValueDto { Value = val.ToString(), Text = TextTextList.XJoin("-") });
            }

            return returnList.ToList();
        }

        public static List<TextIntValueDto> DoGetSelectNumModel<T>(this IEnumerable<T> items,
            params Expression<Func<T, object>>[] fldNamesExp)
        {
            List<TextIntValueDto> returnList = new List<TextIntValueDto>();

            var names = GetFieldNames<T>(fldNamesExp);
            foreach (T item in items)
            {
                var props = item.GetType().GetProperties();

                var val = props.Single(pi => pi.Name == names[0]).GetValue(item);

                var TextTextList = new List<string>();

                foreach (var fldName in names.Skip(1).ToArray())
                {
                    var lbl = props.Single(pi => pi.Name == fldName).GetValue(item);

                    TextTextList.Add(lbl.ToString() == null ? "" : lbl.ToString());
                }

                returnList.Add(new TextIntValueDto { Value = val.ToString().ToInt(), Text = TextTextList.XJoin("-") });
            }

            return returnList.ToList();
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }


        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }


        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}