# 🏗️ Creational Design Patterns in C#

A practical collection of **Creational Design Patterns** implemented in **C#**.

Creational patterns focus on **how objects are created**. Instead of creating objects everywhere with `new`, these patterns provide cleaner and more flexible approaches to object creation.

This repository covers five important creational patterns:

- 🔒 **Singleton**
- 🏭 **Factory Method**
- 🏭 **Abstract Factory**
- 🧱 **Builder**
- 🧬 **Prototype**

---

## 📚 Patterns Overview

| Pattern | Problem | Main Idea | Example Use Cases |
|---|---|---|---|
| 🔒 **Singleton** | Multiple instances are being created when only one is needed | Private constructor + static instance | Logger, Config, Cache |
| 🏭 **Factory Method** | The concrete class to create is decided at runtime | Move creation logic into a factory | Payment, Notification, Export |
| 🏭 **Abstract Factory** | Related objects need to be created consistently as a family | A family factory creates related objects together | Multi-region, Database Layer, UI Theme |
| 🧱 **Builder** | Object creation requires many steps or configurations | Build the object step-by-step | Report, HTTP Request, Email, Query |
| 🧬 **Prototype** | A new object is similar to an existing object and creating it from scratch is expensive | Clone an existing object and modify it | Order Template, Config, Test Data |

---

# 🧠 What Are Creational Design Patterns?

Creational Design Patterns deal with **object creation mechanisms**.

Without these patterns, object creation can become tightly coupled:

```csharp
var payment = new StripePayment();
var notification = new EmailNotification();
var report = new Report();
```

As an application grows, directly creating concrete classes everywhere can make the code:

- Harder to maintain
- Difficult to extend
- Tightly coupled
- Harder to test
- Repetitive

Creational patterns help us **control, simplify, or abstract object creation**.

---

# 🔒 1. Singleton Pattern

## What problem does it solve?

Sometimes an application should have **only one instance** of a particular class.

For example:

- Logger
- Configuration manager
- Cache
- Application-wide service

Instead of allowing:

```text
Object → Object → Object → Object
```

Singleton ensures:

```text
        ┌──────────────┐
        │   Singleton  │
        │   Instance   │
        └──────┬───────┘
               │
       ┌───────┼───────┐
       ↓       ↓       ↓
    Client   Client   Client
```

## Core Idea

The typical implementation uses:

- A **private constructor**
- A **static instance**
- A public way to access that instance

### Example

```csharp
public class Logger
{
    private static Logger? _instance;

    private Logger()
    {
    }

    public static Logger Instance
    {
        get
        {
            return _instance ??= new Logger();
        }
    }

    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}
```

### Usage

```csharp
Logger.Instance.Log("Application started");
```

## When to Use

Use Singleton when:

- Only one instance is logically required
- Multiple parts of the application need access to the same instance
- Creating multiple instances would be unnecessary

### Common Examples

```text
Logger
Configuration
Cache
Application-wide resources
```

---

# 🏭 2. Factory Method Pattern

## What problem does it solve?

Sometimes the application knows **what kind of object it needs**, but the exact concrete class should be decided at runtime.

For example, a payment system may support:

```text
Credit Card
PayPal
Bank Transfer
```

Instead of creating concrete classes directly:

```csharp
new CreditCardPayment();
new PayPalPayment();
```

the creation logic is moved into a factory.

## Core Idea

```text
          Client
             │
             ↓
       Factory Method
             │
       ┌─────┼─────┐
       ↓     ↓     ↓
    Card   PayPal  Bank
```

The client doesn't need to directly control the concrete object creation.

## Example Use Cases

- Payment systems
- Notification systems
- Export systems
- File processing

### Example

```csharp
public interface IPayment
{
    void Pay();
}
```

```csharp
public class CreditCardPayment : IPayment
{
    public void Pay()
    {
        Console.WriteLine("Paid using Credit Card.");
    }
}
```

```csharp
public class PayPalPayment : IPayment
{
    public void Pay()
    {
        Console.WriteLine("Paid using PayPal.");
    }
}
```

A factory can decide which implementation should be created.

```text
Payment Request
      │
      ↓
   Factory
      │
 ┌────┴────┐
 ↓         ↓
Card     PayPal
```

---

# 🏭 3. Abstract Factory Pattern

## What problem does it solve?

Sometimes we don't need just **one object**.

We need a **family of related objects** that should work consistently together.

For example, consider a UI system supporting different themes:

```text
Light Theme
 ├── Button
 └── Checkbox

Dark Theme
 ├── Button
 └── Checkbox
```

The application should not accidentally combine:

```text
Dark Button + Light Checkbox
```

Abstract Factory helps create related objects as a consistent family.

## Core Idea

