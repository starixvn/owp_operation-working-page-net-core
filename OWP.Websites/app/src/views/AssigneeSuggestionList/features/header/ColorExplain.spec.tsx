import useStore from '../../app/useStoreNative';
import ColorExplain from './ColorExplain';
import { render, screen, fireEvent } from '@testing-library/react';

jest.mock('../../app/useStoreNative');

describe('ColorExplain', () => {
  test('renders color info on click', async () => {
    (useStore as unknown as jest.Mock).mockReturnValue({
      colorExplainInfo: [
        { label: 'Red', backgroundColor: 'red', textColor: 'white' },
        { label: 'Blue', backgroundColor: 'blue', textColor: 'white' }
      ],
      queryColorExplain: jest.fn()
    });

    const { container } = render(<ColorExplain />);

    const infoIcon = container.querySelector('.color__explain_info');

    infoIcon instanceof HTMLElement && fireEvent.click(infoIcon);

    const redColorLabel = await screen.findByText('Red');
    const blueColorLabel = await screen.findByText('Blue');

    expect(redColorLabel).toBeInTheDocument();
    expect(blueColorLabel).toBeInTheDocument();
  });
});
