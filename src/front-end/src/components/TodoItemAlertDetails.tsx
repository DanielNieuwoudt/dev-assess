import React, { FC } from 'react';
import { Accordion, Button, Modal, Table } from 'react-bootstrap';

interface TodoItemAlertDetailsProps {
    showModal: boolean;
    handleCloseModal: () => void;
    error: TodoItemError;
}

const TodoItemAlertDetails: FC<TodoItemAlertDetailsProps> = ({ showModal, handleCloseModal, error }) => {
    const tableStyle = {
        fontSize: '12px'
    };

    return (
        <Modal show={showModal} onHide={handleCloseModal}>
            <Modal.Header closeButton>
                <Modal.Title>Problem Details</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Table striped bordered style={tableStyle}>
                    <thead>
                    <tr>
                        <td>Title</td>
                        <td>{error.Title}</td>
                    </tr>
                    <tr>
                        <td>Detail</td>
                        <td>{error.Detail}</td>
                    </tr>
                    <tr>
                        <td>Errors</td>
                        <td>
                            {Object.entries(error.Errors).map(([field, messages]) => (
                                <div key={field}>
                                    {messages.map((message, index) => (
                                        <div key={index}>{field} - {message}</div>
                                    ))}
                                </div>
                            ))}
                        </td>
                    </tr>
                    </thead>
                </Table>
                <Accordion flush>
                    <Accordion.Item eventKey="0">
                        <Accordion.Header>More information</Accordion.Header>
                        <Accordion.Body>
                            <Table striped bordered style={tableStyle}>
                                <thead>
                                <tr>
                                    <td>Type</td>
                                    <td><a href={error.Type} target="_blank" rel="noopener noreferrer">{error.Type}</a></td>
                                </tr>
                                <tr>
                                    <td>Status</td>
                                    <td>{error.Status}</td>
                                </tr>
                                <tr>
                                    <td>Trace ID</td>
                                    <td>{error.TraceId}</td>
                                </tr>
                                </thead>
                            </Table>
                        </Accordion.Body>
                    </Accordion.Item>
                </Accordion>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleCloseModal}>
                    Close
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default TodoItemAlertDetails;
