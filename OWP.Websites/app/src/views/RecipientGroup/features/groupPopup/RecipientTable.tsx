import { DeleteIcon } from 'mdi-react';
import { Dispatch, SetStateAction } from 'react';
import { RecipientModel } from '../Type';

interface Props {
  recipients: RecipientModel[],
  setRecipients: Dispatch<SetStateAction<RecipientModel[]>>
}

const RecipientTable = (props: Props) => {
  const { recipients, setRecipients } = props;

  const handleDelete = (userId: number) => {
    const newRecipients = recipients.filter(item => item.recipientId !== userId);
    setRecipients(newRecipients);
  };

  const renderDeleteIcon = (item: RecipientModel) =>
    <span
      className='icon-edit'
      onClick={() => handleDelete(item.recipientId)}>
      <DeleteIcon size={12} />
    </span>;

  const renderData = () => (
    recipients.map(recipient => (
      <tr key={recipient.recipientId}>
        <td>{recipient.recipientName}</td>
        <td>{renderDeleteIcon(recipient)}</td>
      </tr>
    ))
  );

  return (
    <div className='tableFixHead'>
      <table className='recipient-table'>
        <thead>
          <tr>
            <th>Recipient</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {renderData()}
        </tbody>
      </table>
    </div>
  );
};

export default RecipientTable;