import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { Modal, ModalBody } from 'reactstrap';
import useStore from '../../app/store';
import { DataSource, Target } from '../type/TableType.native';
import EditTargetTable from './EditTargetTable';
import { initDefaultFilterDate } from '../../Utils';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { buildSectionInString } from '../../helper/LaKpiHelper';

interface Props {
  setIsOpenPopup: Dispatch<SetStateAction<boolean>>;
}

const { defaultFromDate, defaultToDate } = initDefaultFilterDate();

const EditTargetPopup = (props: Props) => {
  const { setIsOpenPopup } = props;
  const { entities } = useStore(state => state.dataTable);
  const [updatedData, setUpdatedData] = useState<DataSource[]>(entities);
  const updateTargetLevel = useStore(state => state.updateTargetLevel);
  const queryDataTable = useStore(state => state.queryDataTable);
  const sectionSelected = useStore(state => state.sectionSelected);

  useEffect(() => {
    setUpdatedData([...entities]);
  }, [entities]);

  const handleSave = async () => {
    const differences = updatedData.filter(x => !entities.includes(x));
    const targetData = differences.map(
      x => ({ userId: x.userId, targetId: x.targetId } as Target)
    );
    if (targetData.length > 0) {
      await updateTargetLevel(targetData);
      await queryDataTable(
        defaultFromDate,
        defaultToDate,
        buildSectionInString(sectionSelected)
      );
      ToastrWrapper.showNotifySuccess('Changes are saved successfully');
    }
    setIsOpenPopup(false);
  };

  return (
    <Modal isOpen scrollable className='edit-popup' centered>
      <ModalBody>
        <h2 className='title'>Edit Target</h2>
        <EditTargetTable
          data={entities}
          updatedData={updatedData}
          setUpdatedData={setUpdatedData}
        />
        <div className='list-btn'>
          <button
            className='button-cancel'
            onClick={() => setIsOpenPopup(false)}>
            Cancel
          </button>
          <button className='button-export' onClick={handleSave}>
            Save
          </button>
        </div>
      </ModalBody>
    </Modal>
  );
};

export default EditTargetPopup;
