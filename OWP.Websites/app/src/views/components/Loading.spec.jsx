import Loading from './Loading';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';

describe('Loading Component Tests', () => {
  it('Render <Loading/> match snapshot', () => {
    const utils = render(<Loading />);
    expect(utils.container.getElementsByClassName('loading-process')).toHaveLength(1);
  });
});
