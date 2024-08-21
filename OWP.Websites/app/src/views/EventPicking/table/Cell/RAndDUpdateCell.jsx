import PropTypes from 'prop-types';
import { uniqueId } from 'lodash';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const RAndDUpdateCell = props => {
  const { rowIndex, dataSource } = props;
  const rAndDInfos = dataSource[rowIndex]?.rAndDInfos ?? [];
  const rAndDInfosUniqueByKey = [
    ...new Map(rAndDInfos.map(item => [item.rAndDUpdate, item])).values()
  ];
  return (
    <div className='r-and-d-update-cell'>
      <div className='r-and-d-update'>
        <ul className='r-and-d-update-list'>
          {rAndDInfosUniqueByKey.length > 0
            ? rAndDInfosUniqueByKey.map(info => (
                <li
                  key={uniqueId()}
                  className='r-and-d-update-item'
                  style={{
                    backgroundColor: info.bgColor,
                    color: info.textColor
                  }}>
                  {info.rAndDUpdate}
                </li>
              ))
            : null}
        </ul>
      </div>
    </div>
  );
};

RAndDUpdateCell.propTypes = propTypes;
export default RAndDUpdateCell;
