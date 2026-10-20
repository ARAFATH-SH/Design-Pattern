# 🏭 Factory Method Pattern

> **Factory Method Pattern** is a creational design pattern that separates **object creation logic** from the code that uses those objects.

Instead of creating concrete objects directly with `new`, the client depends on an abstraction and lets a **Factory/Creator** decide which concrete object should be created.

---

## 📌 Core Concepts

| Concept | কী জিনিস |
|---|---|
| 🏭 **Factory Method Pattern** | Object creation-এর responsibility আলাদা class/method-এ রাখা |
| 🔌 **Product Interface** | সব concrete products-এর common interface, যেমন `IPayment` |
| 📦 **Concrete Product** | Actual object যেটা তৈরি হয়, যেমন `BkashPayment`, `NagadPayment` |
| 🏗️ **Factory / Creator** | যে class বা method object তৈরি করে, যেমন `PaymentFactory` |
| ⚡ **Simple Factory** | একটি class-এর মধ্যে `switch`/condition ব্যবহার করে object তৈরি করা |
| 🧩 **Factory Method (GoF)** | Abstract Creator + Concrete Creator ব্যবহার করে object creation — inheritance-based |
| 🟢 **OCP** | নতুন type যোগ হলে existing code পরিবর্তন না করেই extend করা যায় |
| 🔄 **DIP** | Client concrete class-এর উপর নয়, abstraction/interface-এর উপর depend করে |

---

## 🎯 The Problem

ধরো আমাদের একটি payment system আছে:

```go
if paymentType == "bkash" {
    payment := BkashPayment{}
} else if paymentType == "nagad" {
    payment := NagadPayment{}
} else if paymentType == "rocket" {
    payment := RocketPayment{}
}
```

শুরুতে এটি সহজ মনে হলেও application বড় হওয়ার সাথে সাথে সমস্যা তৈরি হয়:

- ❌ Creation logic বিভিন্ন জায়গায় duplicate হয়
- ❌ Client code concrete classes সম্পর্কে জানতে বাধ্য হয়
- ❌ নতুন payment method যোগ করলে পুরোনো code পরিবর্তন করতে হয়
- ❌ Testing এবং maintenance কঠিন হয়ে যায়

Factory Pattern এই object creation responsibility-কে আলাদা করে।

---

## 🧱 Basic Structure

```text
                  ┌──────────────────┐
                  │     Client       │
                  └────────┬─────────┘
                           │
                           ▼
                  ┌──────────────────┐
                  │     Creator      │
                  │  / Factory       │
                  └────────┬─────────┘
                           │
                    creates Product
                           │
              ┌────────────┴────────────┐
              ▼                         ▼
     ┌─────────────────┐       ┌─────────────────┐
     │ BkashPayment    │       │ NagadPayment    │
     │ Concrete Product│       │ Concrete Product│
     └─────────────────┘       └─────────────────┘
              │                         │
              └──────────┬──────────────┘
                         ▼
                  ┌──────────────┐
                  │  IPayment    │
                  │   Interface  │
                  └──────────────┘
```

---

# 💳 Example: Payment System

ধরো আমাদের application-এ একাধিক payment method আছে:

```text
IPayment
   │
   ├── BkashPayment
   ├── NagadPayment
   └── RocketPayment
```

Client সরাসরি `BkashPayment` বা `NagadPayment` তৈরি করবে না।

তার পরিবর্তে:

```text
Client
   │
   ▼
PaymentFactory
   │
   ├── "bkash"  → BkashPayment
   ├── "nagad"  → NagadPayment
   └── "rocket" → RocketPayment
```

এতে client শুধু `IPayment` জানবে।

---

# 🏗️ Simple Factory vs Factory Method

দুটোকে এক মনে করা উচিত নয়।

## ⚡ Simple Factory

একটি factory class-এর মধ্যে condition বা `switch` ব্যবহার করা হয়।

```go
func CreatePayment(paymentType string) IPayment {
    switch paymentType {
    case "bkash":
        return BkashPayment{}
    case "nagad":
        return NagadPayment{}
    case "rocket":
        return RocketPayment{}
    default:
        return nil
    }
}
```

### Structure

```text
Client
  │
  ▼
PaymentFactory
  │
  ├── switch "bkash"  → BkashPayment
  ├── switch "nagad"  → NagadPayment
  └── switch "rocket" → RocketPayment
```

এটি practical এবং ছোট project-এর জন্য অনেক সময় যথেষ্ট।

---

# 🧩 Factory Method — GoF

**Factory Method** pattern-এ object creation-এর responsibility একটি **creator hierarchy**-এর মধ্যে থাকে।

```text
              Creator
                 │
       ┌─────────┴─────────┐
       ▼                   ▼
BkashCreator         NagadCreator
       │                   │
       ▼                   ▼
BkashPayment         NagadPayment
```

এখানে নতুন product type যোগ করার সময় নতুন creator যোগ করা যায়।

এটি **inheritance-based** approach।

---

# 🟢 Open/Closed Principle (OCP)

Factory Pattern-এর একটি গুরুত্বপূর্ণ benefit হলো **Open/Closed Principle**:

> **Software entities should be open for extension but closed for modification.**

