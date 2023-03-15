using System;
using SplashKitSDK;
using Microsoft.Data.Analysis;
using XPlot.Plotly;

namespace DaVersion02
{
    public class Scatter : Visual
    {
        public Scatter(DataFrame data) : base(data)
        {
        }

        public override string ChartMode
        {
            get { return chartMode; }
            set { chartMode = "lines+markers"; }
        }

         public override string Draw()
        {
            var chart = Chart.Plot(
                new XPlot.Plotly.Scatter//  Scatter
                {
                    x = this._data.Columns[Xchart1],
                    y = this._data.Columns[Ychart1],
                    mode = chartMode
                }
            );

            base.Draw();

            chart.WithLayout(chartLayout);
            return chart.GetInlineHtml().ToString();
        }
    }

    public class TwoScatter : Visual
    {
        public TwoScatter(DataFrame data) : base(data)
        {
        }
        public override string ChartMode
        {
            get { return chartMode; }
            set { chartMode = "lines"; }
        }
        public override string Draw()
        {
            // Chart2
            var chart2_list = new List<XPlot.Plotly.Scatter>
            {
            new XPlot.Plotly.Scatter
            {
                x = this._data.Columns["Date"],
                y = this._data.Columns["Open"],
                name="Open",
                mode = ChartMode
            },
            new XPlot.Plotly.Scatter
            {
                x = this._data.Columns["Date"],
                y = this._data.Columns["Close"],
                name="Close",
                mode = ChartMode
            }
            };

            var chart2 = Chart.Plot(
                chart2_list
            );
            base.Draw();

            chart2.WithLayout(chartLayout);

            // Get the HTML code for charts
            return chart2.GetInlineHtml().ToString(); 
        }
    }

    public class Bar : Visual
    {
        public Bar(DataFrame data) : base(data)
        {
        }

        public override string ChartMode
        {
            get { return chartMode; }
            set { chartMode = "Graph.Marker"; }
        }
        
        public override string Draw()
        {
            var chart = Chart.Plot(
            new  XPlot.Plotly.Bar
            {
                x = this._data.Columns[Xchart1],
                y = this._data.Columns[Ychart1],
                marker = new Marker { color = "rgb(0, 0, 109)" }
            }
            );
            base.Draw();
   
            chart.WithLayout(chartLayout);
            return chart.GetInlineHtml().ToString();
        }
    }
}