import FormTradingInfo from './FormTradingInfo';
import MatchInfoService from './Service';
import Toastr from '../../components/ToastrWrapper';
import { act, fireEvent, render, screen } from '@testing-library/react';

jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');
jest.mock('../../components/Spinner');

const onSaveMock = jest.fn();
const onCancel = jest.fn();
const onEdit = jest.fn();

const promise = Promise.resolve();

describe('FormTradingInfo', () => {
  const props = {
    noOfHDPOptions: [
      { value: 1, label: '1HDP' },
      { value: 2, label: '2HDP' }
    ],
    matchOfferOptions: [
      { value: 1, label: '10s Accept' },
      { value: 2, label: '20s Accept' }
    ],
    data: {
      OWPMatchId: 1,
      MatchOffers: [{ value: 1, label: '10s Accept' }],
      NoOfHDP: { value: 1, label: '1HDP' }
    },
    policy: {
      HasRead: true,
      HasWrite: true
    },
    onSave: onSaveMock,
    onCancel,
    onEdit
  };

  it('should match snapshot', () => {
    const { container } = render(<FormTradingInfo {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should update state when change match offers', () => {
    const { container, getByText } = render(<FormTradingInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));
    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), { key: 'ArrowDown', code: 40 });

    fireEvent.click(getByText('20s Accept'));

    expect(getByText('20s Accept')).toBeInTheDocument();
    expect(getByText('10s Accept')).toBeInTheDocument();
  });

  it('should allow to edit', () => {
    const { container } = render(<FormTradingInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));

    expect(container.firstChild).toMatchSnapshot();
  });

  it('should allow to cancel editing & reset model', () => {
    const { container } = render(<FormTradingInfo {...props} />);

    fireEvent.click(container.querySelector('.icon-edit'));
    fireEvent.click(container.querySelector('.icon-cancel'));
    expect(container.firstChild).toMatchSnapshot();
  });

  describe('saving confirmation', () => {
    beforeEach(async () => {
      MatchInfoService.updateTradingInfo.mockClear();
      Toastr.confirm.mockClear();
      Toastr.showInfo.mockClear();
      Toastr.showError.mockClear();
      onSaveMock.mockClear();
    });

    it('should trigger to update with case of success when accept confirmation', async () => {
      Toastr.confirm.mockResolvedValue({ value: 1 });
      MatchInfoService.updateTradingInfo.mockResolvedValue({
        IsSuccess: true,
        Message: 'Updated'
      });
      const { container } = render(<FormTradingInfo {...props} />);

      fireEvent.click(container.querySelector('.icon-edit'));

      fireEvent.click(container.querySelector('.icon-save'));

      await act(async () => await promise);
      expect(MatchInfoService.updateTradingInfo).toBeCalledWith({ "MatchOfferIds": "1", "NoOfHDPId": 1, "OWPMatchId": 1 });
      expect(Toastr.showError).toBeCalledTimes(0);
      expect(Toastr.showInfo).toBeCalledTimes(1);
      expect(onSaveMock).toBeCalledWith({ "MatchOffers": [{ "label": "10s Accept", "value": 1 }], "NoOfHDP": { "label": "1HDP", "value": 1 } });
    });
  })
});
