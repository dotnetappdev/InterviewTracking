import { Interview, InterviewStatus, MeetingPlatform } from '../types';
import DatabaseService from '../services/DatabaseService';

export const sampleInterviews: Interview[] = [
  {
    id: 'sample-1',
    title: 'Technical Interview',
    companyName: 'Acme Corporation',
    jobTitle: 'Senior Software Engineer',
    dateTime: new Date(Date.now() + 86400000).toISOString(), // Tomorrow
    platform: MeetingPlatform.Zoom,
    meetingLink: 'https://zoom.us/j/123456789',
    status: InterviewStatus.Scheduled,
    notes: 'Prepare for system design questions',
    meetingPlatformTypeId: 1,
    userId: 'local-user',
    jobSourceId: undefined,
    jobPortalUrl: '',
    jobPortalUsername: '',
    jobPortalPassword: '',
    isRecurring: false,
    interviewers: [
      {
        id: 'int-1',
        name: 'John Smith',
        email: 'john.smith@acme.com',
        role: 'Engineering Manager',
        interviewId: 'sample-1',
      },
    ],
    reminders: [
      {
        id: 'rem-1',
        interviewId: 'sample-1',
        reminderTime: new Date(Date.now() + 82800000).toISOString(), // 1 hour before
        message: 'Interview in 1 hour',
        isEmailReminder: false,
        isSent: false,
      },
    ],
    feedback: [],
    attachments: [],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSynced: false,
  },
  {
    id: 'sample-2',
    title: 'Product Manager Interview',
    companyName: 'Beta Technologies',
    jobTitle: 'Product Manager',
    dateTime: new Date(Date.now() + 172800000).toISOString(), // 2 days from now
    platform: MeetingPlatform.GoogleMeet,
    meetingLink: 'https://meet.google.com/abc-defg-hij',
    status: InterviewStatus.Stage1,
    notes: 'Focus on product strategy and roadmap planning',
    meetingPlatformTypeId: 2,
    userId: 'local-user',
    jobSourceId: undefined,
    jobPortalUrl: 'https://jobs.betatech.com',
    jobPortalUsername: 'candidate@email.com',
    jobPortalPassword: 'secure123',
    isRecurring: false,
    interviewers: [
      {
        id: 'int-2',
        name: 'Sarah Johnson',
        email: 'sarah.j@betatech.com',
        role: 'VP of Product',
        interviewId: 'sample-2',
      },
    ],
    reminders: [],
    feedback: [],
    attachments: [],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSynced: false,
  },
  {
    id: 'sample-3',
    title: 'DevOps Engineer Interview',
    companyName: 'Cloud Innovations Inc',
    jobTitle: 'DevOps Engineer',
    dateTime: new Date(Date.now() + 259200000).toISOString(), // 3 days from now
    platform: MeetingPlatform.MicrosoftTeams,
    meetingLink: 'https://teams.microsoft.com/l/meetup-join/19%3ameeting',
    status: InterviewStatus.Stage2,
    notes: 'Kubernetes and CI/CD pipeline experience required',
    meetingPlatformTypeId: 3,
    userId: 'local-user',
    jobSourceId: undefined,
    jobPortalUrl: '',
    jobPortalUsername: '',
    jobPortalPassword: '',
    isRecurring: false,
    interviewers: [
      {
        id: 'int-3',
        name: 'Michael Chen',
        email: 'mchen@cloudinnovations.com',
        role: 'DevOps Lead',
        interviewId: 'sample-3',
      },
      {
        id: 'int-4',
        name: 'Emily Davis',
        email: 'edavis@cloudinnovations.com',
        role: 'Technical Director',
        interviewId: 'sample-3',
      },
    ],
    reminders: [
      {
        id: 'rem-2',
        interviewId: 'sample-3',
        reminderTime: new Date(Date.now() + 255600000).toISOString(), // 1 hour before
        message: 'DevOps interview in 1 hour',
        isEmailReminder: false,
        isSent: false,
      },
    ],
    feedback: [],
    attachments: [],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSynced: false,
  },
  {
    id: 'sample-4',
    title: 'Data Scientist Position',
    companyName: 'DataViz Analytics',
    jobTitle: 'Data Scientist',
    dateTime: new Date(Date.now() + 345600000).toISOString(), // 4 days from now
    platform: MeetingPlatform.Zoom,
    meetingLink: 'https://zoom.us/j/987654321',
    status: InterviewStatus.FinalRound,
    notes: 'ML algorithms and statistical analysis focus',
    meetingPlatformTypeId: 1,
    userId: 'local-user',
    jobSourceId: undefined,
    jobPortalUrl: 'https://careers.dataviz.com',
    jobPortalUsername: 'user@example.com',
    jobPortalPassword: 'password456',
    isRecurring: false,
    interviewers: [],
    reminders: [],
    feedback: [],
    attachments: [],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSynced: false,
  },
  {
    id: 'sample-5',
    title: 'UX Designer Interview',
    companyName: 'Design Studios Ltd',
    jobTitle: 'UX Designer',
    dateTime: new Date(Date.now() + 432000000).toISOString(), // 5 days from now
    platform: MeetingPlatform.GoogleMeet,
    meetingLink: 'https://meet.google.com/xyz-uvw-rst',
    status: InterviewStatus.Scheduled,
    notes: 'Bring portfolio and case studies',
    meetingPlatformTypeId: 2,
    userId: 'local-user',
    jobSourceId: undefined,
    jobPortalUrl: '',
    jobPortalUsername: '',
    jobPortalPassword: '',
    isRecurring: false,
    interviewers: [
      {
        id: 'int-5',
        name: 'Amanda Wilson',
        email: 'awilson@designstudios.com',
        role: 'Lead Designer',
        interviewId: 'sample-5',
      },
    ],
    reminders: [
      {
        id: 'rem-3',
        interviewId: 'sample-5',
        reminderTime: new Date(Date.now() + 428400000).toISOString(), // 1 hour before
        message: 'UX interview in 1 hour - prepare portfolio',
        isEmailReminder: true,
        isSent: false,
      },
    ],
    feedback: [],
    attachments: [],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
    isSynced: false,
  },
];

export async function loadSampleData(): Promise<void> {
  for (const interview of sampleInterviews) {
    try {
      await DatabaseService.createInterview(interview);
    } catch (error) {
      console.error('Failed to load sample interview:', interview.id, error);
    }
  }
}
