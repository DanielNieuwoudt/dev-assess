import { render, screen } from '@testing-library/react';
import App from './App';
import { TodoProvider } from './providers/TodoProvider';

test('renders the footer text', async () => {
  render(
      <TodoProvider>
        <App />
      </TodoProvider>
  );

  const footerElement = await screen.findByText(/clearpoint.digital/i);
  expect(footerElement).toBeInTheDocument();
});