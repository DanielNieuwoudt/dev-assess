import axios, { AxiosInstance } from 'axios';

export function createApiClient(): AxiosInstance {
    let agent: AxiosInstance;

    return axios.create();
}