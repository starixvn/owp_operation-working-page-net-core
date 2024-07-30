import { DataSource } from '../Share/Type';

type Props = {
  dataSource: Required<Array<DataSource>>;
  rowIndex: Required<number>;
};

const SportCell = (props: Props): JSX.Element => {
  const { dataSource, rowIndex } = props;
  const { sport } = dataSource[rowIndex];
  return <span className='sport-cell'>{sport}</span>;
};

export default SportCell;
