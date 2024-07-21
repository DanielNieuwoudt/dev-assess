import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import TodoItemAlertDetails from './TodoItemAlertDetails';

const mockError: TodoItemError = {
    Title: 'Sample Error',
    Errors: {
        field1: ['Error message 1', 'Error message 2'],
        field2: ['Error message 3'],
    },
    Type: 'https://example.com/error',
    Status: 400,
    TraceId: 'abc123'
};

describe('Given error When TodoItemAlertDetails rendered Then ', () => {
    const handleCloseModal = jest.fn();

    beforeEach(() => {
        handleCloseModal.mockClear();
    });

    test('renders the modal with error details', () => {
        render(<TodoItemAlertDetails showModal={true} handleCloseModal={handleCloseModal} error={mockError} />);

        expect(screen.getByText('Problem Details'))
            .toBeInTheDocument();

        expect(screen.getByText('Sample Error'))
            .toBeInTheDocument();

        
        expect(screen.getByText('field1:'))
            .toBeInTheDocument();
        expect(screen.getByText('Error message 1'))
            .toBeInTheDocument();
        expect(screen.getByText('Error message 2'))
            .toBeInTheDocument();
        expect(screen.getByText('field2:'))
            .toBeInTheDocument();
        expect(screen.getByText('Error message 3'))
            .toBeInTheDocument();

        expect(screen.getByText('https://example.com/error'))
            .toBeInTheDocument();
    });

    test('calls handleCloseModal when close button is clicked', () => {
        render(<TodoItemAlertDetails showModal={true} handleCloseModal={handleCloseModal} error={mockError} />);

        fireEvent.click(screen.getByText('Close'));

        expect(handleCloseModal)
            .toHaveBeenCalledTimes(1);
    });

    test('renders the modal when showModal is true', () => {
        render(<TodoItemAlertDetails showModal={true} handleCloseModal={handleCloseModal} error={mockError} />);

        expect(screen.getByText('Problem Details'))
            .toBeInTheDocument();
    });

    test('does not render the modal when showModal is false', () => {
        render(<TodoItemAlertDetails showModal={false} handleCloseModal={handleCloseModal} error={mockError} />);

        expect(screen.queryByText('Problem Details'))
            .not
            .toBeInTheDocument();
    });
});
