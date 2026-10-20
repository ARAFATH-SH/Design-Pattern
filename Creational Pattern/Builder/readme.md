# 🏗️ Builder Design Pattern in C#

> **Builder Pattern** is a creational design pattern that separates the construction of a complex object from its representation, allowing the same construction process to create different representations.

সহজ ভাষায়, যখন একটি object তৈরি করতে অনেকগুলো configuration, optional properties এবং validation-এর প্রয়োজন হয়, তখন Builder Pattern ব্যবহার করে object-টিকে ধাপে ধাপে তৈরি করা যায়।

উদাহরণ: একটি Report তৈরি করার সময় title, description, charts, filters এবং export format আলাদাভাবে configure করতে হতে পারে।

---

## 📚 Core Concepts

| Concept | কী জিনিস |
|---|---|
| 🏗️ **Builder Pattern** | Complex object step-by-step তৈরি করা |
| 🔗 **Method Chaining** | প্রতিটি configuration method Builder return করে, যেমন `.SetTitle().AddChart().Build()` |
| 📦 **Product** | যে final object তৈরি হচ্ছে, যেমন `Report` |
| 🛠️ **Builder** | Product ধাপে ধাপে তৈরি করে, যেমন `ReportBuilder` |
| 🎬 **Director** | Predefined construction sequence পরিচালনা করে, যেমন `ReportDirector` |
| ✅ **Build() Method** | Final product return করে এবং প্রয়োজনে validation করে |
| ⚠️ **Telescoping Constructor** | অনেক parameter-যুক্ত constructor-এর জটিলতা Builder কমাতে সাহায্য করে |
| 🎯 **কখন Use করবে** | অনেক properties, optional/mandatory fields এবং বিভিন্ন configurations |

---

## 🎯 The Problem

ধরো, আমাদের একটি `Report` object তৈরি করতে হবে।

একটি Report-এর properties হতে পারে:

- Title
- Description
- Charts
- Filters
- Export format
- Header visibility
- Footer visibility

সরাসরি constructor ব্যবহার করলে সমস্যা হতে পারে।

### ❌ Without Builder

```csharp
var report = new Report(
    "Sales Report",
    "Monthly sales overview",
    charts,
    filters,
    "PDF",
    true,
    false
);
```

এই code-এ:

- ❌ অনেক parameter মনে রাখা কঠিন।
- ❌ কোন argument কী বোঝায়, তা সঙ্গে সঙ্গে বোঝা যায় না।
- ❌ Optional properties manage করা জটিল হতে পারে।
- ❌ নতুন property যোগ হলে constructor পরিবর্তন করতে হতে পারে।

এ ধরনের constructor-কে **Telescoping Constructor Problem**-এর একটি উদাহরণ হিসেবে দেখা যায়।

### ✅ Solution: Builder Pattern

```csharp
var report = new ReportBuilder()
    .SetTitle("Sales Report")
    .SetDescription("Monthly sales overview")
    .AddChart("Revenue")
    .SetExportFormat("PDF")
    .Build();
```

এখানে প্রতিটি method-এর নাম দেখেই বোঝা যায় কোন configuration সেট করা হচ্ছে।

> **Builder Pattern-এর মূল লক্ষ্য শুধু constructor ছোট করা নয়; বরং complex object construction-কে readable, flexible এবং maintainable করা।**

---

## 🧱 Structure of Builder Pattern

```text
             ┌──────────────────┐
             │      Client      │
             └────────┬─────────┘
                      |
                      ▼
             ┌──────────────────┐
             │     Builder      │
             ├──────────────────┤
             │ SetTitle()       │
             │ AddChart()       │
             │ SetFormat()      │
             │ Build()          │
             └────────┬─────────┘
                      |
                      ▼
             ┌──────────────────┐
             │     Product      │
             │      Report      │
             └──────────────────┘
```

### প্রধান Components

1. **Product:** যে final object তৈরি হবে।
2. **Builder:** Product-এর configuration ও construction পরিচালনা করে।
3. **Concrete Builder:** বাস্তবে construction methods implement করে।
4. **Director (Optional):** নির্দিষ্ট build sequence পরিচালনা করে।
5. **Client:** Builder ব্যবহার করে নিজের প্রয়োজন অনুযায়ী object তৈরি করে।

