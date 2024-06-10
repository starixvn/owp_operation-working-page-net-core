import PopupDetail from './PopupDetail';
import useStableDispatch from './../../Shared/customHook/useStableDispatch';
import { render } from '@testing-library/react';
import { useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useRef: jest.fn()
}));

jest.mock('./../../Shared/customHook/useStableDispatch');

const state = {
  events: {
    popupState: {
      isOpen: true,
      matchId: 123
    },
    refreshModel: {
      filterDate: new Date(),
      statusId: '1'
    }
  }
};
const mockDispatch = jest.fn();
(useStableDispatch as jest.Mock).mockResolvedValue(mockDispatch);
(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({ ...state })
);
describe('PopupDetail', () => {
  it('show detail', () => {
    const { container } = render(<PopupDetail />);
    expect(container).toMatchSnapshot();
  });
});
