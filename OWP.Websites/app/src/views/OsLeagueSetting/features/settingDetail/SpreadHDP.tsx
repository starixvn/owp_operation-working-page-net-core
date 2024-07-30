import { IInputFieldMaxbetSpread } from '../../typing/type';
import InputCustom from '../shared/InputCustom';

const SpreadHDP = (props: IInputFieldMaxbetSpread) => {
  return <InputCustom min={0} max={100} label='Spread HDP (%)' {...props} />;
};

export default SpreadHDP;
