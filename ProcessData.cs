using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Microsoft.ML;
using Microsoft.Data.Analysis;
using Microsoft.ML.Data;
using System.Linq;
using XPlot.Plotly;

namespace DaVersion02
{
    public class ProcessData
    {
        public List<Data> _datas = new List<Data>();
        public string label = null;
        public int _dataIndex;

        public async void AddDataFromCSV(string dataName)
        {
            string filePathName = "D:\\2022-T3\\SIT771\\code\\dataAnalysis\\week08\\ohlcdata.csv";
            // How to use the method of DataFrame https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.analysis.dataframe?view=ml-dotnet-preview
            // get the data from https://phplot.sourceforge.net/phplotdocs/ex-ohlcbasic.html
            DataFrame storingData = DataFrame.LoadCsv(filePathName, separator: ',', header: true);

            if (dataName == null)
            {
                dataName = Path.GetFileName(filePathName);
            }

            Data data = new Data(dataName, label);
            data.StoringData = storingData.Clone();
            data.ExistData = true;
            this._datas.Add(data);

            data.Print("Data information", data.GetDataInfo());
            data.Print("the top 5 of Data", data.GetDataHead(5));
        }

        public void SaveData(Data data)
        {
            string filePathName = "D:\\2022-T3\\SIT771\\code\\dataAnalysis\\week08\\";
            DataFrame.SaveCsv(data.StoringData, filePathName, ',');
            if (data.StoringData == null)
            {
                DataFrame aa = new DataFrame();
            }
        }

        public void FindDataName(ProcessData loadData, string dataName)
        {
            for (int i = 0; i < loadData._datas.Count; i++)
            {
                if (loadData._datas[i].DataName.ToLower().Trim() == dataName.ToLower().Trim())
                {
                    _dataIndex = i;
                    break;
                }
                else
                {
                    _dataIndex = -2;
                }
            }
            if (loadData._datas.Count == 0)
            {
                _dataIndex = -1;
                this.Print();
            }
            else if(_dataIndex == -2)
            {
                this.Print();
            }
        }

        public void SetLabel(ProcessData loadData, string dataName, string label)
        {
            FindDataName(loadData, dataName);
            if (_dataIndex >= 0)
            {
                loadData._datas[_dataIndex].Label = label;
            }
        }

        public void FillNulls(ProcessData loadData, string dataName)
        {
            FindDataName(loadData, dataName);
            if (_dataIndex >= 0)
            {
                loadData._datas[_dataIndex].StoringData.Columns["Open"].FillNulls(0, true);
                loadData._datas[_dataIndex].Print("Data Description", loadData._datas[_dataIndex].GetDataDescription());
            }
        }
        public void OrderData(ProcessData loadData, string dataName, String Column)
        {
            //Sort the DataRandom
            FindDataName(loadData, dataName);
            if (_dataIndex >= 0)
            {
                DataFrame _dataOrder = loadData._datas[_dataIndex].StoringData.Clone();
                loadData._datas[_dataIndex].Print("Data Order", _dataOrder.OrderBy(Column).Head(5));
            }
        }

        public void FilterData(ProcessData loadData, string dataName, String Column, int valueFilter)
        {
            // //Filter and Sort the DataRandom
            FindDataName(loadData, dataName);
            if (_dataIndex >= 0)
            {
                DataFrame _dataFilter = loadData._datas[_dataIndex].StoringData.Clone();
                _dataFilter.Filter(_dataFilter.Columns[Column].ElementwiseGreaterThanOrEqual(valueFilter));
                loadData._datas[_dataIndex].Print("Data Filter", _dataFilter.Head(5));
            }
        }

        public List<string> VisualData(ProcessData loadData, string dataName)
        {
            List<string> chartIamgeDev = new List<string>();

            string x = "Date";
            string y = "Open";
            string title = "Open Price";
            string xaxis = "Date";
            string yaxis = "Price (USD)";

            FindDataName(loadData, dataName);
            if (_dataIndex >= 0)
            {
                DataFrame _dataChart = loadData._datas[_dataIndex].StoringData.Clone();
                Scatter scatter = new Scatter(_dataChart);
                scatter.Xchart1 = x;
                scatter.Ychart1 = y;
                scatter.title = title;
                scatter.xaxis = xaxis;
                scatter.yaxis = yaxis;
                chartIamgeDev.Add(scatter.Draw());

                Bar barChart = new Bar(_dataChart);
                barChart.Xchart1 = x;
                barChart.Ychart1 = "Volume";
                barChart.xaxis = xaxis;
                barChart.yaxis = "Unit";
                barChart.title = "Volume";
                chartIamgeDev.Add(barChart.Draw());
            }
            return chartIamgeDev;
        }

        public void Print()
        {
            if (this._dataIndex == -1)
            {
                Console.WriteLine("\nPlease perform the first step to load the data. \n");
            }
            if (this._dataIndex == -2)
            {
                Console.WriteLine("\nInvalid data name, please enter the correct data name. \n");
            }
        }
    }
}