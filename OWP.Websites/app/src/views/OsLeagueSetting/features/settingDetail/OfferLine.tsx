import MultipleSelection from '../shared/MultipleSelection';
import { IFieldMultipleSelection } from '../../typing/type';
import { localStoreKey, simpleApiKey } from '../../osLeague.json';
import useQueryDropdownOption from '../../hook/useQueryDropdownOption';
import Loading from '../../../components/Loading';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';

const { offerLineOption } = localStoreKey;

const OfferLine = (props: IFieldMultipleSelection) => {
  const { loading, options } = useQueryDropdownOption({
    apiKey: simpleApiKey.offerLine,
    localStorageKey: offerLineOption
  });

  const errorMessage = useStore(
    state => state.errorMessage.offerLines
  );

  if (loading) {
    return <Loading />;
  }

  const { onChange, value, disabled } = props;

  const prop = {
    label: 'Offer Lines',
    options,
    disabled,
    value: options.filter(o => value && value.includes(o.value.toString())),
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

export default OfferLine;
