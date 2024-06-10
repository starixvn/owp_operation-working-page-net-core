import { IFieldSelection } from '../../typing/type';
import {
  localStoreKey,
  simpleApiKey,
  optionValueNone
} from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import Selection from '../shared/Selection';
import { buildDefaultSelected } from '../../helper/OsLeagueSettingHelper';

const { weekMatchOption } = localStoreKey;

const WeekMatch = (props: IFieldSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.weekMatch,
    localStorageKey: weekMatchOption
  });

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;

  options.unshift({ label: '-- None --', value: optionValueNone });
  const defaultValue = buildDefaultSelected(options, value);

  const prop = {
    label: 'End Of Preseason before nth Week Match',
    options,
    disabled,
    defaultValue,
    onChange
  };

  return <Selection {...prop} />;
};

export default WeekMatch;
