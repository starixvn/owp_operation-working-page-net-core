import { render } from '@testing-library/react';

import StepControlNodeTemplate from './StepControlNodeTemplate';

describe('StepControlNodeTemplate undef test', () => {
  const props = {
    classNameLiElement: 'li-element',
    classNameDivElement: 'div-element',
    title: 'first'
  };

  it('should match snapshot', () => {
    const wrapper = render(
      <StepControlNodeTemplate {...props}>
        <span>Test note</span>
      </StepControlNodeTemplate>
    );
    expect(wrapper).toMatchSnapshot();
  });
});
