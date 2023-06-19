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

4. Install the required NuGet packages:
   - Npgsql.EntityFrameworkCore.PostgreSQL
   - Npgsql

   You can install these packages using the NuGet Package Manager in Visual Studio.

5. Run the database migrations to create the required tables in the database:

```shell
dotnet ef database update
```

6. Build and run the application.

## Database Creation

To create the necessary database and tables for the application, follow these steps:

1. Open your PostgreSQL database management tool (e.g., pgAdmin).

2. Create a new database with a suitable name for your application.

3. Open the `ConcertPlatform.sql` file located in the project's root directory.

4. Copy the contents of the `ConcertPlatform.sql` file.

5. Execute the SQL script in your PostgreSQL database to create the tables and necessary data.

   > Note: Make sure you are connected to the correct database before executing the script.

6. After executing the script, your database should be ready for use with the Concert Ticket Booking System application.

## Usage

1. Upon launching the application, you will be presented with a list of available concerts.

2. Select a concert and click the "Buy Tickets" button to purchase tickets for that concert.

3. Click the "Add to Basket" button to add the tickets to your basket.

4. To view and manage your basket, click the "Basket" button in the main menu.

5. In the basket view, you can see the tickets you have added, remove tickets, or proceed to checkout.

6. At the checkout, review your ticket purchase and click the "Confirm" button to finalize the purchase.

7. You can also view your purchased tickets in the "Purchased Tickets" section.

## Troubleshooting

- If you encounter any issues related to database connectivity, ensure that you have provided the correct database connection details in the `DALHelper` file.

- Make sure that you have the necessary permissions to create and modify the database tables.

- If you are experiencing any errors or unexpected behavior, try cleaning and rebuilding the solution in Visual Studio.
