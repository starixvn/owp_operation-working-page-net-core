import { fireEvent, render } from '@testing-library/react';
import SearchableCellBody from './SearchableCellBody';

let wrapper = null;
let setIsOpening = null;
let setSearchKey = null;
let onKeyPress = null;
let searchKey = null;
let handelClearAllFilters = null;
let handleClickSearch = null;
let refIcon = null;
beforeEach(() => {
  setIsOpening = jest.fn();
  setSearchKey = jest.fn();
  onKeyPress = jest.fn();
  searchKey = 'haha';
  handelClearAllFilters = jest.fn();
  handleClickSearch = jest.fn();
  refIcon = {};
});

afterEach(() => {
  setIsOpening = null;
  setSearchKey = jest.fn();
  onKeyPress = jest.fn();
  searchKey = 'haha';
  handelClearAllFilters = jest.fn();
  handleClickSearch = jest.fn();
  refIcon = {};
  wrapper = null;
});

describe('SearchableCellBody under test', () => {
  it('when inputField change value, it should callback setSearchKey ', () => {
    const { container } = render(
      <SearchableCellBody
        setIsOpening={setIsOpening}
        setSearchKey={setSearchKey}
        onKeyPress={onKeyPress}
        searchKey={searchKey}
        onClearAllFilters={handelClearAllFilters}
        onClickSearch={handleClickSearch}
        refIcon={refIcon}
      />
    );
    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { name: 'pollName', value: 'spam' } });
    expect(setSearchKey.mock.calls.length).toEqual(1);
  });
  it('after type in input field, press Enter, it should callback onKeyPress', () => {
    const { container } = render(
      <SearchableCellBody
        setIsOpening={setIsOpening}
        setSearchKey={setSearchKey}
        onKeyPress={onKeyPress}
        searchKey={searchKey}
        onClearAllFilters={handelClearAllFilters}
        onClickSearch={handleClickSearch}
        refIcon={refIcon}
      />
    );
    const input = container.querySelector('.input-field');
    fireEvent.keyPress(input, { key: 'Enter', code: 13, charCode: 13 });
    expect(onKeyPress.mock.calls.length).toEqual(1);
  });
  it('when click button search, it should callback handleClickSearch', () => {
    const { container } = render(
      <SearchableCellBody
        setIsOpening={setIsOpening}
        setSearchKey={setSearchKey}
        onKeyPress={onKeyPress}
        searchKey={searchKey}
        onClearAllFilters={handelClearAllFilters}
        onClickSearch={handleClickSearch}
        refIcon={refIcon}
      />
    );

    const button = container.querySelector('.btn-search');
    fireEvent.click(button);
    expect(handleClickSearch.mock.calls.length).toEqual(1);
  });
  it('when click close icon, it should callback handleClickSearch', () => {
    const { container } = render(
      <SearchableCellBody
        setIsOpening={setIsOpening}
        setSearchKey={setSearchKey}
        onKeyPress={onKeyPress}
        searchKey={searchKey}
        onClearAllFilters={handelClearAllFilters}
        onClickSearch={handleClickSearch}
        refIcon={refIcon}
      />
    );
    const icon = container.querySelector('.close-popup');
    fireEvent.click(icon);
    expect(setIsOpening.mock.calls.length).toEqual(1);
  });
  it('when click Clear All Filters, it should callback handelClearAllFilters', () => {
    const { container } = render(
      <SearchableCellBody
        setIsOpening={setIsOpening}
        setSearchKey={setSearchKey}
        onKeyPress={onKeyPress}
        searchKey={searchKey}
        onClearAllFilters={handelClearAllFilters}
        onClickSearch={handleClickSearch}
        refIcon={refIcon}
      />
    );

    const clearAll = container.querySelector('.clear-all');
    fireEvent.click(clearAll);
    expect(handelClearAllFilters.mock.calls.length).toEqual(1);
  });
});
