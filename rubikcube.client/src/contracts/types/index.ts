import { Face, Faces } from "..";

export type FacesHandler = (faces: Face[]) => void

export type RotationHandler = (faceId: Faces, clockwise: boolean) => void