import { useSelector } from 'react-redux';
import { RootState } from '../app/store';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';
import { UPDATE_TYPE } from './filterConst.json';

const UpdateType = () => {
  const { updateTypes } = useSelector((state: RootState) => state.filter);

  return (
    <MultipleSelectionTemplate
      className='update-type'
      label='Update Type'
      options={updateTypes}
      value={updateTypes}
      filterKey={UPDATE_TYPE}
      disabled={false}
      disableSearch={false} />
  );
};

export default UpdateType;