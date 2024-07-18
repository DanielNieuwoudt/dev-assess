import fs from 'fs';
import path from 'path';

export class BaseConfig implements IConfig {
  backendBaseUrl: string = '';
  isAuthenticationEnabled: boolean = false;
}

export interface IConfig{
  /**
   * The base url for the service under test
   */
  backendBaseUrl: string;
  isAuthenticationEnabled: boolean;
}
export let config: IConfig;
(async () => {
  let environment = process.env.ENVIRONMENT_NAME || 'local';

  const configEnvOverridesFile = `./config.${environment}`;
  if (!fs.existsSync(path.join(__dirname, configEnvOverridesFile + '.ts'))) {
    throw new Error(
      `No environment specific configuration for environment name '${environment}'`
    );
  }

  console.log(`Using config environment from ${configEnvOverridesFile}`);
  config = (await import(configEnvOverridesFile)).config;
})();