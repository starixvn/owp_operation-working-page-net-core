import { Fragment, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';

const ConsolidationView = () => {
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
    }
  ];
  const [hasMore, setHasMore] = useState(true);
  const [dataSource, setDataSource] = useState([fakeData[0], fakeData[1]]);
  const fetchMoreData = () => {
    if (dataSource.length > 3) {
      setHasMore(false);
    }
    setTimeout(() => {
      console.log('abc');
      setDataSource(dataSource.concat(dataSource[3]));
    }, 500);
  };

  return (
    <div
      className='main-wrap'
      style={{
        height: 300,
        overflow: 'auto',
        display: 'flex',
        flexDirection: 'column-reverse'
      }}>
      <InfiniteScroll
        loader={<h4>Loading...</h4>}
        hasMore={hasMore}
        next={fetchMoreData}
        dataLength={dataSource.length}>
        {dataSource.map((item, index) => (
          <Fragment key={`${item.messageId}`}>
            <p>{item.message}</p>
          </Fragment>
        ))}
      </InfiniteScroll>
    </div>
  );
};

export default ConsolidationView;
