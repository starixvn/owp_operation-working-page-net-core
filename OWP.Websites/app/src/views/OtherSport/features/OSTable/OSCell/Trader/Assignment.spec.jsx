import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import Assignment from './Assignment';

const props = (assignmentNote, mainTrader) => {
  return {
    osMatchId: 19,
    betTypeName: '1H (HDP)',
    mainTrader: mainTrader,
    assignmentNote: assignmentNote,
    rowIndex: 1
  };
};

describe('Assignment under test', () => {
  it('should render main-trader', () => {
    const { container } = render(
      <Assignment {...props('#esther noted', 'esther')} />
    );
    expect(container.querySelector('span.main-trader').textContent).toEqual(
      'esther   '
    );
  });

  it('should render other-trade', () => {
    const { container } = render(
      <Assignment {...props('#esther noted', 'esther')} />
    );
    expect(container.querySelector('span.other-trader').textContent).toEqual(
      'esther noted'
    );
  });
});
