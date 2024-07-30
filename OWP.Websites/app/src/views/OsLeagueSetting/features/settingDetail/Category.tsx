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

const { categoryOption } = localStoreKey;

const Category = (props: IFieldSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.category,
    localStorageKey: categoryOption
  });

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;

  options.unshift({ label: '-- None --', value: optionValueNone });
  const defaultValue = buildDefaultSelected(options, value);

  const prop = {
    label: 'Category',
    options,
    defaultValue,
    onChange,
    disabled
  };

  return <Selection {...prop} />;
};

export default Category;
