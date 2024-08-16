import { useAppSelector } from '../../../app/hooksNative';
import { isEquals } from 'immutability-helper';
import { Fragment, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';
import { shouldShowUserInfo } from '../../../../Shared/chatBoardNew/helper/chatBoardHelper';
import MessageCell from '../../../../Shared/chatBoardNew/Message/MessageCell';

const ConsolidationView = () => {
  const subTypeSelected = useAppSelector(
    state => state.subtype.subTypeSelected
  );
  const fakeData = [
    {
      ticketId: 0,
      messageId: 45321,
      message: 'test',
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:13:33.183',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45322,
      message:
        '#30728 - *OLYMPICS 2024 WOMEN SOCCER (IN FRANCE) - CORNERS\nHome Team No.of Corners - Thursday -vs- Away Team No.of Corners - Thursday(Jul 25, 2024- 11:00 PM)',
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:14:08.257',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: [
        {
          messageId: 45322,
          iconKey: 'like',
          reactUserId: 8,
          reactUserName: 'sa_nickname',
          reactAvatarName: '895_download.jpg'
        },
        {
          messageId: 45322,
          iconKey: 'checked',
          reactUserId: 8,
          reactUserName: 'sa_nickname',
          reactAvatarName: '895_download.jpg'
        }
      ]
    },
    {
      ticketId: 0,
      messageId: 45323,
      message: 'Team No.of Corners - Thursday(Jul 25, 2024- 11:00 PM)',
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:14:37.023',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45324,
      message:
        "207942-enhance-event-picking  db730ef5\nmira.nguyen's avatar\nfeat: build event picking",
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:15:08.447',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45325,
      message:
        'Tỉnh Belgorod, giáp với tỉnh Kursk, ban bố tình trạng khẩn cấp khi chiến dịch xuyên biên giới của Ukraine đã kéo dài hơn một tuần.\n\n"Tình hình ở vùng Belgorod vẫn cực kỳ khó khăn và căng thẳng"',
      userId: 360418,
      username: 'starix1_nickname',
      avatarFileName: '894_image_2024-04-22_17-27-03.png',
      extension: '5555',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:26:46.29',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45326,
      message:
        'Trà sữa Lara\nTrà sữa vị Đào thơm béo rât đặc biệt. Topping Pudding trứng',
      userId: 360418,
      username: 'starix1_nickname',
      avatarFileName: '894_image_2024-04-22_17-27-03.png',
      extension: '5555',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:57:18.013',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45327,
      message:
        'Trà sữa Lara\nTrà sữa vị Đào thơm béo rât đặc biệt. Topping Pudding',
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:57:31.267',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45328,
      message:
        'Đường nối Bình Dương - TP HCM chưa thông vì vướng 50 m\nĐường giúp người dân Bình Dương thuận tiện tới bến xe lớn nhất nước, metro ở TP HCM đã khai thác, song chưa thông toàn tuyến do vướng 50 m dự án mở rộng xa lộ Hà Nội.',
      userId: 360418,
      username: 'starix1_nickname',
      avatarFileName: '894_image_2024-04-22_17-27-03.png',
      extension: '5555',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-14T15:58:05.623',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    },
    {
      ticketId: 0,
      messageId: 45331,
      message:
        'Báo Lao Động kỷ niệm 95 năm ngày xuất bản số đầu tiên\nHà NộiSáng 14/8, báo Lao Động tổ chức lễ kỷ niệm 95 năm ngày xuất bản số báo đầu tiên (14/8/1929 - 14/8/2024), tri ân các thế hệ lãnh đạo, phóng viên, người lao động của báo qua các thời kỳ.',
      userId: 8,
      username: 'sa_nickname',
      avatarFileName: '895_download.jpg',
      extension: '9933',
      fileId: null,
      isSaved: false,
      isFile: false,
      imageFileName: null,
      createdDate: '2024-08-15T11:01:24.487',
      quoteMessage: null,
      quoteUsername: null,
      quoteMessageId: 0,
      quoteCreatedDate: '0001-01-01T00:00:00',
      messageReactIcons: []
    }
  ];
  const [dataSource, setDataSource] = useState(fakeData);
  const fetchMoreData = () => {
    setDataSource(fakeData);
  };
  const messageGroupByDate = dataSource.reduce((result: any, message) => {
    const date = new Date(message.createdDate);
    if (!result[date.toDateString()]) {
      result[date.toDateString()] = [];
    }
    result[date.toDateString()].push(message);
    return result;
  }, {});

  const renderMessageRow = (
    message: any,
    index: number,
    previousMessage: any
  ) => {
    const key = `${message.messageId}-${index}`;
    const isMyMessage = isEquals(message.userId, 8);
    const showUserInfo = shouldShowUserInfo(previousMessage, message);

    return (
      <li
        key={key}
        className={
          isMyMessage ? 'message-row my-message' : 'message-row others-user'
        }>
        <MessageCell
          policy={'pagePolicy'}
          data={message}
          ticketId={message.ticketId}
          isMyMessage={isMyMessage}
          showUserInfo={showUserInfo}
          isConsolidationView={true}
        />
      </li>
    );
  };

  return (
    <>
      <div className='header-consolidation'>
        <span className='title'>{subTypeSelected.label}</span>
        <span className='describe'>(All messages for 3 last days)</span>
      </div>
      <div className='main-wrap-consolidation'>
        <InfiniteScroll
          loader={<h4>Loading...</h4>}
          hasMore={false}
          next={fetchMoreData}
          dataLength={dataSource.length}>
          {Object.keys(messageGroupByDate).map((date, key) => (
            <Fragment key={`${date}-${key}`}>
              <div>
                <h2 className='date-time-line'>
                  <span>
                    {new Date(date).toLocaleDateString('en-US', {
                      month: 'long',
                      day: 'numeric',
                      year: 'numeric'
                    })}
                  </span>
                </h2>
              </div>

              <ul>
                {messageGroupByDate[date].map((message: any, index: number) =>
                  renderMessageRow(
                    message,
                    index,
                    messageGroupByDate[date][index - 1]
                  )
                )}
              </ul>
            </Fragment>
          ))}
        </InfiniteScroll>
      </div>
    </>
  );
};

export default ConsolidationView;
