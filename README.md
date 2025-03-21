# PayPhoneWallet

## Installation Instructions

1. Clone the repository.
2. Open the solution in Visual Studio
3. Compile and run the application.  (.NET 8)



## Comments about test

1. Test are in project PayPhoneUnit
2. I use mocks for testing
3. Testing are for Wallet

## Important

1. Used dependecy injection
2. Solid principles 
3. Used the Repository pattern
4. I also added Authcontroller for login with JWT and random users
5. For this instance system only use a list in memory, not DB.
6. I added a ErrorHandling class with a Middleware for managing the errores
7. For feature flags are different forms to implement, for this instance i used a featureManager
8. In small projects also you can use Minimal API that is simply and easy, but if you want a project for scaling is better traditional api

## Feature flag 



## Solution

The solution have 5 projects: 

Entites: Project about the entites

Repository: Design pattern, acts as an intermediary layer between an application's business logic and controllers.

PayPhone: API REST with controllers

PayPhoneUnit: Project for TestUnit using XUnit framework

PayPhoneIntegration: Project for integration test 


   