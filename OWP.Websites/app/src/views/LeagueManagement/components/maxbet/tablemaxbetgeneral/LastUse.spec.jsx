import { fireEvent, render } from '@testing-library/react';
import LastUse from './LastUse';

describe('test LastUser', () => {
  const props = {
    lastUseDate: '11-01-2022'
  };

  it('render correct component', () => {
    const { getByText } = render(<LastUse {...props} />);

    expect(getByText('11/01/2022')).toBeInTheDocument();
  });
});
