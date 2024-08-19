import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooksNative';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import Loading from '../../../../components/Loading';
import { querySubtypeThunk } from './subtypeThunk';
import { setSelect } from './subtypeSlice';

const Subtype = () => {
  const dispatch = useAppDispatch();
  const { loading, entities, subTypeSelected} = useAppSelector(
    state => state.subtype
  );

  const handleClick = (typeId: number, subTypeId: number|null, subTypeName: string) =>{
    dispatch(
      setSelect({
        typeId,
        subTypeId,
        label: subTypeName
      })
    );
  };

  useEffect(() => {
    dispatch(querySubtypeThunk());
  }, [dispatch]);

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }
  return (
    <div className='tree-view'>
      {entities?.map(({typeId, subTypeId, subTypeName, isRead}) => (
        <div key={`${typeId}-${subTypeId}`} className='tree-node'>
          <span
            onClick={() => handleClick(typeId, subTypeId, subTypeName)}
            className={`subtype-item ${subTypeName}
              ${subTypeId === subTypeSelected.subTypeId &&
              'selected'
            } ${!isRead && ' unread'}`}>
          </span>
        </div>
      ))}
    </div>
  );
};

export default Subtype;
