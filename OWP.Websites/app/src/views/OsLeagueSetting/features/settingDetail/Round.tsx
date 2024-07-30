import { IFieldMultipleSelection } from '../../typing/type';
import MultipleSelection from '../shared/MultipleSelection';
import { localStoreKey, simpleApiKey } from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import ErrorMessage from './ErrorMessage';
import useStore from '../../app/store';

const { roundOption } = localStoreKey;
const Round = (props: IFieldMultipleSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.round,
    localStorageKey: roundOption
  });
  const errorMessage = useStore(state => state.errorMessage.round);
  if (loading) {
    return <Loading />;
  }
  const { onChange, value, disabled } = props;
  const prop = {
    label: 'Round',
    value: options.filter(o => value && value.includes(o.value.toString())),
    options,
    disabled,
    hasSelectAll: true,
    onChange
  };

  return (
    <div>
      <MultipleSelection {...prop} />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default Round;
