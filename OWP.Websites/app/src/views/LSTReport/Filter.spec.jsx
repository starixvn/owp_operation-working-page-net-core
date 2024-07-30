import { fireEvent, render, waitFor } from '@testing-library/react';
import Filter from './Filter';
import dayjs from 'dayjs';

const mockOnSubmit = jest.fn();
const mockOnExport = jest.fn();
const mockOnShowColumnChange = jest.fn();
const defaultProps = (
  show = true,
  assignmentRead = true,
  perfornmanceRead = true
) => {
  return {
    onSubmit: mockOnSubmit,
    onExport: mockOnExport,
    onShowColumnChange: mockOnShowColumnChange,
    canShowHideColumn: show,
    policy: {
      userName: 'sa',
      traderAssignment: {
        HasRead: assignmentRead
      },
      traderPerformace: {
        HasRead: perfornmanceRead
      }
    },
    serverTime: '2022-06-17'
  };
};

describe('FilterHeader', () => {
  beforeEach(() => {
    jest.useFakeTimers('legacy');
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should not render ToggleColumn when canShowHideColumn=false', () => {
    const { container } = render(<Filter {...defaultProps(false)} />);
    const component = container.querySelector('.btn-block--right');
    expect(component).toBeNull();
  });

  it('should render ToggleColumn when canShowHideColumn=true', () => {
    const { container } = render(<Filter {...defaultProps(true, false)} />);
    const component = container.querySelector('.btn-block--right');
    expect(component).toBeInTheDocument();
  });

  it('should change value when change from date', () => {
    const { container } = render(
      <Filter {...defaultProps(true, true, false)} />
    );
    const component = container.querySelector('.from-date');
    const newDate = '2020-05-24';
    fireEvent.change(component, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(component.getAttribute('value'))
    ).format('MM/DD/YYYY');

    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('should change value when change to date', () => {
    const { container } = render(<Filter {...defaultProps()} />);
    const component = container.querySelector('.to-date');
    const newDate = new Date(2020, 1, 1);
    fireEvent.change(component, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(component.getAttribute('value'))
    ).format('MM/DD/YYYY');

    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('test coverage, when click calendar of date', async () => {
    const { container } = render(<Filter {...defaultProps()} />);

    await waitFor(() => {
      const component = container.querySelectorAll('.icon-calendar');
      expect(fireEvent.click(component[0])).toBeTruthy();
      expect(fireEvent.click(component[1])).toBeTruthy();
    });
  });

  it('test coverage, when click Search button', async () => {
    const { getByText } = render(<Filter {...defaultProps()} />);

    await waitFor(() => {
      const button = getByText('Search');
      expect(button).toBeInTheDocument();
      fireEvent.click(button);
    });
  });

  it('test coverage, when click Export button', async () => {
    const { getByText } = render(<Filter {...defaultProps()} />);

    await waitFor(() => {
      const button = getByText('Export');
      expect(button).toBeInTheDocument();
      fireEvent.click(button);
    });
  });
});
