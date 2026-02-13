import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import { Ionicons } from '@expo/vector-icons';

// Screens
import InterviewListScreen from '../screens/InterviewListScreen';
import InterviewDetailScreen from '../screens/InterviewDetailScreen';
import AddEditInterviewScreen from '../screens/AddEditInterviewScreen';
import SettingsScreen from '../screens/SettingsScreen';
import LoginScreen from '../screens/LoginScreen';
import RegisterScreen from '../screens/RegisterScreen';

export type RootStackParamList = {
  MainTabs: undefined;
  Login: undefined;
  Register: undefined;
  InterviewDetail: { interviewId: string };
  AddEditInterview: { interviewId?: string };
};

export type MainTabsParamList = {
  Interviews: undefined;
  Settings: undefined;
};

const Tab = createBottomTabNavigator<MainTabsParamList>();
const Stack = createStackNavigator<RootStackParamList>();

function MainTabs() {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ focused, color, size }) => {
          let iconName: keyof typeof Ionicons.glyphMap = 'calendar';

          if (route.name === 'Interviews') {
            iconName = focused ? 'calendar' : 'calendar-outline';
          } else if (route.name === 'Settings') {
            iconName = focused ? 'settings' : 'settings-outline';
          }

          return <Ionicons name={iconName} size={size} color={color} />;
        },
        tabBarActiveTintColor: '#007AFF',
        tabBarInactiveTintColor: 'gray',
      })}
    >
      <Tab.Screen 
        name="Interviews" 
        component={InterviewListScreen}
        options={{ title: 'Interview Tracker' }}
      />
      <Tab.Screen 
        name="Settings" 
        component={SettingsScreen}
        options={{ title: 'Settings' }}
      />
    </Tab.Navigator>
  );
}

export default function AppNavigator() {
  return (
    <NavigationContainer>
      <Stack.Navigator
        initialRouteName="MainTabs"
        screenOptions={{
          headerShown: false,
        }}
      >
        <Stack.Screen name="MainTabs" component={MainTabs} />
        <Stack.Screen 
          name="Login" 
          component={LoginScreen}
          options={{ headerShown: true, title: 'Login' }}
        />
        <Stack.Screen 
          name="Register" 
          component={RegisterScreen}
          options={{ headerShown: true, title: 'Register' }}
        />
        <Stack.Screen 
          name="InterviewDetail" 
          component={InterviewDetailScreen}
          options={{ headerShown: true, title: 'Interview Details' }}
        />
        <Stack.Screen 
          name="AddEditInterview" 
          component={AddEditInterviewScreen}
          options={{ headerShown: true, title: 'Add/Edit Interview' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}
