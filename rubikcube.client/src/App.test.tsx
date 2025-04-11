import { expect, test } from 'vitest'
import { render } from '@testing-library/react'
import App from './App.tsx'

test('render App', () => {
    const { container } = render(<App />)
    expect(container).toBeTruthy()
})

