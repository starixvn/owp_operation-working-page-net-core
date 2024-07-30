import { fireEvent, render } from '@testing-library/react';
import WorkingShiftTable from './WorkingShiftTable';

describe('WorkingShiftTable under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render `There is no data`, when not have workingShiftData', () => {
    const { container } = render(<WorkingShiftTable workingShiftData={[]} />);
    const element = container.querySelector('.no-data');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('There is no data');
  });
  it('should render 1 table`, when have workingShiftData', () => {
    const { container } = render(
      <WorkingShiftTable workingShiftData={[1, 2, 3]} />
    );
    const element = container.querySelector('.table-content');
    expect(element).toBeInTheDocument();
  });
});
