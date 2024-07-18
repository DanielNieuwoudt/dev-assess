import { BaseConfig } from './base';

class DevelopmentConfig extends BaseConfig {
  backendBaseUrl: string  = 'http://backend:5000/api';
}

export const config = new DevelopmentConfig();
