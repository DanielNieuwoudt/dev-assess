export class BaseConfig implements IConfig {
  backendBaseUrl: string = '';
  isAuthenticationEnabled: boolean = false;
}

export interface IConfig{
  backendBaseUrl: string;
  isAuthenticationEnabled: boolean;
}
export let config: IConfig;
(async () => {

  let environment = window.env ? window.env.REACT_APP_ENVIRONMENT_NAME : 'local';

  const configMap: { [key: string]: () => Promise<any> } = {
    local: () => import('./config.local'),
    development: () => import('./config.development')
  };

  config = (await configMap[environment]()).config;

  console.log(`Configuration: ${environment}`);
  console.log(`Configuration: ${config.backendBaseUrl}`);
  console.log(`Configuration: ${config.isAuthenticationEnabled}`);
  
})();