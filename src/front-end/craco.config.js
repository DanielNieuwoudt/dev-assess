const NodePolyfillPlugin = require("node-polyfill-webpack-plugin");

module.exports = {
  webpack: {
    plugins: [
      new NodePolyfillPlugin({
        excludeAliases: ["console"]
      })
    ],    
    configure: (webpackConfig) => {
      webpackConfig.resolve.fallback = {
        ...webpackConfig.resolve.fallback,
        http: require.resolve('stream-http'),
        https: require.resolve('https-browserify'),
        url: require.resolve('url/'),
        buffer: require.resolve('buffer/')
      };
      return webpackConfig;
    },
  },
};