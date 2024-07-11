import React from 'react';
import { render } from '@testing-library/react';
import MBCell from './MBCell';

describe('MBCell', () => {
  it('should render HandicapSetting.MaxBetToDisplay correctly', () => {
    const props = {
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' },
      OverUnderSetting: {}
    };
    const { getByText } = render(<MBCell {...props} />);

    expect(getByText('100')).toBeInTheDocument();
  });

  it('should render HandicapSetting.SpreadToDisplay correctly', () => {
    const props = {
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' },
      OverUnderSetting: {}
    };
    const { getByText } = render(<MBCell {...props} />);

    expect(getByText('(2.5')).toBeInTheDocument();
  });

  it('should render OverUnderSetting.SpreadToDisplay correctly', () => {
    const props = {
      HandicapSetting: {},
      OverUnderSetting: { SpreadToDisplay: '1.5' }
    };
    const { getByText } = render(<MBCell {...props} />);

    expect(getByText('1.5')).toBeInTheDocument();
  });

  it('should render both HandicapSetting.SpreadToDisplay and OverUnderSetting.SpreadToDisplay correctly', () => {
    const props = {
      HandicapSetting: { MaxBetToDisplay: '100', SpreadToDisplay: '2.5' },
      OverUnderSetting: { SpreadToDisplay: '1.5' }
    };
    const { getByText, container } = render(<MBCell {...props} />);

    expect(getByText('(2.5')).toBeInTheDocument();
  });

  it('should not render the closing parenthesis when isDisplayHDPSetting is false', () => {
    const props = {
      HandicapSetting: {},
      OverUnderSetting: { SpreadToDisplay: '1.5' }
    };
    const { container } = render(<MBCell {...props} />);

    expect(container.querySelector('span')).not.toHaveTextContent(')');
  });
});
