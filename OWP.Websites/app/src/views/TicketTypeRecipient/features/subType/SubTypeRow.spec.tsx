import { fireEvent, render, screen } from '@testing-library/react';
import SubTypeRow from './SubTypeRow';

const onUpdateSubtype = jest.fn();

describe('subtypeRow under test', () => {
  it('should render subtypeRow', () => {
    const index = 0;
    const subtypeName = 'sa';
    const props = {
      subtypeName,
      index,
      onUpdateSubtype
    };
    const { container } = render(
      <SubTypeRow subtypeId={23} typeId={1} {...props} />
    );
    const input = container.querySelector(`[class="subtype-text-input"]`);
    const divElement = container.querySelector(`[class="control-subtype"]`);
    fireEvent.change(input!, { target: { value: '12345' } });
    fireEvent.focusOut(input!);
    expect(input!).toBeInTheDocument();
    expect(divElement!).toBeInTheDocument();
    fireEvent.change(input!, { target: { value: '' } });
    expect(screen.getByText('This field is required')).toBeInTheDocument();
    expect(onUpdateSubtype).toBeCalled();
  });
});
