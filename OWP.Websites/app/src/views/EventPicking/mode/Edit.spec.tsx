
import store from '../app/store';

import { useAppDispatch } from '../app/hooks';
import Edit from './Edit';
import { fireEvent, render } from '@testing-library/react';

jest.mock('../../Shared/customHook/useStableDispatch',);

jest.mock('../app/hooks');

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useSelector: jest.fn(),
  createSelectorHook: jest.fn(),
}));

(useAppDispatch as jest.Mock).mockReturnValue(jest.fn());

store.getState=()=>({
  filter:{
    param:{
      date: new Date('2024-7-11')
    }
  }
});

const onSetMode = jest.fn();
describe('Edit under test', () => {
  beforeEach(()=>{
    jest.clearAllMocks();
  });

  it('should render component', ()=>{
    const { container } = render(<Edit onSetMode={onSetMode} />);
    fireEvent.click(container.getElementsByClassName('button-submit')[0])
    expect(onSetMode).toBeCalled();
  });
});