// Enums
export enum MeetingPlatform {
  Zoom = 'Zoom',
  GoogleMeet = 'GoogleMeet',
  MicrosoftTeams = 'MicrosoftTeams',
  Other = 'Other',
}

export enum InterviewStatus {
  Scheduled = 'Scheduled',
  Stage1 = 'Stage1',
  Stage2 = 'Stage2',
  Stage3 = 'Stage3',
  Stage4 = 'Stage4',
  Stage5 = 'Stage5',
  FinalRound = 'FinalRound',
  Completed = 'Completed',
  NotInterviewed = 'NotInterviewed',
  DidNotShowUp = 'DidNotShowUp',
  Cancelled = 'Cancelled',
  Rejected = 'Rejected',
  OfferReceived = 'OfferReceived',
}

export enum RecurrencePattern {
  Daily = 'Daily',
  Weekly = 'Weekly',
  BiWeekly = 'BiWeekly',
  Monthly = 'Monthly',
  Custom = 'Custom',
}

// Interfaces
export interface Interview {
  id: string;
  title: string;
  dateTime: string;
  notes: string;
  meetingPlatformTypeId: number;
  platform: MeetingPlatform;
  meetingLink: string;
  userId: string;
  status: InterviewStatus;
  jobSourceId?: number;
  jobTitle: string;
  companyName: string;
  jobPortalUrl: string;
  jobPortalUsername: string;
  jobPortalPassword: string;
  isRecurring: boolean;
  recurrencePattern?: RecurrencePattern;
  recurrenceEndDate?: string;
  interviewers: Interviewer[];
  reminders: Reminder[];
  feedback: InterviewFeedback[];
  attachments: InterviewAttachment[];
  createdAt: string;
  updatedAt: string;
  isSynced: boolean;
}

export interface Interviewer {
  id: string;
  name: string;
  email: string;
  role: string;
  interviewId: string;
}

export interface Reminder {
  id: string;
  interviewId: string;
  reminderTime: string;
  message: string;
  isEmailReminder: boolean;
  isSent: boolean;
}

export interface InterviewFeedback {
  id: string;
  interviewId: string;
  technicalScore: number;
  communicationScore: number;
  culturalFitScore: number;
  overallScore: number;
  strengths: string;
  weaknesses: string;
  recommendation: string;
  notes: string;
  feedbackDate: string;
}

export interface InterviewAttachment {
  id: string;
  interviewId: string;
  fileName: string;
  filePath: string;
  fileType: string;
  attachmentType: AttachmentType;
  uploadedAt: string;
}

export enum AttachmentType {
  Resume = 'Resume',
  CoverLetter = 'CoverLetter',
  JobDescription = 'JobDescription',
  Other = 'Other',
}

export interface JobSource {
  id: number;
  name: string;
  description: string;
}

export interface MeetingPlatformType {
  id: number;
  name: string;
  platformType: MeetingPlatform;
}

export interface User {
  id: string;
  email: string;
  username: string;
  token?: string;
}

export interface AppSettings {
  apiEnabled: boolean;
  apiUrl: string;
  darkMode: boolean;
  notificationsEnabled: boolean;
  emailReminders: boolean;
  emailAddress: string;
  autoSync: boolean;
}
