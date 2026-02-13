# Interview Tracking System

A cross-platform interview scheduling and reminder application available in two versions:
- **.NET MAUI** - Native C# application for iOS, Android, and Windows
- **React Native** - JavaScript/TypeScript application for iOS, Android, and Web

Both versions share the same ASP.NET Web API backend and feature set.

## 📱 Available Versions

### .NET MAUI Version
Located in `/InterviewTracking.Maui/`
- Native performance
- Windows desktop support
- Full .NET ecosystem integration
- See MAUI-specific documentation below

### React Native Version  
Located in `/InterviewTrackingRN/`
- Cross-platform with Expo
- Web browser support
- Hot reloading for rapid development
- **[📖 React Native Quick Start](./InterviewTrackingRN/QUICKSTART.md)**
- **[📚 React Native Documentation](./InterviewTrackingRN/README.md)**

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
1. **InterviewTracking.Maui** - .NET MAUI cross-platform mobile/desktop client
2. **InterviewTrackingRN** - React Native cross-platform mobile/web client (NEW!)
3. **InterviewTracking.Api** - ASP.NET Web API backend (shared by both clients)
4. **InterviewTracking.Shared** - Shared models and DTOs

### Technologies

#### MAUI Version
- .NET 10
- .NET MAUI (Multi-platform App UI)
- Entity Framework Core
- SQLite (local storage)
- MVVM Pattern (CommunityToolkit.Mvvm)

#### React Native Version
- React Native 0.81.5
- Expo SDK ~54
- TypeScript
- SQLite (expo-sqlite)
- React Navigation

#### Shared Backend
- ASP.NET Core Web API
- ASP.NET Identity
- JWT Authentication

## Getting Started

### Choose Your Platform

#### React Native (Recommended for Quick Start)
```bash
cd InterviewTrackingRN
npm install
npm start
# Scan QR code with Expo Go app or press 'w' for web
```

See [React Native Quick Start Guide](./InterviewTrackingRN/QUICKSTART.md) for detailed instructions.

#### .NET MAUI

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
   - Reset to Sample Data: Restore 13 sample interviews with diverse positions
   - Clear All Data: Remove all interviews from database
   
5. **Appearance**
   - Dark Mode: Toggle between light and dark themes

### Sample Data

The app includes 13 pre-loaded sample interviews for testing:

| Position | Company | Platform | Status |
|----------|---------|----------|--------|
| Senior Software Engineer | Acme Corporation | Zoom | Scheduled |
| Product Manager | Beta Technologies | Google Meet | Stage1 |
| DevOps Engineer | Cloud Innovations Inc | Microsoft Teams | Stage2 |
| Data Scientist | DataViz Analytics | Zoom | FinalRound |
| UX Designer | Design Studios Ltd | Google Meet | Scheduled |
| Frontend Developer - React | TechStartup Inc | Zoom | Scheduled |
| Backend Engineer - .NET | Enterprise Solutions Corp | Microsoft Teams | Stage1 |
| Full Stack Developer | FinTech Innovations | Google Meet | Stage2 |
| Mobile Developer - iOS | Mobile Apps Studio | Zoom | Scheduled |
| QA Engineer - Automation | Quality Systems Inc | Microsoft Teams | Stage1 |
| Technical Lead - Java | Global Tech Solutions | Google Meet | FinalRound |
| Database Administrator | Data Systems Corp | Zoom | Scheduled |
| Security Engineer | CyberSec Solutions | Microsoft Teams | Stage2 |

**Sample Interviewers**: The app also includes 8 sample interviewer contacts (e.g., John Smith - Engineering Manager, Sarah Johnson - Senior Software Engineer) linked to specific interviews for testing collaboration features.

**Managing Test Data**:
- Use "Reset to Sample Data" in Settings to restore these samples
- Use "Clear All Data" to start with a clean database
- Both operations require confirmation to prevent accidental data loss

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
