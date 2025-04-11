import { Face, Faces, FacesHandler, GetCubeFacesPath, ResetCubePath, RotateCubePath } from "../contracts"

export async function getCubeFaces(facesHandler: FacesHandler) {
    try {
        const response = await fetch(GetCubeFacesPath)
        if (response.ok) {
            const data: Face[] = await response.json()
            facesHandler(data)
        }

        throw new Error(`[${response.status}] Failed to fetch cube faces (${response.statusText})`)
    } catch (error) {
        console.error("Error fetching cube faces:", error)
    }
}

export async function reset(facesHandler: FacesHandler) {
    try {
        const response = await fetch(ResetCubePath, { method: "POST" })

        if (response.ok) {
            const data: Face[] = await response.json()
            facesHandler(data)
        }
    } catch (error) {
        console.error("Error resetting cube:", error)
    }
}

export async function rotate(faceId: Faces, clockwise: boolean, facesHandler: FacesHandler) {
    try {
        const response = await fetch(
            `${RotateCubePath}?clockwise=${clockwise}&faceId=${faceId}`,
            { method: "POST" }
        )
        if (response.ok) {
            const data: Face[] = await response.json()
            facesHandler(data)
        }
    } catch (error) {
        console.error("Error rotating cube:", error)
    }
}