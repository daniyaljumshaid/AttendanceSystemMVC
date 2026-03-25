# Attendance System MVC

A comprehensive ASP.NET Core MVC application for managing student attendance, courses, and teacher schedules. This system provides an intuitive interface for administrators, teachers, and students to track and manage attendance records.

## Features

- **Student Management**: Register and manage student profiles and course enrollments
- **Course Management**: Create and manage courses with sections and schedules
- **Attendance Tracking**: Record and track student attendance with status management
- **Teacher Scheduling**: Manage teacher assignments to courses and sections
- **Reports & Analytics**: Generate attendance reports with insights
- **User Authentication**: Role-based authentication for students, teachers, and administrators
- **Database Management**: Automated migrations and data validation

## Technology Stack

- **Framework**: ASP.NET Core MVC
- **Database**: SQL Server / SQLite
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Identity
- **Frontend**: Bootstrap, jQuery
- **Validation**: Fluent Validation

## Prerequisites

- .NET 6.0 or higher
- SQL Server or SQLite
- Visual Studio 2022 or Visual Studio Code
- Git

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/daniyaljumshaid/AttendanceSystemMVC.git
   cd AttendanceSystemMVC
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database connection**
   - Update the connection string in `appsettings.json` or `appsettings.Development.json`
   - Example for SQL Server:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=AttendanceSystem;Trusted_Connection=true;"
     }
     ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
AttendanceSystemMVC/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── HomeController.cs
│   ├── ReportsController.cs
│   ├── StudentController.cs
│   └── TeacherController.cs
├── Models/              # Data Models
│   ├── ApplicationUser.cs
│   ├── AttendanceRecord.cs
│   ├── Course.cs
│   ├── CourseSchedule.cs
│   └── ...
├── Views/               # Razor Views
├── Data/                # Database Context
│   └── ApplicationDbContext.cs
├── Migrations/          # Database Migrations
├── ReportService/       # Business Logic for Reports
├── wwwroot/             # Static Files (CSS, JS, Images)
└── appsettings.json    # Configuration
```

## Configuration

### Database
- **Development**: SQLite (default in appsettings.Development.json)
- **Production**: SQL Server (configure in appsettings.Production.json)

### Connection Strings
Update connection strings in `appsettings.json` for your environment:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=attendance.db"
}
```

## Usage

### For Administrators
- Manage users (students, teachers, admins)
- Assign courses to teachers
- View and manage all attendance records
- Generate system reports

### For Teachers
- View assigned courses and schedules
- Record student attendance
- View attendance reports for their classes

### For Students
- View enrolled courses
- Check attendance records
- View personal attendance statistics

## Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Deployment

### Azure Deployment
See [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) for detailed Azure deployment instructions.

### SmarterASP Hosting
See [SMARTERASP_DEPLOYMENT.md](SMARTERASP_DEPLOYMENT.md) for SmarterASP deployment instructions.

### Docker
A Dockerfile is included for containerized deployment:
```bash
docker build -t attendance-system .
docker run -p 5000:80 attendance-system
```

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify connection string in appsettings.json
   - Ensure SQL Server service is running
   - Check database exists and credentials are correct

2. **Migration Issues**
   - Run `dotnet ef database update` to apply pending migrations
   - Check for any conflicting migrations

3. **Port Already in Use**
   - Change the port in Properties/launchSettings.json

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

For questions or support, please contact:
- GitHub: [@daniyaljumshaid](https://github.com/daniyaljumshaid)

## Changelog

### Version 1.0.0 (Initial Release)
- Initial project setup
- Core attendance tracking functionality
- Database schema with migrations
- User authentication and authorization
- Basic reporting features
- Docker support

---

**Last Updated**: March 26, 2026
