import { render } from '@testing-library/react';
import TableConditionConfig from './TableConditionConfig';
import useStore from '../../app/store';

useStore.setState({
  conditionData: [
    {
      progress: 'test',
      level: 'B',
      conditionFirst: {
        totalVirtualMultipleMatches: 5,
        virtualMatches: 0,
        multipleMatches: 0,
        specialProducts: 0,
        fiveMajorLeagues: 0,
        multipleProducts: 0
      },
      conditionSecond: {
        totalVirtualMultipleMatches: 0,
        virtualMatches: 0,
        multipleMatches: 0,
        specialProducts: 0,
        fiveMajorLeagues: 0,
        multipleProducts: 0
      },
      timeRange: '5-10',
      reward: 'Gold'
    }
  ]
});

test('renders table with condition data', () => {
  const { getByText } = render(<TableConditionConfig />);
  expect(getByText('Gold')).toBeInTheDocument();
});
