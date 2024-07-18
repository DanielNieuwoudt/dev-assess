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

  let environment = process.env.ENVIRONMENT_NAME || 'local';
  console.log(`The environment=${environment}`)

  const configMap: { [key: string]: () => Promise<any> } = {
    local: () => import('./config.local'),
    development: () => import('./config.development')
  };

  config = (await configMap[environment]()).config;

})();