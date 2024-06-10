import { TAssigneeSuggestion } from '../TypeNative';
import SuggestionInfo from './Component/SuggestionInfo';
import { isShowPartially } from '../helper/AssigneeSuggestionHelper';

type Props = {
  workingShifts: TAssigneeSuggestion;
};

const TableBody = ({
  workingShifts: { busy, fullyAvailable, partiallyAvailable, matchInfo }
}: Props) => (
  <>
    <SuggestionInfo
      workingShifts={fullyAvailable}
      title='Assignee Suggestion (Available)'
      isOpen
    />
    {isShowPartially(matchInfo) && (
      <SuggestionInfo
        workingShifts={partiallyAvailable}
        title='Assignee Suggestion (Partially available)'
        isOpen
      />
    )}
    <SuggestionInfo workingShifts={busy} title='Busy' />
  </>
);

export default TableBody;
