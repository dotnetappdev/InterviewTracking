# Interview Tracking - React Native

A cross-platform interview scheduling and reminder application built with React Native and Expo. This is a clone of the .NET MAUI Interview Tracking application.

## Features

### Core Functionality
- **Interview Management**: Create, view, edit, and delete interviews
- **Multi-platform Support**: iOS, Android, and Web
- **Offline-First Design**: Works without internet using local SQLite database
- **Configurable API**: Optional sync with ASP.NET Web API
- **Meeting Platform Integration**: Supports Zoom, Google Meet, Microsoft Teams
- **Calendar View**: Visual calendar with interview dates marked
- **Search Functionality**: Search interviews by company name
- **Authentication**: Secure JWT-based authentication (optional when API is enabled)

### Enhanced Features
- **Offline Mode**: Use app without API, all data stored locally
- **Interview Feedback**: Score and provide feedback on interviews (1-5 scale)
- **File Attachments**: Support for attaching resumes, job descriptions, and other files
- **Email Reminders**: Configure email notifications for upcoming interviews
- **Team Collaboration**: Track multiple interviewers per interview
- **Job Portal Credentials**: Securely store job portal login information

### User Interface
- Clean, modern card-based design
- iOS-style design language
- Calendar integration with date selection
- Real-time updates
- Responsive layouts

## Technologies

- React Native 0.81.5
- Expo SDK ~54
- TypeScript
- React Navigation
- SQLite (expo-sqlite)
- AsyncStorage
- Axios for API calls
- React Native Calendars
- React Native Paper (UI components)

## Getting Started

### Prerequisites
- Node.js 14+ and npm
- Expo CLI
- iOS Simulator (for macOS) or Android Studio (for Android development)
- Xcode (for iOS development on Mac)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/dotnetappdev/InterviewTracking.git
cd InterviewTracking/InterviewTrackingRN
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm start
```

### Running the App

For Android:
```bash
npm run android
```

For iOS (Mac only):
```bash
npm run ios
```

For Web:
```bash
npm run web
```

Or scan the QR code with the Expo Go app on your mobile device.

## Project Structure

```
InterviewTrackingRN/
├── src/
│   ├── models/              # Data models
│   ├── screens/             # Screen components
│   │   ├── InterviewListScreen.tsx
│   │   ├── InterviewDetailScreen.tsx
│   │   ├── AddEditInterviewScreen.tsx
│   │   ├── SettingsScreen.tsx
│   │   ├── LoginScreen.tsx
│   │   └── RegisterScreen.tsx
│   ├── services/            # Business logic services
│   │   ├── DatabaseService.ts    # SQLite database operations
│   │   ├── ApiService.ts         # API integration
│   │   └── StorageService.ts     # Settings storage
│   ├── navigation/          # Navigation configuration
│   │   └── AppNavigator.tsx
│   ├── types/               # TypeScript type definitions
│   │   └── index.ts
│   ├── components/          # Reusable components
│   └── utils/               # Utility functions
├── App.tsx                  # Main app component
├── package.json
└── tsconfig.json
```

## Key Features Implementation

### Interview List Screen
- Calendar view with marked interview dates
- Swipe to refresh
- Search by company name
- Filter by selected date
- Quick join meeting button
- Add new interview with FAB button

### Interview Detail Screen
- Complete interview information
- Interviewer details
- Reminder list
- Job portal credentials (with password masking)
- Edit and delete actions

### Add/Edit Interview Screen
- Form for creating/updating interviews
- Meeting platform selection
- Status tracking
- Job portal credentials input
- Notes support

### Settings Screen
- API configuration (enable/disable, URL)
- Notification settings
- Email reminders
- Auto-sync toggle
- Dark mode toggle (coming soon)
- Clear all data option

### Authentication
- Login screen
- Registration screen
- Skip option for offline-only mode
- JWT token storage

## Database Schema

The app uses SQLite with the following tables:
- **interviews**: Main interview data
- **interviewers**: Interviewer contacts
- **reminders**: Interview reminders
- **feedback**: Interview feedback
- **attachments**: File attachments

## API Integration

The app can optionally connect to the ASP.NET Web API backend:
- Base URL configurable in settings
- JWT authentication
- Sync local data with server
- Offline-first with conflict resolution

### API Endpoints Used
- `POST /auth/login` - User login
- `POST /auth/register` - User registration
- `GET /interviews` - Get all interviews
- `GET /interviews/:id` - Get interview by ID
- `POST /interviews` - Create interview
- `PUT /interviews/:id` - Update interview
- `DELETE /interviews/:id` - Delete interview
- `POST /interviews/sync` - Sync local data

## Configuration

### App Settings
Settings are stored locally and can be configured through the Settings screen:

1. **API Configuration**
   - Enable/Disable API
   - API URL (default: `https://localhost:7000/api/`)

2. **Notifications**
   - Enable push notifications
   - Email reminders
   - Email address for reminders

3. **Synchronization**
   - Auto-sync with API
   - Manual sync on demand

4. **Appearance**
   - Dark Mode (coming soon)

### Storage Locations
- **Settings**: AsyncStorage (`app_settings`)
- **Auth Token**: AsyncStorage (`auth_token`)
- **Interview Data**: SQLite (`interviewTracking.db`)

## Development

### Adding New Features

1. Create necessary TypeScript types in `src/types/`
2. Add database schema changes in `DatabaseService.ts`
3. Create screens in `src/screens/`
4. Update navigation in `AppNavigator.tsx`
5. Add API endpoints in `ApiService.ts` if needed

### Testing

The app can be tested using:
- Expo Go app on physical devices
- iOS Simulator
- Android Emulator
- Web browser

### Building for Production

For Android:
```bash
expo build:android
```

For iOS:
```bash
expo build:ios
```

For Web:
```bash
expo build:web
```

## Comparison with MAUI App

This React Native version maintains feature parity with the .NET MAUI version:

| Feature | MAUI | React Native |
|---------|------|--------------|
| Interview CRUD | ✅ | ✅ |
| Offline Storage | ✅ | ✅ |
| API Sync | ✅ | ✅ |
| Calendar View | ✅ | ✅ |
| Search | ✅ | ✅ |
| Authentication | ✅ | ✅ |
| Meeting Integration | ✅ | ✅ |
| Job Portal Creds | ✅ | ✅ |
| Notifications | ✅ | 🚧 In Progress |
| File Attachments | ✅ | 🚧 In Progress |
| Data Export/Import | ✅ | 📅 Planned |

## Future Enhancements

- [ ] Complete notification system implementation
- [ ] File attachment support
- [ ] Data export/import (JSON)
- [ ] Calendar export (ICS format)
- [ ] Dark mode implementation
- [ ] Date/time picker for interview scheduling
- [ ] Interview feedback UI
- [ ] Analytics dashboard
- [ ] Push notifications
- [ ] Biometric authentication

## Known Limitations

1. Date/time picker not yet implemented (uses current time)
2. File attachments UI pending
3. Notification scheduling pending
4. Data export/import pending

## Troubleshooting

### Common Issues

**SQLite not working:**
- Ensure expo-sqlite is installed: `npm install expo-sqlite`
- Clear app data and restart

**Navigation errors:**
- Check all screen imports in AppNavigator.tsx
- Ensure react-navigation packages are installed

**API connection issues:**
- Verify API URL in settings
- Check network permissions
- For localhost on Android, use 10.0.2.2 instead of localhost

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on GitHub.

## Credits

This is a React Native clone of the Interview Tracking MAUI application. Original MAUI app features have been adapted to work with React Native and Expo.
