using System;
using SplashKitSDK;
using Microsoft.ML;
using Microsoft.Data.Analysis;
using Microsoft.ML.Data;

namespace DaVersion02
{
    public class OpenData
    {
        [LoadColumn(0)]
        public DateTime Date { get; set; }

        [LoadColumn(1)]
        [ColumnName("Label")]
        public float Open { get; set; }

        [LoadColumn(2)]
        public float High { get; set; }

        [LoadColumn(3)]
        public float Low { get; set; }

        [LoadColumn(4)]
        public float Close { get; set; }
    }

    public class TrainModel
    {
        public static string FilePath = "D:\\2022-T3\\SIT771\\code\\dataAnalysis\\week08\\ohlcdata.csv";
        // Model training
        public static double MLContextData()
        {
            MLContext mlContext = new MLContext();

            // 1. Load data
            var dataPath = Path.GetFullPath(FilePath);
            IDataView dataML = mlContext.Data.LoadFromTextFile<OpenData>(dataPath, separatorChar: ',', hasHeader: true);
            //mlContext = (IDataView)loadData._datas[0].StoringData;

            // 2. Preprocess data
            DataOperationsCatalog.TrainTestData dataSplit = mlContext.Data.TrainTestSplit(dataML, testFraction: 0.2);
            IDataView trainData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;
            // show data
            //var b = dataML.Preview(5);

            // Normalize Features vector
            IEstimator<ITransformer> dataPrepEstimator =
                mlContext.Transforms.Concatenate("Features", "High", "Low", "Close")
                    .Append(mlContext.Transforms.NormalizeMinMax("Features"));

            // Create data prep transformer
            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(trainData);

            // Apply transforms to training data
            IDataView transformedTrainingData = dataPrepTransformer.Transform(trainData);

            var a = transformedTrainingData.Preview(5);

            // 3. Train model
            var sa = mlContext.Regression.Trainers.Sdca();
            var trainedModel = sa.Fit(transformedTrainingData);

            var trainedModelParameters = trainedModel.Model as Microsoft.ML.Trainers.LinearRegressionModelParameters;

            // 4. Make a prediction
            // Measure trained model performance
            // Apply data prep transformer to test data
            IDataView transformedTestData = dataPrepTransformer.Transform(testData);

            // Use trained model to make inferences on test data
            IDataView testDataPredictions = trainedModel.Transform(transformedTestData);
            //IDataView testDataPredictions = trainedModel.Transform(testData);

            // Extract model metrics and get RSquared
            RegressionMetrics trainedModelMetrics = mlContext.Regression.Evaluate(testDataPredictions);
            double rSquared = trainedModelMetrics.RSquared;
            
            // Save model
            // mlContext.Model.Save(transformedTestData, dataML.Schema,"model.zip");

            return rSquared;

            // Refrence
            // https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/train-machine-learning-model-ml-net
        }
    }
}