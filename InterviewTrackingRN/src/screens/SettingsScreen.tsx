import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  Switch,
  TextInput,
  TouchableOpacity,
  Alert,
} from 'react-native';
import { AppSettings } from '../types';
import StorageService from '../services/StorageService';
import DatabaseService from '../services/DatabaseService';
import { loadSampleData } from '../utils/sampleData';

export default function SettingsScreen() {
  const [settings, setSettings] = useState<AppSettings>({
    apiEnabled: false,
    apiUrl: 'https://localhost:7000/api/',
    darkMode: false,
    notificationsEnabled: true,
    emailReminders: false,
    emailAddress: '',
    autoSync: false,
  });

  useEffect(() => {
    loadSettings();
  }, []);

  const loadSettings = async () => {
    try {
      const savedSettings = await StorageService.getSettings();
      setSettings(savedSettings);
    } catch (error) {
      console.error('Failed to load settings:', error);
    }
  };

  const handleSaveSettings = async () => {
    try {
      await StorageService.saveSettings(settings);
      Alert.alert('Success', 'Settings saved successfully');
    } catch (error) {
      console.error('Failed to save settings:', error);
      Alert.alert('Error', 'Failed to save settings');
    }
  };

  const handleClearAllData = () => {
    Alert.alert(
      'Clear All Data',
      'Are you sure you want to delete all interviews? This action cannot be undone.',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Delete All',
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.clearAllData();
              Alert.alert('Success', 'All data has been cleared');
            } catch (error) {
              console.error('Failed to clear data:', error);
              Alert.alert('Error', 'Failed to clear data');
            }
          },
        },
      ]
    );
  };

  const handleLoadSampleData = () => {
    Alert.alert(
      'Load Sample Data',
      'This will add 5 sample interviews to your database. Continue?',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Load',
          onPress: async () => {
            try {
              await loadSampleData();
              Alert.alert('Success', 'Sample data has been loaded');
            } catch (error) {
              console.error('Failed to load sample data:', error);
              Alert.alert('Error', 'Failed to load sample data');
            }
          },
        },
      ]
    );
  };

  const updateSetting = <K extends keyof AppSettings>(key: K, value: AppSettings[K]) => {
    setSettings(prev => ({ ...prev, [key]: value }));
  };

  return (
    <ScrollView style={styles.container}>
      <View style={styles.content}>
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>API Configuration</Text>
          
          <View style={styles.settingRow}>
            <Text style={styles.settingLabel}>Enable API</Text>
            <Switch
              value={settings.apiEnabled}
              onValueChange={(value) => updateSetting('apiEnabled', value)}
            />
          </View>

          {settings.apiEnabled && (
            <View style={styles.inputGroup}>
              <Text style={styles.label}>API URL</Text>
              <TextInput
                style={styles.input}
                value={settings.apiUrl}
                onChangeText={(value) => updateSetting('apiUrl', value)}
                placeholder="https://..."
                autoCapitalize="none"
                keyboardType="url"
              />
            </View>
          )}
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Notifications</Text>
          
          <View style={styles.settingRow}>
            <Text style={styles.settingLabel}>Enable Notifications</Text>
            <Switch
              value={settings.notificationsEnabled}
              onValueChange={(value) => updateSetting('notificationsEnabled', value)}
            />
          </View>

          <View style={styles.settingRow}>
            <Text style={styles.settingLabel}>Email Reminders</Text>
            <Switch
              value={settings.emailReminders}
              onValueChange={(value) => updateSetting('emailReminders', value)}
            />
          </View>

          {settings.emailReminders && (
            <View style={styles.inputGroup}>
              <Text style={styles.label}>Email Address</Text>
              <TextInput
                style={styles.input}
                value={settings.emailAddress}
                onChangeText={(value) => updateSetting('emailAddress', value)}
                placeholder="your@email.com"
                autoCapitalize="none"
                keyboardType="email-address"
              />
            </View>
          )}
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Synchronization</Text>
          
          <View style={styles.settingRow}>
            <Text style={styles.settingLabel}>Auto Sync</Text>
            <Switch
              value={settings.autoSync}
              onValueChange={(value) => updateSetting('autoSync', value)}
              disabled={!settings.apiEnabled}
            />
          </View>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Appearance</Text>
          
          <View style={styles.settingRow}>
            <Text style={styles.settingLabel}>Dark Mode</Text>
            <Switch
              value={settings.darkMode}
              onValueChange={(value) => updateSetting('darkMode', value)}
            />
          </View>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Data Management</Text>
          
          <TouchableOpacity style={styles.actionButton} onPress={handleLoadSampleData}>
            <Text style={styles.actionButtonText}>Load Sample Data</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={styles.dangerButton} onPress={handleClearAllData}>
            <Text style={styles.dangerButtonText}>Clear All Data</Text>
          </TouchableOpacity>
        </View>

        <TouchableOpacity style={styles.saveButton} onPress={handleSaveSettings}>
          <Text style={styles.saveButtonText}>Save Settings</Text>
        </TouchableOpacity>

        <View style={styles.footer}>
          <Text style={styles.footerText}>Interview Tracking v1.0.0</Text>
          <Text style={styles.footerText}>React Native Version</Text>
        </View>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F2F2F7',
  },
  content: {
    padding: 16,
  },
  section: {
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    padding: 16,
    marginBottom: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 16,
  },
  settingRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 8,
  },
  settingLabel: {
    fontSize: 16,
    color: '#000000',
  },
  inputGroup: {
    marginTop: 12,
  },
  label: {
    fontSize: 14,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 8,
  },
  input: {
    backgroundColor: '#F2F2F7',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
    borderWidth: 1,
    borderColor: '#E5E5EA',
  },
  saveButton: {
    backgroundColor: '#007AFF',
    paddingVertical: 16,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 8,
  },
  saveButtonText: {
    color: '#FFFFFF',
    fontSize: 18,
    fontWeight: '600',
  },
  actionButton: {
    backgroundColor: '#34C759',
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
    marginBottom: 12,
  },
  actionButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  dangerButton: {
    backgroundColor: '#FF3B30',
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
  },
  dangerButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  footer: {
    alignItems: 'center',
    marginTop: 32,
    marginBottom: 24,
  },
  footerText: {
    fontSize: 12,
    color: '#8E8E93',
    marginBottom: 4,
  },
});
