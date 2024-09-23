# ShepherdMeet

**ShepherdMeet** is a scheduling application built with ASP.NET Core, designed to help members of a church schedule meetings with their Abouna (priest). It simplifies the organization of pastoral appointments.

## Features
- Book one-on-one meetings with Abouna.
- View available time slots.
- Easy rescheduling and cancellation.
- Notifications and reminders for upcoming meetings.

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [Visual Studio](https://visualstudio.microsoft.com/)
- [.NET 6.0 SDK](https://dotnet.microsoft.com/)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/YourUsername/ShepherdMeet.git
    cd ShepherdMeet
    ```

2. Open the project in Visual Studio.

3. Restore dependencies:

    ```bash
    dotnet restore
    ```

4. Apply database migrations and start the application:

    ```bash
    dotnet ef database update
    dotnet run
    ```

5. Navigate to `https://localhost:5001` in your browser.

### Contributing

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/my-feature`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/my-feature`).
5. Create a new Pull Request.
