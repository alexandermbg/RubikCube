import { Colors, Face, Faces, RotationHandler } from "../contracts"

export function getButton(facesEnumKey: string, rotationHandler: RotationHandler) {
    const firstLetter = facesEnumKey.slice(0, 1).toUpperCase()

    return (
        <div style={{ display: "flex", flexDirection: "column" }} key={facesEnumKey}>
            <button
                style={{ margin: "5px" }}
                key={`${facesEnumKey}_plus90`}
                onClick={() => {
                    rotationHandler(Faces[facesEnumKey as keyof typeof Faces], true)
                }}
            >
                {firstLetter}
            </button>

            <button
                style={{ margin: "5px" }}
                key={`${facesEnumKey}_minus90`}
                onClick={() => {
                    rotationHandler(Faces[facesEnumKey as keyof typeof Faces], false)
                }}
            >
                {firstLetter}'
            </button>
        </div>
    )
}

export function getFace(face: Face | undefined) {
    if (!face) return null

    return (
        <div key={face.id} style={{ display: "flex", flexDirection: "column" }}>
            <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
                {
                    face.colors.map((row, rowIndex) => (
                        <div key={rowIndex} style={{ display: "flex", flexDirection: "row" }}>
                            {
                                row.map((color, colIndex) => (
                                    <div
                                        key={colIndex}
                                        style={{
                                            width: "50px",
                                            height: "50px",
                                            color: "black",
                                            backgroundColor: Colors[color.value],
                                            border: "1px solid black",
                                            margin: "1px"
                                        }}
                                    ></div>
                                ))
                            }
                        </div>
                    ))
                }
            </div>
        </div>
    )
}