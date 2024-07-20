import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';
import TodoProvider from './providers/TodoProvider';

describe('Given the App Component When rendered Then', () => {
    it('should render the ClearPoint logo', () => {
        render(
            <TodoProvider>
                <App />
            </TodoProvider>
        );
        const logo = screen.getByRole('img');
        expect(logo).toBeInTheDocument();
        expect(logo).toHaveAttribute('src', 'clearPointLogo.png');
    });

    it('should render the alert with the heading', () => {
        render(
            <TodoProvider>
                <App />
            </TodoProvider>
        );
        const alertHeading = screen.getByText('Todo List App');
        expect(alertHeading).toBeInTheDocument();
    });

    it('should render the TodoItemAdd component', () => {
        render(
            <TodoProvider>
                <App />
            </TodoProvider>
        );
        const todoItemAdd = screen.getByText('Add a todo item');
        expect(todoItemAdd).toBeInTheDocument();
    });

    it('should render the TodoItems component', () => {
        render(
            <TodoProvider>
                <App />
            </TodoProvider>
        );
        const todoItems = screen.getByText('Showing 0 Item(s)');
        expect(todoItems).toBeInTheDocument();
    });

    it('should render the footer', () => {
        render(
            <TodoProvider>
                <App />
            </TodoProvider>
        );
        const footer = screen.getByText('© 2021 Copyright:');
        expect(footer).toBeInTheDocument();

        const link = screen.getByRole('link', { name: /clearpoint\.digital/i });
        expect(link).toBeInTheDocument();
        expect(link).toHaveAttribute('href', 'https://clearpoint.digital');
    });
});
