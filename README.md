# CieBackendAssessment

To run the app in your local environment, please follow these steps:

1) On CieBackendAssessment\appsettings.json, change the ConnectionStrings.DefaultConnection value to match your local SQL Server.
2) Open the Package Manager Console and run Update-Database
3) On CieBackendAssessment\appsettings.json, change all the three fields under StripeConfig to match your Keys and set PriceId to match the priceId of a product you have on your personal stripe account.
4) On CieBackendAssessment\ClientApp\src\environments\environment.ts, change the value of the stripePublishableKey variable to match your own publishable key.
5) Run the application through Visual Studio's default run button or F5, as it will automatically run both front-end and back-end apps in a way that they succcesfully work together.
