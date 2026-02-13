# Quick Start Guide

## Installation

1. **Install dependencies:**
   ```bash
   cd InterviewTrackingRN
   npm install
   ```

2. **For web support (optional):**
   ```bash
   npx expo install react-dom react-native-web
   ```

## Running the App

### Mobile Development (Recommended)

1. **Install Expo Go app** on your iOS or Android device:
   - iOS: https://apps.apple.com/app/expo-go/id982107779
   - Android: https://play.google.com/store/apps/details?id=host.exp.exponent

2. **Start the development server:**
   ```bash
   npm start
   ```

3. **Scan the QR code** with:
   - iOS: Camera app
   - Android: Expo Go app

### iOS Simulator (Mac only)

```bash
npm run ios
```

### Android Emulator

```bash
npm run android
```

### Web Browser

```bash
# Install web dependencies first
npx expo install react-dom react-native-web

# Then run
npm run web
```

## First Time Setup

1. Launch the app
2. Skip login to use offline mode
3. Go to Settings tab
4. Tap "Load Sample Data" to populate with 5 sample interviews
5. Go back to Interviews tab to see the sample data

## Features to Try

### Basic Features
- ✅ View interviews on calendar
- ✅ Tap a date to filter interviews
- ✅ Search for interviews by company name
- ✅ Tap an interview to see details
- ✅ Tap the + button to add a new interview
- ✅ Edit or delete interviews from detail screen
- ✅ Join meeting links directly from interview cards

### Settings
- ✅ Enable/disable API integration
- ✅ Configure API URL for sync
- ✅ Manage notification preferences
- ✅ Load sample data
- ✅ Clear all data

### Data Management
- ✅ All data stored locally in SQLite
- ✅ Works completely offline
- ✅ Optional API sync (when enabled)

## Troubleshooting

### "Unable to resolve module" errors
```bash
npm install
npm start -- --clear
```

### Database errors
- Clear app data and restart
- Or reinstall the app

### Expo Go not connecting
- Ensure your phone and computer are on the same WiFi network
- Try scanning the QR code again

## What's Next?

Check out:
- [README.md](./README.md) - Feature overview and comparison with MAUI app
- [IMPLEMENTATION.md](./IMPLEMENTATION.md) - Technical deep dive (600+ lines)

## Quick Tips

1. **Sample Data**: Use "Load Sample Data" in Settings to quickly populate the app
2. **Offline First**: The app works perfectly without an API - enable it only if you have the backend running
3. **Meeting Links**: Add meeting links to interviews to enable the "Join" button
4. **Job Portal**: Store job portal credentials securely for easy reference during interviews
5. **Search**: Use the search bar to quickly find interviews by company name
6. **Calendar**: Dates with interviews are marked with blue dots

## Development

To make changes:

1. Edit files in `src/` directory
2. Changes will hot-reload automatically
3. Check console for any errors

## Building for Production

### Android APK
```bash
eas build --platform android
```

### iOS App
```bash
eas build --platform ios
```

Note: You'll need an Expo account and proper app signing setup.
