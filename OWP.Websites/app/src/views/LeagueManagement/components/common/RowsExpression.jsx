import PropTypes from 'prop-types';
import Expression from './Expression';
import Value from './Value';
import Action from './Action';

const RowsExpression = props => {
  const { models, titleName, ...otherProps } = props;
  return (
    <div className='list'>
      <div className='list-title'>{titleName}</div>
      <div className='list-content'>
        {models.map((item, index) => {
          const rowProps = {
            ...otherProps,
            item,
            index
          };
          return (
            <div className='list-item' key={item.key}>
              <Expression {...rowProps} />
              <Value {...rowProps} />
              <Action {...rowProps} />
            </div>
          );
        })}
      </div>
    </div>
  );
};

RowsExpression.propTypes = {
  models: PropTypes.array.isRequired,
  titleName: PropTypes.string.isRequired
};

export default RowsExpression;
