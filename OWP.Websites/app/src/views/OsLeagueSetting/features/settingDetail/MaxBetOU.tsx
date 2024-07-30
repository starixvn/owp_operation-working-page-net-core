import { IInputFieldMaxbetSpread } from '../../typing/type';
import InputCustom from '../shared/InputCustom';
import useStore from '../../app/store';
import ErrorMessage from './ErrorMessage';

const MaxBetOU = (props: IInputFieldMaxbetSpread) => {
  const errorMessage = useStore(state => state.errorMessage.maxbetOU);
  return (
    <div>
      <InputCustom
        label='Max Bet OU'
        {...props}
        min={0}
        max={99999999}
        isRequired
      />
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default MaxBetOU;
