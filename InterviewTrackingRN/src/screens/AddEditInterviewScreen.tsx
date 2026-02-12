import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TextInput,
  TouchableOpacity,
  Alert,
  Platform,
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { StackNavigationProp } from '@react-navigation/stack';
import { Picker } from '@react-native-picker/picker';
import { Interview, MeetingPlatform, InterviewStatus } from '../types';
import DatabaseService from '../services/DatabaseService';
import { RootStackParamList } from '../navigation/AppNavigator';

type AddEditRouteProp = RouteProp<RootStackParamList, 'AddEditInterview'>;
type NavigationProp = StackNavigationProp<RootStackParamList>;

export default function AddEditInterviewScreen() {
  const navigation = useNavigation<NavigationProp>();
  const route = useRoute<AddEditRouteProp>();
  const interviewId = route.params?.interviewId;
  const isEditing = !!interviewId;

  const [title, setTitle] = useState('');
  const [companyName, setCompanyName] = useState('');
  const [jobTitle, setJobTitle] = useState('');
  const [dateTime, setDateTime] = useState(new Date().toISOString());
  const [platform, setPlatform] = useState<MeetingPlatform>(MeetingPlatform.Zoom);
  const [meetingLink, setMeetingLink] = useState('');
  const [status, setStatus] = useState<InterviewStatus>(InterviewStatus.Scheduled);
  const [notes, setNotes] = useState('');
  const [jobPortalUrl, setJobPortalUrl] = useState('');
  const [jobPortalUsername, setJobPortalUsername] = useState('');
  const [jobPortalPassword, setJobPortalPassword] = useState('');

  useEffect(() => {
    if (isEditing) {
      loadInterview();
    }
  }, [interviewId]);

  const loadInterview = async () => {
    if (!interviewId) return;

    try {
      const interview = await DatabaseService.getInterviewById(interviewId);
      if (interview) {
        setTitle(interview.title);
        setCompanyName(interview.companyName);
        setJobTitle(interview.jobTitle);
        setDateTime(interview.dateTime);
        setPlatform(interview.platform);
        setMeetingLink(interview.meetingLink);
        setStatus(interview.status);
        setNotes(interview.notes);
        setJobPortalUrl(interview.jobPortalUrl);
        setJobPortalUsername(interview.jobPortalUsername);
        setJobPortalPassword(interview.jobPortalPassword);
      }
    } catch (error) {
      console.error('Failed to load interview:', error);
      Alert.alert('Error', 'Failed to load interview');
    }
  };

  const handleSave = async () => {
    if (!title.trim() || !companyName.trim()) {
      Alert.alert('Validation Error', 'Title and Company Name are required');
      return;
    }

    try {
      const interview: Interview = {
        id: interviewId || `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
        title,
        companyName,
        jobTitle,
        dateTime,
        platform,
        meetingLink,
        status,
        notes,
        jobPortalUrl,
        jobPortalUsername,
        jobPortalPassword,
        meetingPlatformTypeId: 1,
        userId: 'local-user',
        isRecurring: false,
        interviewers: [],
        reminders: [],
        feedback: [],
        attachments: [],
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        isSynced: false,
      };

      if (isEditing) {
        await DatabaseService.updateInterview(interview);
        Alert.alert('Success', 'Interview updated successfully');
      } else {
        await DatabaseService.createInterview(interview);
        Alert.alert('Success', 'Interview created successfully');
      }

      navigation.goBack();
    } catch (error) {
      console.error('Failed to save interview:', error);
      Alert.alert('Error', 'Failed to save interview');
    }
  };

  return (
    <ScrollView style={styles.container}>
      <View style={styles.content}>
        <View style={styles.inputGroup}>
          <Text style={styles.label}>Title *</Text>
          <TextInput
            style={styles.input}
            value={title}
            onChangeText={setTitle}
            placeholder="e.g., Technical Interview"
          />
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Company Name *</Text>
          <TextInput
            style={styles.input}
            value={companyName}
            onChangeText={setCompanyName}
            placeholder="e.g., Acme Corporation"
          />
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Job Title</Text>
          <TextInput
            style={styles.input}
            value={jobTitle}
            onChangeText={setJobTitle}
            placeholder="e.g., Senior Software Engineer"
          />
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Date & Time</Text>
          <Text style={styles.dateTimeText}>
            {new Date(dateTime).toLocaleString()}
          </Text>
          <Text style={styles.helperText}>
            Date/Time picker coming soon - using current time for now
          </Text>
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Meeting Platform</Text>
          <View style={styles.pickerContainer}>
            <Picker
              selectedValue={platform}
              onValueChange={(value) => setPlatform(value)}
              style={styles.picker}
            >
              <Picker.Item label="Zoom" value={MeetingPlatform.Zoom} />
              <Picker.Item label="Google Meet" value={MeetingPlatform.GoogleMeet} />
              <Picker.Item label="Microsoft Teams" value={MeetingPlatform.MicrosoftTeams} />
              <Picker.Item label="Other" value={MeetingPlatform.Other} />
            </Picker>
          </View>
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Meeting Link</Text>
          <TextInput
            style={styles.input}
            value={meetingLink}
            onChangeText={setMeetingLink}
            placeholder="https://..."
            autoCapitalize="none"
            keyboardType="url"
          />
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Status</Text>
          <View style={styles.pickerContainer}>
            <Picker
              selectedValue={status}
              onValueChange={(value) => setStatus(value)}
              style={styles.picker}
            >
              <Picker.Item label="Scheduled" value={InterviewStatus.Scheduled} />
              <Picker.Item label="Stage 1" value={InterviewStatus.Stage1} />
              <Picker.Item label="Stage 2" value={InterviewStatus.Stage2} />
              <Picker.Item label="Final Round" value={InterviewStatus.FinalRound} />
              <Picker.Item label="Completed" value={InterviewStatus.Completed} />
              <Picker.Item label="Cancelled" value={InterviewStatus.Cancelled} />
              <Picker.Item label="Rejected" value={InterviewStatus.Rejected} />
              <Picker.Item label="Offer Received" value={InterviewStatus.OfferReceived} />
            </Picker>
          </View>
        </View>

        <View style={styles.inputGroup}>
          <Text style={styles.label}>Notes</Text>
          <TextInput
            style={[styles.input, styles.textArea]}
            value={notes}
            onChangeText={setNotes}
            placeholder="Add any notes or comments..."
            multiline
            numberOfLines={4}
          />
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Job Portal Credentials (Optional)</Text>
          
          <View style={styles.inputGroup}>
            <Text style={styles.label}>Portal URL</Text>
            <TextInput
              style={styles.input}
              value={jobPortalUrl}
              onChangeText={setJobPortalUrl}
              placeholder="https://..."
              autoCapitalize="none"
              keyboardType="url"
            />
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.label}>Username</Text>
            <TextInput
              style={styles.input}
              value={jobPortalUsername}
              onChangeText={setJobPortalUsername}
              placeholder="Username"
              autoCapitalize="none"
            />
          </View>

          <View style={styles.inputGroup}>
            <Text style={styles.label}>Password</Text>
            <TextInput
              style={styles.input}
              value={jobPortalPassword}
              onChangeText={setJobPortalPassword}
              placeholder="Password"
              secureTextEntry
            />
          </View>
        </View>

        <TouchableOpacity style={styles.saveButton} onPress={handleSave}>
          <Text style={styles.saveButtonText}>
            {isEditing ? 'Update Interview' : 'Create Interview'}
          </Text>
        </TouchableOpacity>
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
  inputGroup: {
    marginBottom: 16,
  },
  label: {
    fontSize: 14,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 8,
  },
  input: {
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
    borderWidth: 1,
    borderColor: '#E5E5EA',
  },
  textArea: {
    minHeight: 100,
    textAlignVertical: 'top',
  },
  pickerContainer: {
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#E5E5EA',
  },
  picker: {
    height: 50,
  },
  dateTimeText: {
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
    borderWidth: 1,
    borderColor: '#E5E5EA',
    color: '#000000',
  },
  helperText: {
    fontSize: 12,
    color: '#8E8E93',
    marginTop: 4,
  },
  section: {
    marginTop: 24,
    paddingTop: 16,
    borderTopWidth: 1,
    borderTopColor: '#E5E5EA',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 16,
  },
  saveButton: {
    backgroundColor: '#007AFF',
    paddingVertical: 16,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 24,
    marginBottom: 32,
  },
  saveButtonText: {
    color: '#FFFFFF',
    fontSize: 18,
    fontWeight: '600',
  },
});
