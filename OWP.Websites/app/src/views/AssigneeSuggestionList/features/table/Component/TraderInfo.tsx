import TraderCell from '../Cell/TraderCell';
import ShiftCell from '../Cell/ShiftCell';
import LevelCell from '../Cell/LevelCell';
import MainBetTypeCell from '../Cell/MainBetTypeCell';
import MultipleGameCell from '../Cell/MultipleGameCell';
import ExplainCell from '../Cell/ExplainCell';
import { UserInfo } from '../../TypeNative';

type Props = {
  index: number;
  userInfo?: UserInfo;
};

const TraderInfo = ({ index, userInfo }: Props) => {
  return (
    <td className='traderInfo sticky-col'>
      <div className='col-trader'>
        <ShiftCell
          index={index}
          value={userInfo?.shift}
          shiftCode={userInfo?.shiftCode}
          otPostShift={userInfo?.otPostShift}
          otPreShift={userInfo?.otPreShift} />
        <TraderCell
          index={index}
          value={userInfo?.username}
          isManagement={userInfo?.isManagementRole} />
        <LevelCell index={index} value={userInfo?.levelLabel}></LevelCell>
        <MainBetTypeCell
          index={index}
          value={userInfo?.mainBetType}></MainBetTypeCell>
        <MultipleGameCell
          index={index}
          value={userInfo?.maxMultipleGames}></MultipleGameCell>
        <ExplainCell index={index} value={userInfo?.explanation}></ExplainCell>
      </div>
    </td>
  );
};

export default TraderInfo;
