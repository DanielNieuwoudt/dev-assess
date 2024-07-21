import axios, { AxiosInstance } from 'axios';

export function createApiClient(): AxiosInstance {
    return axios.create();
}