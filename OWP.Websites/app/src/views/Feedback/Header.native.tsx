import { HEADER } from './const.json';

const {
  TITLE,
  DESCRIPTION
} = HEADER;

const Header = () =>
  <div className='header'>
    <p className='title'>{TITLE}</p>
    <span className='description'>{DESCRIPTION.EN}</span>
    <span className='description'>{DESCRIPTION.CN}</span>
  </div>;

export default Header;