import * as SQLite from 'expo-sqlite';
import { Interview, Interviewer, Reminder, InterviewFeedback, InterviewAttachment } from '../types';

const DB_NAME = 'interviewTracking.db';

class DatabaseService {
  private db: SQLite.SQLiteDatabase | null = null;

  async init(): Promise<void> {
    try {
      this.db = await SQLite.openDatabaseAsync(DB_NAME);
      await this.createTables();
    } catch (error) {
      console.error('Failed to initialize database:', error);
      throw error;
    }
  }

  private async createTables(): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');

    await this.db.execAsync(`
      CREATE TABLE IF NOT EXISTS interviews (
        id TEXT PRIMARY KEY,
        title TEXT NOT NULL,
        dateTime TEXT NOT NULL,
        notes TEXT,
        meetingPlatformTypeId INTEGER,
        platform TEXT,
        meetingLink TEXT,
        userId TEXT,
        status TEXT,
        jobSourceId INTEGER,
        jobTitle TEXT,
        companyName TEXT,
        jobPortalUrl TEXT,
        jobPortalUsername TEXT,
        jobPortalPassword TEXT,
        isRecurring INTEGER DEFAULT 0,
        recurrencePattern TEXT,
        recurrenceEndDate TEXT,
        createdAt TEXT,
        updatedAt TEXT,
        isSynced INTEGER DEFAULT 0
      );

      CREATE TABLE IF NOT EXISTS interviewers (
        id TEXT PRIMARY KEY,
        name TEXT NOT NULL,
        email TEXT,
        role TEXT,
        interviewId TEXT,
        FOREIGN KEY (interviewId) REFERENCES interviews(id) ON DELETE CASCADE
      );

      CREATE TABLE IF NOT EXISTS reminders (
        id TEXT PRIMARY KEY,
        interviewId TEXT,
        reminderTime TEXT,
        message TEXT,
        isEmailReminder INTEGER DEFAULT 0,
        isSent INTEGER DEFAULT 0,
        FOREIGN KEY (interviewId) REFERENCES interviews(id) ON DELETE CASCADE
      );

      CREATE TABLE IF NOT EXISTS feedback (
        id TEXT PRIMARY KEY,
        interviewId TEXT,
        technicalScore INTEGER,
        communicationScore INTEGER,
        culturalFitScore INTEGER,
        overallScore INTEGER,
        strengths TEXT,
        weaknesses TEXT,
        recommendation TEXT,
        notes TEXT,
        feedbackDate TEXT,
        FOREIGN KEY (interviewId) REFERENCES interviews(id) ON DELETE CASCADE
      );

      CREATE TABLE IF NOT EXISTS attachments (
        id TEXT PRIMARY KEY,
        interviewId TEXT,
        fileName TEXT,
        filePath TEXT,
        fileType TEXT,
        attachmentType TEXT,
        uploadedAt TEXT,
        FOREIGN KEY (interviewId) REFERENCES interviews(id) ON DELETE CASCADE
      );
    `);
  }

  async getAllInterviews(): Promise<Interview[]> {
    if (!this.db) throw new Error('Database not initialized');

    const result = await this.db.getAllAsync<any>('SELECT * FROM interviews ORDER BY dateTime DESC');
    
    const interviews: Interview[] = [];
    for (const row of result) {
      const interview = await this.getInterviewById(row.id);
      if (interview) {
        interviews.push(interview);
      }
    }
    
    return interviews;
  }

  async getInterviewById(id: string): Promise<Interview | null> {
    if (!this.db) throw new Error('Database not initialized');

    const result = await this.db.getFirstAsync<any>(
      'SELECT * FROM interviews WHERE id = ?',
      [id]
    );

    if (!result) return null;

    const interviewers = await this.db.getAllAsync<Interviewer>(
      'SELECT * FROM interviewers WHERE interviewId = ?',
      [id]
    );

    const reminders = await this.db.getAllAsync<Reminder>(
      'SELECT * FROM reminders WHERE interviewId = ?',
      [id]
    );

    const feedback = await this.db.getAllAsync<InterviewFeedback>(
      'SELECT * FROM feedback WHERE interviewId = ?',
      [id]
    );

    const attachments = await this.db.getAllAsync<InterviewAttachment>(
      'SELECT * FROM attachments WHERE interviewId = ?',
      [id]
    );

    return {
      ...result,
      isRecurring: result.isRecurring === 1,
      isSynced: result.isSynced === 1,
      interviewers,
      reminders: reminders.map(r => ({ ...r, isEmailReminder: r.isEmailReminder === 1, isSent: r.isSent === 1 })),
      feedback,
      attachments,
    };
  }

  async createInterview(interview: Interview): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');

    await this.db.runAsync(
      `INSERT INTO interviews (id, title, dateTime, notes, meetingPlatformTypeId, platform, meetingLink, 
       userId, status, jobSourceId, jobTitle, companyName, jobPortalUrl, jobPortalUsername, jobPortalPassword,
       isRecurring, recurrencePattern, recurrenceEndDate, createdAt, updatedAt, isSynced)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)`,
      [
        interview.id,
        interview.title,
        interview.dateTime,
        interview.notes,
        interview.meetingPlatformTypeId,
        interview.platform,
        interview.meetingLink,
        interview.userId,
        interview.status,
        interview.jobSourceId || null,
        interview.jobTitle,
        interview.companyName,
        interview.jobPortalUrl,
        interview.jobPortalUsername,
        interview.jobPortalPassword,
        interview.isRecurring ? 1 : 0,
        interview.recurrencePattern || null,
        interview.recurrenceEndDate || null,
        interview.createdAt,
        interview.updatedAt,
        interview.isSynced ? 1 : 0,
      ]
    );

    // Insert related data
    for (const interviewer of interview.interviewers || []) {
      await this.createInterviewer(interviewer);
    }

    for (const reminder of interview.reminders || []) {
      await this.createReminder(reminder);
    }
  }

  async updateInterview(interview: Interview): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');

    await this.db.runAsync(
      `UPDATE interviews SET title = ?, dateTime = ?, notes = ?, meetingPlatformTypeId = ?, platform = ?, 
       meetingLink = ?, status = ?, jobTitle = ?, companyName = ?, jobPortalUrl = ?, jobPortalUsername = ?,
       jobPortalPassword = ?, updatedAt = ? WHERE id = ?`,
      [
        interview.title,
        interview.dateTime,
        interview.notes,
        interview.meetingPlatformTypeId,
        interview.platform,
        interview.meetingLink,
        interview.status,
        interview.jobTitle,
        interview.companyName,
        interview.jobPortalUrl,
        interview.jobPortalUsername,
        interview.jobPortalPassword,
        new Date().toISOString(),
        interview.id,
      ]
    );
  }

  async deleteInterview(id: string): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');
    await this.db.runAsync('DELETE FROM interviews WHERE id = ?', [id]);
  }

  async createInterviewer(interviewer: Interviewer): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');
    
    await this.db.runAsync(
      'INSERT INTO interviewers (id, name, email, role, interviewId) VALUES (?, ?, ?, ?, ?)',
      [interviewer.id, interviewer.name, interviewer.email, interviewer.role, interviewer.interviewId]
    );
  }

  async createReminder(reminder: Reminder): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');
    
    await this.db.runAsync(
      'INSERT INTO reminders (id, interviewId, reminderTime, message, isEmailReminder, isSent) VALUES (?, ?, ?, ?, ?, ?)',
      [reminder.id, reminder.interviewId, reminder.reminderTime, reminder.message, reminder.isEmailReminder ? 1 : 0, reminder.isSent ? 1 : 0]
    );
  }

  async searchByCompany(searchTerm: string): Promise<Interview[]> {
    if (!this.db) throw new Error('Database not initialized');

    const result = await this.db.getAllAsync<any>(
      'SELECT * FROM interviews WHERE companyName LIKE ? ORDER BY dateTime DESC',
      [`%${searchTerm}%`]
    );

    const interviews: Interview[] = [];
    for (const row of result) {
      const interview = await this.getInterviewById(row.id);
      if (interview) {
        interviews.push(interview);
      }
    }
    
    return interviews;
  }

  async getInterviewsByDate(date: string): Promise<Interview[]> {
    if (!this.db) throw new Error('Database not initialized');

    const result = await this.db.getAllAsync<any>(
      `SELECT * FROM interviews WHERE date(dateTime) = date(?) ORDER BY dateTime`,
      [date]
    );

    const interviews: Interview[] = [];
    for (const row of result) {
      const interview = await this.getInterviewById(row.id);
      if (interview) {
        interviews.push(interview);
      }
    }
    
    return interviews;
  }

  async clearAllData(): Promise<void> {
    if (!this.db) throw new Error('Database not initialized');
    
    await this.db.execAsync(`
      DELETE FROM attachments;
      DELETE FROM feedback;
      DELETE FROM reminders;
      DELETE FROM interviewers;
      DELETE FROM interviews;
    `);
  }
}

export default new DatabaseService();
