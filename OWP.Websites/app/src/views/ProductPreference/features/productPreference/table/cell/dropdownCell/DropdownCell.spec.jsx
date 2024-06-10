import DropdownCell from './DropdownCell';
import { post } from '../../../../../../Shared/Service/Service';
import { fireEvent, render } from '@testing-library/react';

jest.mock('../../../../../../Shared/Service/Service');

const defaultProps = {
  options: [
    { value: 1, label: 'A' },
    { value: 2, label: 'B' }
  ],
  userId: 8,
  label: 'Level',
  selected: 2,
  hasWrite: true,
  actionName: 'delete',
  actionId: 123,
  actorId: 123,
  username: 'sa',
  actorName: 'sa'
};
describe('DropdownCell under test', () => {
  it('match snapshot', () => {
    const { container } = render(<DropdownCell {...defaultProps} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should call axios when onSelectedItem change', () => {
    const { getByTestId } = render(<DropdownCell {...defaultProps} />);
    fireEvent.change(getByTestId('select'), { target: { value: 1 } });
    expect(post).toBeCalled();
  });
});