সব implementation-এ Director থাকা বাধ্যতামূলক নয়। Fluent Builder-এ Client সরাসরি Builder methods chain করতে পারে।

---

## ⚙️ How It Works

```text
       Create Builder
             |
             ▼
       Set Mandatory Fields
             |
             ▼
       Configure Optional Fields
             |
             ▼
       Add Components
             |
             ▼
       Call Build()
             |
             ▼
       Validate Configuration
             |
        ┌────┴────┐
        |         |
      Valid     Invalid
        |         |
        ▼         ▼
      Return    Throw
      Product   Exception
```

এই approach-এ object creation-এর প্রতিটি step আলাদাভাবে নিয়ন্ত্রণ করা যায়।

---

## 💻 Example: Builder Pattern in C#

চলো, C# ব্যবহার করে একটি `ReportBuilder` তৈরি করি।

### Project Structure

```text
BuilderPattern/
├── Program.cs
├── Report.cs
├── ReportBuilder.cs
└── ReportDirector.cs
```

প্রথমে একটি Console Application তৈরি করো:

```bash
dotnet new console -n BuilderPattern
cd BuilderPattern
```

এরপর নিচের files তৈরি করো।

### 1. `Report.cs` — Product

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderPattern;

public sealed class Report
{
    public string Title { get; }
    public string Description { get; }
    public IReadOnlyList<string> Charts { get; }
    public string ExportFormat { get; }
    public bool ShowHeader { get; }

    public Report(
        string title,
        string description,
        IEnumerable<string> charts,
        string exportFormat,
        bool showHeader)
    {
        Title = title;
        Description = description;

        Charts = Array.AsReadOnly(charts.ToArray());

        ExportFormat = exportFormat;
        ShowHeader = showHeader;
    }
}
```

`Report` হলো final product, যা Builder তৈরি করবে।

**কেন `IReadOnlyList<string>` ব্যবহার করছি?**

- Report-এর charts বাইরে থেকে পরিবর্তন করা যায় না।
- Constructor charts-এর একটি copy সংরক্ষণ করে।
- Final object-এর state নিয়ন্ত্রণ করা সহজ হয়।

### 2. `ReportBuilder.cs` — Builder

```csharp
using System;
using System.Collections.Generic;

namespace BuilderPattern;

public sealed class ReportBuilder
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private readonly List<string> _charts = new();

    private string _exportFormat = "PDF";
    private bool _showHeader = true;

    public ReportBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public ReportBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public ReportBuilder AddChart(string chart)
    {
        _charts.Add(chart);
        return this;
    }

    public ReportBuilder SetExportFormat(string format)
    {
        _exportFormat = format;
        return this;
    }

    public ReportBuilder SetShowHeader(bool show)
    {
        _showHeader = show;
        return this;
    }

    public Report Build()
    {
        if (string.IsNullOrWhiteSpace(_title))
        {
            throw new InvalidOperationException(
                "Report title is required."
            );
        }

        if (_charts.Count == 0)
        {
            throw new InvalidOperationException(
                "At least one chart is required."
            );
        }

        return new Report(
            _title,
            _description,
            _charts,
            _exportFormat,
            _showHeader
        );
    }
}
```

### Code Explanation

| Code | কাজ |
|---|---|
| `ReportBuilder()` | নতুন Builder instance তৈরি করে |
| `SetTitle()` | Report-এর title সেট করে |
| `SetDescription()` | Description সেট করে |
| `AddChart()` | Report-এ একটি chart যোগ করে |
| `SetExportFormat()` | Export format সেট করে |
| `SetShowHeader()` | Header visibility configure করে |
| `Build()` | Mandatory fields validate করে final Report return করে |

**কেন প্রতিটি configuration method `ReportBuilder` return করছে?**

```csharp
public ReportBuilder SetTitle(string title)
{
    _title = title;
    return this;
}
```

`return this` বর্তমান Builder instance-টিকেই return করে। এর ফলে method chaining সম্ভব হয়:

```csharp
var builder = new ReportBuilder()
    .SetTitle("Sales Report")
    .AddChart("Revenue")
    .SetExportFormat("PDF");
