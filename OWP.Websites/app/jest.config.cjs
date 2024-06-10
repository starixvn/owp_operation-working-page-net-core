module.exports = {
  'roots': ['<rootDir>'],
  'collectCoverageFrom': [
    'src/**/*.{js,jsx,mjs,ts,tsx}',
    '!src/apps/**/*.{js,jsx,mjs,ts,tsx}'
  ],
  'testMatch': ['<rootDir>/src/**/*.spec.{js,jsx,mjs,ts,tsx}'],
  'testEnvironment': 'jsdom',
  'testEnvironmentOptions': {
    'url': 'http://localhost'
  },
  'transform': {
    '^.+\\.(js|jsx|tsx|ts|cjs)$': 'babel-jest',
    '^.+\\.svg$': '<rootDir>/config/jest/svgTransform.cjs'
  },
  'setupFilesAfterEnv': ['<rootDir>/config/jest/testSetup.cjs'],
  'transformIgnorePatterns': [
    '[/\\\\]node_modules[/\\\\].+\\.(js|jsx|mjs|ts|tsx)$'
  ],
  'moduleNameMapper': {
    '^react-native$': 'react-native-web',
    '^uuid$': '<rootDir>/node_modules/uuid/dist/index.js',
    '\\.(css|less|sass|scss)$': '<rootDir>/config/jest/cssTransform.cjs'
  },
  'moduleFileExtensions': [
    'web.js',
    'js',
    'json',
    'web.jsx',
    'jsx',
    'ts',
    'tsx',
    'node',
    'mjs',
    '*.css'
  ],
  'snapshotSerializers': [],
  'testResultsProcessor': 'jest-sonar-reporter',
  'coverageReporters': [
    [
      'lcov',
      {
        'projectRoot': '<rootDir>'
      }
    ],
    'text-summary'
  ],
  'coverageDirectory': '<rootDir>'
};
