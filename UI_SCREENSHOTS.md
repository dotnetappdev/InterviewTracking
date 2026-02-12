# UI Screenshots and Mockups

## Settings Page - Data Management Section

### Layout Overview

```
┌─────────────────────────────────────────────┐
│  Settings                                    │
├─────────────────────────────────────────────┤
│                                              │
│  [Previous sections above...]                │
│                                              │
│  ┌───────────────────────────────────────┐  │
│  │ 📊 Data Management                    │  │
│  │                                        │  │
│  │  ┌──────────────────────────────────┐ │  │
│  │  │     Export Data                   │ │  │
│  │  └──────────────────────────────────┘ │  │
│  │                                        │  │
│  │  ┌──────────────────────────────────┐ │  │
│  │  │     Import Data                   │ │  │
│  │  └──────────────────────────────────┘ │  │
│  │  ────────────────────────────────────  │  │
│  │                                        │  │
│  │  Test Data Management                  │  │
│  │                                        │  │
│  │  ┌──────────────────────────────────┐ │  │
│  │  │  🔄 Reset to Sample Data         │ │  │ ← Orange/Amber
│  │  └──────────────────────────────────┘ │  │   button
│  │                                        │  │
│  │  ┌──────────────────────────────────┐ │  │
│  │  │  🗑️ Clear All Data               │ │  │ ← Red button
│  │  └──────────────────────────────────┘ │  │
│  │                                        │  │
│  │  Export or import your interview data  │  │
│  │  as JSON. Reset to restore sample     │  │
│  │  data, or clear all to start fresh.   │  │
│  └───────────────────────────────────────┘  │
│                                              │
│  [Account section below...]                  │
│                                              │
└─────────────────────────────────────────────┘
```

## Sample Data Overview

### 13 Pre-loaded Interviews

The app comes with 13 diverse sample interviews covering various tech positions:

**Technical Roles:**
1. 💻 Senior Software Engineer (Acme Corporation)
2. 🔧 DevOps Engineer (Cloud Innovations Inc)
3. 🎨 Frontend Developer - React (TechStartup Inc)
4. ⚙️ Backend Engineer - .NET (Enterprise Solutions Corp)
5. 📱 Mobile Developer - iOS (Mobile Apps Studio)
6. 🔒 Security Engineer (CyberSec Solutions)

**Data & Analytics:**
7. 📊 Data Scientist (DataViz Analytics)
8. 💾 Database Administrator (Data Systems Corp)

**Quality & Testing:**
9. ✅ QA Engineer - Automation (Quality Systems Inc)

**Leadership & Management:**
10. 👔 Technical Lead - Java (Global Tech Solutions)
11. 📋 Product Manager (Beta Technologies)

**Design & UX:**
12. 🎨 UX Designer (Design Studios Ltd)

**Full Stack:**
13. 🔄 Full Stack Developer (FinTech Innovations)

### 8 Sample Interviewers/Contacts

Each with realistic details:
- 👤 John Smith - Engineering Manager @ Acme Corporation
- 👤 Sarah Johnson - Senior Software Engineer @ Acme Corporation
- 👤 Michael Chen - VP of Product @ Beta Technologies
- 👤 Emily Rodriguez - DevOps Lead @ Cloud Innovations
- 👤 David Park - Cloud Architect @ Cloud Innovations
- 👤 Lisa Anderson - Lead Frontend Developer @ TechStartup
- 👤 Robert Williams - CTO @ Global Tech Solutions
- 👤 Jennifer Lee - Senior Engineering Manager @ Global Tech Solutions

## Confirmation Dialogs

### Reset to Sample Data Dialog
```
┌────────────────────────────────────┐
│  Reset to Test Data                │
├────────────────────────────────────┤
│                                    │
│  This will delete all current      │
│  data and restore the original     │
│  sample interviews. Continue?      │
│                                    │
│  ┌──────────┐  ┌──────────┐       │
│  │  Cancel  │  │Yes, Reset│       │
│  └──────────┘  └──────────┘       │
└────────────────────────────────────┘
```

### Clear All Data Dialog
```
┌────────────────────────────────────┐
│  Clear All Data                    │
├────────────────────────────────────┤
│                                    │
│  Are you sure you want to delete   │
│  ALL interview data? This action   │
│  cannot be undone.                 │
│                                    │
│  ┌──────────┐  ┌───────────────┐  │
│  │  Cancel  │  │Yes, Delete All│  │
│  └──────────┘  └───────────────┘  │
└────────────────────────────────────┘
```

## Features Demonstrated

### Data Variety
- **13 Interviews** across different:
  - Job positions (Frontend, Backend, Full Stack, Mobile, QA, etc.)
  - Companies (startups to enterprises)
  - Interview stages (Scheduled, Stage1, Stage2, FinalRound)
  - Meeting platforms (Zoom, Google Meet, Microsoft Teams)
  - Job sources (LinkedIn, Indeed, Glassdoor, Referral, etc.)

### User/Contact Data
- **8 Interviewers** with:
  - Professional names
  - Email addresses
  - Job titles
  - Company affiliations
  - Linked to specific interviews

### Data Management Tools
1. **Export Data**: Save all interviews to JSON file for backup
2. **Import Data**: Restore interviews from JSON file
3. **Reset to Sample Data**: Restore 13 original samples (requires app restart)
4. **Clear All Data**: Remove everything for fresh start

### Safety Features
- ⚠️ Color-coded buttons (orange for reset, red for clear)
- ✅ Confirmation dialogs for destructive operations
- 💾 Import includes duplicate detection
- 📝 Clear help text explaining each operation

## Use Cases

### For Testing
1. Use sample data to test UI with realistic content
2. Test interview list, detail views, and filtering
3. Test sync functionality with pre-existing data
4. Verify calendar export with multiple interviews

### For Development
1. Quickly populate database with diverse data
2. Test edge cases with different interview types
3. Validate interviewer/contact relationships
4. Test data export/import functionality

### For Demos
1. Show app with professional-looking sample data
2. Demonstrate various features without manual setup
3. Reset to clean state after demo
4. Export sample data for sharing

## File Locations

### Source Code
- Seed data: `InterviewTracking.Maui/Data/LocalDbContext.cs` (lines 169-365)
- Clear/Reset methods: `InterviewTracking.Maui/Services/InterviewLocalService.cs`
- UI: `InterviewTracking.Maui/Views/SettingsPage.xaml`
- ViewModel: `InterviewTracking.Maui/ViewModels/SettingsViewModel.cs`

### Database
- Windows: `C:\ProgramData\InterviewTracking\interviews.db`
- iOS/Android: App data directory

## Technical Notes

### Seed Data Implementation
- Uses Entity Framework Core's `HasData()` method
- Sample GUIDs ensure consistent IDs across installations
- Dates relative to `DateTime.UtcNow` for current relevance
- Realistic company names and job descriptions

### Data Management
- `ClearAllDataAsync()`: Removes all records, preserves schema
- `ResetToSeedDataAsync()`: Drops and recreates database
- Both methods include error handling
- Operations logged for debugging

### User Experience
- Confirmation dialogs prevent accidental data loss
- Color coding indicates operation severity
- Help text explains each feature
- Async operations with loading indicators
