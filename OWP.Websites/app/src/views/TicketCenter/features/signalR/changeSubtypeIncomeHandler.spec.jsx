import store from '../../app/storeNative';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
import {
  checkAllFilterCondition,
  handleCorrectFilter,
  handleInCorrectFilter
} from './signalRHelper';


jest.mock('./signalRHelper', () => ({
  checkAllFilterCondition: jest.fn(),
  handleCorrectFilter: jest.fn(),
  handleInCorrectFilter: jest.fn(),
}));


jest.mock('../../app/storeNative');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));

store.getState = () => ({
  chatBoard:{
    isOpenPopup: true
  },
  signalR:{
    filterModel: {}
  },
  eventChecking:{
    entities:[]
  }
});


describe('changeStatusIncomeHandler under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should have call handle correct filter func', () => {
    checkAllFilterCondition.mockReturnValue(true);
    changeSubTypeIncomeHandler();

    expect(handleCorrectFilter).toBeCalledTimes(1);
  });

  it('should have call handle incorrect filter func', () => {
    checkAllFilterCondition.mockReturnValue(false);
    changeSubTypeIncomeHandler();

    expect(handleInCorrectFilter).toBeCalledTimes(1);
  });
});
