import { Colors, Faces } from ".."

export interface Color {
    value: Colors
}

export interface Face {
    id: Faces,
    colors: Color[][]
}

