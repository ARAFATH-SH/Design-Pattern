# 🏭 Abstract Factory Design Pattern

> **Abstract Factory Pattern** is a creational design pattern that provides an interface for creating families of related objects without specifying their concrete classes.

সহজ ভাষায়, যখন একটি application-এ একাধিক related object একসঙ্গে তৈরি করতে হয় এবং বিভিন্ন variant অনুযায়ী তাদের combination পরিবর্তন করতে হয়, তখন Abstract Factory Pattern ব্যবহার করা যায়।

ধরো, একটি e-commerce application-এ প্রতিটি region-এর জন্য আলাদা payment, shipping এবং invoice system আছে। Abstract Factory নিশ্চিত করতে পারে যে প্রতিটি region-এর জন্য সঠিক family-র objects তৈরি হচ্ছে।

---

## 📚 Core Concepts

| Concept | কী জিনিস |
|---|---|
| 🏭 **Abstract Factory Pattern** | Related objects-এর একটি family তৈরি করার interface |
| 🔌 **Product Interface** | প্রতিটি product type-এর common interface, যেমন `IPayment`, `IShipping`, `IInvoice` |
| 🧩 **Abstract Factory Interface** | কোন কোন product তৈরি করা যাবে, তা declare করে |
| 🏗️ **Concrete Factory** | নির্দিষ্ট family-র concrete products তৈরি করে, যেমন `DhakaFactory`, `ChittagongFactory` |
| 🔗 **Product Family** | পরস্পর সম্পর্কিত products-এর একটি group |
| 🛡️ **Mismatch Prevention** | একই family-র compatible products একসঙ্গে ব্যবহার করতে সাহায্য করে |
| ⚖️ **Factory Method vs Abstract Factory** | Factory Method সাধারণত একটি product তৈরির extension point; Abstract Factory একটি product family তৈরির interface দেয় |
| ✅ **কখন Use করবে** | Related objects, multiple variants, regional configurations, UI themes, database families |

---

## 🎯 The Problem

ধরো, একটি e-commerce application বাংলাদেশে একাধিক region-এ operate করে।

প্রতিটি region-এর জন্য আলাদা payment, shipping এবং invoice implementation দরকার।

```text
Dhaka Region
├── DhakaPayment
├── DhakaShipping
└── DhakaInvoice

Chittagong Region
├── ChittagongPayment
├── ChittagongShipping
└── ChittagongInvoice
```

Factory ছাড়া client code-এ বারবার condition লিখতে হতে পারে:

```go
if region == "dhaka" {
    payment = DhakaPayment{}
    shipping = DhakaShipping{}
    invoice = DhakaInvoice{}
} else if region == "chittagong" {
    payment = ChittagongPayment{}
    shipping = ChittagongShipping{}
    invoice = ChittagongInvoice{}
}
```

এতে কয়েকটি সমস্যা তৈরি হয়:

- ❌ Object creation logic client code-এ ছড়িয়ে পড়ে।
- ❌ ভুল region-এর products একসঙ্গে ব্যবহার হয়ে যেতে পারে।
- ❌ নতুন region যোগ করলে একাধিক জায়গায় পরিবর্তন লাগতে পারে।
- ❌ Testing এবং maintenance কঠিন হতে পারে।

### ✅ Solution: Abstract Factory

প্রতিটি region-এর জন্য একটি Concrete Factory থাকবে, যা সংশ্লিষ্ট family-র সব products তৈরি করবে।

```text
                   Client
                      |
                      ▼
              Abstract Factory
                      |
             ┌────────┴────────┐
             ▼                 ▼
       DhakaFactory      ChittagongFactory
             |                 |
       ┌─────┼─────┐     ┌─────┼─────┐
       ▼     ▼     ▼     ▼     ▼     ▼
    Payment Ship  Invoice Payment Ship Invoice
```

Client শুধু factory interface ব্যবহার করবে। কোন concrete products তৈরি হবে, তা নির্বাচিত factory নির্ধারণ করবে।

---

## 🧱 Structure of Abstract Factory

