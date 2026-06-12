# 🌤 WeatherApp-DotNet

A full-stack weather dashboard built with ASP.NET MVC and C#, featuring secure user authentication and real-time weather data.

[GitHub](https://github.com/adahmahoney/WeatherApp-DotNet)

---

WeatherApp-DotNet is a secure web application where users must register and log in before accessing weather data. It pulls real-time conditions and forecasts from a free API with no API key required.

---

## Quick Start

```
git clone https://github.com/adahmahoney/WeatherApp-DotNet.git
cd WeatherApp-DotNet/WeatherApp
dotnet ef database update
dotnet run
```

Then open your browser and go to `http://localhost:5243`

---

## Core Features

### Authentication
| Feature | Description |
|---|---|
| Register | Create an account with a username, email, and password |
| Login | Secure login with hashed password verification |
| Protected Routes | Dashboard is inaccessible without logging in |
| Logout | Clears authentication cookie and returns to login |

### Weather Dashboard
| Feature | Description |
|---|---|
| Current Weather | Displays temperature, humidity, and wind speed |
| 5-Day Forecast | Shows high and low temps with weather descriptions |
| City Search | Search any city in the world by name |

---

## Technologies Used

| Technology | Purpose |
|---|---|
| ASP.NET MVC (.NET 10) | Web framework and routing |
| C# | Backend logic |
| Entity Framework Core 10 | Database management |
| SQL Server Express | User data storage |
| ASP.NET Identity | Authentication and password hashing |
| Open-Meteo API | Free weather and geocoding data |
| HTML/CSS | Frontend styling |

---

## Prerequisites
- .NET 10 SDK
- SQL Server Express
- Visual Studio Code

---

## Setup Instructions

1. Clone the repository
```
git clone https://github.com/adahmahoney/WeatherApp-DotNet.git
```

2. Navigate into the project
```
cd WeatherApp-DotNet/WeatherApp
```

3. Update `appsettings.json` with your SQL Server credentials
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=WeatherAppDB;User Id=sa;Password=YOURPASSWORD;TrustServerCertificate=True;"
```

4. Run database migrations
```
dotnet ef database update
```

5. Start the app
```
dotnet run
```

---

## Known Limitations
- City search works best with city name only — state filtering is limited
- For production a more robust geocoding API such as Google Maps is recommended

---

## Author

Adah Mahoney
- GitHub: [github.com/adahmahoney](https://github.com/adahmahoney)
- LinkedIn: [linkedin.com/in/adah-mahoney](https://linkedin.com/in/adah-mahoney)
- Email: adahmahoney@gmail.com
