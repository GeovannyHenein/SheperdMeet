# ShepherdMeet

**ShepherdMeet** is a scheduling application built with Django, designed to help members of a church schedule meetings with their Abouna (priest). It facilitates the process of organizing pastoral appointments efficiently.

## Features
- Book one-on-one meetings with Abouna.
- View available time slots.
- Easy rescheduling and cancellation.
- Notifications and reminders for upcoming meetings.

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed on your system:

- [Python 3.x](https://www.python.org/)
- [Django](https://www.djangoproject.com/)
- [Git](https://git-scm.com/)
- [Node.js](https://nodejs.org/) (optional if using frontend tools like npm)

### Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/YourUsername/ShepherdMeet.git
    cd ShepherdMeet
    ```

2. Set up a virtual environment (optional but recommended):

    ```bash
    python -m venv venv
    source venv/bin/activate  # On Windows use `venv\Scripts\activate`
    ```

3. Install the required Python packages:

    ```bash
    pip install -r requirements.txt
    ```

4. Apply migrations and start the Django development server:

    ```bash
    python manage.py migrate
    python manage.py runserver
    ```

5. Access the app by going to [http://127.0.0.1:8000](http://127.0.0.1:8000).

### Contributing

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/my-feature`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature/my-feature`).
5. Create a new Pull Request.

All changes must be reviewed and approved before being merged into the main branch.

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
