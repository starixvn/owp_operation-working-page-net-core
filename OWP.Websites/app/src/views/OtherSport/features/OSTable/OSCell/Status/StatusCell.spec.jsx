import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import StatusCell from './StatusCell';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { noop } from 'lodash';
const middleWares = [];
const mockStore = configureStore(middleWares);
const store = mockStore({
  osAssignmentTable: {
    isEdit: true
  },
  getState: ()=>noop()
});


const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;


const setUp = (matchStatus, offerLive, adminMatchId) => {
  return {
    dataSource: [
      {
        adminsiteMatchId: adminMatchId,
        osMatchId: 19,
        offerLive: offerLive,
        matchStatus: matchStatus,
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        service: 'InPlay',
        remark: '',
        assignments: [
          {
            osMatchId: 19,
            betTypeName: '1H (HDP)',
            assignmentType: 1,
            userId: 360418,
            username: 'starix1',
            assignmentNote: null
          }
        ],
        maxBets: []
      }
    ],
    rowIndex: 0
  };
};
describe('StatusCell under test', () => {
  it('should render StatusCell = live', () => {
    const { container } = render(<Wrapper>
      <StatusCell {...setUp('In-play', 'Live', 123)} />
      </Wrapper>);

    expect(container.querySelectorAll('.link').length).toEqual(1);
    expect(container.querySelectorAll('.icon-live').length).toEqual(1);
    expect(container.querySelector('.icon-live').textContent).toEqual('LIVE');
  });
  it('should render StatusCell = canceled', () => {
    const { container } = render(<Wrapper>
      <StatusCell {...setUp('In-play', 'Canceled', null)} />
    </Wrapper>);
    expect(container.querySelectorAll('.icon-canceled').length).toEqual(1);
    expect(container.querySelector('.icon-canceled').textContent).toEqual('');
  });
  it('should render StatusCell = postponed', () => {
    const { container } = render(
      <Wrapper><StatusCell {...setUp('In-play', 'Postponed', null)} />
    </Wrapper>);
    expect(container.querySelectorAll('.icon-postponed').length).toEqual(1);
    expect(container.querySelector('.icon-postponed').textContent).toEqual('');
  });
  it('should not render StatusCell', () => {
    const { container } = render(<Wrapper><StatusCell {...setUp()} /></Wrapper>);
    expect(container.querySelectorAll('.status-cell').length).toEqual(0);
  });
  it('should return default offer live', () => {
    const { container } = render(<Wrapper>
      <StatusCell {...setUp('In-play', 'abc', null)} />
      </Wrapper>);
    expect(container.querySelectorAll('.status-cell').length).toEqual(1);
  });
});
