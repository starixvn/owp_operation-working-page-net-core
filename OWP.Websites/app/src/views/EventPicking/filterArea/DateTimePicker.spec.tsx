import { render, fireEvent } from "@testing-library/react";
import DateTimePicker from "./DateTimePicker";
import {screen} from '@testing-library/dom'

jest.mock('react-redux');

describe('DateTimePicker under test', () => {
  it('should update date when DatePicker changes', ()=>{
   const {container}= render(<DateTimePicker/>)

   const dateInput=container.getElementsByClassName('from-date-picker')[0];
   fireEvent.change(dateInput, { target: { value: '2023-01-01' } });

   expect(dateInput.value).toBe('2023-01-01');
   console.debug(screen.debug());
  });
});