import { render } from '@testing-library/react';
import { TooltipButton } from '.';

const props = {
  innerClassName: 'iptv',
  rowIndex: 0,
  toolTipContent: 'Euro40',
  btnName: 'IPTV'
};

describe('TooltipButton under test', () => {
  it('should render TooltipButton', () => {
    const { container } = render(<TooltipButton {...props} />);
    expect(container).toMatchSnapshot();
  });
});
