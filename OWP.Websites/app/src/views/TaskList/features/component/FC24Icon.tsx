import { PesOrFiFa } from '../../type/TTaskListTable';
import { UncontrolledTooltip } from 'reactstrap';

const FC24Icon = (props: PesOrFiFa) => {
  const { rowIndex } = props;
  return (
    <>
      <i
        className='icon-match icon-fc24'
        id={`link-fc24-${rowIndex}`}
        original-title='FC 24'></i>
      <UncontrolledTooltip placement='bottom' target={`link-fc24-${rowIndex}`}>
        FC 24
      </UncontrolledTooltip>
    </>
  );
};

export default FC24Icon;
