import { BaseConfig } from './base';

class LocalConfig extends BaseConfig {
    backendBaseUrl: string  = 'https://localhost:5001/api';
}

export const config = new LocalConfig();
