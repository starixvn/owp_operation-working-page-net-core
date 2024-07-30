import { render, fireEvent } from '@testing-library/react';
import ButtonGroup from './ButtonGroup';

describe('ButtonGroup', () => {
  it('calls handleOnClosePopup when Cancel button is clicked', () => {
    const handleOnClosePopup = jest.fn();
    const handleSubmit = jest.fn();
    const { getByText } = render(
      <ButtonGroup
        handleOnClosePopup={handleOnClosePopup}
        handleSubmit={handleSubmit}
      />
    );

    const cancelButton = getByText('Cancel');
    fireEvent.click(cancelButton);

    expect(handleOnClosePopup).toHaveBeenCalledTimes(1);
    expect(handleSubmit).not.toHaveBeenCalled();
  });

  it('calls handleSubmit when Submit button is clicked', () => {
    const handleOnClosePopup = jest.fn();
    const handleSubmit = jest.fn();
    const { getByText } = render(
      <ButtonGroup
        handleOnClosePopup={handleOnClosePopup}
        handleSubmit={handleSubmit}
      />
    );

    const submitButton = getByText('Submit');
    fireEvent.click(submitButton);

    expect(handleSubmit).toHaveBeenCalledTimes(1);
    expect(handleOnClosePopup).not.toHaveBeenCalled();
  });
});
