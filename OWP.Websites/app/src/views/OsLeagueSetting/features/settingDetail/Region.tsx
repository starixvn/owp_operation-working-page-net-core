import { IFieldSelection } from '../../typing/type';
import Selection from '../shared/Selection';
import {
  localStoreKey,
  simpleApiKey,
  optionValueNone
} from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import { buildDefaultSelected } from '../../helper/OsLeagueSettingHelper';

const { regionOption } = localStoreKey;

const Region = (props: IFieldSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.region,
    localStorageKey: regionOption
  });

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;

  options.unshift({ label: '-- None --', value: optionValueNone });
  const defaultValue = buildDefaultSelected(options, value);

  const selectProps = {
    label: 'Region',
    options,
    defaultValue,
    onChange,
    disabled
  };

  return <Selection {...selectProps} />;
};

export default Region;
