import { MouseEvent, useState } from 'react';
import EditSettingPopup from '../../editPopup/EditSettingPopup';
import { TDataSource } from '../TableType';
import { setInitSetting } from '../../editPopup/slice/editSettingSlice';
import { useAppDispatch } from '../../app/hooks';

interface Props {
  className: string;
  dataSource: TDataSource;
}

const LeagueCell = (props: Props) => {
  const { className, dataSource } = props;
  const [isOpenPopup, setIsOpenPopup] = useState(false);
  const dispatch = useAppDispatch();

  const handleOpenPopup = (event: MouseEvent<HTMLDivElement>) => {
    event.preventDefault();
    dispatch(setInitSetting(dataSource));
    setIsOpenPopup(true);
  };

  return (
    <td className={className}>
      <span onClick={handleOpenPopup} className='row-table league'>
        {dataSource.league}
      </span>
      {isOpenPopup && (
        <EditSettingPopup closePopup={() => setIsOpenPopup(false)} />
      )}
    </td>
  );
};

export default LeagueCell;
