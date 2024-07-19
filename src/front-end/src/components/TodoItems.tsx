import { useEffect, useState, FC } from 'react';
import { Table, Button } from 'react-bootstrap';
import { TodoItem } from '../services/generated';
import TodoApi from "../services/TodoApi";

interface TodoItemsProps { }

const TodoItems: FC<TodoItemsProps> = ({ }) => {
  const [items, setItems] = useState<TodoItem[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const fetchedItems = await TodoApi.getTodoItems();
        setItems(fetchedItems);
      } catch (error) {
        console.error(error);
      }
    };

    fetchData();
  }, []);

  const markAsComplete = async (item: TodoItem) => {
    try {
      item.isCompleted = true;
      
      await TodoApi.putTodoItem(item.id!, item);

      const updatedItems = items.map(i =>
          i.id === item.id ? { ...i, isCompleted: true } : i
      );
      setItems(updatedItems);
    } catch (error) {
      console.error(error);
    }
  };

  const refreshItems = async () => {
    try {
      const fetchedItems = await TodoApi.getTodoItems();
      setItems(fetchedItems);
    } catch (error) {
      console.error(error);
    }
  };

  return (
      <>
        <h1>
          Showing {items.length} Item(s){' '}
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
                  <Button variant='warning' size='sm' onClick={() => markAsComplete(item)}>
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
