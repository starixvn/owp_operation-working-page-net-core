import PropTypes from 'prop-types';
import Utils from '../../../../../../views/Shared/Utils';

const { convertThousandToK, convertSpreadToPercent } = Utils;

const Convert = (nameProduc, value) => {
  if (nameProduc.includes('Spread')) {
    return convertSpreadToPercent(value, '%');
  }
  if (nameProduc.includes('Remark')) {
    return value;
  }
  return convertThousandToK(value, 'k');
};

const RenderBody = props => {
  const { dataRows } = props;
  const renderRow = dataRows.map((row, index) => {
    const { Product, Action, OldValue, NewValue, ModifyDate, ModifyBy } = row;
    return (
      <tr key={index}>
        <td className='product'>{Product}</td>
        <td className='action'>{Action}</td>
        <td className='old-value'>{Convert(Product, OldValue)}</td>
        <td className='new-value'>{Convert(Product, NewValue)}</td>
        <td className='modifyDate'>{ModifyDate}</td>
        <td className='modifyBy'>{ModifyBy}</td>
      </tr>
    );
  });

  return (
    <>
      <tbody>{renderRow}</tbody>
    </>
  );
};
RenderBody.propTypes = {
  dataRows: PropTypes.array.isRequired
};

export default RenderBody;
