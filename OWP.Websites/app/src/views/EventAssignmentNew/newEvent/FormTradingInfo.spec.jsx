import { fireEvent, render, screen } from '@testing-library/react';
import Converter from '../Shared/Converter';
import FormTradingInfo from './FormTradingInfo';
jest.mock('../Shared/Converter');

const matchOffer = [
  { value: 1, label: 'match offer 1' },
  { value: 2, label: 'match offer 2' },
  { value: 3, label: 'match offer 3' },
  { value: 4, label: 'match offer 4' },
  { value: 5, label: 'match offer 5' }
];

const noHDP = [
  { value: 1, label: 'no hdp 1' },
  { value: 2, label: 'no hdp 2' },
  { value: 3, label: 'no hdp 3' },
  { value: 4, label: 'no hdp 4' },
  { value: 5, label: 'no hdp 5' }
];

Converter.convertMatchOffersToOptions.mockReturnValue([
  { value: 1, label: 'match offer 1' },
  { value: 2, label: 'match offer 2' },
  { value: 3, label: 'match offer 3' },
  { value: 4, label: 'match offer 4' },
  { value: 5, label: 'match offer 5' }
]);

Converter.convertBettypesToOptions.mockReturnValue([
  { value: 1, label: 'HDP' },
  { value: 2, label: 'O/U' },
]);

Converter.convertNoOfHDPToOptions.mockReturnValue([
  { value: 1, label: 'no hdp 1' },
  { value: 2, label: 'no hdp 2' },
  { value: 3, label: 'no hdp 3' },
  { value: 4, label: 'no hdp 4' },
  { value: 5, label: 'no hdp 5' }
]);
describe('<FormTradingInfo/>', () => {
  const props = {
    matchOffer: matchOffer,
    noHDP: noHDP
  };

  it('update state when change matchOffer', () => {
    const { container, getByText } = render(<FormTradingInfo {...props} />);

    fireEvent.focus(container.querySelectorAll('input')[1]);
    fireEvent.keyDown(container.querySelectorAll('input')[1], { key: 'ArrowDown', code: 40 });
    fireEvent.click(getByText('match offer 2'));

    expect(getByText('match offer 2')).toBeInTheDocument();
  });

  it('update state when change no.HDP', () => {
    const { container, getByText } = render(<FormTradingInfo {...props} />);

    fireEvent.focus(container.querySelectorAll('input')[2]);
    fireEvent.keyDown(container.querySelectorAll('input')[2], { key: 'ArrowDown', code: 40 });
    fireEvent.click(getByText('no hdp 4'));

    expect(getByText('no hdp 4')).toBeInTheDocument();
  });
});
