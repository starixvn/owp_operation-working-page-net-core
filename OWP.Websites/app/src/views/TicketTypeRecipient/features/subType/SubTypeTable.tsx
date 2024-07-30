import { Subtype } from '../TypeNative';
import SubtypeRow from './SubTypeRow';
import useStore from '../../app/zustand/ticketTypeStore';

type Props = {
  subtypes: Array<Subtype>;
  onUpdateSubtype: (value: string, index: number) => void;
  handleSelectAnotherSubtype: (subtypeId: Subtype) => void;
};

const SubTypeTable = (props: Props) => {
  const { subtypes, onUpdateSubtype, handleSelectAnotherSubtype } = props;
  const subtypeSelected = useStore(state => state.subtypeSelected);

  const handleOnclickSubtype = (subtype: Subtype) => {
    subtypeSelected.subtypeName !== subtype.subtypeName &&
      handleSelectAnotherSubtype(subtype);
  };

  const renderRows = (subtypeList: Array<Subtype>) => {
    return subtypeList.map((subtype, index) => {
      const key = index;
      return (
        <tr key={key}>
          <td
            className={`${
              subtypeSelected.subtypeName === subtype.subtypeName
                ? 'active-subtype-row'
                : ''
            }`}
            onClick={() => handleOnclickSubtype(subtype)}>
            <SubtypeRow
              {...subtype}
              onUpdateSubtype={onUpdateSubtype}
              index={index}
            />
          </td>
        </tr>
      );
    });
  };

  const renderBody = () => {
    if (!subtypes.length) {
      return (
        <tr>
          <td>There is no data</td>
        </tr>
      );
    } else {
      return renderRows(subtypes);
    }
  };

  return (
    <div className='tableFixHead'>
      <table className='recipient-table'>
        <thead>
          <tr>
            <th className='col-subtype'>subtype</th>
          </tr>
        </thead>
        <tbody>{renderBody()}</tbody>
      </table>
    </div>
  );
};

export default SubTypeTable;
