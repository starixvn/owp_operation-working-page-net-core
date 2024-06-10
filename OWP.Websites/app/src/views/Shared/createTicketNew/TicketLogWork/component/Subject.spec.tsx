import { render, fireEvent } from '@testing-library/react';
import Subject from './Subject';

describe('Subject', () => {
  const subjectValue = 'Test Subject';
  const errorValue = 'Subject is required';
  const handleOnChangeSubject = jest.fn();

  it('should call handleOnChangeSubject when subject value changes', () => {
    const { container } = render(
      <Subject
        subjectValue={subjectValue}
        errorValue={errorValue}
        handleOnChangeSubject={handleOnChangeSubject}
      />
    );
    const newSubjectValue = 'New Test Subject';
    const subject = container.querySelector('.MuiInputBase-input');
    subject &&
      fireEvent.change(subject, {
        target: { value: newSubjectValue }
      });

    expect(handleOnChangeSubject).toHaveBeenCalledTimes(1);
  });
});
