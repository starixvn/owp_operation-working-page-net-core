import OfferLine from './OfferLine';
import {
  IFieldMultipleSelection,
  ITimeRange,
  IInputFieldMaxbetSpread,
  IRemark,
  IUpdateScore,
  IFieldSelection
} from '../../typing/type';
import Provider from './Provider';
import SpreadHDP from './SpreadHDP';
import SpreadOU from './SpreadOU';
import MaxBetHDP from './MaxBetHDP';
import MaxBetOU from './MaxBetOU';
import WeekMatch from './WeekMatch';
import BetDelay from './BetDelay';
import KORangeTime from './KORangeTime';
import Remark from './Remark';
import UpdateScore from './UpdateScore';
import useStore from '../../app/store';
import { ISettingDetail } from '../../typing/settingDetailType';
import BetTypeCheckBox from './BetTypeCheckBox';
import { IBettypeType } from '../../typing/bettypeType';
import Round from './Round';
import { shallow } from 'zustand/shallow';

const SettingContent = (props: ISettingDetail) => {
  const { setSettingInfo, getPolicy } = useStore(
    state => ({
      setSettingInfo: state.setSettingInfo,
      getPolicy: state.getPolicy
    }),
    shallow
  );
  const policy = getPolicy();

  const weekMatch: IFieldSelection = {
    disabled: !policy.HasWrite,
    value: props.weekMatch,
    onChange: selected => {
      setSettingInfo('weekMatch', selected);
    }
  };

  const rounds: IFieldMultipleSelection = {
    disabled: !policy.HasWrite,
    value: props.rounds,
    onChange: selected => {
      const value = selected.map(x => x.value);
      setSettingInfo('rounds', value);
    }
  };

  const offerLine: IFieldMultipleSelection = {
    disabled: !policy.HasWrite,
    value: props.offerLines,
    onChange: selected => {
      const value = selected.map(x => x.value);
      setSettingInfo('offerLines', value);
    }
  };

  const betDelay: IFieldMultipleSelection = {
    disabled: !policy.HasWrite,
    value: props.betDelay,
    onChange: selected => {
      const value = selected.map(x => x.value);
      setSettingInfo('betDelay', value);
    }
  };

  const providers: IFieldMultipleSelection = {
    disabled: !policy.HasWrite,
    value: props.providers,
    onChange: selected => {
      const value = selected.map(x => x.value);
      setSettingInfo('providers', value);
    }
  };

  const timeRange: ITimeRange = {
    disabled: !policy.HasWrite,
    koFromTime: props.koFromTime,
    koToTime: props.koToTime,
    onChange: (key, value) => setSettingInfo(key, value)
  };

  const spreadHDP: IInputFieldMaxbetSpread = {
    disabled: !policy.HasWrite,
    value: props.spreadHDP,
    onChange: value => setSettingInfo('spreadHDP', value)
  };

  const spreadOU: IInputFieldMaxbetSpread = {
    disabled: !policy.HasWrite,
    value: props.spreadOU,
    onChange: value => setSettingInfo('spreadOU', value)
  };

  const maxbetHDP: IInputFieldMaxbetSpread = {
    disabled: !policy.HasWrite,
    value: props.maxbetHDP,
    onChange: value => setSettingInfo('maxbetHDP', value)
  };

  const maxbetOU: IInputFieldMaxbetSpread = {
    disabled: !policy.HasWrite,
    value: props.maxbetOU,
    onChange: value => setSettingInfo('maxbetOU', value)
  };

  const remark: IRemark = {
    disabled: !policy.HasWrite,
    value: props.remark,
    onChange: value => setSettingInfo('remark', value)
  };

  const score: IUpdateScore = {
    disabled: !policy.HasWrite,
    isIptvOnly: props.isIptvOnly,
    isManualUpdateScore: props.isManualUpdateScore,
    onChange: (key, value) => setSettingInfo(key, value)
  };

  const bettypes: IBettypeType = {
    disabled: !policy.HasWrite,
    value: props.betTypes || [],
    onChange: value => setSettingInfo('betTypes', value)
  };

  return (
    <div>
      <h4 className='setting-info'> Setting Information </h4>
      <div className='setting-row'>
        <Round {...rounds} />
        <WeekMatch {...weekMatch} />
        {props.children}
      </div>
      <div className='setting-row'>
        <OfferLine {...offerLine} />
        <BetDelay {...betDelay} />
        <Provider {...providers} />
        <KORangeTime {...timeRange} />
        <SpreadHDP {...spreadHDP} />
        <SpreadOU {...spreadOU} />
        <MaxBetHDP {...maxbetHDP} />
        <MaxBetOU {...maxbetOU} />
        <span className='hint'>
          Hint: One of 2 following fields must be filled in: Maxbet HDP and
          Maxbet OU
        </span>
      </div>

      <div className='setting-row'>
        <Remark {...remark} />
        <UpdateScore {...score} />
        <BetTypeCheckBox {...bettypes} />
      </div>
    </div>
  );
};

export default SettingContent;
