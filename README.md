# Interview Tracking System

A cross-platform interview scheduling and reminder application built with .NET MAUI and ASP.NET Web API.

## Features

### Core Functionality
- **Interview Management**: Create, view, edit, and delete interviews
- **Multi-platform Support**: iOS, Android, Windows
- **Offline-First Design**: Works without internet using local SQLite database
- **Configurable API**: Optional sync with ASP.NET Web API, API URL configurable in settings
- **Meeting Platform Integration**: Supports Zoom, Google Meet, Microsoft Teams
- **Reminders**: Multiple customizable reminders per interview
- **Authentication**: Secure JWT-based authentication (optional when API is enabled)

### Enhanced Features
- **Offline Mode**: Use app without API, all data stored locally
- **Calendar Export**: Export interviews to device calendar (ICS format)
- **Data Export/Import**: Export and import interview data as JSON
- **Interview Feedback**: Score and provide feedback on interviews (1-5 scale)
- **File Attachments**: Support for attaching resumes, job descriptions, and other files
- **Email Reminders**: Configure email notifications for upcoming interviews
- **Team Collaboration**: Track multiple interviewers per interview
- **Job Source Tracking**: Track where job opportunities came from (LinkedIn, Indeed, etc.)

### User Interface
- Clean, modern card-based design
- Light and Dark mode support
- Responsive layouts
- Real-time updates

## Architecture

### Projects
1. **InterviewTracking.Maui** - Cross-platform mobile/desktop client
2. **InterviewTracking.Api** - ASP.NET Web API backend
3. **InterviewTracking.Shared** - Shared models and DTOs

### Technologies
- .NET 10
- .NET MAUI (Multi-platform App UI)
- ASP.NET Core Web API
- Entity Framework Core
- SQLite (local storage)
- ASP.NET Identity
- JWT Authentication
- MVVM Pattern (CommunityToolkit.Mvvm)

## Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022 or JetBrains Rider
- Android SDK (for Android development)
- Xcode (for iOS/macOS development on Mac)

### Building the Solution

1. Clone the repository:
```bash
git clone https://github.com/dotnetappdev/InterviewTracking.git
cd InterviewTracking
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

### Running the API

```bash
cd InterviewTracking.Api
dotnet run
```

The API will start on `https://localhost:7000` (or the port specified in launchSettings.json)

### Running the MAUI App

For Android:
```bash
cd InterviewTracking.Maui
dotnet build -t:Run -f net10.0-android
```

For iOS (Mac only):
```bash
cd InterviewTracking.Maui
dotnet build -t:Run -f net10.0-ios
```

For Windows:
```bash
cd InterviewTracking.Maui
dotnet build -t:Run -f net10.0-windows
```

## Configuration

### App Settings

The app can be configured through the Settings page:

1. **API Configuration**
   - Enable/Disable API: Toggle to use the app in offline-only or online mode
   - API URL: Configure the base URL for your API server (default: `https://localhost:7000/api/`)
   
2. **Notifications**
   - Enable push notifications for upcoming interviews
   - Email reminders: Configure email address to receive reminders
   
3. **Synchronization**
   - Enable/Disable sync with API
   - Auto-sync: Automatically sync data when changes are made
   - Manual sync: Trigger sync on demand
   
4. **Data Management**
   - Export Data: Export all interviews to JSON file
   - Import Data: Import interviews from JSON file
   
5. **Appearance**
   - Dark Mode: Toggle between light and dark themes

### Database Storage

- **Windows**: SQLite database stored in `C:\ProgramData\InterviewTracking\interviews.db`
- **iOS/Android**: SQLite database stored in app's data directory

### API Configuration

Update `appsettings.json` in the API project:

```json
{
  "JwtSettings": {
    "SecretKey": "YourVerySecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "InterviewTrackingApi",
    "Audience": "InterviewTrackingApp"
  }
}
```

### MAUI App Configuration

The API URL can be configured directly in the app's Settings page. No code changes required!

## Project Structure

```
InterviewTracking/
├── InterviewTracking.Api/
│   ├── Controllers/          # API controllers
│   ├── Data/                 # Database context
│   ├── Services/             # Business logic services
│   └── Program.cs            # API startup configuration
├── InterviewTracking.Maui/
│   ├── Views/                # XAML pages
│   ├── ViewModels/           # View models (MVVM pattern)
│   ├── Services/             # App services
│   ├── Data/                 # Local database context
│   ├── Converters/           # XAML value converters
│   └── MauiProgram.cs        # App startup configuration
└── InterviewTracking.Shared/
    ├── Models/               # Domain models
    └── DTOs/                 # Data transfer objects
```

## Key Features Implementation

### Authentication
- JWT token-based authentication
- Secure password hashing with ASP.NET Identity
- Token storage in device secure storage

### Offline-First
- Local SQLite database for offline access
- Background sync when online (optional)
- Conflict resolution (last-write-wins)
- Works completely offline when API is disabled

### Notifications
- Platform-specific notification handlers
- Multiple reminders per interview
- Customizable reminder times
- Email reminders (when API is enabled)

### Data Management
- Export all data to JSON format
- Import data from JSON files
- Calendar export (ICS format)
- Share exported data via device sharing

### Interview Feedback
- 1-5 scoring system for multiple criteria
- Track strengths and weaknesses
- Recommendation tracking (Strong Yes to Strong No)
- Multiple feedback entries per interview

### File Attachments
- Support for resumes, cover letters, job descriptions
- File metadata tracking
- Attachment types categorization

### Meeting Integration
- Deep linking to native meeting apps
- Fallback to web browser if app not installed
- Support for Zoom, Google Meet, Microsoft Teams

## Future Enhancements

- [ ] Calendar integration (Google Calendar, Outlook) - Native API integration
- [ ] Team collaboration features - Real-time collaboration
- [ ] Recurring interviews with advanced patterns - More complex recurrence rules
- [ ] Analytics dashboard - Interview statistics and insights
- [ ] Advanced search and filtering
- [ ] Interview preparation notes and checklists
- [ ] Video interview recording integration

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or contributions, please open an issue on GitHub.
