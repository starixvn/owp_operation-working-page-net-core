import { CNavItem, CNavTitle, CSidebar, CSidebarNav } from '@coreui/react';
import { useEffect, useRef } from 'react';
import logo from '../../assets/img/brand/logo_none.svg';
import logoText from '../../assets/img/brand/logo_text.svg';
import Utils from '../Shared/Utils';

type Root = {
  navigation: Navigation;
};

type Navigation = {
  items: Item[];
};

type Item = {
  title?: boolean;
  name: string;
  wrapper?: Wrapper;
  attributes: Attributes2;
  url?: string;
  icon?: string;
  variant?: string;
};

type Wrapper = {
  element: string;
  attributes: Attributes;
};

export interface Attributes { }

export interface Attributes2 {
  title: string;
}

const Title = (item: Item) => (
  <CNavTitle key={item.name}>{item.name}</CNavTitle>
);

const NavItem = (item: Item) => {
  return (
    <CNavItem
      href={item.url}
      key={item.name}
      title={item.name}
      className={item.variant ? '' : 'active'}>
      <i className={`nav-icon ${item.icon}`} />
      {item.name}
    </CNavItem>
  );
};

const Sidebar = (navigation: Root) => {
  const sideBarRef = useRef<HTMLDivElement>(null);
  useEffect(() => {
    const handleClickOutSide = (event: MouseEvent) => {
      sideBarRef.current &&
        !sideBarRef.current.contains(event.target as Node) &&
        document.body.classList.remove('sidebar-lg-show');
    };
    document.addEventListener('mousedown', handleClickOutSide);
    return () => {
      document.removeEventListener('mousedown', handleClickOutSide);
    };
  }, [sideBarRef]);
  return (
    <CSidebar
      ref={sideBarRef}>
      <a href={Utils.rootUrl} className='brand-logo'>
        <span className='logo-img'>
          <img src={logo} alt='OWP logo' />
        </span>
        <span className='logo-text'>
          <img src={logoText} alt='OWP logo' />
        </span>
      </a>
      <CSidebarNav>
        {navigation.navigation.items.map(item => {
          return item.title ? Title(item) : NavItem(item);
        })}
      </CSidebarNav>
    </CSidebar>
  );
};

export default Sidebar;
