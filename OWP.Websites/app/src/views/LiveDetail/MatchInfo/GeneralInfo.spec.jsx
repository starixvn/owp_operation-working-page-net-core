import GeneralInfo from './GeneralInfo';
import Toastr from '../../components/ToastrWrapper';
import { fireEvent, render, screen } from '@testing-library/react';

jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');
jest.mock('../../components/Spinner');

const onSaveMock = jest.fn();
const onEdit = jest.fn();
const onCancel = jest.fn();
Toastr.showError = jest.fn();
Toastr.confirm = jest.fn();

const mainMatch = {
  OWPMatchId: 1,
  MatchOffers: [
    { value: 1, label: 'KO+15' },
    { value: 2, label: 'Fast Market' }
  ],
  OtherTime: '08:00',
  LeagueName: 'L',
  HomeName: 'H',
  AwayName: 'A',
  MatchNo: 'ABC',
  EventDateDisplay: 'Jun 16, 2018',
  KickOffTimeDisplay: '9:00 AM'
};

const minorMatch = {
  OWPMatchId: 2,
  ParentMatchId: 1,
  MatchOffers: [],
  OtherTime: '18:00',
  LeagueName: 'L - BOOKING',
  HomeName: 'H Next Booking',
  AwayName: 'A Next Booking',
  MatchNo: '',
  EventDateDisplay: 'Jun 16, 2018',
  KickOffTimeDisplay: '9:00 PM'
};

const props = {
  policy: {
    HasRead: true,
    HasWrite: true
  },
  onSave: onSaveMock,
  onEdit,
  onCancel
};

describe('GeneralInfo', () => {
  it('should allow to edit', () => {
    const matchProps = {
      ...props,
      data: mainMatch
    };

    const { container, getByTestId, getByPlaceholderText } = render(
      <GeneralInfo {...matchProps} />
    );

    fireEvent.click(getByTestId('edit-testId'));

    expect(container.querySelector('.match-no')).toBeInTheDocument();
    expect(getByPlaceholderText('Select time')).toBeInTheDocument();
    expect(getByPlaceholderText('League Name')).toBeInTheDocument();
    expect(getByPlaceholderText('Home Team')).toBeInTheDocument();
    expect(getByPlaceholderText('Away Team')).toBeInTheDocument();
  });

  describe('Main match', () => {
    let component;
    let editableButtonGroup;
    let matchProps;
    beforeEach(() => {
      matchProps = {
        ...props,
        data: mainMatch
      };
    });

    it('should render FlagWrapper', async () => {
      const { container, getByTestId } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      expect(container.firstChild).toMatchSnapshot();
    });

    it('should update state when change league name', async () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { getByTestId, getByPlaceholderText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      fireEvent.change(getByPlaceholderText('League Name'), {
        target: {
          value: 'League 1'
        }
      });

      expect(getByPlaceholderText('League Name').value).toEqual('League 1');
    });

    it('should update state when change match no', async () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { container, getByTestId, getByPlaceholderText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      fireEvent.change(container.querySelector('.match-no'), {
        target: {
          value: '9999'
        }
      });

      expect(container.querySelector('.match-no').value).toEqual('9999');
    });

    it('should update state when change home name', async () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { getByTestId, getByPlaceholderText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      fireEvent.change(getByPlaceholderText('Home Team'), {
        target: {
          value: 'Home 1'
        }
      });

      expect(getByPlaceholderText('Home Team').value).toEqual('Home 1');
    });

    it('should update state when change away name', async () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { getByTestId, getByPlaceholderText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      fireEvent.change(getByPlaceholderText('Away Team'), {
        target: {
          value: 'Away 1'
        }
      });

      expect(getByPlaceholderText('Away Team').value).toEqual('Away 1');
    });

    it('should allow to cancel editing & reset model', () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { container, getByTestId, getByText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));
      fireEvent.click(container.querySelector('.icon-cancel'));

      expect(getByText('Jun 16, 2018')).toBeInTheDocument();
      expect(getByText('ABC')).toBeInTheDocument();
      expect(getByText('9:00 AM')).toBeInTheDocument();
      expect(getByText('8:00 AM')).toBeInTheDocument();
      expect(getByText('H -vs- A')).toBeInTheDocument();
    });

    it('show error when saving if input is invalid', () => {
      const matchProps = {
        ...props,
        data: mainMatch
      };

      const { container, getByTestId, getByPlaceholderText } = render(
        <GeneralInfo {...matchProps} />
      );

      fireEvent.click(getByTestId('edit-testId'));

      fireEvent.change(getByPlaceholderText('Away Team'), {
        target: {
          value: ''
        }
      });
      fireEvent.click(container.querySelector('.icon-save'));

      expect(Toastr.showError).toBeCalled();
    });
  });
});
