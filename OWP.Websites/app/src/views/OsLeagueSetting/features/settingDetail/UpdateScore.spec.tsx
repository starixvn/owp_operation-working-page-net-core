import { fireEvent, render } from '@testing-library/react';
import UpdateScore from './UpdateScore';
import { IUpdateScore } from '../../typing/type';

jest.mock('../../hook/useQueryDropdownOption');

const onChange = jest.fn();
const props: IUpdateScore = {
  isIptvOnly: false,
  isManualUpdateScore: false,
  disabled: false,
  onChange
};

describe('<UpdateScore/> under test', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('should call onchange when change selected', () => {
    const { getByTestId } = render(<UpdateScore {...props} />);
    fireEvent.click(getByTestId('update-score-testid'), { target: { value: true } })
    expect(props.onChange).toHaveBeenCalledTimes(1);
  });
});