```

C#-এ `this` বর্তমান object-কে বোঝায়।

**Validation কেন `Build()` method-এ?**

Builder-এর configuration সম্পূর্ণ হওয়ার পর `Build()` যাচাই করে যে final Report তৈরির জন্য প্রয়োজনীয় তথ্য আছে কি না।

এই উদাহরণে title ফাঁকা হলে অথবা কোনো chart না থাকলে `InvalidOperationException` throw করা হয়।

### 3. `Program.cs` — Client

```csharp
using System;

namespace BuilderPattern;

public class Program
{
    public static void Main()
    {
        var report = new ReportBuilder()
            .SetTitle("Monthly Sales Report")
            .SetDescription("Sales performance for October")
            .AddChart("Revenue")
            .AddChart("Orders")
            .SetExportFormat("PDF")
            .SetShowHeader(true)
            .Build();

        Console.WriteLine($"Title: {report.Title}");
        Console.WriteLine($"Description: {report.Description}");
        Console.WriteLine($"Charts: {string.Join(", ", report.Charts)}");
        Console.WriteLine($"Format: {report.ExportFormat}");
        Console.WriteLine($"Show Header: {report.ShowHeader}");
    }
}
```

### Output

```text
Title: Monthly Sales Report
Description: Sales performance for October
Charts: Revenue, Orders
Format: PDF
Show Header: True
```

এখন Client সহজেই বিভিন্ন configuration দিয়ে Report তৈরি করতে পারে, এবং `Build()` mandatory fields validate করতে পারে।

### Run the Project

```bash
dotnet run
```

---

## 🔗 Method Chaining

Method Chaining হলো Builder Pattern-এর একটি common technique।

```csharp
var report = new ReportBuilder()
    .SetTitle("Quarterly Report")
    .SetDescription("Q3 business performance")
    .AddChart("Revenue")
    .AddChart("Profit")
    .SetExportFormat("PDF")
    .Build();
```

### কীভাবে কাজ করে?

```text
New ReportBuilder()
         |
         ▼
      SetTitle()
         |
         ▼
   SetDescription()
         |
         ▼
      AddChart()
         |
         ▼
   SetExportFormat()
         |
         ▼
       Build()
         |
         ▼
       Report
```

এর প্রধান সুবিধা:

- ✅ Code পড়া সহজ হয়।
- ✅ প্রতিটি configuration-এর উদ্দেশ্য পরিষ্কার থাকে।
- ✅ Optional properties সেট করা সুবিধাজনক হয়।
- ✅ Construction steps ধারাবাহিকভাবে বোঝা যায়।

---

## 🎬 What Is a Director?

Director হলো এমন একটি component, যা predefined build sequence পরিচালনা করে।

ধরো, application-এ দুই ধরনের Report দরকার:

- Monthly Report
- Quarterly Report

প্রতিবার একই configuration manually লেখার পরিবর্তে Director predefined sequence দিতে পারে।

### 4. `ReportDirector.cs` — Optional Director

```csharp
namespace BuilderPattern;

public sealed class ReportDirector
{
    public Report BuildMonthlyReport()
    {
        return new ReportBuilder()
            .SetTitle("Monthly Report")
            .SetDescription("Monthly business overview")
            .AddChart("Revenue")
            .SetExportFormat("PDF")
            .Build();
    }

    public Report BuildQuarterlyReport()
    {
        return new ReportBuilder()
            .SetTitle("Quarterly Report")
            .SetDescription("Quarterly business overview")
            .AddChart("Revenue")
            .AddChart("Profit")
            .SetExportFormat("PDF")
            .Build();
    }
}
```

### ব্যবহার

```csharp
var director = new ReportDirector();

var monthly = director.BuildMonthlyReport();
var quarterly = director.BuildQuarterlyReport();

