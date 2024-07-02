import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import BettypeLabel from './BettypeLabel';

describe('BettypeLabel under test', () => {
  it('should render BettypeLabel', () => {
    const { container } = render(<BettypeLabel betTypeName='HDP' />);
    expect(container.querySelectorAll('.betTypeName').length).toEqual(1);
  });
});
