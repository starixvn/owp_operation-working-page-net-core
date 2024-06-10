
import { TMaxbetSetting } from '../../../type/table';

const MBCell = ({ HandicapSetting, OverUnderSetting }: TMaxbetSetting) => {
  const isDisplayHDPSetting =
    HandicapSetting?.SpreadToDisplay ?? OverUnderSetting?.SpreadToDisplay;
  const isHDPSpreadExisting = HandicapSetting?.SpreadToDisplay;

  return (
    <div>
      {HandicapSetting?.MaxBetToDisplay}
      {isDisplayHDPSetting && (
        <span>({HandicapSetting?.SpreadToDisplay}</span>
      )}
      {OverUnderSetting?.SpreadToDisplay && (
        <>
          {isHDPSpreadExisting && <span>-</span>}
          {OverUnderSetting.SpreadToDisplay}
        </>
      )}
      {isDisplayHDPSetting && <span>)</span>}
    </div>
  );
};

export default MBCell;