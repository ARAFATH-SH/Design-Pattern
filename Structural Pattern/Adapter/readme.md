# 🔌 Adapter Design Pattern in C#

The **Adapter Design Pattern** is a structural design pattern that allows two **incompatible interfaces** to work together.

In simple words:

> **Adapter = মাঝখানে বসে দুইটা incompatible interface-এর মধ্যে translation করে।**

It is especially useful when integrating **third-party libraries, legacy systems, or external APIs** whose interfaces don't match the interface expected by your application.

---

# 🔌 What is Adapter Pattern?

Imagine your application expects this interface:

```text
IPayment
   │
   ├── Pay()
   └── Refund()
```

But a third-party payment library provides:

```text
StripeGateway
   │
   ├── MakeCharge()
   └── ReverseTransaction()
```

The two interfaces don't match.

You cannot directly use `StripeGateway` where your application expects `IPayment`.

Instead, we introduce an **Adapter**:

```text
                    Your Application
                           │
                           ↓
                       IPayment
                           │
                           ↓
                 StripePaymentAdapter
                           │
                           ↓
                    StripeGateway
```

The Adapter translates your application's expected calls into calls understood by the third-party library.

---

# ❌ The Problem

Suppose our application is designed around:

```csharp
public interface IPayment
{
    void Pay(decimal amount);
}
```

Our application expects every payment provider to implement `IPayment`.

But a third-party library provides:

```csharp
public class StripeGateway
{
    public void MakeCharge(decimal amount)
    {
        Console.WriteLine($"Stripe charged {amount}");
    }
}
```

The problem is:

```text
Application expects:

IPayment.Pay()


Stripe provides:

StripeGateway.MakeCharge()
```

The names and interfaces are different.

We cannot simply do:

```csharp
IPayment payment = new StripeGateway();
```

because `StripeGateway` does not implement `IPayment`.

---

# ✅ The Solution

Create an Adapter between them.

```text
┌─────────────────┐
│   Application   │
└────────┬────────┘
         │
         ↓
    ┌──────────┐
    │ IPayment │
    └────┬─────┘
         │
         ↓
┌────────────────────────┐
│ StripePaymentAdapter   │
└───────────┬────────────┘
            │
            ↓
┌────────────────────────┐
│    StripeGateway       │
│    MakeCharge()        │
└────────────────────────┘
```

Now the application doesn't need to know how Stripe's API works.

It only knows about:

```csharp
IPayment
```

The Adapter handles the translation.

---

# 🧩 Core Components

The Adapter Pattern has four important components.

| Component | Responsibility | Example |
|---|---|---|
| 🎯 **Target** | Interface expected by the client | `IPayment` |
| 🔧 **Adaptee** | Existing incompatible class | `StripeGateway` |
| 🔌 **Adapter** | Translates Target calls into Adaptee calls | `StripePaymentAdapter` |
| 👤 **Client** | Uses the Target interface | `PaymentService` |

---

## 🎯 1. Target

The **Target** is the interface that your application expects.

```csharp
public interface IPayment
{
    void Pay(decimal amount);
}
```

The client is designed around this interface.

---

## 🔧 2. Adaptee

The **Adaptee** is the existing class whose interface doesn't match what your application expects.

```csharp
public class StripeGateway
{
    public void MakeCharge(decimal amount)
    {
        Console.WriteLine($"Stripe charged {amount}");
    }
}
```

This class could come from:

- A third-party library
- Legacy code
- External API SDK
- Existing internal system

---

## 🔌 3. Adapter

The **Adapter** implements the Target interface and internally uses the Adaptee.

```csharp
public class StripePaymentAdapter : IPayment
{
    private readonly StripeGateway _stripeGateway;

    public StripePaymentAdapter(StripeGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public void Pay(decimal amount)
    {
        _stripeGateway.MakeCharge(amount);
    }
}
```

Here:

```text
IPayment.Pay()
      ↓
StripePaymentAdapter
      ↓
StripeGateway.MakeCharge()
```

The Adapter performs the translation.

---

## 👤 4. Client

The Client only knows about the Target interface.

```csharp
public class PaymentService
{
    private readonly IPayment _payment;

    public PaymentService(IPayment payment)
    {
        _payment = payment;
    }

    public void ProcessPayment(decimal amount)
    {
        _payment.Pay(amount);
    }
}
```

The client doesn't need to know:

- Stripe's API
- `MakeCharge()`
- Stripe-specific implementation details

It only knows:

```csharp
IPayment
```

---

# 💻 Complete C# Example

## Target

```csharp
public interface IPayment
{
    void Pay(decimal amount);
}
```

## Adaptee

```csharp
public class StripeGateway
{
    public void MakeCharge(decimal amount)
    {
        Console.WriteLine($"Stripe charged: ${amount}");
    }
}
```

## Adapter

