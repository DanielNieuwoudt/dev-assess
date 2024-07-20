import TodoItemStatusMessages from './TodoItemStatusMessages';
describe('Given TodoItemStatusMessages When used Then', () => {
    const testCases = [
        { status: TodoItemStatusMessages.None, expectedMessage: 'Please enter a description for your todo item.' },
        { status: TodoItemStatusMessages.Added, expectedMessage: 'Your todo item has been added.' },
        { status: TodoItemStatusMessages.Completed, expectedMessage: 'Your todo item has been marked as completed.' },
        { status: TodoItemStatusMessages.Error, expectedMessage: 'We encountered a problem with your todo item.' },
        { status: TodoItemStatusMessages.Refreshed, expectedMessage: 'Your todo items have been refreshed.' },
    ];

    test.each(testCases)(
        'returns the correct message for $status',
        ({ status, expectedMessage }) => {
            expect(status).toBe(expectedMessage);
        }
    );
});