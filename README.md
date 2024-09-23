ShepherdMeet
ShepherdMeet is a scheduling application built with ASP.NET Core, designed to help members of a church schedule meetings with their Abouna (priest). It simplifies the organization of pastoral appointments.

Features
Book one-on-one meetings with Abouna.
View available time slots.
Easy rescheduling and cancellation.
Notifications and reminders for upcoming meetings.
Getting Started
Prerequisites
Before you begin, ensure you have the following installed:

Visual Studio
.NET 6.0 SDK
SQL Server Express
Installation
Clone the repository:

bash
Copy code
git clone https://github.com/YourUsername/ShepherdMeet.git
cd ShepherdMeet
Open the project in Visual Studio.

Restore dependencies:

bash
Copy code
dotnet restore
Apply database migrations and start the application:

bash
Copy code
dotnet ef database update
dotnet run
Navigate to https://localhost:5001 in your browser.

Contributing
Fork the repository.
Create a feature branch (git checkout -b feature/my-feature).
Commit your changes (git commit -am 'Add new feature').
Push to the branch (git push origin feature/my-feature).
Create a new Pull Request.


All changes must be reviewed and approved before being merged into the main branch.

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
