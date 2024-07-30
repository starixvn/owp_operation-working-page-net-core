import { buildRequestURL, formatFractionDigit } from './helpers';

global = Object.create(window);
Object.defineProperty(window, 'netCoreRootUrl', {
  value: 'v1/test'
});

describe('helpers under test', () => {
  afterEach(() => {
    delete process.env.NODE_ENV;
    jest.clearAllMocks();
  });

  it('env!=development, should return window.netCoreRootUrl url', () => {
    process.env.NODE_ENV = 'test';
    const devURL = buildRequestURL('key');
    expect(devURL).toEqual('/api/OvertimeReport/key');
  });

  it('env=development, should return netCoreURL url', () => {
    process.env.NODE_ENV = 'development';
    const devURL = buildRequestURL('key');
    expect(devURL).toEqual('/api/OvertimeReport/key');
  });

  it('inputValue=null, should return -', () => {
    const value = formatFractionDigit(null, 2);
    expect(value).toEqual('-');
  });

  it('inputValue=number, should return format number', () => {
    const value = formatFractionDigit(3.5, 2);
    expect(value).toEqual('3.50');
  });
});
