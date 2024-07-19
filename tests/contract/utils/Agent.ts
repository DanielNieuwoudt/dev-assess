import axios, { AxiosInstance } from 'axios';
import * as https from 'https';

export function createApiClient(): AxiosInstance {
    let agent: AxiosInstance;

    let environment = (process.env.ENVIRONMENT_NAME || 'local').toLowerCase();
        
    if (environment === 'local' || environment === 'development') {
        process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
        agent = axios.create({
            httpsAgent: new https.Agent({
                rejectUnauthorized: false
            })
        });
    }
    else {
        agent = axios.create();
    }

    return agent;
}