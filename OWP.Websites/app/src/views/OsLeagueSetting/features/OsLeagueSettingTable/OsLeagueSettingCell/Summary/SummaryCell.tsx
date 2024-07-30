import { CellProps } from '../../../../typing/tableType';
import { UncontrolledTooltip } from 'reactstrap';

const SummaryCell = (props: CellProps): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { summaryInfo, summaryRemark, leagueSettingId } = dataSource[rowIndex];

  return (
    <div className='summary-cell'>
      <div className='summary-info'>
        {summaryInfo.length > 0 &&
          summaryInfo.map((item, index) => (
            <div key={`${item.text}-${index}`}>
              <span
                id={`info-${index}-${leagueSettingId}`}
                className={item.style}>
                {item.text}
              </span>
              <UncontrolledTooltip
                placement='right'
                target={`info-${index}-${leagueSettingId}`}>
                {item.text}
              </UncontrolledTooltip>
            </div>
          ))}
      </div>
      <div className='summary-remark'>
        {summaryRemark.length > 0 &&
          summaryRemark.map((item, index) => (
            <div key={`${item.text}-${index}`}>
              <span
                id={`remark-${index}-${leagueSettingId}`}
                className={item.style}>
                {item.text}
              </span>
              <UncontrolledTooltip
                placement='right'
                target={`remark-${index}-${leagueSettingId}`}>
                {item.text}
              </UncontrolledTooltip>
            </div>
          ))}
      </div>
    </div>
  );
};

export default SummaryCell;
