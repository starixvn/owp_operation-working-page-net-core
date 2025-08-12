import { render } from '@testing-library/react';
import ConditionColumn from './ConditionColumn';

test('renders table with condition data', () => {
  const conditionFirst = {
    totalVirtualMultipleMatches: 0,
    virtualMatches: 0,
    multipleMatches: 0,
    specialProducts: 112,
    fiveMajorLeagues: 0,
    multipleProducts: 0
  };
  const conditionSecond = {
    totalVirtualMultipleMatches: 0,
    virtualMatches: 1332,
    multipleMatches: 0,
    specialProducts: 0,
    fiveMajorLeagues: 0,
    multipleProducts: 0
  };
  const conditionThird = {
    totalVirtualMultipleMatches: 0,
    virtualMatches: 0,
    multipleMatches: 0,
    specialProducts: 0,
    fiveMajorLeagues: 0,
    multipleProducts: 0
  };
  const { getByText } = render(
    <ConditionColumn
      conditionFirst={conditionFirst}
      conditionSecond={conditionSecond}
      conditionThird={conditionThird}
    />
  );
  expect(getByText('112')).toBeInTheDocument();
});
