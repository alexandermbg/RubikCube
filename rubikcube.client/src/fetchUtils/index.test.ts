import { vi, describe, it, expect, beforeEach, Mock } from "vitest";
import { getCubeFaces, reset, rotate } from "./index";
import { Colors, Face, Faces, FacesHandler } from "../contracts";

describe("fetchUtils", () => {
    const mockFacesHandler: FacesHandler = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
        global.fetch = vi.fn();
    });

    describe("getCubeFaces", () => {
        it("should call facesHandler with data when fetch is successful", async () => {
            expect.assertions(2);
            const mockData: Face[] = [{ id: Faces.Down, colors: [[{ value: Colors.Red }]] }];
            (fetch as Mock).mockResolvedValueOnce({
                ok: true,
                json: vi.fn().mockResolvedValueOnce(mockData),
            });

            await getCubeFaces(mockFacesHandler);

            expect(fetch).toHaveBeenCalledWith(expect.any(String));
            expect(mockFacesHandler).toHaveBeenCalledWith(mockData);
        });

        it("should log an error when fetch fails", async () => {
            expect.assertions(1);
            const consoleErrorSpy = vi.spyOn(console, "error").mockImplementation(() => { });
            (fetch as Mock).mockRejectedValueOnce(new Error("Network error"));

            await getCubeFaces(mockFacesHandler);

            expect(consoleErrorSpy).toHaveBeenCalledWith("Error fetching cube faces:", expect.any(Error));
            consoleErrorSpy.mockRestore();
        });
    });

    describe("reset", () => {
        it("should call facesHandler with data when reset is successful", async () => {
            expect.assertions(2);
            const mockData: Face[] = [{ id: Faces.Down, colors: [[{ value: Colors.Red }]] }];
            (fetch as Mock).mockResolvedValueOnce({
                ok: true,
                json: vi.fn().mockResolvedValueOnce(mockData),
            });

            await reset(mockFacesHandler);

            expect(fetch).toHaveBeenCalledWith(expect.any(String), { method: "POST" });
            expect(mockFacesHandler).toHaveBeenCalledWith(mockData);
        });

        it("should log an error when reset fails", async () => {
            expect.assertions(1);
            const consoleErrorSpy = vi.spyOn(console, "error").mockImplementation(() => { });
            (fetch as Mock).mockRejectedValueOnce(new Error("Network error"));

            await reset(mockFacesHandler);

            expect(consoleErrorSpy).toHaveBeenCalledWith("Error resetting cube:", expect.any(Error));
            consoleErrorSpy.mockRestore();
        });
    });

    describe("rotate", () => {
        it("should call facesHandler with data when rotation is successful", async () => {
            expect.assertions(2);
            const mockData: Face[] = [{ id: Faces.Down, colors: [[{ value: Colors.Red }]] }];
            const faceId = Faces.Front
            const clockwise = true;

            (fetch as Mock).mockResolvedValueOnce({
                ok: true,
                json: vi.fn().mockResolvedValueOnce(mockData),
            });

            await rotate(faceId, clockwise, mockFacesHandler);

            expect(fetch).toHaveBeenCalledWith(
                expect.stringContaining(`clockwise=${clockwise}&faceId=${faceId}`),
                { method: "POST" }
            );
            expect(mockFacesHandler).toHaveBeenCalledWith(mockData);
        });

        it("should log an error when rotation fails", async () => {
            expect.assertions(1);
            const consoleErrorSpy = vi.spyOn(console, "error").mockImplementation(() => { });
            const faceId = Faces.Front
            const clockwise = true;

            (fetch as Mock).mockRejectedValueOnce(new Error("Network error"));

            await rotate(faceId, clockwise, mockFacesHandler);

            expect(consoleErrorSpy).toHaveBeenCalledWith("Error rotating cube:", expect.any(Error));
            consoleErrorSpy.mockRestore();
        });
    });
});