const path = require('path');
const fs = require('fs');
const url = require('url');

// Make sure any symlinks in the project folder are resolved:
// https://github.com/facebookincubator/create-react-app/issues/637
const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

const envPublicUrl = process.env.PUBLIC_URL;

const ensureSlash = (path, needsSlash) => {
  const hasSlash = path.endsWith('/');
  if (hasSlash && !needsSlash) {
    return path.substr(path, path.length - 1);
  } else if (!hasSlash && needsSlash) {
    return `${path}/`;
  } else {
    return path;
  }
};

const getPublicUrl = appPackageJson =>
  envPublicUrl || require(appPackageJson).homepage;

// We use `PUBLIC_URL` environment variable or "homepage" field to infer
// "public path" at which the app is served.
// Webpack needs to know it to put the right <script> hrefs into HTML even in
// single-page apps that may serve index.html for nested URLs like /todos/42.
// We can't use a relative path in HTML because we don't want to load something
// like /todos/42/static/js/bundle.7289d.js. We have to know the root.
const getServedPath = appPackageJson => {
  const publicUrl = getPublicUrl(appPackageJson);
  const servedUrl =
    envPublicUrl || (publicUrl ? url.parse(publicUrl).pathname : '/');
  return ensureSlash(servedUrl, true);
};

let app = '';
for (let i = 0; i < process.argv.length; i++) {
  const argv = process.argv[i];
  if (argv.indexOf('--app=') === 0) {
    app = argv.replace('--app=', '');
    break;
  }
}

if (!app && !process.argv[1].endsWith('test.cjs')) {
  throw new Error('missing argument --app');
}

const packageJson = resolveApp('package.json');

// config after eject: we're in ./config/
module.exports = {
  dotenv: resolveApp('.env'),
  appBuild: resolveApp('../wwwroot/' + app),
  appPublic: resolveApp('public'),
  appHtml: resolveApp('public/index.html'),
  serverHtml: resolveApp('public/server.html'),
  appIndexJs: resolveApp(`src/apps/${app}.jsx`),
  appPackageJson: packageJson,
  appSrc: resolveApp('src'),
  yarnLockFile: resolveApp('yarn.lock'),
  testsSetup: resolveApp('src/setupTests.js'),
  appNodeModules: resolveApp('node_modules'),
  publicUrl: getPublicUrl(packageJson),
  servedPath: getServedPath(packageJson),
  publicPath: `wwwroot/${app}/`
};
