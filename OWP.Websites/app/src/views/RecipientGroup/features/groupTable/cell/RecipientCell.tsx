import { GroupDataSource, RecipientModel } from '../../Type';

const RecipientCell = (data: GroupDataSource) => {
  const { recipients } = data;
  const recipientsInString = recipients.map((item: RecipientModel) =>
    item.recipientName
  ).reduce((prev, curr) => `${prev}, ${curr}`);

  return (
    <div>
      {recipientsInString}
    </div>
  );
};

export default RecipientCell;
