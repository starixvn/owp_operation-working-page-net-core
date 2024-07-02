import { render } from '@testing-library/react';
import NoCell from './NoCell';

describe('NoCell under test', () => {
  it('renders NoCell components when has `No`', () => {
    const { container } = render(
      <NoCell rowIndex={0} dataSource={[{ No: 999 }]} />
    );
    expect(container.querySelector('.no')).toBeInTheDocument();
    expect(container.querySelector('.no').textContent).toEqual('999');
  });

  it('renders NoCell components when has MatchOffer=Opened', () => {
    const { container } = render(
      <NoCell rowIndex={0} dataSource={[{ MatchOffer: 'Opened' }]} />
    );
    expect(container.querySelector('.no')).toBeNull();
    expect(container.querySelector('.icon-completed')).toBeInTheDocument();
  });

  it('renders NoCell components when has MatchOffer != Opened', () => {
    const { container } = render(
      <NoCell rowIndex={0} dataSource={[{ MatchOffer: 'HDP (?)' }]} />
    );
    expect(container.querySelector('.no')).toBeNull();
    expect(container.querySelector('.icon-completed')).toBeNull();
    expect(container.querySelector('.match-offer').textContent).toEqual(
      'HDP (?)'
    );
  });
});
