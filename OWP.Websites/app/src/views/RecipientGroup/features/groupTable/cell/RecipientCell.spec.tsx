import { render, screen } from '@testing-library/react';

import { GroupDataSource } from '../../Type';
import RecipientCell from './RecipientCell';

const data = {
  groupId: 1,
  groupName: 'LST',
  recipients: [
    { recipientId: 1, recipientName: 'Mimi' },
    { recipientId: 2, recipientName: 'Mira' }
  ]
} as GroupDataSource;

describe('test RecipientCell', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('mount RecipientCell, should render Cell', () => {
    render(<RecipientCell {...data} />);
    expect(screen.getByText('Mimi, Mira')).toBeInTheDocument();
  });
});
