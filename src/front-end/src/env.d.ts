interface Window {
    env: {
        REACT_APP_ENVIRONMENT_NAME: string;
        [key: string]: string | undefined;
    };
}