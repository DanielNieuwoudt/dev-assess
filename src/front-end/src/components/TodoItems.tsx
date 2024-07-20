import {FC} from 'react';
import {Button, Table} from 'react-bootstrap';
import {useTodoContext} from '../contexts/TodoContext';
import TodoItemStatus from '../enumerations/TodoItemStatus';

interface TodoItemsProps { }

const TodoItems: FC<TodoItemsProps> = ({ }) => {
  const { items, fetchItems, markItemAsComplete, setItemStatus } = useTodoContext();

  const refreshItems = async () => {
      await fetchItems();
      await setItemStatus(TodoItemStatus.Refreshed);
  }
  
  return (
      <>
        <h1>
          Showing {items.filter(item => !item.isCompleted).length} Item(s){' '}
          <Button variant='primary' className='pull-right' onClick={refreshItems}>
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
