import { RefObject } from 'react';
import { UncontrolledPopover, PopoverBody } from 'reactstrap';
import DeskPhoneIcon from 'mdi-react/DeskphoneIcon';
import SkypeIcon from 'mdi-react/SkypeIcon';
import CellPhoneIcon from 'mdi-react/CellphoneIphoneIcon';
import { TAssignment } from '../../type/TTaskListTable';

type Props = {
  target:
  | RefObject<HTMLInputElement>
  | RefObject<HTMLAnchorElement>;
  assignment: TAssignment;
};

const ContactInfo = ({ target, assignment }: Props) => {
  const { Skype, Phone, Extension } = assignment;
  const renderSkypeInfo = (skype: string) => (
    <li>
      <a className='link-skype' href={`skype:${skype}?chat`}>
        <SkypeIcon className='icn-skype' />
        <span className='skype-value'> {skype}</span>
      </a>
    </li>
  );

  const renderExtension = (extension: string) => (
    <li>
      <a className='disabled-phone'>
        <DeskPhoneIcon className='icn-phone' />
        <span className='extension-value'>{extension}</span>
      </a>
    </li>
  );

  const renderPhoneNumber = (phone: string) => (
    <li>
      <a className='disabled-phone'>
        <CellPhoneIcon className='icn-cellphone' />
        <span className='phone-value'>{phone}</span>
      </a>
    </li>
  );

  return (
    <UncontrolledPopover
      className='popup-contact'
      trigger='legacy'
      placement='right'
      target={target}>
      <PopoverBody>
        <ul className='menu'>
          {Skype && renderSkypeInfo(Skype)}
          {Extension && renderExtension(Extension)}
          {Phone && renderPhoneNumber(Phone)}
        </ul>
      </PopoverBody>
    </UncontrolledPopover>
  );
};
export default ContactInfo;
