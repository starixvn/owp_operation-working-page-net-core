import { renderHook } from '@testing-library/react';
import { useSelector, useDispatch } from 'react-redux';
import useDeletedLeague from './useDeletedLeague';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
    useSelector: jest.fn(),
    useDispatch: jest.fn(),
    useEffect: jest.fn()
}));

const mockedDispatch = jest.fn();

const setUp = date => {
    useDispatch.mockReturnValue(mockedDispatch);
    useSelector.mockImplementation(selectorFn =>
        selectorFn({
            filter: {
                filterDate: date
            }
        })
    );
};

describe('test useDeletedLeague', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });
    it('should not call dispatch when filterDate invalid', () => {
        setUp(undefined);
        renderHook(() => useDeletedLeague());
        expect(mockedDispatch).toHaveBeenCalledTimes(0);
    });

    it('should call dispatch when filterDate valid', () => {
        setUp(new Date());
        renderHook(() => useDeletedLeague());
        expect(mockedDispatch).toHaveBeenCalledTimes(1);
    });
});
