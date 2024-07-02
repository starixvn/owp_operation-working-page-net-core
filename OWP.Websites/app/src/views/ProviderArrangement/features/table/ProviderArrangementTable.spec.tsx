import '@testing-library/jest-dom';
import { render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import { ProviderData } from '../TypeNative';
import ProviderArrangementTable from './ProviderArrangementTable';

jest.mock('react', () => ({
  ...(jest.requireActual('react') as object),
  useState: jest.fn(),
  useCallback: jest.fn()
}));
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
const mockData: ProviderData[] = [
  { "id": 1, "value": "RB2", "order": 1 },
  { "id": 5, "value": "BET", "order": 2 },
  { "id": 6, "value": "FC", "order": 3 },
  { "id": 4, "value": "RTS", "order": 4 },
  { "id": 2, "value": "BG", "order": 5 },
  { "id": 3, "value": "SM", "order": 6 }
];

const setUp = (isLoading: boolean = false, isEdit: boolean = false) => {
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn({
      providerReducer: {
        isLoading: isLoading,
        isEdit: isEdit,
        originalProviders: mockData,
        providers: mockData
      }
    })
  );
};

describe('test ProviderArrangementTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('loading=true, should render Loading', () => {
    setUp(true, false);
    const { container } = render(<ProviderArrangementTable />);
    const loadingElement = container.querySelector('.loading-process');
    expect(loadingElement).toBeInTheDocument();
  });

  it('loading=false, should render provider table', () => {
    setUp(false, false);
    const { container } = render(<ProviderArrangementTable />);
    const element = container.querySelector('.provider-table');
    expect(element).toBeInTheDocument();
    expect(mockDispatch).toBeCalledTimes(1);
  });
});
