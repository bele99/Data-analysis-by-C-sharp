using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.Data.Analysis;
using Microsoft.ML.Data;

namespace DaVersion02
{
    public class Data
    {
        private static DataFrame _storingData = new DataFrame();
        private string _dataName;
        private string _label;
        private bool _existData;

        public bool ExistData { 
            set {this._existData = false;}
            get {return this._existData;} 
        }

        public string DataName
        {
            get
            {
                return _dataName;
            }
        }
        public string Label { get; set; }
        
        public DataFrame StoringData { get; set; }

        public Data(string dataname, string label)
        {
            _dataName = dataname;
            _label = label;
        }

        public DataFrame GetDataHead(int rowNumber)
        {
            DataFrame dataHead = StoringData.Head(rowNumber);
            return dataHead;
        }

        public DataFrame GetDataInfo()
        {
            DataFrame dataInfo = StoringData.Info();
            return dataInfo;
        }

        public DataFrame GetDataDescription()
        {
            DataFrame dataDescription = StoringData.Description();
            return dataDescription;
        }

        public void GetRowAndColumnCount()
        {
            long rowsCount = this.StoringData.Rows.Count;
            long columnsCount = this.StoringData.Columns.Count;
            Console.WriteLine($"\nRows: {rowsCount}, Columns: {columnsCount}");
        }

        public void Print(String title, DataFrame results)
        {
            Console.WriteLine($"\n------------------------------{title}------------------------------");
            Console.WriteLine("------------------------------ Start data ----------------------------");
            Console.Write(results.ToString());
            Console.WriteLine("------------------------------ End data ------------------------------\n");
        }
    }
}