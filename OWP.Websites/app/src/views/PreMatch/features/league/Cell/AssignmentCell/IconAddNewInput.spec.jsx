import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import IconAddNewInput from './IconAddNewInput';
import { PlusCircleIcon } from 'mdi-react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
const handleShowHideIconPlus = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn => selectorFn({}));

const createProps = (hdp, otherHdp) => {
  const dataSource = [
    {
      HDP: hdp,
      OtherHDPNote: otherHdp
    }
  ];
  const rowIndex = 0;
  const columnId = 'HDP';
  return { dataSource, rowIndex, columnId };
};

describe('IconAddNewInput under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('when have assignment and not have otherAssignment, should render PlusCircleIcon', () => {
    const props = createProps('esther', '');
    const { container } = render(<IconAddNewInput {...props} />);
    const element = container.querySelector('.btn-add');
    expect(element).toBeInTheDocument();
  });

  it('click Plus button, should call back', () => {
    const props = createProps('esther', '');
    const { container } = render(
      <IconAddNewInput
        {...props}
        handleShowHideIconPlus={handleShowHideIconPlus}
      />
    );
    const button = container.querySelector('.btn-add');
    fireEvent.click(button);
    expect(handleShowHideIconPlus).toHaveBeenCalledTimes(1);
  });

  it('when note have assignment, should not render PlusCircleIcon', () => {
    const props = createProps('', '');
    const { container } = render(<IconAddNewInput {...props} />);
    const element = container.querySelector('.btn-add');
    expect(element).toBeNull();
  });

  it('when note have assignment and have otherAssignment, should not render PlusCircleIcon', () => {
    const props = createProps('', 'esther test');
    const { container } = render(<IconAddNewInput {...props} />);
    const element = container.querySelector('.btn-add');
    expect(element).toBeNull();
  });
});
