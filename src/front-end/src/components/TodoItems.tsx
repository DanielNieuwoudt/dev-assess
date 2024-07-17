import React, { useEffect, FC } from 'react'
import { Table, Button } from 'react-bootstrap'
import axios from 'axios'

interface TodoItem {
  id: number;
  description: string;
  completed?: boolean;
}

interface TodoItemsProps {
  items: TodoItem[];
  fetchItems: () => void;
}

const TodoItems: FC<TodoItemsProps> = ({ items, fetchItems }) => {
  useEffect(() => {
    fetchItems()
  }, [fetchItems])

  const markAsComplete = async (item: TodoItem) => {
    try {
      await axios.put(`/api/todos/${item.id}`, { ...item, completed: true })
      fetchItems()
    } catch (error) {
      console.error(error)
    }
  }

  return (
    <>
      <h1>
        Showing {items.length} Item(s){' '}
        <Button variant="primary" className="pull-right" onClick={fetchItems}>
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
                <Button variant="warning" size="sm" onClick={() => markAsComplete(item)}>
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
