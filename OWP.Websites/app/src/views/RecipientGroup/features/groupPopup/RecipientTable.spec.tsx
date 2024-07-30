import { fireEvent, render } from '@testing-library/react';
import { Dispatch, SetStateAction } from 'react';
import { RecipientModel } from '../Type';
import RecipientTable from './RecipientTable';

const setState = jest.fn() as Dispatch<SetStateAction<RecipientModel[]>>;
const recipients = [
  { recipientId: 1, recipientName: 'Mimi' },
  { recipientId: 2, recipientName: 'Mira' }
] as RecipientModel[];

describe('test RecipientTable', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('mount RecipientTable, should render Form and 2 row', () => {
    const { container } = render(
      <RecipientTable recipients={recipients} setRecipients={setState} />
    );
    expect(container.querySelector('.recipient-table')).toBeInTheDocument();
    expect(container.querySelector('.icon-edit')).toBeInTheDocument();
  });

  it('click Delete Icon, should call SetState', () => {
    const { container } = render(
      <RecipientTable recipients={recipients} setRecipients={setState} />
    );
    const element = container.querySelector('.icon-edit');
    fireEvent.click(element!);
    expect(setState).toHaveBeenCalledTimes(1);
  });
});
