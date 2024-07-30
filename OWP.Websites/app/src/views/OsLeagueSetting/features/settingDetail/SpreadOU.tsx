import { IInputFieldMaxbetSpread } from '../../typing/type';
import InputCustom from '../shared/InputCustom';

const SpreadOU = (props: IInputFieldMaxbetSpread) => {
  return <InputCustom min={0} max={100} label='Spread OU (%)' {...props} />;
};

export default SpreadOU;
