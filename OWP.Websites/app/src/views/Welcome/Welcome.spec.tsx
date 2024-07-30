import React from 'react';
import { render, screen } from '@testing-library/react';
import Welcome from './Welcome';

jest.mock('../../assets/img/welcome.png', () => 'test-file-stub');

describe('Welcome Component', () => {
  test('renders the welcome image with correct attributes', () => {
    render(<Welcome />);

    const imgElement = screen.getByAltText('welcome');
    expect(imgElement).toBeInTheDocument();
    expect(imgElement).toHaveAttribute('src', 'test-file-stub');
  });
});
