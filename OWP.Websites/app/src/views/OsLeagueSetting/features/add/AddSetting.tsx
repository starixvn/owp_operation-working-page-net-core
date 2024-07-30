import LeagueContent from '../settingDetail/LeagueContent';
import LeagueName from '../settingDetail/LeagueName';
import SettingPopup from '../settingDetail/SettingPopup';
import useStore from '../../app/store';
import { shallow } from 'zustand/shallow';
import { ILeagueContent } from '../../typing/type';
import SettingContent from '../settingDetail/SettingContent';
import { addSetting } from '../../osLeague.json';
import ToastrWrapper from '../../../components/ToastrWrapper';

const AddSetting = () => {
  const {
    setSettingInfo,
    saveSetting,
    queryDataSource,
    resetSetting,
    validateBeforeSubmit,
    setPopupType,
    resetErrorMessage
  } = useStore(
    state => ({
      setSettingInfo: state.setSettingInfo,
      saveSetting: state.saveSetting,
      queryDataSource: state.queryDataSource,
      resetSetting: state.resetSetting,
      validateBeforeSubmit: state.validateBeforeSubmit,
      setPopupType: state.setPopupType,
      resetErrorMessage: state.resetErrorMessage
    }),
    shallow
  );

  const leagueNameProp = {
    disabled: false,
    onChange: (value: string | null) => setSettingInfo('leagueName', value)
  };

  const leagueProps: ILeagueContent = {
    category: {
      disabled: false,
      onChange: value => setSettingInfo('category', value)
    },
    region: {
      disabled: false,
      onChange: value => setSettingInfo('region', value)
    }
  };

  const showSuccessMessage = async () => {
    const toast = await ToastrWrapper.showInfo(
      'The setting has been added successfully!'
    );
    if (toast.value) {
      resetSetting();
      setPopupType(null);
      queryDataSource();
      resetErrorMessage();
    }
  };

  const handleSave = async () => {
    if (validateBeforeSubmit()) {
      const status = await saveSetting(addSetting);
      status && showSuccessMessage();
    }
  };

  return (
    <SettingPopup title='Add Setting' isAdd onSave={handleSave}>
      <>
        <LeagueContent {...leagueProps}>
          <LeagueName {...leagueNameProp} />
        </LeagueContent>
        <SettingContent />
      </>
    </SettingPopup>
  );
};

export default AddSetting;