```csharp
public class StripePaymentAdapter : IPayment
{
    private readonly StripeGateway _stripeGateway;

    public StripePaymentAdapter(StripeGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public void Pay(decimal amount)
    {
        _stripeGateway.MakeCharge(amount);
    }
}
```

## Client

```csharp
public class PaymentService
{
    private readonly IPayment _payment;

    public PaymentService(IPayment payment)
    {
        _payment = payment;
    }

    public void ProcessPayment(decimal amount)
    {
        _payment.Pay(amount);
    }
}
```

## Program

```csharp
var stripeGateway = new StripeGateway();

IPayment payment = new StripePaymentAdapter(stripeGateway);

var paymentService = new PaymentService(payment);

paymentService.ProcessPayment(100);
```

### Output

```text
Stripe charged: $100
```

---

# 🔄 How the Adapter Works

The complete flow is:

```text
Client
   │
   │ Pay(100)
   ↓
IPayment
   │
   ↓
StripePaymentAdapter
   │
   │ MakeCharge(100)
   ↓
StripeGateway
   │
   ↓
Stripe Payment System
```

The important point is:

> The Client talks to the **Target**, not directly to the **Adaptee**.

---

# 🧱 Object Adapter

The **Object Adapter** uses **composition**.

The Adapter contains an instance of the Adaptee.

```csharp
public class StripePaymentAdapter : IPayment
{
    private readonly StripeGateway _stripeGateway;

    public StripePaymentAdapter(StripeGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public void Pay(decimal amount)
    {
        _stripeGateway.MakeCharge(amount);
    }
}
```

Relationship:

```text
StripePaymentAdapter
        │
        │ has-a
        ↓
StripeGateway
```

### In C#

> **Object Adapter is generally the recommended approach.**

It provides loose coupling and works well with dependency injection.

---

# 🧬 Class Adapter

A **Class Adapter** uses inheritance.

Conceptually:

```text
Adapter
   │
   ├── implements Target
   │
   └── inherits Adaptee
```

For example:

```csharp
public class StripePaymentAdapter 
    : StripeGateway, IPayment
{
    public void Pay(decimal amount)
    {
        MakeCharge(amount);
    }
}
```

Now the Adapter both:

- Inherits from `StripeGateway`
- Implements `IPayment`

Relationship:

```text
StripePaymentAdapter
       │
       ├── is-a → StripeGateway
       │
       └── is-a → IPayment
```

However, this approach has limitations in C# because C# supports **single class inheritance**.

---

# ⚖️ Object Adapter vs Class Adapter

| Feature | Object Adapter | Class Adapter |
|---|---|---|
| Technique | Composition | Inheritance |
| Relationship | Has-a | Is-a |
| Flexibility | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| Multiple adaptees | Easier | Limited |
| Dependency Injection | Excellent | Less flexible |
| C# Recommendation | ✅ Recommended | ⚠️ Limited use |
| Coupling | Lower | Higher |

### Recommended in C#

```text
Object Adapter
      ↓
Composition
      ↓
Recommended
```

Instead of:

```text
Class Adapter
      ↓
Inheritance
      ↓
Limited in C#
```

---

# 🎯 When to Use

## 1. Third-Party Library Interface Doesn't Match

This is one of the most common use cases.

Your application expects:

```csharp
IPayment.Pay()
```

But the library provides:

```csharp
StripeGateway.MakeCharge()
```

Use an Adapter to connect them.

---

## 2. Legacy System Integration

Suppose an old system has:

```csharp
LegacyPayment.ProcessTransaction();
```

But your new system expects:

```csharp
IPayment.Pay();
```

Instead of changing the legacy system—which may be risky—put an Adapter in between.

```text
New System
    │
    ↓
IPayment
    │
    ↓
Adapter
    │
    ↓
Legacy System
```

---

## 3. You Cannot or Don't Want to Change Either Side

Sometimes:

- Third-party code cannot be changed
- Legacy code shouldn't be changed
- Existing application code shouldn't be redesigned

The Adapter becomes the middle layer.

```text
Existing System
      ↕
   Adapter
      ↕
External System
```

---

## 4. Multiple Similar Libraries Need One Uniform Interface

Suppose your application supports:

```text
Stripe
PayPal
SSLCommerz
```

Each provider has a different API.

Instead of spreading provider-specific code throughout your application:

```text
Stripe API
PayPal API
SSLCommerz API
```

adapt them all to:

```csharp
IPayment
```

Architecture:

```text
                    IPayment
                       │
          ┌────────────┼────────────┐
          ↓            ↓            ↓
      Stripe        PayPal      SSLCommerz
      Adapter       Adapter       Adapter
          │            │            │
          ↓            ↓            ↓
       Stripe        PayPal      SSLCommerz
       Gateway        API           API
```

Now the rest of the application works with one interface.

---

# ❌ When Not to Use

