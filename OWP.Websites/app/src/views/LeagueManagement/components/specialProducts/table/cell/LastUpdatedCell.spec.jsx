import { render, fireEvent, screen } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';

import LastUpdatedCell from './LastUpdatedCell';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);

useSelector.mockImplementation(selectorFn =>
  selectorFn({
    logHistoryReducer: {
      Pending: false,
      Data: [
        {
          Product: '',
          Action: '',
          OldValue: 100,
          NewValue: 122,
          ModifyDate: '',
          ModifyBy: ''
        }
      ]
    }
  })
);

describe('LastUpdatedCell component under unit test', () => {
  it('render component correct', () => {
    const props = { productId: 10, maxbetFTHDP: 100, updatedBy: 'sa' };
    const { container, getByText } = render(<LastUpdatedCell {...props} />);
    expect(getByText('sa')).toBeInTheDocument();
  });

  it('render modal when click history icon', async () => {
    const props = { productId: 10, maxbetFTHDP: 100, updatedBy: 'sa' };
    const { container, getByText } = render(<LastUpdatedCell {...props} />);
    const historyIcon = container.querySelector('.loghistory-button');

    fireEvent.click(historyIcon);

    const modal = await screen.getByText('Action');
    expect(modal).toBeInTheDocument();
  });
});
