import { FC } from 'react';
import { Table, Button } from 'react-bootstrap';
import { useTodoContext } from '../contexts/TodoContext';

interface TodoItemsProps { }

const TodoItems: FC<TodoItemsProps> = ({ }) => {
  const { items, markItemAsComplete, fetchItems } = useTodoContext();

  return (
      <>
        <h1>
          Showing {items.filter(item => !item.isCompleted).length} Item(s){' '}
          <Button variant='primary' className='pull-right' onClick={fetchItems}>
            Refresh
          </Button>
        </h1>

        <Table striped bordered hover>
          <thead>
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Action</th>
          </tr>
          </thead>
          <tbody>
          {items
              .filter(item => !item.isCompleted)
              .map((item) => (
                  <tr key={item.id}>
                    <td>{item.id}</td>
                    <td>{item.description}</td>
                    <td>
                      <Button variant='warning' size='sm' onClick={() => markItemAsComplete(item.id!)}>
                        Mark as completed
                      </Button>
                    </td>
                  </tr>
              ))}
          </tbody>
        </Table>
      </>
  );
};

export default TodoItems;
