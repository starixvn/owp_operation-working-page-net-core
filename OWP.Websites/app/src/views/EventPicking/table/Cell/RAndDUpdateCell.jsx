import PropTypes from 'prop-types';
import { uniqueId } from 'lodash';

const propTypes = {
    dataSource: PropTypes.array.isRequired,
    rowIndex: PropTypes.number.isRequired
};

const RAndDUpdateCell = props => {
    const { rowIndex, dataSource } = props;
    const rAndDInfo = dataSource[rowIndex]?.rAndDInfo ?? [];
    const rAndDInfoUniqueByKey = [
        ...new Map(rAndDInfo.map(item => [item.rAndDUpdate, item])).values()
    ];

    const correctLabel = value => value.toLowerCase() === 'match cancelled' ? value : value.replace(/(Update|Cancel)/, '').trim();
    return (
        <div className='r-and-d-update-cell'>
            <div className='r-and-d-update'>
                <ul className='r-and-d-update-list'>
                    {rAndDInfoUniqueByKey.length > 0
                        ? rAndDInfoUniqueByKey.map(info => (
                            <li
                                key={uniqueId()}
                                className='r-and-d-update-item'
                                style={{
                                    backgroundColor: info.bgColor,
                                    color: info.textColor
                                }}>
                                {correctLabel(info.rAndDUpdate)}
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
