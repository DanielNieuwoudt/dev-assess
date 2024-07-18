import { useEffect, FC } from 'react'
import { Table, Button } from 'react-bootstrap'
import { TodoItem } from '../services/generated';

interface TodoItemsProps {
  items: TodoItem[];
  fetchItems: () => Promise<TodoItem[]>;
}

const TodoItems: FC<TodoItemsProps> = ({ items, fetchItems }) => {
  useEffect(() => {
    fetchItems()
  }, [fetchItems])

  const markAsComplete = async (item: TodoItem) => {
    try {
      // TODO: Call the api
      await fetchItems()
    } catch (error) {
      console.error(error)
    }
  }

  return (
    <>
      <h1>
        Showing {items.length} Item(s){' '}
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
          {items.map((item) => (
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
  )
}

export default TodoItems
