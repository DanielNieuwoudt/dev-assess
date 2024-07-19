import { useState, ChangeEvent, FC } from 'react';
import { Container, Row, Col, Form, Button, Stack } from 'react-bootstrap';
import { TodoItem } from '../services/generated';
import TodoApi from "../services/TodoApi";
import TodoItems from "./TodoItems";
import { v4 as uuidv4 } from 'uuid';

interface AddTodoItemProps { }

const AddTodoItem: FC<AddTodoItemProps> = () => {
  const [description, setDescription] = useState<string>('');
  const handleDescriptionChange = (event: ChangeEvent<HTMLInputElement>) => {
    setDescription(event.target.value);
  };

  const handleAdd = async () => {
    try {
      let todoItem: TodoItem = {
        id: uuidv4().toString(),
        description: description,
        isCompleted: false
      };

      await TodoApi.postTodoItem(todoItem);
      setDescription('');
    } catch (error) {
      console.error(error);
    }
  };

  const handleClear = () => {
    setDescription('');
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
            <Button variant='primary' onClick={handleAdd}>
              Add Item
            </Button>
            <Button variant='secondary' onClick={handleClear}>
              Clear
            </Button>
          </Stack>
        </Form.Group>
      </Container>
  );
};

export default AddTodoItem;
