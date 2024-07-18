import axios, { AxiosInstance } from 'axios';

export function createApiClient(): AxiosInstance {
    let agent: AxiosInstance;

    const environment = (process.env.REACT_APP_ENVIRONMENT_NAME || 'local').toLowerCase();

    if (environment === 'local' || environment === 'development') {
        if (typeof window === 'undefined') {
            //const https = require('https');
            //process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
            //agent = axios.create({
            //    httpsAgent: new https.Agent({
            //        rejectUnauthorized: false
            //   })
            //});
            agent = axios.create();
        } else {
            agent = axios.create();
        }
    } else {
        agent = axios.create();
    }
    return agent;
}