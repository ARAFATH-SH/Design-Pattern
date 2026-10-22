# 🧬 Prototype Design Pattern in C#

> **Prototype Pattern** is a creational design pattern that creates new objects by cloning existing instances instead of constructing them from scratch.

সহজ ভাষায়, যখন একটি existing object-এর মতো আরেকটি object তৈরি করতে হয়, তখন শুরু থেকে সব properties configure না করে original object-কে clone করে প্রয়োজনীয় পরিবর্তন করা যায়।

**Core Idea:** Create an object once, clone it when needed, and customize the copy.

## 📚 Core Concepts

| Concept | Description |
|---|---|
| 🧬 **Prototype Pattern** | Existing object clone করে নতুন object তৈরি করা |
| 📋 **Prototype** | যে original object-কে clone করা হবে |
| 📦 **Clone** | Original object-এর copy |
| ⚠️ **Shallow Copy** | Value types copy হয়, কিন্তু reference types share হতে পারে |
| 🔒 **Deep Copy** | Nested reference objects-সহ independent copy তৈরি করা |
| 🧩 **`IPrototype<T>`** | Custom, type-safe cloning interface |
| 🛠️ **`MemberwiseClone()`** | C#-এর built-in shallow-copy method |
| ✨ **Record with-expression** | Record object-এর পরিবর্তিত copy তৈরি করার modern C# approach |

---

## 🧠 The Problem

ধরো, তোমাকে একটি application-এ একই configuration-সহ অনেকগুলো report তৈরি করতে হবে।

প্রতিবার নতুন করে সব properties configure করলে repetitive code তৈরি হতে পারে।

### ❌ Without Prototype

```csharp
var report1 = new Report
{
    Title = "Monthly Sales",
    Format = "PDF",
    ShowHeader = true
};

var report2 = new Report
{
    Title = "Quarterly Sales",
    Format = "PDF",
    ShowHeader = true
};
```

এখানে দুটি report-এর configuration-এর বেশিরভাগ একই।

### ✅ With Prototype

```csharp
var template = new Report
{
    Title = "Default Report",
    Format = "PDF",
    ShowHeader = true
};

var monthlyReport = template.Clone();
monthlyReport.Title = "Monthly Sales";

var quarterlyReport = template.Clone();
quarterlyReport.Title = "Quarterly Sales";
```

এখন common configuration একবার তৈরি করে clone থেকে আলাদা report তৈরি করা যায়।

> **মূল ধারণা:** একই ধরনের object বারবার তৈরি করতে হলে existing object-কে prototype হিসেবে ব্যবহার করো।

---

## ⚙️ How It Works

```text
       Create Prototype
              |
              ▼
      Configure Original
              |
              ▼
         Clone Object
              |
              ▼
       Customize the Copy
              |
              ▼
        New Object Ready
```

প্রক্রিয়াটি:

1. একটি prototype object তৈরি করো।
2. সেটিকে common configuration দিয়ে initialize করো।
3. প্রয়োজন অনুযায়ী clone তৈরি করো।
4. Clone-এর আলাদা properties পরিবর্তন করো।
5. Original object অক্ষত রাখো।

---

## 🎯 When to Use Prototype Pattern

### 1. Object Creation Is Expensive

যখন object তৈরি করতে expensive operations লাগে, যেমন:

- Database query
- API call
- File reading
- Complex calculations
- Large configuration initialization

Prototype ব্যবহার করলে আগে তৈরি করা state reuse করা যেতে পারে।

**Important:** Clone নিজে সব সময় expensive initialization skip করে না। এটা নির্ভর করে original object কীভাবে তৈরি হয়েছে এবং clone implementation কী copy করছে তার ওপর।

### 2. Objects Are Mostly Similar

যখন নতুন object-এর প্রায় ৮০% বা তার বেশি configuration একই, তখন template clone করে শুধু আলাদা properties পরিবর্তন করা সুবিধাজনক হতে পারে।

```csharp
var template = new Report
{
    Format = "PDF",
    ShowHeader = true
};

var report = template.Clone();
report.Title = "Annual Report";
```

৮০% কোনো কঠোর technical rule নয়; এটি একটি practical heuristic।

### 3. Runtime-এ Object Type বা Configuration নির্ধারিত হয়

যখন compile time-এ জানা থাকে না কোন preconfigured object লাগবে, তখন runtime-এ prototype registry থেকে object নিয়ে clone করা যায়।

### 4. Template-Based Creation

Examples:

- Order templates
- Configuration cloning
- Test data generation
- Document templates
- Game level templates

---

## ❌ When Not to Use Prototype Pattern

### 1. Object Creation Is Cheap

```csharp
var student = new Student("Rahim", 101, 85);
```

এখানে সাধারণ constructor ব্যবহার করাই সহজ। Clone implement করার জন্য অতিরিক্ত complexity যোগ করার প্রয়োজন নেই।

### 2. Objects Are Deeply Nested

৫–৬ level nested objects থাকলে deep copy implement করা জটিল ও error-prone হতে পারে। কোন nested object copy হবে আর কোনটি share করা নিরাপদ, তা নির্ধারণ করতে হয়।