```text
┌────────────────────────────┐
│      AbstractFactory       │
├────────────────────────────┤
│ + CreatePayment()          │
│ + CreateShipping()         │
│ + CreateInvoice()          │
└──────────────┬─────────────┘
               |
       ┌───────┴────────┐
       ▼                ▼
┌──────────────┐  ┌──────────────────┐
│ DhakaFactory │  │ ChittagongFactory│
└──────┬───────┘  └────────┬─────────┘
       |                   |
       ▼                   ▼
 Dhaka Products       Chittagong Products
       |                   |
       ▼                   ▼
 Payment, Shipping, Invoice
       |                   |
       └─────────┬─────────┘
                 ▼
           Product Interfaces
```

### প্রধান Components

1. **Abstract Factory:** সব factory-র common contract নির্ধারণ করে।
2. **Concrete Factory:** একটি নির্দিষ্ট product family তৈরি করে।
3. **Abstract Products:** প্রতিটি product category-র interface নির্ধারণ করে।
4. **Concrete Products:** Interface implement করে বাস্তব functionality দেয়।
5. **Client:** Factory ও product interfaces ব্যবহার করে business logic চালায়।

---

## 🛡️ Mismatch Prevention

Abstract Factory-র একটি গুরুত্বপূর্ণ benefit হলো compatible products-এর family তৈরি করা সহজ হয়।

### ❌ Without Abstract Factory

```text
Client
  ├── DhakaPayment
  ├── ChittagongShipping
  └── DhakaInvoice
```

এখানে ভুল combination তৈরি হওয়ার সম্ভাবনা থাকে।

### ✅ With Abstract Factory

```text
DhakaFactory
  ├── DhakaPayment
  ├── DhakaShipping
  └── DhakaInvoice
```

অথবা:

```text
ChittagongFactory
  ├── ChittagongPayment
  ├── ChittagongShipping
  └── ChittagongInvoice
```

Client নির্বাচিত factory থেকে products নিলে consistent family পাওয়া যায়।

> **Important:** Abstract Factory compatible objects তৈরি করার structure দেয়; compatibility পুরোপুরি enforce হবে কি না, তা implementation-এর উপর নির্ভর করে। Go-এর মতো language-এ product interfaces একই হওয়ায় compiler একা সব regional mismatch আটকাতে পারে না।

---

## ⚖️ Factory Method vs Abstract Factory

| Feature | Factory Method | Abstract Factory |
|---|---|---|
| কী তৈরি করে | একটি product type-এর object | Related products-এর একটি family |
| মূল উদ্দেশ্য | কোন concrete product তৈরি হবে তা নির্ধারণের extension point | একাধিক related product তৈরি করার interface |
| Creation methods | সাধারণত একটি factory method | সাধারণত একাধিক creation methods |
| Structure | প্রায়ই Creator ও Concrete Creator hierarchy | Abstract Factory ও Concrete Factory implementations |
| Mismatch prevention | একাধিক product family coordinate করা এর মূল উদ্দেশ্য নয় | Compatible products একসঙ্গে তৈরি করতে সাহায্য করে |
| Example | `PaymentCreator → IPayment` | `DhakaFactory → IPayment + IShipping + IInvoice` |
| কখন Use করবে | একটি product-এর implementation vary করে | একসঙ্গে একাধিক related products-এর family vary করে |

### সহজভাবে মনে রাখো

```text
Factory Method
      |
      ▼
  One Product
      |
      ▼
   IPayment


Abstract Factory
      |
      ▼
  Product Family
      |
      ├── IPayment
      ├── IShipping
      └── IInvoice
```

**Note:** Factory Method-এর GoF definition-এ subclassing গুরুত্বপূর্ণ। অন্যদিকে Simple Factory হলো একটি প্রচলিত implementation technique, কিন্তু এটি নিজে GoF-এর Factory Method pattern-এর সমতুল্য নয়।

---

## ✅ কখন Abstract Factory ব্যবহার করবো?

### 1. Related Objects-এর Family তৈরি করতে হয়

যখন একাধিক object একই feature বা workflow-এর অংশ।

