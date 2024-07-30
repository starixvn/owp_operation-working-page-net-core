import { queryServerTime, queryData } from '../Shared/Service/ServiceCommon';
import service from './Service';

jest.mock('../Shared/Service/ServiceCommon', () => ({
  queryServerTime: jest.fn(),
  queryData: jest.fn()
}));

const queryServerTimeMock = jest.fn();
const queryDataMock = jest.fn();
const setup = () => {
  queryServerTime.mockReturnValue(queryServerTimeMock);
  queryDataMock.mockReturnValue(queryDataMock);
};
describe('QuickSearch service test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should call func queryData, when service.query excute', async () => {
    setup();
    const model = { id: 33, text: 'abc' };
    await service.query(model);
    expect(queryData).toBeCalledTimes(1);
  });
});
