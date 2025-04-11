import { render, screen, fireEvent } from "@testing-library/react"
import { describe, it, vi, expect } from "vitest"
import { getButton, getFace } from "./index"
import { Colors, Faces, Face } from "../contracts"

describe("getButton", () => {
    it("renders buttons with correct labels and calls rotationHandler on click", () => {
        const mockRotationHandler = vi.fn()
        const facesEnumKey = "Front"

        render(getButton(facesEnumKey, mockRotationHandler))

        const plus90Button = screen.getByText("F")
        const minus90Button = screen.getByText("F'")

        fireEvent.click(plus90Button)
        expect(mockRotationHandler).toHaveBeenCalledWith(Faces.Front, true)

        fireEvent.click(minus90Button)
        expect(mockRotationHandler).toHaveBeenCalledWith(Faces.Front, false)
    })
})

describe("getFace", () => {
    it("renders face with correct colors", () => {
        const mockFace: Face = {
            id: Faces.Front,
            colors: [
                [{ value: Colors.Red }, { value: Colors.Green }],
                [{ value: Colors.Blue }, { value: Colors.Yellow }]
            ]
        }

        const { container } = render(getFace(mockFace))
        expect(container.firstChild).toBeTruthy()
    })

    it("returns null if face is undefined", () => {
        const { container } = render(getFace(undefined))
        expect(container.firstChild).toBeNull()
    })
})