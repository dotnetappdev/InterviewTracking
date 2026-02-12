import React, { useEffect } from 'react';
import { StatusBar } from 'expo-status-bar';
import AppNavigator from './src/navigation/AppNavigator';
import DatabaseService from './src/services/DatabaseService';
import ApiService from './src/services/ApiService';

export default function App() {
  useEffect(() => {
    initializeApp();
  }, []);

  const initializeApp = async () => {
    try {
      await DatabaseService.init();
      await ApiService.init();
    } catch (error) {
      console.error('Failed to initialize app:', error);
    }
  };

  return (
    <>
      <AppNavigator />
      <StatusBar style="auto" />
    </>
  );
}
