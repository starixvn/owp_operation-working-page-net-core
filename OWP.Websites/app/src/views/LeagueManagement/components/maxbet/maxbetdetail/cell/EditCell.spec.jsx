import Router from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import EditCell from './EditCell';
import '@testing-library/jest-dom';

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));
jest.spyOn(Router, 'useParams').mockReturnValue({ id: 1 });

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    conditionReducer: {
      conditionPending: false,
      leagueSettingId: 123,
      tv: {
        hasTV: true,
        tv: 'abc'
      },
      round: [],
      division: []
    }
  })
);

describe('test EditCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  const props = {
    leagueSettingId: 11
  };
  it('render correct', () => {
    const { container } = render(<EditCell {...props} />);

    expect(
      container.querySelector('.fixedDataTableCellLayout_wrap1')
    ).toBeInTheDocument();
  });

  it('should render Condition component correct when click EditIcon', async () => {
    const { container } = render(<EditCell {...props} />);

    const editIcon = container.querySelector('.icon-edit');

    fireEvent.click(editIcon);

    expect(screen.getAllByText('Has TV')[0]).toBeInTheDocument();

    const buttonClose = screen.getByText('Cancel');
    fireEvent.click(buttonClose);
    const newButton = screen.queryByText('Cancel');
    expect(newButton).toBeNull();
  });
});