Console.WriteLine(monthly.Title);
Console.WriteLine(quarterly.Title);
```

Output:

```text
Monthly Report
Quarterly Report
```

Director ব্যবহার করলে common construction sequences পুনরায় ব্যবহার করা যায়।

**মনে রাখবে:** Director optional। প্রতিটি Builder implementation-এ আলাদা Director class দরকার হয় না।

---

## 🆚 Builder vs Constructor

| Feature | Constructor | Builder |
|---|---|---|
| Object creation | একবারে initialize করে | ধাপে ধাপে configure করে |
| Parameters | অনেক parameter হতে পারে | Named methods দিয়ে configure করা যায় |
| Optional properties | অনেক overload বা default value লাগতে পারে | শুধু প্রয়োজনীয় methods call করা যায় |
| Readability | Parameter বেশি হলে কমতে পারে | Method chaining readable হতে পারে |
| Validation | Constructor-এ করা যায় | `Build()`-এ কেন্দ্রীভূত করা যায় |
| Best for | Simple objects | Complex, configurable objects |

### Example

**Constructor:**

```csharp
var user = new User("Arafath", 21, "Dhaka", true);
```

**Builder:**

```csharp
var user = new UserBuilder()
    .SetName("Arafath")
    .SetAge(21)
    .SetCity("Dhaka")
    .SetActive(true)
    .Build();
```

যদি object-এর মাত্র দুই বা তিনটি simple properties থাকে, constructor-ই সাধারণত যথেষ্ট।

---

## 🆚 Builder vs Factory

| Feature | Builder Pattern | Factory Pattern |
|---|---|---|
| Main purpose | Complex object step-by-step তৈরি করা | কোন concrete object তৈরি হবে তা encapsulate করা |
| Focus | Construction process | Object creation/selection |
| Configuration | একাধিক ধাপে করা যায় | সাধারণত factory call-এর মাধ্যমে |
| Example | `ReportBuilder` | `PaymentFactory` |
| Best for | অনেক configuration ও optional fields | বিভিন্ন concrete type তৈরি বা নির্বাচন করা |

সহজভাবে:

- **Factory:** কোন object তৈরি হবে?
- **Builder:** Object-টি কীভাবে ধাপে ধাপে তৈরি হবে?

দুটি pattern একসঙ্গেও ব্যবহার করা যায়।

---

## ✅ কখন Builder Pattern ব্যবহার করবো?

### 1. Object-এ অনেক Configuration আছে

যখন object-এ অনেক properties configure করতে হয় এবং constructor অস্বস্তিকর হয়ে যায়।

Examples:

- Report Builder
- HTTP Request Builder
- Query Builder
- Email Builder
- Configuration Builder

### 2. Optional ও Mandatory Properties Mix আছে

কিছু properties অবশ্যই দিতে হবে, অন্যগুলো optional।

```text
Report
├── Title         (Mandatory)
├── Charts        (Mandatory)
├── Description   (Optional)
├── Export Format (Optional)
└── Show Header   (Optional)
```

Builder-এর `Build()` method-এ mandatory properties validate করা যায়।

### 3. বিভিন্ন Configuration দরকার

একই ধরনের object বিভিন্ন configuration-এ তৈরি করতে হলে Builder উপকারী।

Examples:

- Monthly Report
- Quarterly Report
- Dashboard Configuration
- PDF Export Settings

### 4. Code Readability গুরুত্বপূর্ণ

```csharp
var report = new ReportBuilder()
    .SetTitle("Annual Report")
    .AddChart("Revenue")
    .SetExportFormat("PDF")
    .Build();
