# 🔒 Singleton Design Pattern

> **Singleton Pattern** is a creational design pattern that ensures a class has only one instance and provides a global access point to that instance.

সহজ ভাষায়, পুরো application-এ একটি class-এর **একটিমাত্র shared instance** থাকবে, এবং application-এর বিভিন্ন অংশ সেই instance-টি ব্যবহার করবে।

---

## 📚 Core Concepts

| Concept | কী জিনিস |
|---|---|
| 🔒 **Singleton Pattern** | পুরো application-এ একটি class-এর একটিমাত্র instance নিশ্চিত করা |
| 🔐 **Private Constructor** | বাইরে থেকে সরাসরি object তৈরি করা বন্ধ করা |
| 📦 **Static Instance** | Class নিজেই নিজের instance ধরে রাখে |
| 🚪 **GetInstance / Instance** | Singleton instance পাওয়ার public access point |
| 🧵 **Thread Safety** | Multiple thread যেন একই সময়ে নিরাপদে instance access বা initialization করতে পারে |
| ⚠️ **Singleton-এর সমস্যা** | Hidden dependency, testing কঠিন, এবং tight coupling |
| ✅ **কখন Use করবে** | Logger, Configuration, Cache, Connection Pool |
| ❌ **কখন Use করবে না** | শুধু global access-এর সুবিধার জন্য, বিশেষ করে Dependency Injection (DI) available থাকলে |

---

## 🎯 The Problem

ধরো, একটি application-এর বিভিন্ন অংশে Logger ব্যবহার করতে হবে।

```text
Application
    |
    ├── UserService
    |      └── Logger Instance A
    |
    ├── PaymentService
    |      └── Logger Instance B
    |
    └── OrderService
           └── Logger Instance C
```

এখানে প্রতিটি service আলাদা Logger instance তৈরি করলে unnecessary resource consumption এবং inconsistent state-এর সমস্যা হতে পারে।

আমরা চাই, সব service একই shared Logger instance ব্যবহার করুক।

### ✅ Solution: Singleton Pattern

```text
                Application
                     |
          ┌──────────┼──────────┐
          |          |          |
          ▼          ▼          ▼
     UserService PaymentService OrderService
          |          |          |
          └──────────┼──────────┘
                     ▼
              ┌─────────────┐
              │  Singleton  │
              │   Logger    │
              │  Instance   │
              └─────────────┘
```

সব service একই Singleton instance ব্যবহার করে।

---

## 🧱 Structure of Singleton

```text
┌─────────────────────────────┐
│          Singleton          │
├─────────────────────────────┤
│ - static instance           │
│ - private constructor       │
├─────────────────────────────┤
│ + getInstance()             │
└──────────────┬──────────────┘
               |
               ▼
       Single Object Instance
```

### প্রধান বৈশিষ্ট্য

1. **Private Constructor:** বাইরে থেকে সরাসরি instance তৈরি করা যায় না।
2. **Static Instance:** Class নিজের instance ধরে রাখে।
3. **Public Access Method:** Instance পাওয়ার জন্য নির্দিষ্ট method ব্যবহার করা হয়।
4. **Single Instance:** Instance তৈরি হয়ে গেলে পরবর্তী calls-এ একই object ফেরত দেওয়া হয়।

> Implementation language অনুযায়ী Singleton-এর structure আলাদা হতে পারে। যেমন Go-তে constructor private করার পরিবর্তে unexported fields এবং package-level functions ব্যবহার করা যায়।

---

## ⚙️ How It Works

```text
Client requests instance
          |
          ▼
   Does instance exist?
          |
      ┌───┴───┐
      |       |
     NO      YES
      |       |
      ▼       ▼
 Create     Return
 instance   existing instance
      |       |
      └───┬───┘
          ▼
     Same Instance
```

- প্রথমবার instance request করলে object তৈরি হয়।
- পরবর্তী request-গুলোতে existing instance ফেরত দেওয়া হয়।
- Thread-safe implementation-এ concurrent calls থেকেও initialization নিরাপদ রাখতে হয়।