### 3. Objects Are Immutable

যদি object পরিবর্তন করার প্রয়োজন না থাকে, immutable instance share করাই অনেক ক্ষেত্রে যথেষ্ট। আলাদা copy তৈরি করার দরকার নাও হতে পারে।

### 4. Objects Are Very Different

যদি নতুন object-এর অধিকাংশ properties-ই পরিবর্তন করতে হয়, তাহলে prototype clone করে বারবার পরিবর্তন করার পরিবর্তে নতুন object তৈরি করাই পরিষ্কার হতে পারে।

> **Simple object → Constructor. Mostly similar objects → Consider Prototype.**

---

## 🔍 Shallow Copy vs Deep Copy

Prototype Pattern ব্যবহার করার সময় copy-এর ধরন বোঝা অত্যন্ত গুরুত্বপূর্ণ।

### 1. Shallow Copy

Shallow copy-তে value-type fields-এর values copy হয়, কিন্তু reference-type fields-এর referenced objects সাধারণত share হয়।

```csharp
using System;

public class Student
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
}

public class Address
{
    public string City { get; set; } = string.Empty;
}
```

Shallow copy তৈরি করলে original ও clone একই `Address` object reference করতে পারে।

```text
Original Student ───┐
                    ├──> Address Object
Cloned Student ─────┘
```

ফলে clone-এর address পরিবর্তন করলে original student-এর address-ও পরিবর্তিত হতে পারে।

### 2. Deep Copy

Deep copy-তে nested mutable objects-সহ প্রয়োজনীয় object graph-এর independent copy তৈরি করা হয়।

```text
Original Student ───> Original Address
                               
Cloned Student ─────> Copied Address
```

এখানে cloned student's address পরিবর্তন করলেও original address অপরিবর্তিত থাকে।

**মনে রাখবে:** Deep copy মানে সব reference-এর নির্বিচার duplication নয়। কোন objects share করা হবে আর কোনগুলো copy হবে, তা application-এর semantics অনুযায়ী নির্ধারণ করতে হয়।

---

## 💻 Implementation Approaches in C#

### Approach 1: Custom `IPrototype<T>` Interface

একটি custom interface ব্যবহার করে cloning contract নির্ধারণ করা যায়।

```csharp
public interface IPrototype<T>
{
    T Clone();
}
```

যে class এই interface implement করবে, সে নিজের cloning logic নির্ধারণ করবে।

**Best for:** Explicit, type-safe cloning behavior।

### Approach 2: `MemberwiseClone()`

C#-এর `MemberwiseClone()` একটি shallow copy তৈরি করে।

```csharp
public class Report
{
    public string Title { get; set; } = string.Empty;

    public Report Clone()
    {
        return (Report)MemberwiseClone();
    }
}
```

এখানে `MemberwiseClone()`-এর মাধ্যমে current object-এর shallow copy তৈরি হয়।

**Best for:** Simple object, যেখানে shallow-copy semantics গ্রহণযোগ্য।

**Limitation:** Nested mutable reference objects independent হয় না।

### Approach 3: Record with-expression

Modern C#-এ `record` এবং `with` expression দিয়ে একটি record-এর পরিবর্তিত copy তৈরি করা যায়।

```csharp
public record Report(
    string Title,
    string Format,
    bool ShowHeader
);
```

ব্যবহার:

```csharp
var original = new Report(
    "Default Report",
    "PDF",
    true
);

var copy = original with
{
    Title = "Monthly Report"
};
```

এখানে `copy` একটি নতুন record instance। তবে nested reference-type properties থাকলে সেগুলো স্বয়ংক্রিয়ভাবে deep-copy হয় না।

**Best for:** Simple data models এবং immutable-style objects।

---

## 🌍 Real-World Use Cases

| Use Case | Prototype কীভাবে সাহায্য করে |
|---|---|
| 📄 Document Templates | Preconfigured document clone করে নতুন document তৈরি করা |
| 🧪 Test Data Generation | Base test object clone করে নির্দিষ্ট fields পরিবর্তন করা |
| ⚙️ Configuration | Existing configuration থেকে variant তৈরি করা |
| 🛒 Order Templates | Reusable order details থেকে নতুন order তৈরি করা |
| 🎮 Game Development | Preconfigured game entities বা level templates থেকে instances তৈরি করা |
| 🧩 Runtime Object Registry | Registered prototypes থেকে runtime-এ clone তৈরি করা |

---

## ⭐ Benefits

- ✅ Reuses preconfigured object state
- ✅ Reduces repeated configuration code
- ✅ Makes template-based object creation convenient
- ✅ Supports runtime selection of prototypes
- ✅ Can avoid repeating expensive initialization when the clone strategy permits it
- ✅ Helps create related objects with small variations

## ⚠️ Trade-offs

- ❌ Deep copying can be complex
- ❌ Shallow copies may share mutable references unexpectedly
- ❌ Cloning logic can become difficult to maintain for nested objects
- ❌ Immutable objects may not need cloning
- ❌ Simple objects may be easier to create with constructors
- ❌ Clone semantics must be clearly defined

---
