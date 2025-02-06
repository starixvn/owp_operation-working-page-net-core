import { PesOrFiFa } from '../../type/TTaskListTable';
import PesIcon from '../component/PesIcon';
import FifaIcon from '../component/FifaIcon';
import FC24Icon from '../component/FC24Icon';
import VLeagueIcon from '../component/VLeagueIcon';

const FlagFactory = (props: PesOrFiFa) => {
  const { IsPESOther, IsFIFA, IsPES23, IsFC24, IsVietNamVLeague } = props;
  let flag = null;

  if (IsPESOther || IsPES23) {
    flag = <PesIcon {...props} />;
  }
  if (IsFIFA) {
    flag = <FifaIcon {...props} />;
  }
  if (IsFC24) {
    flag = <FC24Icon {...props} />;
  }
  if (IsVietNamVLeague) {
    flag = <VLeagueIcon {...props} />;
  }
  return flag;
};

export default FlagFactory;
