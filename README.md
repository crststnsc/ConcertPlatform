# Concert Ticket Booking System

This is a concert ticket booking system application developed using WPF (Windows Presentation Foundation) and PostgreSQL as the database.

## Features

- View available concerts
- Purchase tickets for concerts
- Manage user basket
- View purchased tickets

## Technologies Used

- C# programming language
- WPF (Windows Presentation Foundation)
- Entity Framework Core
- PostgreSQL database

## Prerequisites

Before running the application, make sure you have the following prerequisites installed:

- Visual Studio (or any compatible C# IDE)
- .NET Framework
- PostgreSQL database

## Setup

1. Clone the repository:

```shell
git clone https://github.com/crststnsc/ConcertPlatform.git
```

2. Open the project in Visual Studio.

3. Configure the database connection in the `DALHelper` file. Update the connection string with your PostgreSQL database details.

4. Run the database migrations to create the required tables in the database:

```shell
dotnet ef database update
```

5. Build and run the application.

## Usage

1. Upon launching the application, you will be presented with a list of available concerts.

2. Select a concert and click the "Buy Tickets" button to purchase tickets for that concert.

3. Click the "Add to Basket" button to add the tickets to your basket.

4. To view and manage your basket, click the "Basket" button in the main menu.

5. In the basket view, you can see the tickets you have added, remove tickets, or proceed to checkout.

6. At the checkout, review your ticket purchase and click the "Confirm" button to finalize the purchase.

7. You can also view your purchased tickets in the "Purchased Tickets" section.
