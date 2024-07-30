import { fireEvent, render, screen } from '@testing-library/react';
import UserItem from './UserItem';
import '@testing-library/jest-dom';

describe('UserItem Component Tests', () => {
  it('should render UserItem with displayMode is false', () => {
    //Arrange
    const props = {
      user: {
        NickName: 'sa',
        UserName: 'superman'
      },
      displayMode: false,
      highlightedDisplay: null
    };
    const utils = render(<UserItem {...props} />);

    // Act
    const element = utils.getByTestId('content-input');

    // Assert
    expect(element).toBeInTheDocument();
  });
});
