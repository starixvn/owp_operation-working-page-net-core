import { render } from '@testing-library/react';
import TooltipText from './TooltipText';

const props = {
  innerClassName: 'iptv',
  rowIndex: 0,
  toolTipContent: 'Euro40',
  text: 'IPTV'
};

describe('TooltipText under test', () => {
  it('should render TooltipText', () => {
    const { container } = render(<TooltipText {...props} />);
    expect(container).toMatchSnapshot();
  });
});
