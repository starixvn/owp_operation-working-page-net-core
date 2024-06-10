import { IInputFieldMaxbetSpread } from '../../typing/type';
import InputCustom from '../shared/InputCustom';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';

const MaxBetHDP = (props: IInputFieldMaxbetSpread) => {
  const errorMessage = useStore(state => state.errorMessage.maxbetHDP);
  return (
    <div>
      <InputCustom
        label='Max Bet HDP'
        min={0}
        max={99999999}
        {...props}
        isRequired
      />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default MaxBetHDP;
