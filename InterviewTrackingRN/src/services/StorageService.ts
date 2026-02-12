import AsyncStorage from '@react-native-async-storage/async-storage';
import { AppSettings } from '../types';

const SETTINGS_KEY = 'app_settings';

const defaultSettings: AppSettings = {
  apiEnabled: false,
  apiUrl: 'https://localhost:7000/api/',
  darkMode: false,
  notificationsEnabled: true,
  emailReminders: false,
  emailAddress: '',
  autoSync: false,
};

class StorageService {
  async getSettings(): Promise<AppSettings> {
    try {
      const settingsJson = await AsyncStorage.getItem(SETTINGS_KEY);
      if (settingsJson) {
        return { ...defaultSettings, ...JSON.parse(settingsJson) };
      }
      return defaultSettings;
    } catch (error) {
      console.error('Error loading settings:', error);
      return defaultSettings;
    }
  }

  async saveSettings(settings: AppSettings): Promise<void> {
    try {
      await AsyncStorage.setItem(SETTINGS_KEY, JSON.stringify(settings));
    } catch (error) {
      console.error('Error saving settings:', error);
      throw error;
    }
  }

  async updateSetting(key: keyof AppSettings, value: any): Promise<void> {
    const settings = await this.getSettings();
    settings[key] = value;
    await this.saveSettings(settings);
  }

  async clearSettings(): Promise<void> {
    await AsyncStorage.removeItem(SETTINGS_KEY);
  }
}

export default new StorageService();
