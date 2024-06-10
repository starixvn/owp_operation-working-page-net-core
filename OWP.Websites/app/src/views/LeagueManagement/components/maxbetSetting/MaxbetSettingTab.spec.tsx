
import { act, render } from '@testing-library/react';
import MaxbetSettingTab from './MaxbetSettingTab';
import useStore from './store/storeNative';

const mockQueryPolicy = jest.fn();
useStore.setState({
  queryPolicy: mockQueryPolicy,
});

describe('MaxbetSettingTab under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('Should call mockQueryPolicy', async () => {
    await act(async () => {
      render(<MaxbetSettingTab />)
    });

    expect(mockQueryPolicy).toBeCalled();
  });
});
