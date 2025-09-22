import Spinner from '../../../components/Spinner';
import { noop } from 'lodash';
import { fireEvent, render } from '@testing-library/react';
import NoCell from './NoCell';
import store from '../../app/store';
import { useAppSelector } from '../../app/hooks';

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
  useStore: jest.fn(),
  createSelectorHook: jest.fn()
}));
jest.mock('../../app/store');
jest.mock('../../app/hooks', () => ({
  useAppSelector: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
useAppSelector.mockImplementation(selector => {
  const mockState = {
    eventPickingTable: {
      previousNoOfferIndex: 0,
      matchSelected: []
    }
  };
  return selector(mockState);
});
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

  it('Should render checkbox for main match', () => {
    const props = {
      rowIndex: 0,
      dataSource: [
        {
          infoSiteMatchId: 10,
          isChildMatch: false,
          adminSiteMatchId: 10
        },
        {
          infoSiteMatchId: 10,
          isChildMatch: true,
          adminSiteMatchId: 111
        }
      ]
    };
    const { container } = render(<NoCell {...props} />);
    const checkbox = container.querySelector('input[type="checkbox"]');
    expect(checkbox).toBeInTheDocument();
  });

  it('Should not render checkbox for child match', () => {
    const props = {
      rowIndex: 1,
      dataSource: [
        {
          infoSiteMatchId: 10,
          isChildMatch: false,
          adminSiteMatchId: 10
        },
        {
          infoSiteMatchId: 10,
          isChildMatch: true,
          adminSiteMatchId: 111
        }
      ]
    };
    const { container } = render(<NoCell {...props} />);
    const checkbox = container.querySelector('input[type="checkbox"]');
    expect(checkbox).not.toBeInTheDocument();
  });
});
