using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using SmartEnergy.Library.Measurements.Models;

namespace SmartEnergy.Client.Components.Pages;

public partial class PeakVisualizer
{
    //Texts
    private string loadingMessage = "";
    private bool dataHidden = false;
    
    //Barchart data
    private BarChart barChart = default!;
    private BarChartOptions barChartOptions = default!;
    private ChartData chartData = default!;
    private bool firstInitialize = true;
    private int chartWidth = 800;
    private int chartHeight = 300;
    
    //Measurement data
    private List<Measurement>? usageMeasurements;
    private List<KeyValuePair<string, double?>> topValues;
    
    //Meters
    private List<string> meterIds = new()
    {
        "0F009F",
        "0F5A95",
        "105C4E",
        "107550",
        "107560",
        "164A93",
        "172425",
        "17B3C7",
        "17D392",
        "17E36B",
        "17E437",
        "2564D9",
        "256A8E",
        "256EF1",
        "257331",
        "259411",
        "2597AE",
        "25C512",
        "25CD2C",
        "25D439",
        "26FE39",
        "26FE53",
        "32CD98",
        "32CFAA",
        "3589FE",
        "3B86BD",
        "3BACE3",
        "41384C",
        "4138AD",
        "413E0C",
        "53F18E",
        "593971",
        "616EE5",
        "6174C0",
        "61796C",
        "62D925",
        "62D933",
        "62E0BE",
        "62EA69",
        "631140",
        "670616",
        "7321BD",
        "879BF5",
        "919125",
        "9197BD",
        "9198F5",
        "91A015",
        "98DCBB",
        "98DD26",
        "98E086",
        "98E15B",
        "98E1C0",
        "98E1C9",
        "98E216",
        "98E25E",
        "98E265",
        "98E291",
        "9EC2A2",
        "9FB0E7",
        "BF3C69",
        "BF5F83",
        "BF5FA2",
        "BF647E",
        "BF67F4",
        "C86555",
        "C8A9DB",
        "C8C9FE",
        "C8CB7C",
        "C8D0AA",
        "C90FF8",
        "C92CA5",
        "C9341B",
        "C9BA1C",
        "C9FBDF",
        "CD1A6C",
        "CD1F5D",
        "CD2083",
        "CD20AB",
        "CD25CC",
        "CD2854",
        "CD2AB9",
        "CD410A",
        "CD44D2",
        "CD486F",
        "CD5A38",
        "CE23F3",
        "CE241D",
        "CE252D",
        "CE2A05",
        "CE2ABE",
        "CE2BE1",
        "CE2F26",
        "EB78C9",
        "EB78F9",
        "EB7B03",
        "EB7B10",
        "EB7B8D",
        "EB7C66",
        "EB7CCD",
        "EB7CE3",
        "EB7D01",
        "EB7D52",
        "EB7D87",
        "EB7D95",
        "EB7DEE",
        "EB7E0D",
        "EB7E37",
        "EB7E82",
        "EB7EB6",
        "EB7F32",
        "EB7F7B",
        "EB7F82",
        "EB7FAC",
        "EB8024",
        "EB80A4",
        "EB80B6",
        "EB80BB",
        "EB80D1",
        "EB81B6",
        "EB81C9",
        "EB81E0",
        "EB836D",
        "EC2AAB",
        "EE6235",
        "F10872"
    };
    
    //Meter parameters
    private string meterId_HEX = "";
    private int meterId;
    private int numberOfDays = 1;
    /* The time window to summarize the data. Examples are 20s (20 seconds), 5m (5 minutes) or 1h (1 hour)*/
    private string aggegationWindow = "5m"; 
    private DateOnly datePast = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    private DateOnly dateToday = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

    private async Task CalculateChart()
    {
        //Convert the meter hex to decimal
        meterId = Convert.ToInt32(meterId_HEX, 16);
        
        //Get all the needed measurement data
        usageMeasurements = await MeasureUsage();
        var powerUsageValues = usageMeasurements.Select(m => m.Value).ToList();
        var timestamps = usageMeasurements.Select(m => m.Timestamp.ToString() ?? "date").ToList();

        //Calculate the usage per hour
        var powerDiffrences = new List<double?>();
        double? previousUsage = 0;
        foreach (var powerUsage in powerUsageValues)
        {
            if (previousUsage != 0) powerDiffrences.Add(powerUsage.Value - previousUsage);
            previousUsage = powerUsage;
        }

        //Take out the first value
        powerDiffrences.Remove(0);

        //Get the top 3 per day
        topValues = GetDailyPeaks(powerDiffrences, timestamps, numberOfDays);
        topValues.Sort((x, y) => DateTime.Parse(x.Key).CompareTo(DateTime.Parse(y.Key)));

        //X and Y axis data
        var xAxis = topValues.Select(t => t.Key).ToList(); //Timestamps
        var yAxis = topValues.Select(t => (double?)t.Value).ToList(); //Power values

        //Plot the usageMeasurements
        var plot = PlotChart(
            xAxis,
            yAxis,
            "Time", "Vermogen (Watt)");

        chartData = plot.Key;
        barChartOptions = plot.Value;
        await InvokeAsync(StateHasChanged);
    }

