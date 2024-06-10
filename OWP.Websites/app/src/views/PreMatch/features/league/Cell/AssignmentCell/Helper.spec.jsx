import { prepareValueCopy, isTakeOver, notificationContent } from './Helper';

const createProps = key => {
  const dataRow = {
    No: 1,
    LeagueId: 32440,
    PreMatchLeagueId: 14043,
    LeagueName: 'ARGENTINA COPA LIGA PROFESIONAL',
    MatchCount: 3,
    HDP: 'HDP',
    OU: 'OU',
    HDPTakeOver: 'HDPTakeOver',
    OUTakeOver: 'OUTakeOver',
    OtherHDP: 'OtherHDP',
    OtherOU: 'OtherOU',
    OtherHDPTakeOver: 'OtherHDPTakeOver',
    OtherOUTakeOver: 'OtherOUTakeOver',
    Spread: null,
    Maxbet: null,
    Blacklist: false,
    Remark: null,
    HasMessages: false,
    IsRead: false,
    PreMatchIssueId: null,
    HasNotes: false,
    PreMatchNoteId: null,
    isExpand: false
  };
  return {
    dataRow,
    key
  };
};
describe('Helper underTest', () => {
  it('prepareValueCopy should return HDP, OU, when columnKey not include XXXTakeOver', () => {
    const props = createProps('OU');
    const result = prepareValueCopy(props.dataRow, props.key);
    expect(result).toEqual({
      MainAssignee1: 'HDP',
      MainAssignee2: 'OU',
      OtherAssignee1: 'OtherHDP',
      OtherAssignee2: 'OtherOU'
    });
  });

  it('prepareValueCopy should return HDPTakeOver, OUTakeOver, when columnKey include XXXTakeOver', () => {
    const props = createProps('OUTakeOver');
    const result = prepareValueCopy(props.dataRow, props.key);
    expect(result).toEqual({
      MainAssignee1: 'HDPTakeOver',
      MainAssignee2: 'OUTakeOver',
      OtherAssignee1: 'OtherHDPTakeOver',
      OtherAssignee2: 'OtherOUTakeOver'
    });
  });

  it('isTakeOver func should return true if key include XXXTakeOver', () => {
    const props = createProps('OUTakeOver');
    const result = isTakeOver(props.key);
    expect(result).toEqual(true);
  });

  it('isTakeOver func should return false if key not include XXXTakeOver', () => {
    const props = createProps('OU');
    const result = isTakeOver(props.key);
    expect(result).toEqual(false);
  });

  it('notificationContent should render exactly format', () => {
    const result = notificationContent('messageContent');
    expect(result).toEqual(
      <div className='toast-content'>
        <div
          className='message-content'
          dangerouslySetInnerHTML={{ __html: 'messageContent' }}
        />
      </div>
    );
  });
});
