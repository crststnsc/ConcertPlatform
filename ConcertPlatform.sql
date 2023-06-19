-- Create Artists table
CREATE TABLE Artists (
    artist_id SERIAL PRIMARY KEY,
    artist_name VARCHAR(100) NOT NULL,
    genre VARCHAR(50),
    description TEXT
);

-- Create Venues table
CREATE TABLE Venues (
    venue_id SERIAL PRIMARY KEY,
    venue_name VARCHAR(100) NOT NULL,
    location VARCHAR(100),
    capacity INTEGER
);

-- Create Concerts table
CREATE TABLE Concerts (
    concert_id SERIAL PRIMARY KEY,
    artist_id INTEGER REFERENCES Artists (artist_id),
    venue_id INTEGER REFERENCES Venues (venue_id),
    date DATE,
    time TIME,
    ticket_price NUMERIC(8,2),
    description TEXT
);

-- Create Tickets table
CREATE TABLE Tickets (
    ticket_id SERIAL PRIMARY KEY,
    concert_id INTEGER REFERENCES Concerts (concert_id),
    seat_number INTEGER,
    ticket_holder_name VARCHAR(100)
);

-- Create Basket table
CREATE TABLE Basket (
    user_id INTEGER,
    ticket_id INTEGER REFERENCES Tickets (ticket_id),
    PRIMARY KEY (user_id, ticket_id)
);

-- Create Users table (optional, if needed for authentication)
CREATE TABLE Users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    full_name VARCHAR(100)
);

-- Create Purchases table
CREATE TABLE Purchases (
    user_id INTEGER REFERENCES Users (user_id),
    ticket_id INTEGER REFERENCES Tickets (ticket_id),
);
