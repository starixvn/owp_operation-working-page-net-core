import MultipleSelection from '../shared/MultipleSelection';
import { IFieldMultipleSelection } from '../../typing/type';
import { localStoreKey, simpleApiKey } from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import ErrorMessage from './ErrorMessage';
import useStore from '../../app/store';

const { providerOption } = localStoreKey;

const Provider = (props: IFieldMultipleSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.provider,
    localStorageKey: providerOption
  });

  const errorMessage = useStore(state => state.errorMessage.remark);

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;

  const prop = {
    label: 'Provider',
    options,
    value: options.filter(o => value && value.includes(o.value.toString())),
    disableSearch: true,
    hasSelectAll: true,
    disabled,
    labelledBy: 'Select...',
    onChange
  };

  return (
    <div>
      <MultipleSelection {...prop} />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default Provider;
