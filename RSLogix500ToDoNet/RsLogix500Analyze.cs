using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSLogix500ToDoNet
{
    public class RsLogix500Analyze
    {
        public string[] RawFile { get; }
        public string Mode { get; }
        public string ProjectName { get; }
        public int LADCount { get; }
        public List<LogixData> RawData { get; }
        public List<LogixData> ConvertedData { get; }
        public RsLogix500Analyze(string FilePath)
        {
            CheckFile(FilePath);
            RawFile = File.ReadAllLines(FilePath);

            Mode = GetMode(RawFile);
            ProjectName = GetProjectName(RawFile);
            LADCount = GetLADCount(RawFile);
            RawData = GetAllLogix(RawFile);

            ConvertedData = ConvertRawData(RawData);

            foreach (LogixData ld in ConvertedData)
            {
                Console.WriteLine(ld.Data);
            }
        }

        private bool CheckFile(string FilePath)
        {
            #region Check File Exist
            if (!File.Exists(FilePath))
            {
                throw new Exception("This Path is not Exist");
            }
            #endregion

            #region Check File Read
            try
            {
                File.ReadAllLines(FilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Read File Fail");
            }
            #endregion

            return true;
        }
        private string GetMode(string[] RawData)
        {
            Match match = Regex.Match(RawData[0], @"(%)(.*)(%)");
            if (match.Success)
            {
                return match.Groups[2].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        private string GetProjectName(string[] RawData)
        {
            string[] tempdata = RawData.Where(x => x.Contains("PROJECT")).ToArray();

            foreach (string temprow in tempdata)
            {
                Match match = Regex.Match(temprow, @"(PROJECT )("")(.*)("")");
                if (match.Success)
                {
                    return match.Groups[3].Value;
                }
            }
            return string.Empty;
        }
        private int GetLADCount(string[] RawData)
        {
            return RawData.Where(x => x.Contains("LADDER")).ToArray().Count();
        }
        private List<LogixData> GetAllLogix(string[] RawData)
        {
            List<LogixData> result = new List<LogixData>();

            int NowLine = Array.IndexOf(RawData, "LADDER 2");

            bool flag = false;
            int Ladder = 2;
            int Rung = 0;

            while (true)
            {
                if (Regex.Match(RawData[NowLine], @"(LADDER )(.*)").Success)
                {
                    Ladder = Convert.ToInt32(Regex.Match(RawData[NowLine], @"(LADDER )(.*)").Groups[2].Value);
                    flag = false;
                }
                else if (Regex.Match(RawData[NowLine], @"(% Rung: )(.*)( %)").Success)
                {
                    Rung = Convert.ToInt32(Regex.Match(RawData[NowLine], @"(% Rung: )(.*)( %)").Groups[2].Value);
                    flag = false;
                }
                else if (Regex.Match(RawData[NowLine], @"(SOR )(.*)( EOR )").Success)
                {
                    result.Add(new LogixData(Ladder, Rung, Regex.Match(RawData[NowLine], @"(SOR )(.*)(EOR )").Groups[2].Value) );
                    flag = false;
                }
                else if (RawData[NowLine] == string.Empty)
                {
                    if (flag)
                    {
                        break;
                    }

                    flag = true;
                }

                NowLine++;
            }

            return result;
        }
        public class LogixData
        {
            public int Ladder { get; }
            public int Rung { get; }
            public string Data { get; }
            public LogixData(int ladder,int rung,string data)
            {
                Ladder = ladder;
                Rung = rung;
                Data = data;
            }
        }
        private List<LogixData> ConvertRawData(List<LogixData> rawdata)
        {
            //用來複寫用的Input資料，因為有可能同一行程式碼有多個同一種類型，所以要多次改寫
            string UndoneData = string.Empty;
            bool ConvertFlag = false;
            List<LogixData> temp = new List<LogixData>();

            for (int i = 0; i < rawdata.Count; i++)
            {

                string tempdata;

                if (UndoneData == string.Empty)
                {
                    tempdata = rawdata[i].Data;
                }
                else
                {
                    tempdata = UndoneData;
                }

                #region Bit
                if (Regex.Match(tempdata, @"(.*)(XIO)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_XIO = Regex.Match(tempdata, @"(.*)(XIO)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_XIO[1]} 如果 {Group_XIO[3]} 為 False , {Group_XIO[4]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(XIC)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_XIC = Regex.Match(tempdata, @"(.*)(XIC)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_XIC[1]} 如果 {Group_XIC[3]} 為 True , {Group_XIC[4]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(OTE)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_OTE = Regex.Match(tempdata, @"(.*)(OTE)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_OTE[1]} {Group_OTE[3]} 設定為此行啟用即輸出 , {Group_OTE[4]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(OTU)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_OTU = Regex.Match(tempdata, @"(.*)(OTU)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_OTU[1]} {Group_OTU[3]} 設定為解鎖狀態(False) , {Group_OTU[4]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(OTL)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_OTL = Regex.Match(tempdata, @"(.*)(OTL)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_OTL[1]} {Group_OTL[3]} 設定為上鎖狀態(True) , {Group_OTL[4]}";
                    ConvertFlag = true;
                }

                #endregion

                #region Timer/Counter
                if (Regex.Match(tempdata, @"(.*)(TON)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_TON = Regex.Match(tempdata, @"(.*)(TON)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_TON[1]} 啟動正向時間延遲器 ( 時間變數: {Group_TON[3]} , 計算基數: {Group_TON[4]}秒 , 目標秒數: {Group_TON[5]}秒 , 已算秒數: {Group_TON[6]} , {Group_TON[7]})";
                    ConvertFlag = true;
                }
                if (Regex.Match(tempdata, @"(.*)(TOF)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_TOF = Regex.Match(tempdata, @"(.*)(TON)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_TOF[1]} 啟動反向時間延遲器 ( 時間變數: {Group_TOF[3]} , 計算基數: {Group_TOF[4]}秒 , 目標秒數: {Group_TOF[5]}秒 , 已算秒數: {Group_TOF[6]} , {Group_TOF[7]})";
                    ConvertFlag = true;
                }
                #endregion

                #region Input/Output

                string[] Communication_Command = new string[] {"500CPU Read", "500CPU Write", "485CIF Read", "485CIF Write" , "PLC5 Read", "PLC5 Write","CIP Generic" };

                if (Regex.Match(tempdata, @"(.*)(MSG)\s{1}\S*\s{1}(\S*)\s{1}\S*\s{1}\S*\s{1}(\S*)\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}(\S*)\s{1}(\S*)\s{1}\S*\s{1}(\S*)\s{1}MultiHop[(](.*)[)]\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}(.*)").Success)
                {
                    GroupCollection Group_MSG = Regex.Match(tempdata, @"(.*)(MSG)\s{1}\S*\s{1}(\S*)\s{1}\S*\s{1}\S*\s{1}(\S*)\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}(\S*)\s{1}(\S*)\s{1}\S*\s{1}(\S*)\s{1}MultiHop[(](.*)[)]\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}\S*\s{1}(.*)").Groups;
                    tempdata = $"{Group_MSG[1]} 啟動資料傳輸器 ( 設定變數: {Group_MSG[4]} , 傳輸模式: {Communication_Command[Convert.ToInt32(Group_MSG[3].Value)]} , 傳輸目標: {Group_MSG[6]} , 傳輸來源: {Group_MSG[5]} , 傳輸逾時: {Group_MSG[7]} , 連線資訊: {Group_MSG[8]} ) , {Group_MSG[9]}";
                    ConvertFlag = true;
                }

                #endregion

                #region Compare

                if (Regex.Match(tempdata, @"(.*)(GEQ)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_GEQ = Regex.Match(tempdata, @"(.*)(GEQ)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_GEQ[1]} 如果 {Group_GEQ[3]} 大於或等於 {Group_GEQ[4]} , {Group_GEQ[5]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(LES)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_LES = Regex.Match(tempdata, @"(.*)(LES)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_LES[1]} 如果 {Group_LES[3]} 小於 {Group_LES[4]} , {Group_LES[5]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(LIM)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_LIM = Regex.Match(tempdata, @"(.*)(LIM)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_LIM[1]} 如果 {Group_LIM[4]} 介於 {Group_LIM[3]} 和 {Group_LIM[5]}之間 , {Group_LIM[6]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(EQU)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_EQU = Regex.Match(tempdata, @"(.*)(EQU)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_EQU[1]} 如果 {Group_EQU[3]} 等於 {Group_EQU[4]} , {Group_EQU[5]}";
                    ConvertFlag = true;
                }

                #endregion

                #region Compute/Math

                if (Regex.Match(tempdata, @"(.*)(ADD)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_ADD = Regex.Match(tempdata, @"(.*)(ADD)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_ADD[1]} 設定 {Group_ADD[5]} 為 {Group_ADD[3]} 和 {Group_ADD[4]} 的和 , {Group_ADD[6]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(SUB)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_SUB = Regex.Match(tempdata, @"(.*)(SUB)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_SUB[1]} 設定 {Group_SUB[5]} 為 {Group_SUB[3]} 和 {Group_SUB[4]} 的差 , {Group_SUB[6]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(MUL)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_MUL = Regex.Match(tempdata, @"(.*)(MUL)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_MUL[1]} 設定 {Group_MUL[5]} 為 {Group_MUL[3]} 乘與 {Group_MUL[4]} 的值 , {Group_MUL[6]}";
                    ConvertFlag = true;
                }
                if (Regex.Match(tempdata, @"(.*)(DIV)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_DIV = Regex.Match(tempdata, @"(.*)(DIV)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_DIV[1]} 設定 {Group_DIV[5]} 為 {Group_DIV[3]} 除與 {Group_DIV[4]} 的值 , {Group_DIV[6]}";
                    ConvertFlag = true;
                }

                #endregion

                #region Move/Logical

                if (Regex.Match(tempdata, @"(.*)(CLR)\s{1}(\S*)(.*)").Success)
                {
                    GroupCollection Group_CLR = Regex.Match(tempdata, @"(.*)(CLR)\s{1}(\S*)(.*)").Groups;
                    tempdata = $"{Group_CLR[1]} 清除變數(歸零): {Group_CLR[3]} , {Group_CLR[4]}";
                    ConvertFlag = true;
                }

                if (Regex.Match(tempdata, @"(.*)(MOV)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_MOV = Regex.Match(tempdata, @"(.*)(MOV)\s{1}(\S*)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_MOV[1]} {Group_MOV[4]} 設定為 {Group_MOV[3]} , {Group_MOV[5]}";
                    ConvertFlag = true;
                }

                #endregion

                #region File/Misc

                #endregion

                #region File Shirt/Sequencer

                #endregion

                #region Program Control

                if (Regex.Match(tempdata, @"(.*)(JSR)\s{1}(\S*)\s{1}(.*)").Success)
                {
                    GroupCollection Group_JSR = Regex.Match(tempdata, @"(.*)(JSR)\s{1}(\S*)\s{1}(.*)").Groups;
                    tempdata = $"{Group_JSR[1]} 啟動副程式 : LAD {Group_JSR[3]} , {Group_JSR[4]}";
                    ConvertFlag = true;
                }

                #endregion

                #region Ascii Control
                #endregion

                #region Ascii String

                #endregion

                #region Micro High Spod Cntr

                #endregion

                #region Trig Functions

                #endregion

                #region Advanced Math

                if (Regex.Match(tempdata, @"(.*)(SCP)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)(.*)").Success)
                {
                    GroupCollection Group_SCP = Regex.Match(tempdata, @"(.*)(SCP)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)\s{1}(\S*)(.*)").Groups;
                    tempdata = $"{Group_SCP[1]} 數值等比縮放: 輸入: {Group_SCP[3]} 輸入最小值: {Group_SCP[4]} 輸入最大值: {Group_SCP[5]} 輸出最小值: {Group_SCP[6]} 輸出最大值: {Group_SCP[8]} 輸出: {Group_SCP[9]} , {Group_SCP[4]}";
                    ConvertFlag = true;
                }

                #endregion

                tempdata = tempdata.Replace("BST", " 開啟分支 ( ");
                tempdata = tempdata.Replace("NXB", " ) ( ");
                tempdata = tempdata.Replace("BND", " ) 結束分支");

                // 如果修改過資料，就再次檢查
                if (ConvertFlag)
                {
                    UndoneData = tempdata;
                    i--;
                    ConvertFlag = false;
                    continue;
                }
                else
                {
                    temp.Add(new LogixData(rawdata[i].Ladder, rawdata[i].Rung, tempdata));
                    UndoneData = string.Empty;
                }       
                
            }

            return temp;
        }
    }
}
