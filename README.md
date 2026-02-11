# Interview Tracking System

A cross-platform interview scheduling and reminder application built with .NET MAUI and ASP.NET Web API.

## Features

### Core Functionality
- **Interview Management**: Create, view, edit, and delete interviews
- **Multi-platform Support**: iOS, Android, Windows
- **Offline-First Design**: Works without internet using local SQLite database
- **Cloud Synchronization**: Optional sync with ASP.NET Web API
- **Meeting Platform Integration**: Supports Zoom, Google Meet, Microsoft Teams
- **Reminders**: Multiple customizable reminders per interview
- **Authentication**: Secure JWT-based authentication

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

Update the API base URL in `MauiProgram.cs`:

```csharp
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri("https://your-api-url.com/api/");
});
```

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
- Background sync when online
- Conflict resolution (last-write-wins)

### Notifications
- Platform-specific notification handlers
- Multiple reminders per interview
- Customizable reminder times

### Meeting Integration
- Deep linking to native meeting apps
- Fallback to web browser if app not installed
- Support for Zoom, Google Meet, Microsoft Teams

## Future Enhancements

- [ ] Email reminders
- [ ] Calendar integration (Google Calendar, Outlook)
- [ ] Interview feedback and scoring
- [ ] File attachments (resume, job description)
- [ ] Team collaboration features
- [ ] Recurring interviews with advanced patterns
- [ ] Export/import functionality
- [ ] Analytics dashboard

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or contributions, please open an issue on GitHub.