অর্থাৎ নতুন payment method এলে existing client/business logic বারবার পরিবর্তন না করে নতুন implementation যোগ করা যায়।

```text
Existing:

IPayment
 ├── BkashPayment
 └── NagadPayment


New:

IPayment
 ├── BkashPayment
 ├── NagadPayment
 └── RocketPayment  ← নতুন implementation
```

---

# 🔄 Dependency Inversion Principle (DIP)

Client যেন concrete implementation-এর উপর depend না করে।

### ❌ Without Factory

```text
Client
 ├── BkashPayment
 ├── NagadPayment
 └── RocketPayment
```

Client সব concrete class সম্পর্কে জানে।

### ✅ With Factory

```text
Client
   │
   ▼
IPayment
   ▲
   │
Factory
   │
   ├── BkashPayment
   ├── NagadPayment
   └── RocketPayment
```

Client শুধু `IPayment` abstraction-এর সাথে কাজ করে।

---

# ✅ কখন Factory Pattern ব্যবহার করবো?

### 1. Runtime-এ Object Type Decide হয়

যখন user input, configuration অথবা condition অনুযায়ী different object তৈরি করতে হয়।

```text
User Input
    │
    ▼
Payment Type
    │
 ┌──┼──────┐
 ▼  ▼      ▼
bKash Nagad Rocket
```

---

### 2. Frequently নতুন Type আসে

যেমন:

- 💳 Payment Methods
- 📧 Notification Channels
- 📄 Export Formats
- 🗄️ Database Drivers
- 🔐 Authentication Providers

নতুন type বারবার যোগ হলে Factory useful হয়।

---

### 3. একই Creation Logic অনেক জায়গায় আছে

ধরো ৫টা service-এ একই ধরনের logic আছে:

```go
if type == "email" {
    ...
} else if type == "sms" {
    ...
}
```

এগুলো একটি Factory-তে centralize করা যায়।

```text
Service A ─┐
Service B ─┤
Service C ─┼──→ NotificationFactory
Service D ─┤
Service E ─┘
```

---

### 4. Client Code কে Concrete Class থেকে Decouple করতে চাই

Testing এবং maintainability-এর জন্য client যেন concrete implementation-এর উপর tightly coupled না থাকে।

```text
Client → Interface
          ↑
       Factory
          │
    Concrete Products
```

---

# ❌ কখন Factory Pattern ব্যবহার করবো না?

Factory সব জায়গায় ব্যবহার করা উচিত নয়।

### 1. শুধু ২–৩টা Type এবং Change হওয়ার Chance নেই

যেমন:

```go
if isAdmin {
    dashboard := NewAdminDashboard()
} else {
    dashboard := NewUserDashboard()
}
```

এখানে Factory যোগ করলে unnecessary abstraction তৈরি হতে পারে।

> **Simple problem → Simple solution.**

---

### 2. Object Creation খুব Simple

যদি constructor-এ কোনো logic না থাকে:

```go
user := NewUser()
```

তাহলে শুধু Factory বানানোর জন্য Factory বানানোর দরকার নেই।

---

### 3. Single Use

যদি কোনো object creation logic শুধু একটি জায়গায় একবার ব্যবহার হয়, তাহলে আলাদা Factory class তৈরি করার effort অনেক সময় worth it নয়।

---

# 🧠 Quick Decision Guide

Ask yourself:

```text
              Do I need different
              object types?
                    │
                   YES
                    │
                    ▼
          Is the type decided
             dynamically?
                    │
             ┌──────┴──────┐
            YES            NO
             │              │
             ▼              ▼
          Factory       Direct creation
             │
             ▼
     Is creation logic
     complex/repeated?
             │
        ┌────┴────┐
       YES        NO
        │          │
        ▼          ▼
     Factory    Keep it simple
```

---

# ⭐ Benefits

- ✅ Separates object creation from business logic
- ✅ Reduces coupling
- ✅ Improves maintainability
- ✅ Makes testing easier
- ✅ Supports extension
- ✅ Centralizes creation logic
- ✅ Works well with **OCP** and **DIP**
- ✅ Useful when object types grow over time

---

# ⚠️ Trade-offs

Factory Pattern-এর কিছু cost-ও আছে:

- ❌ Extra classes/files তৈরি হতে পারে
- ❌ Simple application-এ unnecessary abstraction হতে পারে
- ❌ Codebase নতুনদের কাছে initially বেশি complex মনে হতে পারে
- ❌ শুধু pattern ব্যবহার করার জন্য pattern ব্যবহার করা উচিত নয়

> **Use Factory when it solves a real creation problem — not just because it's a design pattern.**

---
### The main idea:

> **"Don't make the client responsible for deciding which concrete object to create."**

Instead:

```text
Client
   │
   ▼
Factory
   │
   ▼
Concrete Product
   │
   ▼
Product Interface
```

---

## 🚀 Remember

**Factory Pattern-এর মূল উদ্দেশ্য শুধু `new` লুকানো নয়।**

এর আসল উদ্দেশ্য হলো:

> **Object creation-এর responsibility আলাদা করা এবং client code-কে concrete implementations থেকে decouple করা।**

**Simple problem → direct construction.**  
**Growing/complex creation logic → consider Factory.**