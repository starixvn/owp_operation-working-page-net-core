import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import Filter from './FilterNative';
import Service from '../ResourceManagement/Service';
import Utils from '../Shared/Utils';
import dayjs from 'dayjs';
import Toastr from '../components/ToastrWrapper';

jest.mock('../ResourceManagement/Service');
jest.mock('../Shared/Utils');
jest.mock('../components/ToastrWrapper');

Toastr.showError = jest.fn(() => 'Error');
const responseData = [
  { Id: 1, Value: 'Live', RefName: 'MatchStatus' },
  { Id: 2, Value: 'In-play', RefName: 'MatchStatus' },
  { Id: 3, Value: 'Completed', RefName: 'MatchStatus' },
  { Id: 4, Value: 'Cancel', RefName: 'MatchStatus' },
  { Id: 5, Value: 'Postponed', RefName: 'MatchStatus' },
  { Id: 6, Value: 'Suspended', RefName: 'MatchStatus' }
];

describe('Filter', () => {
  const onSubmit = jest.fn();
  const onChangeDate = jest.fn();
  const onExport = jest.fn();
  beforeEach(() => {
    onSubmit.mockReset();
    Toastr.showError.mockReset();
  });
  const defaultProps = {
    submit: onSubmit,
    onChangeDate: onChangeDate,
    matchStatuses: [
      { RefName: 'MatchStatus', Id: 4, Value: 'Cancel' },
      { RefName: 'MatchStatus', Id: 3, Value: 'Completed' },
      { RefName: 'MatchStatus', Id: 2, Value: 'In-play' },
      { RefName: 'MatchStatus', Id: 1, Value: 'Live' },
      { RefName: 'MatchStatus', Id: 5, Value: 'Postponed' },
      { RefName: 'MatchStatus', Id: 6, Value: 'Suspended' }
    ],
    export: onExport
  };
  Utils.buildMatchStatusOption.mockReturnValue([
    { label: 'In-play', value: 2 },
    { label: 'Live', value: 1 }
  ]);
  Utils.buildSelectedMatchStatus.mockReturnValue([1, 2]);

  it('should load current date on init', () => {
    render(<Filter {...defaultProps} />);
    expect(screen.getByText('Submit'));
    expect(screen.getByText('Exclude Special Products'));
    expect(screen.getByText('Export'));
    expect(screen.getByText('All items are selected.'));
    expect(screen.getByText('Match Status'));
  });

  it('should invoke submit when component loaded', done => {
    Service.getByRefName.mockReturnValue(responseData);
    render(<Filter {...defaultProps} />);
    process.nextTick(() => {
      expect(onSubmit).toBeCalledTimes(1);
      done();
    });
  });

  it('should open calendar when click on icon', () => {
    const { container } = render(<Filter {...defaultProps} />);
    const icon = container.querySelector('.icon-calendar');
    waitFor(() => {
      fireEvent.click(icon);
    });
    expect(icon).toBeInTheDocument();
    expect(screen.getByText('Previous Month')).toBeInTheDocument();
  });

  it('should display Match Status when component loaded', () => {
    const { container } = render(<Filter {...defaultProps} />);
    expect(container.querySelector('.multiselect')).toBeInTheDocument();
  });

  it('should update state when click Export ', () => {
    const { container } = render(<Filter {...defaultProps} />);
    const icon = container.querySelector('.button-export');

    waitFor(() => {
      fireEvent.click(icon);
    });

    expect(icon).toBeInTheDocument();
    expect(onExport).toBeCalledTimes(1);
  });

  it('should update state when select match status', () => {
    const { container } = render(<Filter {...defaultProps} />);
    const icon = container.querySelector('.button-submit');
    fireEvent.click(container.querySelector('.dropdown-heading-value'));
    fireEvent.click(screen.getByText('Live'));
    fireEvent.click(screen.queryByText('Submit'));

    expect(icon).toBeInTheDocument();
    expect(onSubmit).toBeCalledTimes(2);
  });

  it('should update state when change date', async () => {
    onSubmit.mockReset();
    const { container } = render(<Filter {...defaultProps} />);
    const datePicker = container
      .querySelector('.react-datepicker__input-container')
      .querySelector('input');
    const newDate = new Date(2020, 1, 1);
    fireEvent.change(datePicker, { target: { value: newDate } });
    const selectedDate = dayjs(
      new Date(datePicker.getAttribute('value'))
    ).format('MM/DD/YYYY');

    expect(selectedDate).toEqual(dayjs(newDate).format('MM/DD/YYYY'));
  });

  it('should update state when change league', () => {
    onSubmit.mockReset();
    const { container } = render(<Filter {...defaultProps} />);
    const input = container.querySelector('.input-field');

    fireEvent.change(input, { target: { value: 'Liam' } });
    fireEvent.focusOut(input);
    expect(input).toBeInTheDocument();
  });

  it('should update state when call showError ', async () => {
    Utils.buildSelectedMatchStatus.mockReturnValue([]);
    const { container } = render(<Filter {...defaultProps} />);
    const icon = container.querySelector('.button-submit');
    fireEvent.click(container.querySelector('.dropdown-heading-value'));
    fireEvent.click(screen.getByText('Select All'));
    fireEvent.click(screen.queryByText('Submit'));

    expect(icon).toBeInTheDocument();
    expect(Toastr.showError).toBeCalledTimes(1);
  });
});
