using System;
using SplashKitSDK;
using Microsoft.Data.Analysis;
using XPlot.Plotly;

namespace DaVersion02
{
    public abstract class Visual
    {
        protected string chartMode;
        public string Xchart1 { set; get; }
        public string Ychart1 { set; get; }
        public string title { set; get; }
        public string xaxis { set; get; }
        public string yaxis { set; get; }
        public XPlot.Plotly.Layout.Layout chartLayout {set; get;}
        protected DataFrame _data { get; set; }

        public abstract string ChartMode{get;set;}

        public Visual(DataFrame data)
        {
            _data = data.Clone();
        }

        public virtual string Draw()
        {
            chartLayout = new Layout.Layout
            {
                title = title,
                xaxis = new Xaxis { title = xaxis },
                yaxis = new Yaxis { title = yaxis }
            };
            return null;
        }
    }
}