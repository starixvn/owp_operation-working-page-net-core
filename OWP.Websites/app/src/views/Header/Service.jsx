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
      name: menu.Name,
      wrapper: {
        element: '',
        attributes: {}
      }
    });

    if (menu.Children) {
      menu.Children.forEach(childMenu => {
        const item = {
          name: childMenu.Name,
          url: Utils.resolveUrl(childMenu.Path),
          icon: childMenu.IconCssClass
        };

        if (!isEqual(childMenu.Name.toLowerCase(), title.toLowerCase())) {
          item.variant = 'unactive';
        }
        navigation.items.push(item);
      });
    }
  });

  return navigation;
};

const buildUrl = () => `/api/application/header`;

const query = async title => {
  const response = await axios.get(buildUrl());
  const data = response.data;
  return {
    username: data.Username,
    logoutLink: Utils.resolveUrl(data.LogoutPath),
    changePasswordPath: Utils.resolveUrl(data.ChangePasswordPath),
    notificationSettingLink: Utils.resolveUrl(data.NotificationSettingPath),
    accountSettingPath: Utils.resolveUrl(data.AccountSettingPath),
    navigation: transformNavigation(data.Menus, title)
  };
};
const Service = {
  query
};

export default Service;
