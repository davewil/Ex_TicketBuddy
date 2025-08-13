import {setupServer, SetupServerApi} from "msw/node";
import {delay, http, HttpResponse, type JsonBodyType} from "msw";

export class MockServer {
    private server: SetupServerApi;
    public headers: Headers = new Headers();
    public content: string | null = null;

    constructor() {
        this.server = setupServer()
    }

    public static New() {
        return new MockServer();
    }

    public start() {
        this.server.listen();
    }

    public reset() {
        this.server.resetHandlers();
        this.headers = new Headers();
        this.content = null;
    }

    public get(url: string, response: JsonBodyType, delayValue?: number, success = true): () => boolean {
        let called = false;
        const was_called = () => called;
        this.server.use(http.get(url, async (req) => {
            if (delayValue) await delay(delayValue)
            called = true;
            this.headers = req.request.headers
            if (!success) return HttpResponse.json(response, {status: 422})
            return HttpResponse.json(response, {status: 200})
        }));
        return was_called
    }

    public post(url: string, response?: JsonBodyType, success = true, delayValue?: number, onCall?: () => void): () => boolean {
        let called = false;
        const was_called = () => called;
        this.server.use(http.post(url, async (req) => {
            if (delayValue) await delay(delayValue)
            called = true;
            this.headers = req.request.headers;
            if (onCall) onCall();
            req.request.body!.getReader().read().then(response => {
                this.content = JSON.parse(new TextDecoder().decode(response.value));
            })
            if (!success) return HttpResponse.json(response, {status: 422})
            if (response) return HttpResponse.json(response, {status: 201})
            return HttpResponse.json({}, {status: 204})
        }));
        return was_called
    }

    public put(url: string, response = {}, success = true) {
        let called = false;
        const was_called = () => called;
        this.server.use(http.put(url, (req) => {
            called = true;
            this.headers = req.request.headers
            req.request.body!.getReader().read().then(response => {
                this.content = JSON.parse(new TextDecoder().decode(response.value));
            })
            if (!success) return HttpResponse.json(response, {status: 422})
            return HttpResponse.json({}, {status: 200})
        }));
        return was_called
    }

    public delete(url: string, response = {}, success = true) {
        let called = false;
        const was_called = () => called;
        this.server.use(http.delete(url, (req) => {
            called = true;
            this.headers = req.request.headers
            if (!success) return HttpResponse.json(response, {status: 422})
            return HttpResponse.json({}, {status: 200})
        }));
        return was_called
    }
}