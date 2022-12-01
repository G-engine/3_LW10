using WebApplication1.Models;

using (var db = new TickerdbContext())
{
    db.Prices.RemoveRange(db.Prices);
    db.Tickers.RemoveRange(db.Tickers);
    db.TodaysConditions.RemoveRange(db.TodaysConditions);
    db.SaveChanges();
    
    StreamReader tickerFile = new StreamReader("ticker.txt");
    string [] tickerArray = tickerFile.ReadToEnd().Split('\n', StringSplitOptions.TrimEntries);
    tickerFile.Close();

    using HttpClient client = new HttpClient();
    double[] newPrices = new double[tickerArray.Length];
    for (int i = 0; i < tickerArray.Length; ++i)
    {
        try
        {
            List<double> lastPerDay = new List<double>();
            var content = await client.GetStringAsync(
                $"https://query1.finance.yahoo.com/v7/finance/download/{tickerArray[i].Trim('\r')}?period1=1669680000&period2=1669852799&interval=1d&events=history&includeAdjustedClose=true");
            string [] dayContent = content.Split('\n', StringSplitOptions.TrimEntries);
            dayContent[0] = "";
            for (int j = 1; j < dayContent.Length; ++j) //2 дня = вчерашний и сегодняшний
            {
                string [] prices = dayContent[j].Split(','); //разные цены в течение дня (лоу, хай, открытие, закрытие)
                double last = Double.Parse(prices[4].Replace('.', ',')); //цена закрытия
                lastPerDay.Add(last);
            }
            
            Ticker t = new Ticker { Ticker1 = tickerArray[i] };
            db.Tickers.Add(t);
            db.Prices.Add(new Price { TickerId = t.Id, Price1 = lastPerDay[0], Date = new DateTime(2022, 11, 29), Ticker = t});
            db.Prices.Add(new Price { TickerId = t.Id, Price1 = lastPerDay[1], Date = new DateTime(2022, 11, 30), Ticker = t});
            db.TodaysConditions.Add(new TodaysCondition { TickerId = t.Id, Ticker = t, State = lastPerDay[1] > lastPerDay[0] });
            db.SaveChanges(); 
        }
        catch(Exception e)
        {
            Console.WriteLine("Error: " + tickerArray[i] + ", " + e.Message);
        }
    }

    string? tick;
    tick = Console.ReadLine();
    if (tick != null)
    {
        int id = db.Tickers.FirstOrDefault(t => t.Ticker1 == tick).Id;
        Console.Write(db.TodaysConditions.FirstOrDefault(s => s.TickerId == id).State);
    }
}       