    private KeyValuePair<ChartData, BarChartOptions> PlotChart(List<string> labels, List<double?> yData, string xLabel, string yLabel, int rounding = 3)
    {
        var datasets = new List<IChartDataset>();

        var dataset1 = new BarChartDataset()
        {
            Data = yData,
            BorderWidth = new List<double>() { 0 },
            BackgroundColor = new List<string>()
            {
                "#ECDFCC",
            }
        };

        datasets.Add(dataset1);

        chartData = new ChartData()
        {
            Labels = labels,
            Datasets = datasets
        };

        barChartOptions = new BarChartOptions
        {
            Responsive = true,
            Interaction = new() { Mode = InteractionMode.Y },
            Scales =
            {
                X = new ChartAxes()
                {
                    Ticks = new()
                    {
                        Color = "#D3D3D3FF"
                    },
                    Grid = new()
                    {
                        Color = "#ECDFCC"
                    }
                },
                Y = new ChartAxes()
                {
                    Min = Math.Round(yData.Min().Value - (yData.Max().Value - yData.Min().Value) / 10, rounding),
                    Ticks = new()
                    {
                        Color = "#D3D3D3FF"
                    },
                    Grid = new()
                    {
                        Color = "#ECDFCC"
                    }
                }
            }
        };
        barChartOptions.Scales.X!.Title = new ChartAxesTitle
        {
            Text = xLabel,
            Display = true,
            Color = "#FFFFFF",
            Font = new()
            {
                Size = 15
            }
        };
        barChartOptions.Scales.Y!.Title = new ChartAxesTitle
        {
            Text = yLabel,
            Display = true,
            Color = "#FFFFFF",
            Font = new()
            {
                Size = 15
            }
        };

        barChartOptions.Plugins.Legend.Display = false;
        return new(chartData, barChartOptions);
    }

    private async Task<List<Measurement>> MeasureUsage() => 
        await this.measurementRepository.GetPower(meterId, numberOfDays, aggegationWindow);

    private List<KeyValuePair<string, double?>> GetDailyPeaks(List<double?> powerList, List<string> timestamps, int days)
    {
        //Combines the list of timestamps and power values as a keyvalue pair
        List<KeyValuePair<string, double?>> powerTimeList =
            powerList.Select((v, i) => new { v, i })
                .ToDictionary(x => timestamps[x.i], x => x.v).ToList();

        List<KeyValuePair<string, double?>> powerPeaks = [];

        //Calculates the top 3 peaks per day
        while (powerTimeList.Count > 0)
        {
            //Groups the power values per day
            var dayDate = DateTime.Parse(powerTimeList[0].Key).Date;
            List<KeyValuePair<string, double?>> dayValues = [];
            foreach (var powerTimePair in powerTimeList)
            {
                var currentDate = DateTime.Parse(powerTimePair.Key).Date;
                if (currentDate != dayDate) break;
                dayValues.Add(powerTimePair);
            }

            //Gets the top values of those days
            List<KeyValuePair<string, double?>> dayPeaks =
                dayValues.Select((v, i) => new { v, i })
                    .OrderByDescending(x => x.v.Value)
                    .ThenByDescending(x => x.i)
                    .Take(3).ToDictionary(x => x.v.Key, x => x.v.Value).ToList();

            //Adds the peaks to the list, and removes them from the collection
            powerPeaks.AddRange(dayPeaks);
            int lastIndex = powerTimeList.FindLastIndex(
                x => DateTime.Parse(x.Key).Date == dayDate.Date
            ) + 1; //To grab the whole range, do plus 1 because counting starts from 0
            powerTimeList.RemoveRange(0, lastIndex);
        }

        return powerPeaks;
    }

    private DateOnly GetdateOnly(DateTime dateTime) => DateOnly.Parse($"{dateTime:d}");

    private async void onDatepickerChange(DateOnly dateOnly)
    {
        datePast = dateOnly;
        numberOfDays = (DateTime.Today.Date - dateOnly.ToDateTime(new TimeOnly()).Date).Days + 1;
        await Task.Run(CalculateChart);
        await barChart.UpdateAsync(chartData, barChartOptions);
    }

    private async void OnSelectChange(ChangeEventArgs e)
    {
        //Hides the barchart and table and tells the user data will be loaded
        loadingMessage = "Data wordt geladen...";
        dataHidden = true;
        try
        {
            meterId_HEX = e.Value.ToString();
            await Task.Run(CalculateChart);
            await barChart.UpdateAsync(chartData, barChartOptions);
        }
        catch
        {
            loadingMessage = "Error loading data";
            await InvokeAsync(StateHasChanged);
            return;
        }

        dataHidden = false;
        loadingMessage = " ";
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync()
    {
        meterId_HEX = "BF67F4";
        await Task.Run(CalculateChart);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await barChart.InitializeAsync(chartData, barChartOptions);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}