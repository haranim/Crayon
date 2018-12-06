
This project is developed on Visual Studio 2017 version 15.9.3

**Dependencies:**
Dotnet Core 2.1.1


Requirements:
1. Provides a service that gives historical exchange rate Info for two different currencies during a date period.

It Calculates the maximum, minimum and the average rates during the date periods

Steps to run the application :

1. Clone the repository into a local folder
2. Build the application without errors and host the api

GET API has required parameters as Currency and Dates 

Curency format : 
valid symbols with '>' delimiter

eg: SEK>NOK

Date format : 
yyyy-mm-dd with ',' delimiter

eg: 2018-02-01,2018-02-15,2018-03-01


After the hosting the application the following url gives a result

Port number : 44315

https://localhost:44315/exchange?Currency=SEK>NOK&Dates=2018-02-01,2018-02-15,2018-03-01



Improvements :
1. use Moq Framework to test calculation logic in service layer
2. develop client application in Angular to display a dropwdown of currency values, date Inputs
