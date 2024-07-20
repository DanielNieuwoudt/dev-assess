import TodoItemStatus from './TodoItemStatus';

describe('Given a TodoItemStatus Enum When used Then', () => {
    it('should have the correct values', () => {
        expect(TodoItemStatus.None).toBe(0);
        expect(TodoItemStatus.Added).toBe(1);
        expect(TodoItemStatus.Completed).toBe(2);
        expect(TodoItemStatus.Error).toBe(3);
        expect(TodoItemStatus.Refreshed).toBe(4);
    });

    it('should map string keys to correct values', () => {
        expect(TodoItemStatus['None']).toBe(0);
        expect(TodoItemStatus['Added']).toBe(1);
        expect(TodoItemStatus['Completed']).toBe(2);
        expect(TodoItemStatus['Error']).toBe(3);
        expect(TodoItemStatus['Refreshed']).toBe(4);
    });

    it('should map values to correct string keys', () => {
        expect(TodoItemStatus[0]).toBe('None');
        expect(TodoItemStatus[1]).toBe('Added');
        expect(TodoItemStatus[2]).toBe('Completed');
        expect(TodoItemStatus[3]).toBe('Error');
        expect(TodoItemStatus[4]).toBe('Refreshed');
    });
});
