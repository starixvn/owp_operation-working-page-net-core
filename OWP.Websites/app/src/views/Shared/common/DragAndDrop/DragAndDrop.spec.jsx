import { useState } from 'react';
import { render, fireEvent } from '@testing-library/react';
import DragAndDrop from './DragAndDrop';

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const setState = jest.fn();
const mockedOnDrop = jest.fn();

const setUp = () => {
  useState.mockImplementation(init => [init, setState]);
};

describe('DragAndDrop under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render  props.children, and child must have classNam contain `drag-able`', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drag-able');
    expect(dropZone).toBeInTheDocument();
  });

  it('should setState when dragOver `drag-able` child ', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drop-zone');
    fireEvent.dragOver(dropZone, {
      target: { className: 'drag-able' },
      preventDefault: jest.fn()
    });

    expect(setState).toBeCalledTimes(1);
  });
  it('should don`t setState when dragOver not `drag-able` child ', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drop-zone');
    fireEvent.dragOver(dropZone, {
      target: { className: 'something' },
      preventDefault: jest.fn()
    });

    expect(setState).toBeCalledTimes(1);
  });

  it('should setState when DragLeave `drag-able` child ', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drop-zone');
    fireEvent.dragLeave(dropZone, {
      target: { className: 'drag-able' },
      preventDefault: jest.fn()
    });

    expect(setState).toBeCalledTimes(1);
  });
  it('should don`t setState when DragLeave not `drag-able` child ', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drop-zone');
    fireEvent.dragLeave(dropZone, {
      target: { className: 'something' },
      preventDefault: jest.fn()
    });

    expect(setState).toBeCalledTimes(1);
  });
  it('should callback when Drop in `drag-able` child ', () => {
    setUp();
    const { container } = render(
      <DragAndDrop onDrop={mockedOnDrop}>
        <div className='drag-able'>Test</div>
      </DragAndDrop>
    );
    const dropZone = container.querySelector('.drop-zone');

    fireEvent.drop(dropZone, {
      dataTransfer: {
        files: [1, 2, 3]
      },
      preventDefault: jest.fn()
    });
    expect(mockedOnDrop).toBeCalledTimes(1);
  });
});
