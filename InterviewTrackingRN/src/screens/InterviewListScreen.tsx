import React, { useState, useEffect, useCallback } from 'react';
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  TouchableOpacity,
  RefreshControl,
  TextInput,
  Alert,
  Linking,
  Platform,
} from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { StackNavigationProp } from '@react-navigation/stack';
import { Calendar } from 'react-native-calendars';
import { format, parseISO } from 'date-fns';
import { Interview, InterviewStatus } from '../types';
import DatabaseService from '../services/DatabaseService';
import { RootStackParamList } from '../navigation/AppNavigator';

type NavigationProp = StackNavigationProp<RootStackParamList>;

export default function InterviewListScreen() {
  const navigation = useNavigation<NavigationProp>();
  const [interviews, setInterviews] = useState<Interview[]>([]);
  const [filteredInterviews, setFilteredInterviews] = useState<Interview[]>([]);
  const [refreshing, setRefreshing] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [isSearchMode, setIsSearchMode] = useState(false);
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]);

  useEffect(() => {
    initDatabase();
  }, []);

  const initDatabase = async () => {
    try {
      await DatabaseService.init();
      await loadInterviews();
    } catch (error) {
      console.error('Failed to initialize database:', error);
      Alert.alert('Error', 'Failed to initialize database');
    }
  };

  const loadInterviews = useCallback(async () => {
    try {
      const allInterviews = await DatabaseService.getAllInterviews();
      setInterviews(allInterviews);
      filterInterviewsByDate(allInterviews, selectedDate);
    } catch (error) {
      console.error('Failed to load interviews:', error);
      Alert.alert('Error', 'Failed to load interviews');
    }
  }, [selectedDate]);

  const filterInterviewsByDate = (allInterviews: Interview[], date: string) => {
    const filtered = allInterviews.filter(interview => {
      const interviewDate = interview.dateTime.split('T')[0];
      return interviewDate === date;
    });
    setFilteredInterviews(filtered);
  };

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadInterviews();
    setRefreshing(false);
  }, [loadInterviews]);

  const handleSearch = async () => {
    if (!searchText.trim()) {
      setFilteredInterviews(interviews);
      setIsSearchMode(false);
      return;
    }

    try {
      const results = await DatabaseService.searchByCompany(searchText);
      setFilteredInterviews(results);
      setIsSearchMode(true);
    } catch (error) {
      console.error('Search failed:', error);
      Alert.alert('Error', 'Search failed');
    }
  };

  const handleClearSearch = () => {
    setSearchText('');
    setIsSearchMode(false);
    filterInterviewsByDate(interviews, selectedDate);
  };

  const handleDateSelect = (date: any) => {
    setSelectedDate(date.dateString);
    setIsSearchMode(false);
    filterInterviewsByDate(interviews, date.dateString);
  };

  const handleInterviewPress = (interview: Interview) => {
    navigation.navigate('InterviewDetail', { interviewId: interview.id });
  };

  const handleAddInterview = () => {
    navigation.navigate('AddEditInterview', {});
  };

  const handleJoinMeeting = (interview: Interview) => {
    if (interview.meetingLink) {
      Linking.openURL(interview.meetingLink).catch(err => {
        console.error('Failed to open meeting link:', err);
        Alert.alert('Error', 'Failed to open meeting link');
      });
    }
  };

  const getStatusColor = (status: InterviewStatus): string => {
    switch (status) {
      case InterviewStatus.Scheduled:
        return '#007AFF';
      case InterviewStatus.Completed:
        return '#34C759';
      case InterviewStatus.Cancelled:
      case InterviewStatus.Rejected:
        return '#FF3B30';
      case InterviewStatus.OfferReceived:
        return '#FFD700';
      default:
        return '#8E8E93';
    }
  };

  const renderInterviewItem = ({ item }: { item: Interview }) => (
    <TouchableOpacity
      style={styles.interviewCard}
      onPress={() => handleInterviewPress(item)}
    >
      <View style={styles.cardHeader}>
        <Text style={styles.title}>{item.title}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(item.status) }]}>
          <Text style={styles.statusText}>{item.status}</Text>
        </View>
      </View>
      
      <Text style={styles.companyName}>{item.companyName}</Text>
      <Text style={styles.dateTime}>
        {format(parseISO(item.dateTime), 'MMM dd, yyyy - hh:mm a')}
      </Text>
      <Text style={styles.platform}>Platform: {item.platform}</Text>
      
      {item.jobTitle && (
        <Text style={styles.jobTitle}>Position: {item.jobTitle}</Text>
      )}

      {item.meetingLink && (
        <TouchableOpacity
          style={styles.joinButton}
          onPress={() => handleJoinMeeting(item)}
        >
          <Text style={styles.joinButtonText}>Join Meeting</Text>
        </TouchableOpacity>
      )}
    </TouchableOpacity>
  );

  const markedDates = React.useMemo(() => {
    const marked: any = {};
    interviews.forEach(interview => {
      const date = interview.dateTime.split('T')[0];
      marked[date] = { marked: true, dotColor: '#007AFF' };
    });
    marked[selectedDate] = {
      ...marked[selectedDate],
      selected: true,
      selectedColor: '#007AFF',
    };
    return marked;
  }, [interviews, selectedDate]);

  return (
    <View style={styles.container}>
      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder="Search by company name..."
          value={searchText}
          onChangeText={setSearchText}
          onSubmitEditing={handleSearch}
        />
        <TouchableOpacity style={styles.searchButton} onPress={handleSearch}>
          <Text style={styles.searchButtonText}>🔍</Text>
        </TouchableOpacity>
        {isSearchMode && (
          <TouchableOpacity style={styles.clearButton} onPress={handleClearSearch}>
            <Text style={styles.clearButtonText}>✖</Text>
          </TouchableOpacity>
        )}
      </View>

      {!isSearchMode && (
        <Calendar
          onDayPress={handleDateSelect}
          markedDates={markedDates}
          theme={{
            selectedDayBackgroundColor: '#007AFF',
            todayTextColor: '#007AFF',
            arrowColor: '#007AFF',
          }}
        />
      )}

      <View style={styles.listHeader}>
        <Text style={styles.listTitle}>
          {isSearchMode
            ? `Search Results (${filteredInterviews.length})`
            : `Interviews on ${format(parseISO(selectedDate), 'MMM dd, yyyy')} (${filteredInterviews.length})`}
        </Text>
      </View>

      <FlatList
        data={filteredInterviews}
        renderItem={renderInterviewItem}
        keyExtractor={item => item.id}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>No interviews scheduled</Text>
            <Text style={styles.emptySubtext}>Tap + to add an interview</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      <TouchableOpacity style={styles.fab} onPress={handleAddInterview}>
        <Text style={styles.fabText}>+</Text>
      </TouchableOpacity>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F2F2F7',
  },
  searchContainer: {
    flexDirection: 'row',
    padding: 10,
    backgroundColor: '#FFFFFF',
    alignItems: 'center',
  },
  searchInput: {
    flex: 1,
    height: 40,
    borderWidth: 1,
    borderColor: '#E5E5EA',
    borderRadius: 8,
    paddingHorizontal: 12,
    backgroundColor: '#FFFFFF',
  },
  searchButton: {
    marginLeft: 8,
    padding: 8,
  },
  searchButtonText: {
    fontSize: 20,
  },
  clearButton: {
    marginLeft: 8,
    padding: 8,
  },
  clearButtonText: {
    fontSize: 16,
    color: '#FF3B30',
  },
  listHeader: {
    padding: 12,
    backgroundColor: '#FFFFFF',
    borderBottomWidth: 1,
    borderBottomColor: '#E5E5EA',
  },
  listTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#000000',
  },
  listContent: {
    padding: 10,
  },
  interviewCard: {
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    padding: 16,
    marginBottom: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  cardHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  title: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#000000',
    flex: 1,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
  },
  statusText: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: '600',
  },
  companyName: {
    fontSize: 16,
    color: '#000000',
    marginBottom: 4,
  },
  dateTime: {
    fontSize: 14,
    color: '#8E8E93',
    marginBottom: 4,
  },
  platform: {
    fontSize: 14,
    color: '#8E8E93',
    marginBottom: 4,
  },
  jobTitle: {
    fontSize: 14,
    color: '#007AFF',
    marginTop: 4,
  },
  joinButton: {
    marginTop: 12,
    backgroundColor: '#007AFF',
    paddingVertical: 8,
    paddingHorizontal: 16,
    borderRadius: 8,
    alignSelf: 'flex-start',
  },
  joinButtonText: {
    color: '#FFFFFF',
    fontWeight: '600',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 60,
  },
  emptyText: {
    fontSize: 18,
    color: '#8E8E93',
    marginBottom: 8,
  },
  emptySubtext: {
    fontSize: 14,
    color: '#8E8E93',
  },
  fab: {
    position: 'absolute',
    right: 20,
    bottom: 20,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: '#007AFF',
    justifyContent: 'center',
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.3,
    shadowRadius: 4,
    elevation: 8,
  },
  fabText: {
    fontSize: 32,
    color: '#FFFFFF',
    fontWeight: 'bold',
  },
});
