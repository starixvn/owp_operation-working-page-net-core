import { IFieldMultipleSelection } from '../../typing/type';
import MultipleSelection from '../shared/MultipleSelection';
import { localStoreKey, simpleApiKey } from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';
const { betDelayOption } = localStoreKey;

const BetDelay = (props: IFieldMultipleSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.betDelay,
    localStorageKey: betDelayOption
  });

  const errorMessage = useStore(state => state.errorMessage.betDelay);

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;
  const prop = {
    label: 'Bet Delay',
    disabled,
    value: options.filter(o => value && value.includes(o.value.toString())),
    options,
    onChange,
    isRequired: true
  };

  return (
    <div>
      <MultipleSelection {...prop} />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default BetDelay;
