import React, { useEffect, useState } from 'react'
import './App.css'
import { Face, Faces } from './contracts'
import { getCubeFaces, reset, rotate } from './fetchUtils'
import { getButton, getFace } from './components'

function App() {
    const [faces, setFaces] = useState<Face[]>()

    useEffect(
        () => { getCubeFaces(setFaces) },
        []
    )

    const getFaceById = React.useCallback(
        (faceId: Faces) => getFace(faces?.find((f) => f.id === faceId)),
        [faces]
    )

    const getRotationButton = React.useCallback(
        (facesEnumKey: string) => getButton(facesEnumKey,
            (faceId: Faces, clockwise: boolean) => { rotate(faceId, clockwise, setFaces) }
        ),
        [setFaces]
    )

    return (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "start", rowGap: "50px" }}>
            <div style={{ display: "flex", flexDirection: "row" }}>
                <div style={{ display: "flex", flexDirection: "column" }}>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                    <div>{getFaceById(Faces.Left)}</div>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                </div>

                <div style={{ display: "flex", flexDirection: "column" }}>
                    <div>{getFaceById(Faces.Upper)}</div>
                    <div>{getFaceById(Faces.Front)}</div>
                    <div>{getFaceById(Faces.Down)}</div>
                </div>

                <div style={{ display: "flex", flexDirection: "column" }}>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                    <div>{getFaceById(Faces.Right)}</div>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                </div>

                <div style={{ display: "flex", flexDirection: "column" }}>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                    <div>{getFaceById(Faces.Back)}</div>
                    <div style={{ minWidth: "162px", minHeight: "162px", }}></div>
                </div>
            </div>

            <div style={{ display: "flex", flexDirection: "row", width: "100%" }}>
                <div style={{ display: "flex", flexDirection: "column", width: "100%" }}>
                    <button
                        style={{ margin: "5px" }}
                        onClick={() => { reset(setFaces) }}
                    >
                        Reset
                    </button>

                    <div style={{ display: "flex", flexDirection: "row", width: "100%", justifyContent: "space-between" }}>                        {
                        Object.values(Faces)
                            .filter((value) => typeof value === "string")
                            .map((k: string) => getRotationButton(k))
                    }
                    </div>
                </div>
            </div>
        </div>
    )
}

export default App