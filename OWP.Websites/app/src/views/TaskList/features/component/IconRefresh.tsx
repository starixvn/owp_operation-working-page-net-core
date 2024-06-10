import { useEffect, useRef, useState } from 'react';
import {
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem
} from 'reactstrap';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { taskListThunk } from '../table/tableThunk';
import {
  timeRefreshTable,
  timeRefresh,
  intervalInit
} from '../const/TableSchema.json';

type Props = {
  eventDate: Date;
};
const IconRefresh = ({ eventDate }: Props) => {
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [timer, setTimer] = useState(timeRefreshTable[2]);
  const selectedTime = useRef(timeRefreshTable[2]);
  const dispatch = useStableDispatch();

  useEffect(() => {
    let interval = intervalInit;
    if (timer !== -1) {
      interval = window.setInterval(() => {
        setTimer(prevState =>
          !prevState ? selectedTime.current : prevState - 1
        );
      }, timeRefresh);
    }
    return () => clearInterval(interval);
  }, [timer]);

  useEffect(() => {
    if (timer === 0) {
      dispatch(
        taskListThunk({
          eventDateTime: eventDate
        })
      );
    }
  }, [timer, dispatch, eventDate]);

  const toggle = () => setDropdownOpen(prevState => !prevState);

  const handleOnclick = (value: number) => {
    setTimer(value);
    selectedTime.current = value;
  };

  return (
    <div className='button-refresh'>
      <Dropdown isOpen={dropdownOpen} toggle={toggle}>
        <DropdownToggle>
          {timer === -1 ? 'Not Refresh' : `${timer}s`}
        </DropdownToggle>
        <DropdownMenu>
          <DropdownItem onClick={() => handleOnclick(-1)}>
            Not Refresh
          </DropdownItem>
          {timeRefreshTable.map((value, index) => (
            <DropdownItem
              key={`${value}-${index}`}
              onClick={() => handleOnclick(value)}>
              {value}s
            </DropdownItem>
          ))}
        </DropdownMenu>
      </Dropdown>
    </div>
  );
};

export default IconRefresh;
