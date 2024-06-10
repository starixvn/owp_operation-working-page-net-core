import Spinner from '../../../components/Spinner';
import { noop } from 'lodash';
import { fireEvent, render } from '@testing-library/react';
import NoCell from './NoCell';
import store from '../../app/store';
import {
  buildNoOfferChildMatchInfo,
  buildNoOfferMainMatchInfo
} from '../../helper/noOfferSingleMatchPipeline';
import { useDispatch } from 'react-redux';

jest.mock('../../../components/Spinner');
jest.mock('../../helper/noOfferSingleMatchPipeline');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
jest.mock('../../app/store');

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
store.getState = () => ({
  pagePolicy: {
    permission: {
      HasWrite: true
    }
  }
});

Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

describe('NoCell under test', () => {
  afterAll(() => {
    jest.resetAllMocks();
  });

  it('Should call buildNoOfferChildMatchInfo', () => {
    const props = {
      rowIndex: 1,
      dataSource: [{
        infoSiteMatchId: 10,
        isChildMatch: false,
        adminSiteMatchId: 10
      },
      {
        infoSiteMatchId: 10,
        isChildMatch: true,
        adminSiteMatchId: 111
      }]
    }
    const { container } = render(<NoCell {...props} />);
    const element = container.querySelector('Input');
    fireEvent.click(element);
    expect(buildNoOfferChildMatchInfo).toBeCalled();
  });

  it('Should call buildNoOfferMainMatchInfo', () => {
    const props = {
      rowIndex: 0,
      dataSource: [{
        infoSiteMatchId: 10,
        isChildMatch: false,
        adminSiteMatchId: 10
      },
      {
        infoSiteMatchId: 10,
        isChildMatch: true,
        adminSiteMatchId: 111
      }]
    }
    const { container } = render(<NoCell {...props} />);
    const element = container.querySelector('Input');
    fireEvent.click(element);
    expect(buildNoOfferMainMatchInfo).toBeCalled();
  });
});