import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Alert,
  Linking,
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { StackNavigationProp } from '@react-navigation/stack';
import { format, parseISO } from 'date-fns';
import { Interview } from '../types';
import DatabaseService from '../services/DatabaseService';
import { RootStackParamList } from '../navigation/AppNavigator';

type DetailRouteProp = RouteProp<RootStackParamList, 'InterviewDetail'>;
type NavigationProp = StackNavigationProp<RootStackParamList>;

export default function InterviewDetailScreen() {
  const navigation = useNavigation<NavigationProp>();
  const route = useRoute<DetailRouteProp>();
  const { interviewId } = route.params;
  const [interview, setInterview] = useState<Interview | null>(null);

  useEffect(() => {
    loadInterview();
  }, [interviewId]);

  const loadInterview = async () => {
    try {
      const data = await DatabaseService.getInterviewById(interviewId);
      setInterview(data);
    } catch (error) {
      console.error('Failed to load interview:', error);
      Alert.alert('Error', 'Failed to load interview details');
    }
  };

  const handleEdit = () => {
    navigation.navigate('AddEditInterview', { interviewId });
  };

  const handleDelete = () => {
    Alert.alert(
      'Delete Interview',
      'Are you sure you want to delete this interview?',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Delete',
          style: 'destructive',
          onPress: async () => {
            try {
              await DatabaseService.deleteInterview(interviewId);
              navigation.goBack();
            } catch (error) {
              console.error('Failed to delete interview:', error);
              Alert.alert('Error', 'Failed to delete interview');
            }
          },
        },
      ]
    );
  };

  const handleJoinMeeting = () => {
    if (interview?.meetingLink) {
      Linking.openURL(interview.meetingLink).catch(err => {
        console.error('Failed to open meeting link:', err);
        Alert.alert('Error', 'Failed to open meeting link');
      });
    }
  };

  if (!interview) {
    return (
      <View style={styles.container}>
        <Text style={styles.loadingText}>Loading...</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.title}>{interview.title}</Text>
          <Text style={styles.companyName}>{interview.companyName}</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Interview Details</Text>
          <DetailRow label="Date & Time" value={format(parseISO(interview.dateTime), 'MMM dd, yyyy - hh:mm a')} />
          <DetailRow label="Status" value={interview.status} />
          <DetailRow label="Job Title" value={interview.jobTitle} />
          <DetailRow label="Platform" value={interview.platform} />
        </View>

        {interview.notes && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Notes</Text>
            <Text style={styles.notes}>{interview.notes}</Text>
          </View>
        )}

        {interview.meetingLink && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Meeting Link</Text>
            <TouchableOpacity onPress={handleJoinMeeting}>
              <Text style={styles.link}>{interview.meetingLink}</Text>
            </TouchableOpacity>
          </View>
        )}

        {interview.jobPortalUrl && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Job Portal Credentials</Text>
            <DetailRow label="URL" value={interview.jobPortalUrl} />
            {interview.jobPortalUsername && (
              <DetailRow label="Username" value={interview.jobPortalUsername} />
            )}
            {interview.jobPortalPassword && (
              <DetailRow label="Password" value="••••••••" />
            )}
          </View>
        )}

        {interview.interviewers && interview.interviewers.length > 0 && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Interviewers</Text>
            {interview.interviewers.map((interviewer, index) => (
              <View key={index} style={styles.interviewer}>
                <Text style={styles.interviewerName}>{interviewer.name}</Text>
                {interviewer.role && (
                  <Text style={styles.interviewerRole}>{interviewer.role}</Text>
                )}
                {interviewer.email && (
                  <Text style={styles.interviewerEmail}>{interviewer.email}</Text>
                )}
              </View>
            ))}
          </View>
        )}

        {interview.reminders && interview.reminders.length > 0 && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Reminders</Text>
            {interview.reminders.map((reminder, index) => (
              <View key={index} style={styles.reminder}>
                <Text style={styles.reminderTime}>
                  {format(parseISO(reminder.reminderTime), 'MMM dd, yyyy - hh:mm a')}
                </Text>
                {reminder.message && (
                  <Text style={styles.reminderMessage}>{reminder.message}</Text>
                )}
              </View>
            ))}
          </View>
        )}

        <View style={styles.actionButtons}>
          <TouchableOpacity style={styles.editButton} onPress={handleEdit}>
            <Text style={styles.editButtonText}>Edit</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.deleteButton} onPress={handleDelete}>
            <Text style={styles.deleteButtonText}>Delete</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
}

function DetailRow({ label, value }: { label: string; value: string }) {
  return (
    <View style={styles.detailRow}>
      <Text style={styles.detailLabel}>{label}:</Text>
      <Text style={styles.detailValue}>{value}</Text>
    </View>
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
  loadingText: {
    textAlign: 'center',
    marginTop: 50,
    fontSize: 16,
    color: '#8E8E93',
  },
  header: {
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
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#000000',
    marginBottom: 8,
  },
  companyName: {
    fontSize: 18,
    color: '#007AFF',
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
    marginBottom: 12,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  detailLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#8E8E93',
    width: 120,
  },
  detailValue: {
    fontSize: 14,
    color: '#000000',
    flex: 1,
  },
  notes: {
    fontSize: 14,
    color: '#000000',
    lineHeight: 20,
  },
  link: {
    fontSize: 14,
    color: '#007AFF',
    textDecorationLine: 'underline',
  },
  interviewer: {
    marginBottom: 12,
    paddingBottom: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5EA',
  },
  interviewerName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 4,
  },
  interviewerRole: {
    fontSize: 14,
    color: '#8E8E93',
    marginBottom: 2,
  },
  interviewerEmail: {
    fontSize: 14,
    color: '#007AFF',
  },
  reminder: {
    marginBottom: 12,
    paddingBottom: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5EA',
  },
  reminderTime: {
    fontSize: 14,
    fontWeight: '600',
    color: '#000000',
    marginBottom: 4,
  },
  reminderMessage: {
    fontSize: 14,
    color: '#8E8E93',
  },
  actionButtons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8,
  },
  editButton: {
    flex: 1,
    backgroundColor: '#007AFF',
    paddingVertical: 12,
    borderRadius: 8,
    marginRight: 8,
    alignItems: 'center',
  },
  editButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  deleteButton: {
    flex: 1,
    backgroundColor: '#FF3B30',
    paddingVertical: 12,
    borderRadius: 8,
    marginLeft: 8,
    alignItems: 'center',
  },
  deleteButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
});
