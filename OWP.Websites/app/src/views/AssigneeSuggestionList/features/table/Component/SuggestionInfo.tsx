import { useEffect, useState } from 'react';
import { UserWorkingSchedule } from '../../TypeNative';
import TraderInfo from './TraderInfo';
import WorkingShift from './WorkingShift';
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import { shallow } from 'zustand/shallow';
import useStore from '../../../app/useStoreNative';

type Props = {
  workingShifts: UserWorkingSchedule[] | null;
  title: string;
  isOpen?: boolean;
};

const SuggestionInfo = ({ workingShifts, title, isOpen = false }: Props) => {
  const [isOpenExpand, setOpenExpand] = useState<boolean>(isOpen);
  const { searchText } = useStore(
    state => ({
      searchText: state.searchText
    }),
    shallow
  );

  useEffect(() => {
    Boolean(searchText) && setOpenExpand(true);
  }, [searchText]);

  return (
    <>
      <tr className='full-row'>
        <td
          className='sticky-col collapse-col'
          onClick={() => setOpenExpand(!isOpenExpand)}>
          <span>
            {title}
            <span className='collapse'>
              {isOpenExpand ? (
                <ArrowUpwardIcon fontSize='small' />
              ) : (
                <ArrowDownwardIcon fontSize='small' />
              )}
            </span>
          </span>
        </td>
        <td className='col-second' colSpan={39}></td>
      </tr>
      {isOpenExpand &&
        workingShifts?.map((item, index) => (
          <tr key={index}>
            <TraderInfo userInfo={item.userInfo} index={index}></TraderInfo>
            <WorkingShift data={item} index={index}></WorkingShift>
          </tr>
        ))}
    </>
  );
};

export default SuggestionInfo;