Examples:
- Payment + Shipping + Invoice
- Database Connection + Query Builder + Transaction Manager
- UI Button + Checkbox + Dialog

### 2. Multiple Variants বা Configurations আছে

যখন application-এর configuration অনুযায়ী পুরো product family বদলে যায়।

Examples:
- 🌍 Multi-region applications
- 🎨 Light theme / Dark theme
- 🗄️ Different database providers
- 💻 Cross-platform applications

### 3. Mismatch Prevention গুরুত্বপূর্ণ

যখন incompatible product combination ব্যবহার করলে business logic বা system behavior ভুল হতে পারে।

যেমন, নির্দিষ্ট provider-এর payment এবং invoice integration একসঙ্গে ব্যবহার করা দরকার।

### 4. Product Families সময়ের সঙ্গে বাড়বে

নতুন region, platform বা provider যোগ হলে Concrete Factory দিয়ে নতুন family যোগ করা যেতে পারে।

---

## ❌ কখন Abstract Factory ব্যবহার করবো না?

### 1. শুধু একটি Product Type আছে

যদি শুধু payment implementation নির্বাচন করতে হয়, তাহলে Factory Method বা Simple Factory যথেষ্ট হতে পারে।

### 2. Family ছোট এবং স্থায়ী

মাত্র দুটি variant আছে এবং ভবিষ্যতে বাড়ার সম্ভাবনা নেই—এমন ক্ষেত্রে সাধারণ conditional logic যথেষ্ট হতে পারে।

### 3. Products Independent

Payment এবং logging-এর মধ্যে যদি কোনো family relationship না থাকে, তাহলে তাদের একটি Abstract Factory-তে জোর করে রাখা উচিত নয়।

### 4. Over-engineering হচ্ছে

যদি একটি সাধারণ কাজের জন্য অনেক interface, factory এবং concrete implementation তৈরি করতে হয়, তাহলে pattern-এর complexity বাস্তব সমস্যার চেয়ে বেশি হয়ে যেতে পারে।

> **Design for actual variation, not imaginary future requirements.**

---

## ⭐ Benefits

- ✅ Creates families of related objects
- ✅ Reduces coupling between clients and concrete classes
- ✅ Centralizes product-family creation
- ✅ Makes switching between variants easier
- ✅ Helps prevent incompatible combinations
- ✅ Supports extension with new concrete factories
- ✅ Works well with Dependency Inversion Principle (DIP)

## ⚠️ Trade-offs

- ❌ More interfaces and concrete classes
- ❌ Adding a new product category may require changing every factory
- ❌ Can increase complexity for small applications
- ❌ Compatibility is not automatically guaranteed by every language's type system
- ❌ Requires careful design of product families

---

## 🧠 Quick Decision Guide

```text
Do you need multiple related products?
                 |
             ┌───┴───┐
            NO      YES
             |        |
             ▼        ▼
      Simple Factory  Do products vary
      or direct       as a family?
      creation             |
                       ┌───┴───┐
                      NO      YES
                       |        |
                       ▼        ▼
                 Separate    Consider
                 factories   Abstract Factory
                                |
                                ▼
                     Are families likely to
                     grow or switch at runtime?
                                |
                           ┌────┴────┐
                          YES       NO
                           |         |
                           ▼         ▼
                      Good fit    Keep it simple
```

---

## 📌 Summary

```text
Abstract Factory Pattern
          |
          ├── Creates product families
          |
          ├── Uses product interfaces
          |
          ├── Uses an abstract factory contract
          |
          ├── Provides concrete family factories
          |
          ├── Reduces concrete-class coupling
          |
          └── Helps maintain product compatibility
```

### The Main Idea

> **"Create families of related objects without coupling the client to their concrete classes."**

মনে রাখবে:

- **One product varies →** Consider Factory Method.
- **A family of related products varies →** Consider Abstract Factory.
- **Products are independent →** Separate their creation responsibilities.
- **The problem is simple →** Prefer a simpler solution.

**Use Abstract Factory when coordinating a product family solves a real design problem—not just because a design pattern exists.**