```

এটি অনেক positional constructor arguments-এর তুলনায় পড়তে সহজ হতে পারে।

### 5. Validation দরকার

`Build()` method-এ final object return করার আগে required fields এবং configuration যাচাই করা যায়।

---

## ❌ কখন Builder Pattern ব্যবহার করবো না?

### 1. Object Simple

```csharp
var user = new User
{
    Name = "Arafath",
    Roll = 101
};
```

দুই বা তিনটি properties-এর জন্য Builder বানালে অপ্রয়োজনীয় complexity বাড়তে পারে।

### 2. সব Properties Mandatory

যদি সব values constructor-এ সরাসরি দেওয়া যায় এবং optional configuration না থাকে, তাহলে constructor আরও পরিষ্কার হতে পারে।

### 3. Performance-Critical Code

Builder অতিরিক্ত object allocation বা method-call overhead তৈরি করতে পারে। তবে Builder সবসময় performance খারাপ করে না। বাস্তব workload-এ benchmark করে সিদ্ধান্ত নেওয়া উচিত।

### 4. Over-engineering

একটি simple `Student` object-এর `Name`, `Roll`, `Marks` properties-এর জন্য Builder তৈরি করা সাধারণত unnecessary।

> **Simple object → Simple constructor. Complex object → Consider Builder.**

---

## ⚠️ Common Mistakes

- ❌ সব object-এর জন্য Builder ব্যবহার করা।
- ❌ `Build()` method-এ mandatory fields validate না করা।
- ❌ Optional fields-এর sensible defaults না রাখা।
- ❌ একই mutable Builder instance একাধিক thread থেকে synchronization ছাড়া ব্যবহার করা।
- ❌ Final product-এর mutable collections অপ্রত্যাশিতভাবে share করা।
- ❌ Director সব সময় বাধ্যতামূলক মনে করা।

### C#-Specific Considerations

**1. Collection isolation**

Builder-এর `Build()` method final Report-এ charts-এর copy পাঠায়। ফলে পরে Builder-এ নতুন chart যোগ করলেও আগে তৈরি হওয়া Report-এর charts পরিবর্তিত হয় না।

**2. Read-only properties**

`Report`-এর properties read-only রাখা হয়েছে, যাতে object তৈরির পরে সেগুলো সরাসরি পরিবর্তন করা না যায়।

**3. Exception-based validation**

এই উদাহরণে invalid configuration পেলে `Build()` একটি `InvalidOperationException` throw করে। প্রয়োজনে application-এর error-handling strategy অনুযায়ী validation design পরিবর্তন করা যায়।

**4. Builder thread safety**

`ReportBuilder` mutable state ব্যবহার করে। একই Builder instance একাধিক thread-এ একসঙ্গে ব্যবহার করলে synchronization ছাড়া race condition হতে পারে। সাধারণত প্রতিটি construction-এর জন্য আলাদা Builder instance ব্যবহার করাই সহজ।

---

## ⭐ Benefits

- ✅ Improves readability
- ✅ Simplifies complex object construction
- ✅ Supports optional configuration
- ✅ Centralizes validation
- ✅ Makes construction steps reusable
- ✅ Reduces long parameter lists
- ✅ Supports multiple configurations of the same product

## ⚠️ Trade-offs

- ❌ Adds extra code and types
- ❌ May be unnecessary for simple objects
- ❌ Mutable Builder state requires careful handling
- ❌ Can introduce overhead in performance-sensitive paths
- ❌ Director and Concrete Builder may add unnecessary abstraction

---

## 🧠 Quick Decision Guide

```text
Does the object have complex configuration?
                 |
              ┌──┴───┐
             NO      YES
              |        |
              ▼        ▼
         Constructor  Many optional properties?
                         |
                      ┌──┴───┐
                     NO      YES
                      |        |
                      ▼        ▼
                 Constructor  Is step-by-step
                              construction useful?
                                  |
                               ┌──┴───┐
                              NO      YES
                               |        |
                               ▼        ▼
                           Keep it   Consider
                           simple    Builder
```

---

## 📌 Summary

```text
Builder Pattern
       |
       ├── Step-by-step construction
       |
       ├── Readable configuration
       |
       ├── Optional and mandatory fields
       |
       ├── Build-time validation
       |
       ├── Method chaining
       |
       └── Optional Director for predefined sequences
```

### The Main Idea

> **"Separate the construction of a complex object from its final representation."**

মনে রাখবে:

- **Many configuration options →** Consider Builder.
- **Optional and mandatory fields →** Validate with `Build()`.
- **Repeated construction sequences →** Consider a Director.
- **Simple object →** Prefer a constructor.

**Builder Pattern ব্যবহার করো যখন এটি object construction-কে বাস্তবিকভাবে সহজ, readable এবং maintainable করে।**
