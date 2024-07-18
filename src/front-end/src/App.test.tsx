import { render, screen, act } from '@testing-library/react'
import App from './App'

test('renders the footer text', () => {
  act(() => {
    render(<App />);
  });
  const footerElement = screen.getByText(/clearpoint.digital/i)
  expect(footerElement).toBeInTheDocument()
})
