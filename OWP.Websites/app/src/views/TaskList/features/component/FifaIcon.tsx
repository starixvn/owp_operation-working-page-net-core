import { PesOrFiFa } from '../../type/TTaskListTable';
import { UncontrolledTooltip } from 'reactstrap';

const FifaIcon = (props: PesOrFiFa) => {
  const { rowIndex } = props;
  return (
    <>
      <i
        className='icon-match icon-fifa'
        id={`link-fifa-${rowIndex}`}
        original-title='FIFA'></i>
      <UncontrolledTooltip placement='bottom' target={`link-fifa-${rowIndex}`}>
        FIFA
      </UncontrolledTooltip>
    </>
  );
};

export default FifaIcon;
