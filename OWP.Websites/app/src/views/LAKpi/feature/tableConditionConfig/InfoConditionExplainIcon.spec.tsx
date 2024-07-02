import { render } from '@testing-library/react';
import InfoConditionExplainIcon from './InfoConditionExplainIcon';

test('matches snapshot', () => {
  const { container } = render(<InfoConditionExplainIcon />);
  expect(container).toMatchSnapshot();
});
