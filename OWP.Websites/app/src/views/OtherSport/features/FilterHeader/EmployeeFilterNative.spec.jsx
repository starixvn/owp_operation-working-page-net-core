import { setImmediate } from 'timers';
import EmployeeFilterNative from './EmployeeFilterNative';
import { render, fireEvent } from '@testing-library/react';

const onLoadEmployeeDone = jest.fn();

describe('EmployeeFilterNative under test', () => {
  it('should render correct ', () => {
    const { getByText } = render(
      <EmployeeFilterNative onLoadEmployeeDone={onLoadEmployeeDone} />
    );
    expect(getByText('Employee')).toMatchSnapshot();
  });

  it('should render icon active by default', () => {
    const { container } = render(
      <EmployeeFilterNative onLoadEmployeeDone={onLoadEmployeeDone} />
    );
    const employeeIcon = container.querySelectorAll('.emp-icon-activated');
    expect(employeeIcon).toHaveLength(1);
  });

  it('should render icon un-active when click icon', () => {
    const ref = {
      current: {}
    };
    const { container } = render(
      <EmployeeFilterNative ref={ref} onLoadEmployeeDone={onLoadEmployeeDone} />
    );
    const employeeIcon = container.querySelector('.emp-icon-activated');

    fireEvent.click(employeeIcon);

    expect(onLoadEmployeeDone).toHaveBeenCalled();
  });
});
