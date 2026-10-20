/*
//HTTP Request Builder
public class HttpRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public string Body { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
    public string AuthToken { get; set; }

    public void Send()
    {
        Console.WriteLine($"{Method} {Url}");
        foreach (var header in Headers)
        {
            Console.WriteLine($"{header.Key}: {header.Value}");
        }

        if (!string.IsNullOrEmpty(AuthToken))
        {
            Console.WriteLine($"Authorization: Bearer {AuthToken[..10]}...");
        }

        if (!string.IsNullOrEmpty(Body))
        {
            Console.WriteLine($"Body: {Body}...");
        }
        Console.WriteLine($"Timeout: {TimeoutSeconds} seconds");
        Console.WriteLine("Request Sent\n");
    }
}

//Builder
public class HttpRequestBuilder
{
    private HttpRequest _httpRequest = new HttpRequest();

    public HttpRequestBuilder GET(string url)
    {
        _httpRequest.Method = "GET";
        _httpRequest.Url = url;
        return this;
    }

    public HttpRequestBuilder POST(string url)
    {
        _httpRequest.Method = "POST";
        _httpRequest.Url = url;
        return this;
    }

    public HttpRequestBuilder PUT(string url)
    {
        _httpRequest.Method = "PUT";
        _httpRequest.Url = url;
        return this;
    }

    public HttpRequestBuilder AddHeader(string key, string value)
    {
        _httpRequest.Headers.Add(key, value);
        return this;
    }

    public HttpRequestBuilder SetBody(string body)
    {
        _httpRequest.Body = body;
        return this;
    }

    public HttpRequestBuilder SetTimeoutSeconds(int timeoutSeconds)
    {
        _httpRequest.TimeoutSeconds = timeoutSeconds;
        return this;
    }
    public HttpRequestBuilder SetAuthToken(string token)
    {
        _httpRequest.AuthToken = token;
        return this;
    }

    public HttpRequest Build()
    {
        if (string.IsNullOrEmpty(_httpRequest.Url))
        {
            throw new InvalidOperationException("URL is required");
        }

        HttpRequest result = _httpRequest;
        _httpRequest = new HttpRequest();
        return result;
    }
}

//Director
public class HttpRequestDirector
{
    private HttpRequestBuilder  _httpRequestBuilder;
    public HttpRequestDirector(HttpRequestBuilder httpRequestBuilder)
    {
        _httpRequestBuilder = httpRequestBuilder;
    }

    public HttpRequest getRequest(string url,string key, string value)
    {
        return _httpRequestBuilder.GET(url).AddHeader(key, value).Build();
    }

    public HttpRequest postRequest(string url, string key, string value, string authToken, string body,
        int timeoutSeconds)
    {
        return _httpRequestBuilder.POST(url).AddHeader(key, value).SetAuthToken(authToken).SetBody(body)
            .SetTimeoutSeconds(timeoutSeconds).Build();
    }

    public HttpRequest putRequest(string url, string key, string value, string authToken, string body)
    {
        return _httpRequestBuilder.PUT(url).AddHeader(key, value).SetAuthToken(authToken).SetBody(body)
            .Build();
    }
}
//Client Code
public class Program
{
    public static void Main(string[] args)
    {
        HttpRequestBuilder builder = new HttpRequestBuilder();
        HttpRequestDirector director = new HttpRequestDirector(builder);
        
        HttpRequest getReq = director.getRequest("https://api.example.com","Accept","application/json");
        getReq.Send();
    }
}
*/

//Report Generation

public class Report
{
    public string Title { get; set; }
    public string Header { get; set; }
    public string Logo { get; set; }
    public string Data { get; set; }
    public string DateRange { get; set; }
    public bool IncludeChart { get; set; }
    public string ChartType { get; set; }
    public string Footer { get; set; }
    public string PageSize { get; set; } = "A4";
    public string Orientation { get; set; } = "Portrait";
    public string Watermark { get; set; }
    public string ExportFormat { get; set; } = "PDF";

    public void Print()
    {
        Console.WriteLine(Title);
        if (!string.IsNullOrEmpty(Logo))
            Console.WriteLine($"  Logo: {Logo}");
        if (!string.IsNullOrEmpty(DateRange))
            Console.WriteLine($"  Date: {DateRange}");
        if (!string.IsNullOrEmpty(Data))
            Console.WriteLine($"  Data: {Data}");
        if (IncludeChart)
            Console.WriteLine($"  Chart: {ChartType}");
        if (!string.IsNullOrEmpty(Footer))
            Console.WriteLine($"  Footer: {Footer}");
        
        Console.WriteLine($"Page: {PageSize} {Orientation}");
        
        if (!string.IsNullOrEmpty(Watermark))
            Console.WriteLine($"  Watermark: {Watermark}");
        Console.WriteLine($"  ExportFormat: {ExportFormat}");
    }
}

public class ReportBuilder
{
    private Report _report = new Report();
    
    public ReportBuilder SetTitle(string title)
    {
        _report.Title = title;
        return this;
    }

    public ReportBuilder SetHeader(string header)
    {
        _report.Header = header;
        return this;
    }

    public ReportBuilder SetLogo(string logo)
    {
        _report.Logo = logo;
        return this;
    }

    public ReportBuilder SetDateRange(string dateRange)
    {
        _report.DateRange = dateRange;
        return this;
    }

    public ReportBuilder SetData(string data)
    {
        _report.Data = data;
        return this;
    }

    public ReportBuilder AddChart(string chartType)
    {
        _report.IncludeChart = true;
        _report.ChartType = chartType;
        return this;
    }

    public ReportBuilder SetFooter(string footer)
    {
        _report.Footer = footer;
        return this;
    }

    public ReportBuilder SetPageSize(string size)
    {
        _report.PageSize = size;
        return this;
    }

    public ReportBuilder SetOrientation(string orientation)
    {
        _report.Orientation = orientation;
        return this;
    }

    public ReportBuilder SetWatermark(string watermark)
    {
        _report.Watermark = watermark;
        return this;
    }

    public ReportBuilder SetExportFormat(string format)
    {
        _report.ExportFormat = format;
        return this;
    }

    public Report Build()
    {
        if(string.IsNullOrEmpty(_report.Title))
            throw  new Exception("Title not set");
        if (string.IsNullOrEmpty(_report.Data))
            throw new Exception("Data not set");

        Report report = _report;
        _report = new Report();
        return report;
    }
}
public class ReportBuilderDirector
{
    private ReportBuilder _builder;

    public ReportBuilderDirector(ReportBuilder builder)
    {
        _builder = builder;
    }
    
    public Report BuildMonthlyReport(string month, string data)
    {
        return _builder
            .SetTitle($"Monthly Sales Report - {month}")
            .SetHeader("Claude")
            .SetLogo("logo.png")
            .SetDateRange(month)
            .SetData(data)
            .AddChart("Bar")
            .SetFooter("Confidential - Internal Use Only")
            .SetPageSize("A4")
            .SetOrientation("Landscape")
            .SetWatermark("DRAFT")
            .SetExportFormat("PDF")
            .Build();
    }
    
    public Report BuildDashboardReport(string month, string data)
    {
        return _builder
            .SetTitle($"Dashboard Summary - {month}")
            .SetHeader("Claude")
            .SetLogo("logo.png")
            .SetData(data)
            .SetExportFormat("PDF")
            .Build();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ReportBuilder builder = new ReportBuilder();
        ReportBuilderDirector  director = new ReportBuilderDirector(builder);

        var report = director.BuildDashboardReport("january 2026", "Revenue: 50000 Dollar | Order: 150");
        report.Print();
        
    }
}
