```text
              Abstract Factory
                    │
          ┌─────────┴─────────┐
          ↓                   ↓
     Light Factory        Dark Factory
          │                   │
      ┌───┴───┐           ┌───┴───┐
      ↓       ↓           ↓       ↓
   Button  Checkbox     Button  Checkbox
```

## Example Use Cases

- Multi-region systems
- Database layers
- UI themes
- Platform-specific components

### Example Families

```text
Region A
 ├── Payment
 ├── Notification
 └── Database

Region B
 ├── Payment
 ├── Notification
 └── Database
```

The factory ensures that the related objects belong to the same family.

---

# 🧱 4. Builder Pattern

## What problem does it solve?

Some objects require **many configuration steps** before they can be created.

For example:

```text
Report
 ├── Title
 ├── Author
 ├── Header
 ├── Footer
 ├── Sections
 └── Format
```

Creating everything through one large constructor can become difficult to read:

```csharp
new Report(
    title,
    author,
    header,
    footer,
    sections,
    format
);
```

Builder allows us to construct the object **step-by-step**.

## Core Idea

```text
Builder
   │
   ├── SetTitle()
   ├── SetAuthor()
   ├── AddSection()
   ├── SetFormat()
   │
   ↓
 Final Object
```

### Method Chaining

```csharp
var report = new ReportBuilder()
    .SetTitle("Monthly Report")
    .SetAuthor("Arafath")
    .AddSection("Introduction")
    .SetFormat("PDF")
    .Build();
```

This makes object construction easier to understand.

## Example Use Cases

- Reports
- HTTP Requests
- Emails
- Database Queries
- Complex configuration objects

## When to Use

Builder is useful when:

- There are many optional properties
- Object construction has multiple steps
- You want readable object creation
- You want to avoid huge constructors

---

# 🧬 5. Prototype Pattern

## What problem does it solve?

Sometimes we already have an object that is **very similar** to the object we want.

Creating another object from scratch may be unnecessary, especially when the initial setup is expensive.

Instead:

```text
Existing Object
      │
      ↓
    Clone
      │
      ↓
Modify a few properties
      │
      ↓
 New Object
```

## Core Idea

```text
       Prototype
           │
      ┌────┴────┐
      ↓         ↓
   Clone A   Clone B
      │         │
   Modify    Modify
```

### Example

Suppose we have an order template:

```text
Order Template
 ├── Product
 ├── Shipping
 ├── Customer
 └── Configuration
```

Instead of rebuilding everything:

```text
Order Template
       │
   ┌───┴────┐
   ↓        ↓
Clone 1   Clone 2
   │        │
Modify    Modify
```

## Example Use Cases

- Order templates
- Configuration cloning
- Test data generation
- Document templates
- Game level templates

## When to Use

Prototype is useful when:

- Object creation is expensive
- New objects are mostly similar to an existing object
- Only a few properties need to change
- Objects need to be created dynamically at runtime

---

# 🔍 Comparing the Five Patterns

The easiest way to remember them is to focus on **what problem each pattern solves**.

### 🔒 Singleton

> **"I need only ONE instance."**

```text
Many requests → One shared instance
```

### 🏭 Factory Method

> **"I need ONE object, but I don't want to decide its concrete class here."**

```text
Request → Factory → Concrete Object
```

### 🏭 Abstract Factory

> **"I need a FAMILY of related objects."**

```text
Factory → Related Object A
        → Related Object B
        → Related Object C
```

### 🧱 Builder

> **"I need to construct ONE complex object step-by-step."**

```text
Step 1 → Step 2 → Step 3 → Final Object
```

### 🧬 Prototype

> **"I already have a similar object; clone it and modify it."**

```text
Existing Object → Clone → Modify
```

---

# 📊 Quick Decision Guide

```text
                    Need to create an object?
                              │
                              ↓
                    ┌─────────────────────┐
                    │ Need only ONE       │
                    │ instance?           │
                    └──────────┬──────────┘
                               │ Yes
                               ↓
                          Singleton


If not:

    Need a family of related objects?
              │
             Yes
              ↓
       Abstract Factory


If not:

    Need to build a complex object
    step-by-step?
              │
             Yes
              ↓
           Builder


If not:

    Already have a similar object
    that you can clone?
              │
             Yes
              ↓
          Prototype


If not:

    Need to decide the concrete
    object at runtime?
              │
             Yes
              ↓
        Factory Method
```

---

# 🗂️ Pattern Cheat Sheet

| Pattern | Remember It As | Main Question |
|---|---|---|
| 🔒 Singleton | **One** | Do I need exactly one instance? |
| 🏭 Factory Method | **Which one?** | Which concrete object should I create? |
| 🏭 Abstract Factory | **Which family?** | Which related family of objects should I create? |
| 🧱 Builder | **Step-by-step** | How should I construct this complex object? |
| 🧬 Prototype | **Clone** | Can I copy an existing object instead? |
