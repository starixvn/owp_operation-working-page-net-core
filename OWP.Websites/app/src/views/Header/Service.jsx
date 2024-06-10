import axios from 'axios';
import Utils from '../Shared/Utils';
import { isEqual } from 'lodash';

const transformNavigation = (menus, title) => {
  const navigation = {
    items: []
  };
  menus.forEach(menu => {
    navigation.items.push({
      title: true,
      name: menu.name,
      wrapper: {
        element: '',
        attributes: {}
      }
    });

    if (menu.children) {
      menu.children.forEach(childMenu => {
        const item = {
          name: childMenu.name,
          url: Utils.resolveUrl(childMenu.path),
          icon: childMenu.iconCssClass
        };

        if (!isEqual(childMenu.name.toLowerCase(), title.toLowerCase())) {
          item.variant = 'unactive';
        }
        navigation.items.push(item);
      });
    }
  });

  return navigation;
};

const buildUrl = () =>
  process.env.NODE_ENV === 'development'
    ? 'api-netcore/application/header'
    : `${window.netCoreRootUrl}/api/application/header`;

const query = async title => {
  const response = await axios.get(buildUrl());
  const data = response.data;
  return {
    username: data.username,
    logoutLink: Utils.resolveUrl(data.logoutPath),
    notificationSetingLink: Utils.resolveUrl(data.notificationSetingPath),
    accountSettingPath: Utils.resolveUrl(data.accountSettingPath),
    navigation: transformNavigation(data.menus, title)
  };
};
const Service = {
  query
};

export default Service;
