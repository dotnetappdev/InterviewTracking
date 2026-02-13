import axios, { AxiosInstance } from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { Interview, User } from '../types';

const TOKEN_KEY = 'auth_token';
const API_URL_KEY = 'api_url';

class ApiService {
  private axiosInstance: AxiosInstance | null = null;
  private apiUrl: string = 'https://localhost:7000/api/';

  async init(): Promise<void> {
    const savedUrl = await AsyncStorage.getItem(API_URL_KEY);
    if (savedUrl) {
      this.apiUrl = savedUrl;
    }
    await this.createAxiosInstance();
  }

  private async createAxiosInstance(): Promise<void> {
    const token = await AsyncStorage.getItem(TOKEN_KEY);
    
    this.axiosInstance = axios.create({
      baseURL: this.apiUrl,
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
      },
    });
  }

  async setApiUrl(url: string): Promise<void> {
    this.apiUrl = url;
    await AsyncStorage.setItem(API_URL_KEY, url);
    await this.createAxiosInstance();
  }

  async login(email: string, password: string): Promise<User> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.post<{ token: string; userId: string }>('/auth/login', {
      email,
      password,
    });

    await AsyncStorage.setItem(TOKEN_KEY, response.data.token);
    await this.createAxiosInstance();

    return {
      id: response.data.userId,
      email,
      username: email,
      token: response.data.token,
    };
  }

  async register(email: string, password: string, username: string): Promise<User> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.post<{ token: string; userId: string }>('/auth/register', {
      email,
      password,
      username,
    });

    await AsyncStorage.setItem(TOKEN_KEY, response.data.token);
    await this.createAxiosInstance();

    return {
      id: response.data.userId,
      email,
      username,
      token: response.data.token,
    };
  }

  async logout(): Promise<void> {
    await AsyncStorage.removeItem(TOKEN_KEY);
    await this.createAxiosInstance();
  }

  async getInterviews(): Promise<Interview[]> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.get<Interview[]>('/interviews');
    return response.data;
  }

  async getInterview(id: string): Promise<Interview> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.get<Interview>(`/interviews/${id}`);
    return response.data;
  }

  async createInterview(interview: Interview): Promise<Interview> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.post<Interview>('/interviews', interview);
    return response.data;
  }

  async updateInterview(interview: Interview): Promise<Interview> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.put<Interview>(`/interviews/${interview.id}`, interview);
    return response.data;
  }

  async deleteInterview(id: string): Promise<void> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    await this.axiosInstance.delete(`/interviews/${id}`);
  }

  async syncInterviews(localInterviews: Interview[]): Promise<Interview[]> {
    if (!this.axiosInstance) throw new Error('API service not initialized');

    const response = await this.axiosInstance.post<Interview[]>('/interviews/sync', localInterviews);
    return response.data;
  }

  async isAuthenticated(): Promise<boolean> {
    const token = await AsyncStorage.getItem(TOKEN_KEY);
    return !!token;
  }
}

export default new ApiService();
