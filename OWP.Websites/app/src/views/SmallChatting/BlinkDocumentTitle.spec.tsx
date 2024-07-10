import React from 'react';
import { render, screen, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import { useAppSelector } from './app/hooks';
import BlinkDocumentTitle from './BlinkDocumentTitle';

jest.mock('./app/hooks');

describe('BlinkDocumentTitle', () => {
  beforeEach(() => {
    jest.useFakeTimers();
    document.title = 'Operation Working Page';
  });

  afterEach(() => {
    jest.clearAllTimers();
  });

  test('toggles the document title when isBlinkTitle is true', () => {
    useAppSelector.mockReturnValue(true);
    render(<BlinkDocumentTitle />);

    act(() => {
      jest.advanceTimersByTime(1000);
    });

    expect(document.title).toBe('New message');

    act(() => {
      jest.advanceTimersByTime(1000);
    });

    expect(document.title).toBe('Operation Working Page');
  });

  test('does not toggle the document title when isBlinkTitle is false', () => {
    useAppSelector.mockReturnValue(false);
    render(<BlinkDocumentTitle />);

    act(() => {
      jest.advanceTimersByTime(1000);
    });

    expect(document.title).toBe('Operation Working Page');
  });
});
