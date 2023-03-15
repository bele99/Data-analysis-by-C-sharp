using System;
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
    public class Program
    {
        public static string dataName { get; set; }
        public static string dataLabel { get; set; }

        private enum MenuOption
        {
            LoadData, ProcessData, DisplayDataOnChart, TrainModel, Quit
        }

        private enum ProcessMenuOption
        {
            SetLabel, FillNulls, OrderData, FilterData, ReturnToUpperMenu
        }

        private enum DataMenuOption
        {
            DataInfo, DataDescription, DataDisplay, CountRowColumn, ReturnToUpperMenu
        }

        public static void Main()
        {
            MenuOption userSelection;
            Console.WriteLine("The data analysis tool: ");
            ProcessData loadData = new ProcessData();

            do
            {
                userSelection = readUserOption();
                switch (userSelection)
                {
                    case MenuOption.LoadData:
                        doLoadData(loadData);
                        break;
                    case MenuOption.ProcessData:
                        doProcessData(loadData);
                        break;
                    case MenuOption.DisplayDataOnChart:
                        doDisplayDataOnChart(loadData);
                        break;
                    case MenuOption.TrainModel:
                        doTrainModel();
                        break;
                    case MenuOption.Quit:
                        Console.WriteLine("Quit");
                        break;
                }
            } while (userSelection != MenuOption.Quit);

            // Refrence
            // https://towardsdatascience.com/getting-started-with-c-dataframe-and-xplot-ploty-6ea6ce0ce8e3
            // https://data-adventurer.com/2021/12/09/exploring-microsoft-data-analysis-for-c-in-a-net-interactive-notebook/
            // https://devblogs.microsoft.com/dotnet/an-introduction-to-dataframe/
        }

        private static MenuOption readUserOption()
        {
            ReadUserOption readUserOption = new ReadUserOption();

            readUserOption.MenuPrint();
            int option = readUserOption.CheckOption();

            return (MenuOption)(option - 1);
        }

        private static ProcessMenuOption readProcessOption()
        {
            ReadProcessOption readProcessOption = new ReadProcessOption();
            readProcessOption.MenuPrint();
            int option = readProcessOption.CheckOption();

            return (ProcessMenuOption)(option - 1);
        }

        private static DataMenuOption readDataOption()
        {
            ReadDataOption readDataOption = new ReadDataOption();
            readDataOption.MenuPrint();
            int option = readDataOption.CheckOption();
            return (DataMenuOption)(option - 1);
        }

        public static void doLoadData(ProcessData loadData)
        {
            Console.Write("Please enter the data name: ");
            try
            {
                dataName = Console.ReadLine();
                loadData.AddDataFromCSV(dataName);
                if (loadData._datas.Count == null)
                {
                    throw new Exception("Failed to load data. Please check the data file (.csv).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DataMenuOption readDataMenuOption;
            do
            {
                readDataMenuOption = readDataOption();
                switch (readDataMenuOption)
                {
                    case DataMenuOption.DataInfo:
                        doDataInfo(loadData);
                        break;
                    case DataMenuOption.DataDescription:
                        doDataDescription(loadData);
                        break;
                    case DataMenuOption.DataDisplay:
                        doDataDisplay(loadData);
                        break;
                    case DataMenuOption.CountRowColumn:
                        doCountRowColumn(loadData);
                        break;
                    case DataMenuOption.ReturnToUpperMenu:
                        Console.WriteLine("Return To Upper MainMenu");
                        break;
                }
            } while (readDataMenuOption != DataMenuOption.ReturnToUpperMenu);
        }

        public static void doDataInfo(ProcessData loadData)
        {
            Data data = new Data(dataName, dataLabel);
            data.StoringData = loadData._datas[0].StoringData;
            data.Print("Data information", data.GetDataInfo());
        }

        public static void doDataDescription(ProcessData loadData)
        {
            Data data = new Data(dataName, dataLabel);
            data.StoringData = loadData._datas[0].StoringData;
            data.Print("the data description", data.GetDataDescription());
        }

        public static void doDataDisplay(ProcessData loadData)
        {
            Data data = new Data(dataName, dataLabel);
            data.StoringData = loadData._datas[0].StoringData;
            data.Print("the top 5 of Data", data.GetDataHead(5));
        }

        public static void doCountRowColumn(ProcessData loadData)
        {
            Data data = new Data(dataName, dataLabel);
            data.StoringData = loadData._datas[0].StoringData;
            data.GetRowAndColumnCount();
        }

        private static void doProcessData(ProcessData loadData)
        {
            ProcessMenuOption userProcessOption;
            do
            {
                userProcessOption = readProcessOption();
                switch (userProcessOption)
                {
                    case ProcessMenuOption.SetLabel:
                        doSetLabel(loadData);
                        break;
                    case ProcessMenuOption.FillNulls:
                        doFillNulls(loadData);
                        break;
                    case ProcessMenuOption.OrderData:
                        doOrderData(loadData);
                        break;
                    case ProcessMenuOption.FilterData:
                        doFilterData(loadData);
                        break;
                    case ProcessMenuOption.ReturnToUpperMenu:
                        Console.WriteLine("Return To Upper MainMenu");
                        break;
                }
            } while (userProcessOption != ProcessMenuOption.ReturnToUpperMenu);
        }

        private static void doSetLabel(ProcessData loadData)
        {
            Console.Write("Please enter the data name: ");
            string dataName = Console.ReadLine();

            try
            {
                Console.Write("Please enter a data label name: ");
                dataLabel = Console.ReadLine();

                if (dataLabel == null)
                {
                    throw new Exception("Invalid data label name. Please enter a data label name.");
                }

                loadData.SetLabel(loadData, dataName, dataLabel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void doFillNulls(ProcessData loadData)
        {
            Console.Write("Please enter the data name: ");
            try
            {
                string dataName = Console.ReadLine();
                loadData.FillNulls(loadData, dataName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void doOrderData(ProcessData loadData)
        {
            try
            {
                Console.Write("Please enter the data name: ");
                string dataName = Console.ReadLine();

                Console.Write("Please enter the data Column: ");
                string dataColumn = Console.ReadLine();

                loadData.OrderData(loadData, dataName, dataColumn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void doFilterData(ProcessData loadData)
        {
            try
            {
                Console.Write("Please enter the data name: ");
                string dataName = Console.ReadLine();

                Console.Write("Please enter the data Column: ");
                string dataColumn = Console.ReadLine();

                Console.Write("Please enter the data valueFilter: ");
                int dataValueFilter = Convert.ToInt32(Console.ReadLine());

                loadData.FilterData(loadData, dataName, dataColumn, dataValueFilter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void doDisplayDataOnChart(ProcessData loadData)
        {
            try
            {
                Console.Write("Please enter the data name: ");
                string dataName = Console.ReadLine();

                List<string> chartIamgeDev = loadData.VisualData(loadData, dataName);
                WebsitePageForChart(chartIamgeDev);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void doTrainModel()
        {
            var rSquared = TrainModel.MLContextData();
            Console.WriteLine($"\nThe regression model's R-Squared is {rSquared}.\n");
        }
        // Visual Main Page
        public static void WebsitePageForChart(List<string> chartIamgeDev)//(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            HttpServer.Start();

            // Handle requests
            Task listenTask = HttpServer.HandleIncomingConnections(chartIamgeDev);
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            HttpServer.Stop();
        }
    }
}

// ML.NET 2.0.0 API
// https://learn.microsoft.com/en-us/dotnet/api/?view=ml-dotnet&preserve-view=true