Adapter is useful, but it shouldn't become the solution for every mismatch.

---

## 1. Interface Design Itself Is Wrong

If the interface is fundamentally poorly designed, don't automatically add an Adapter.

Ask first:

> **Should I redesign the interface instead?**

An Adapter can hide a bad abstraction rather than actually solving the design problem.

---

## 2. Adaptee Frequently Changes

If the third-party API changes constantly:

```text
API v1
 ↓
Adapter update

API v2
 ↓
Adapter update

API v3
 ↓
Adapter update
```

The Adapter can become a maintenance headache.

---

## 3. Simple Type Conversion

Don't create an Adapter just to perform a trivial conversion.

For example:

```text
int → string
```

doesn't require a design pattern.

A simple utility operation is enough:

```csharp
int number = 100;

string text = number.ToString();
```

---

## 4. Adapter Everywhere

If your architecture looks like:

```text
Class A → Adapter
Class B → Adapter
Class C → Adapter
Class D → Adapter
Class E → Adapter
```

and almost everything requires an Adapter, stop and reconsider your interface design.

It may indicate that the abstraction itself is wrong.

---

# 🔀 Adapter ≠ Facade

Adapter and Facade are sometimes confused because both introduce another class between the client and existing code.

But they solve **different problems**.

| Pattern | Main Purpose |
|---|---|
| 🔌 **Adapter** | Translate an incompatible interface |
| 🏛️ **Facade** | Simplify a complicated interface |

### Adapter

```text
Client
  │
  ↓
Expected Interface
  │
  ↓
Adapter
  │
  ↓
Incompatible Interface
```

**Think:**

> "Make these two interfaces compatible."

---

### Facade

```text
             Facade
                │
       ┌────────┼────────┐
       ↓        ↓        ↓
    System A  System B  System C
```

**Think:**

> "Hide this complicated system behind a simple interface."

---

## 🧠 Easy Way to Remember

```text
Adapter → Translate
Facade  → Simplify
```

Or:

> **Adapter changes the interface.**  
> **Facade simplifies the interface.**

---

# 🌍 Real-World Examples

### 💳 Payment Integration

```text
IPayment
   │
   ├── Stripe Adapter
   ├── PayPal Adapter
   └── SSLCommerz Adapter
```

### 📦 Legacy System

```text
New Application
       │
       ↓
    Adapter
       │
       ↓
Legacy Application
```

### ☁️ Cloud Provider Integration

Different cloud SDKs may expose different APIs:

```text
Application
     │
     ↓
IStorage
     │
 ┌───┼────┐
 ↓   ↓    ↓
AWS Azure GCP
```

Adapters can provide one consistent application-level interface.

### 📧 Notification Providers

```text
INotification
      │
 ┌────┼─────┐
 ↓    ↓     ↓
Email SMS  Push
Adapter Adapter Adapter
```

---

# ✅ Advantages

### 1. ♻️ Reuse Existing Code

You can use existing classes without modifying them.

### 2. 🔓 Loose Coupling

The client depends on the Target interface instead of the concrete Adaptee.

### 3. 🔌 Easy Third-Party Integration

External libraries can be integrated without spreading their APIs throughout the application.

### 4. 🧹 Cleaner Application Code

Provider-specific logic stays inside the Adapter.

### 5. 🔄 Easier Provider Replacement

If your application depends on:

```csharp
IPayment
```

you can replace:

```text
Stripe
   ↓
PayPal
```

without changing the rest of the application significantly.

---

# ⚠️ Disadvantages

### 1. More Classes

Adapter introduces another abstraction layer.

```text
Client
 ↓
Adapter
 ↓
Adaptee
```

### 2. Additional Complexity

For a very simple conversion, an Adapter may be unnecessary.

### 3. Maintenance

If the Adaptee's API changes frequently, the Adapter must also be updated.

### 4. Too Many Adapters Can Indicate Bad Design

If almost every class requires an Adapter, reconsider your architecture.

---


# 📊 Adapter Pattern Cheat Sheet

| Concept | Meaning |
|---|---|
| 🔌 **Adapter Pattern** | Incompatible interface → compatible interface |
| 🎯 **Target** | Interface the Client expects |
| 🔧 **Adaptee** | Existing incompatible class |
| 🔌 **Adapter** | Translates Target calls to Adaptee calls |
| 🧱 **Object Adapter** | Composition-based |
| 🧬 **Class Adapter** | Inheritance-based |
| ⭐ **Recommended in C#** | Object Adapter |
| 🎯 **Main Use Case** | Third-party / legacy integration |
| ⚠️ **Avoid When** | Interface should simply be redesigned |
| 🔌 **Adapter ≠ Facade** | Adapter translates; Facade simplifies |

---

# 🧠 The One-Sentence Definition

> **The Adapter Pattern allows incompatible interfaces to work together by converting the interface of an existing class into the interface expected by the client.**

