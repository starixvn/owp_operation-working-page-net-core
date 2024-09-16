import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooksNative';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import Loading from '../../../../components/Loading';
import { querySubtypeThunk } from './subtypeThunk';
import { setSelect } from './subtypeSlice';
import { queryConsolidationThunk } from '../consolidationView/consolidationThunk';

const Subtype = () => {
  const dispatch = useAppDispatch();
  const { loading, entities, subTypeSelected } = useAppSelector(
    state => state.subtype
  );

  const handleClick = (typeId: number, subTypeId: number | null, subTypeName: string, eventType: number) => {
    dispatch(
      setSelect({
        typeId,
        subTypeId,
        label: subTypeName,
        eventType
      })
    );
    dispatch(queryConsolidationThunk({
      subTypeId,
      typeId
    }));
  };

  useEffect(() => {
    dispatch(querySubtypeThunk());
  }, [dispatch]);

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }
  return (
    <>
      <div className='title__wrapper'>
        <span className='title'>
          Subtype
        </span>
      </div>
      <div className='tree-view'>
        {entities?.map(({ typeId, subTypeId, subTypeName, isRead, eventType }) => (
          <div key={`${typeId}-${subTypeId}`} className='tree-node'>
            <span
              onClick={() => handleClick(typeId, subTypeId, subTypeName, eventType)}
              className={`subtype-item ${subTypeId === subTypeSelected.subTypeId && typeId === subTypeSelected.typeId &&
                'selected'
                } ${!isRead && ' unread'}`}>
              {subTypeName}
            </span>
          </div>
        ))}
      </div>
    </>
  );
};

export default Subtype;
