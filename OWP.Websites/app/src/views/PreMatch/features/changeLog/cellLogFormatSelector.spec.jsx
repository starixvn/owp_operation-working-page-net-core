import { render } from '@testing-library/react';
import { logAction } from '../league/constants/preMatchConst';
import cellLogFormatSelector from './cellLogFormatSelector';
import changeLogColumn from './changeLogColumn';

describe('cellLogFormatSelector under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should handel OldValue column', () => {
    const { container } = render(
      cellLogFormatSelector(
        changeLogColumn.Columns[2].id,
        'valueText',
        'action'
      )
    );
    const element = container.querySelector('.nomalcell');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('valueText');
  });
  it('should handel NewValue column', () => {
    const { container } = render(
      cellLogFormatSelector(
        changeLogColumn.Columns[3].id,
        'valueText',
        'action'
      )
    );
    const element = container.querySelector('.nomalcell');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('valueText');
  });
  it('should handel NewValue column, when update Assignment', () => {
    const { container } = render(
      cellLogFormatSelector(
        changeLogColumn.Columns[3].id,
        'valueText',
        logAction.updateAssignment
      )
    );
    const element = container.querySelector('.text-left');
    expect(element).toBeInTheDocument();
  });

  it('should handel another column', () => {
    const { container } = render(
      cellLogFormatSelector('another column', 'valueText', 'action')
    );
    const element = container.querySelector('.text-left');
    expect(element).toBeInTheDocument();
    expect(element.textContent).toEqual('valueText');
  });
});
