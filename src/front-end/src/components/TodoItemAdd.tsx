import { useState, ChangeEvent, FC } from 'react';
import { Container, Row, Col, Form, Button, Stack } from 'react-bootstrap';
import { TodoItem } from '../services/generated';
import { v4 as uuidv4 } from 'uuid';
import { useTodoContext } from '../contexts/TodoContext';
import  TodoItemAlert from './TodoItemAlert'

interface TodoItemProps { }

const TodoItemAdd: FC<TodoItemProps> = () => {
  const { clearError, addItem } = useTodoContext();
  const [description, setDescription] = useState<string>('');

  const handleDescriptionChange = (event: ChangeEvent<HTMLInputElement>) => {
    setDescription(event.target.value);
  };

  const handleAddItem = async () => {
      let todoItem: TodoItem = {
        id: uuidv4().toString(),
        description: description,
        isCompleted: false
      };
      await addItem(todoItem);
      setDescription('');
  };

  const handleClearDescription = async () => {
    setDescription('');
    await clearError();
  };

  return (
      <Container>
        <h1>Add Item</h1>
        <Form.Group as={Row} className='mb-3' controlId='formAddTodoItem'>
          <Form.Label column sm='2'>
            Description
          </Form.Label>
          <Col md='6'>
            <Form.Control
                type='text'
                placeholder='Enter description...'
                value={description}
                onChange={handleDescriptionChange}
            />
          </Col>
        </Form.Group>
        <Form.Group as={Row} className='mb-3 offset-md-2' controlId='formAddTodoItem'>
          <Stack direction='horizontal' gap={2}>
            <Button variant='primary' onClick={handleAddItem}>
              Add Item
            </Button>
            <Button variant='secondary' onClick={handleClearDescription}>
              Clear
            </Button>
          </Stack>
        </Form.Group>
        <Form.Group as={Row} className='mb-3' controlId='formAddTodoItem'>
          <TodoItemAlert />
        </Form.Group>
      </Container>
  );
};

export default TodoItemAdd;
