import { fireEvent, render, screen } from '@testing-library/react';
import { values } from 'lodash';
import SubTypeInput from './SubTypeInput';

const props = {
  onAddNew: jest.fn(),
  subtypes: [{ subtypeId: 85, typeId: 1, subtypeName: 'Sub1' }]
};

describe('EditRecipientPopup under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should match snapshot', () => {
    const { container } = render(<SubTypeInput {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('should not call onAddNew when values is fail validate & click submit', () => {
    const { container } = render(<SubTypeInput {...props} />);
    const element = container.querySelector(`[class="button button-add"]`);

    fireEvent.click(element!);

    expect(props.onAddNew).toHaveBeenCalledTimes(0);
  });

  it('should call onAddNew when values is passed validate & click submit', () => {
    const { container } = render(<SubTypeInput {...props} />);
    const element = container.querySelector(`[class="subtype-text-input"]`);
    fireEvent.change(element!, { target: { value: 'Liam ' } });
    const button = container.querySelector(`[class="button button-add"]`);
    fireEvent.click(button!);

    expect(props.onAddNew).toHaveBeenCalledTimes(1);
  });
});
